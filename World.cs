using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Voxels {
    public struct VoxelVertex {
        [VertexAttrib(0, 3, typeof(float))] public Vector3 Position;
    }

    public class World : IDisposable {
        public IReadOnlyDictionary<(int, int, int), Voxel> Voxels => _voxels;
        private Dictionary<(int, int, int), Voxel> _voxels = new Dictionary<(int, int, int), Voxel>();
        private VertexArray _vao;
        private int _ppo;
        private Camera _camera = new Camera {
            Position = Vector3.Zero,
            Front = Vector3.UnitZ,
            Fov = 90f,
            AspectRatio = Program.AspectRatio
        };
        private const float _cameraMovementSpeed = 5f, _cameraRotationSpeed = 0.2f;
        private int _lastMouseX = int.MaxValue, _lastMouseY;
        private bool _forward, _backward, _left, _right;
        private Vector2 _camDir = Vector2.Zero;

        public World() {
            var window = Program.Window;
            window.KeyDown += OnKeyDown;
            window.KeyUp += OnKeyUp;

            _vao = VertexArray.Create(() => {
                var vertices = new VoxelVertex[Voxel.BlockCount];
                foreach (var x in Enumerable.Range(0, Voxel.Size))
                foreach (var y in Enumerable.Range(0, Voxel.Size))
                foreach (var z in Enumerable.Range(0, Voxel.Size))
                    vertices[z * Voxel.Size * Voxel.Size + y * Voxel.Size + x] = new VoxelVertex {
                        Position = new Vector3(x, y, z)
                    };
                return new[] {
                    ArrayBuffer.CreateAsVertices(vertices, BufferUsageHint.StaticDraw)
                };
            });

            _ppo = GL.GenProgramPipeline();
            GL.UseProgramStages(_ppo, ProgramStageMask.VertexShaderBit, Program.Resources.VoxelVS.ProgramID);
            GL.UseProgramStages(_ppo, ProgramStageMask.FragmentShaderBit, Program.Resources.VoxelFS.ProgramID);
            GL.UseProgramStages(_ppo, ProgramStageMask.GeometryShaderBit, Program.Resources.SolidBlockGS.ProgramID);

            var colorLocation = GL.GetUniformLocation(Program.Resources.SolidBlockGS.ProgramID, "u_primaryColor");
            GL.ProgramUniform3(Program.Resources.SolidBlockGS.ProgramID, colorLocation, 0.2f, 0.5f, 1.0f);
            colorLocation = GL.GetUniformLocation(Program.Resources.SolidBlockGS.ProgramID, "u_secondaryColor");
            GL.ProgramUniform3(Program.Resources.SolidBlockGS.ProgramID, colorLocation, 0.1f, 0.25f, 0.5f);
        }

        public void Dispose() {
            _vao?.Dispose();

            var window = Program.Window;
            window.KeyDown -= OnKeyDown;
            window.KeyUp -= OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs args) {
            switch (args.Key) {
                case Key.A: _left = true; break;
                case Key.D: _right = true; break;
                case Key.W: _forward = true; break;
                case Key.S: _backward = true; break;
            }
        }

        private void OnKeyUp(object sender, KeyboardKeyEventArgs args) {
            switch (args.Key) {
                case Key.A: _left = false; break;
                case Key.D: _right = false; break;
                case Key.W: _forward = false; break;
                case Key.S: _backward = false; break;
            }
        }

        public void Update(float delta) {
            var mouse = Mouse.GetState();
            if (_lastMouseX == int.MaxValue) {
                _lastMouseX = mouse.X;
                _lastMouseY = mouse.Y;
                return;
            }
            _camDir.X += (mouse.X - _lastMouseX) * delta * _cameraRotationSpeed;
            _camDir.Y -= (mouse.Y - _lastMouseY) * delta * _cameraRotationSpeed;
            _lastMouseX = mouse.X;
            _lastMouseY = mouse.Y;
            _camDir.Y = MathF.Max(MathF.Min(_camDir.Y, 1.57f), -1.57f);

            var front = Vector3.Normalize(new Vector3 {
                X = MathF.Cos(_camDir.Y) * MathF.Cos(_camDir.X),
                Y = MathF.Sin(_camDir.Y),
                Z = MathF.Cos(_camDir.Y) * MathF.Sin(_camDir.X)
            });
            var right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            if (_forward) _camera.Position += front * delta * _cameraMovementSpeed;
            if (_backward) _camera.Position -= front * delta * _cameraMovementSpeed;
            if (_left) _camera.Position -= right * delta * _cameraMovementSpeed;
            if (_right) _camera.Position += right * delta * _cameraMovementSpeed;
            _camera.Front = front;
        }

        public void Render() {
            var viewProjLocation = GL.GetUniformLocation(Program.Resources.SolidBlockGS.ProgramID, "u_viewProj");
            var floats = new float[16];
            Helper.MatrixToFloats(_camera.CalculateViewProjectionMatrix(), floats);
            GL.ProgramUniformMatrix4(Program.Resources.SolidBlockGS.ProgramID, viewProjLocation, 1, true, floats);

            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.BindProgramPipeline(_ppo);
            GL.BindVertexArray(_vao.Vao);
            GL.DrawArrays(PrimitiveType.Points, 0, Voxel.BlockCount);
            GL.BindVertexArray(0);
            GL.BindProgramPipeline(0);
        }

        public void GenerateChunk(int x, int y, int z) {
            if (_voxels.ContainsKey((x, y, z))) return;
            _voxels[(x, y, z)] = new Voxel();
        }
    }
}