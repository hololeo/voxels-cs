using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Voxels {
    public struct VoxelVertex {
        [VertexAttrib(0, 3, typeof(float))] public Vector3 Position;
        [VertexAttrib(1, 1, typeof(uint), Offset = 12)] public uint BlockType;
    }

    public class World : IDisposable {
        public IReadOnlyDictionary<(int, int, int), Voxel> Voxels => _voxels;
        private Dictionary<(int, int, int), Voxel> _voxels = new Dictionary<(int, int, int), Voxel>();
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

            _ppo = GL.GenProgramPipeline();
            GL.UseProgramStages(_ppo, ProgramStageMask.VertexShaderBit, Program.Resources.VoxelVS.ProgramID);
            GL.UseProgramStages(_ppo, ProgramStageMask.FragmentShaderBit, Program.Resources.VoxelFS.ProgramID);
        }

        public void Dispose() {
            foreach (var (_, voxel) in Voxels) voxel.Dispose();
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
            var floats = new float[16];

            GL.CullFace(CullFaceMode.Back);
            GL.BindProgramPipeline(_ppo);
            foreach (var (id, block) in Block.Blocks) {
                var programID = block.GeometryShader?.ProgramID ?? 0;
                if (programID == 0) continue;

                GL.UseProgramStages(_ppo, ProgramStageMask.GeometryShaderBit, programID);

                var mvpLocation = GL.GetUniformLocation(programID, "u_mvp");

                var colorLocation = GL.GetUniformLocation(programID, "u_primaryColor");
                GL.ProgramUniform3(programID, colorLocation,
                    block.PrimaryColor.X, block.PrimaryColor.Y, block.PrimaryColor.Z);
                colorLocation = GL.GetUniformLocation(programID, "u_secondaryColor");
                GL.ProgramUniform3(programID, colorLocation,
                    block.SecondaryColor.X, block.SecondaryColor.Y, block.SecondaryColor.Z);
                var idLocation = GL.GetUniformLocation(programID, "u_blockID");
                GL.ProgramUniform1((uint) programID, idLocation, id);

                foreach (var ((x, y, z), voxel) in Voxels) {
                    var model = Matrix4.CreateTranslation(x * Voxel.Size, y * Voxel.Size, z * Voxel.Size);
                    Helper.MatrixToFloats(model * _camera.CalculateViewProjectionMatrix(), floats);
                    GL.ProgramUniformMatrix4(programID, mvpLocation, 1, true, floats);

                    GL.BindVertexArray(voxel.Vao.Vao);
                    GL.DrawArrays(PrimitiveType.Points, 0, Voxel.BlockCount);
                    GL.BindVertexArray(0);
                }
            }
            GL.BindProgramPipeline(0);
        }

        public void GenerateChunk(int x, int y, int z, Random random = null) {
            if (_voxels.ContainsKey((x, y, z))) return;
            _voxels[(x, y, z)] = new Voxel(random ?? new Random());
        }
    }
}