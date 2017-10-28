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
            Position = new Vector3(0f, 0f, -10f),
            Direction = Vector3.Zero,
            Fov = 90f,
            AspectRatio = Program.AspectRatio
        };
        private const float _cameraMovementSpeed = 8f, _cameraRotationSpeed = 8f;
        private int _lastMouseX = -1, _lastMouseY = -1;

        public World() {
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

            var colorLocation = GL.GetUniformLocation(Program.Resources.VoxelFS.ProgramID, "u_color");
            GL.ProgramUniform3(Program.Resources.VoxelFS.ProgramID, colorLocation, 0.2f, 0.5f, 1.0f);
        }

        public void Dispose() {
            _vao?.Dispose();
        }

        public void Update(float delta, KeyboardState keyboard, MouseState mouse) {
            if (keyboard[Key.W]) _camera.Position.Z -= _cameraMovementSpeed * delta;
            if (keyboard[Key.A]) _camera.Position.X -= _cameraMovementSpeed * delta;
            if (keyboard[Key.S]) _camera.Position.Z += _cameraMovementSpeed * delta;
            if (keyboard[Key.D]) _camera.Position.X += _cameraMovementSpeed * delta;

            if (_lastMouseX != -1 && _lastMouseY != -1) {
                var deltaX = (float) (mouse.X - _lastMouseX);
                var deltaY = (float) (mouse.Y - _lastMouseY);
                _camera.Direction.X += deltaY * delta * _cameraRotationSpeed;
                _camera.Direction.Y += deltaX * delta * _cameraRotationSpeed;
                //_camera.Direction.X = MathF.Min(MathF.Max(_camera.Direction.X, -0.99f), 0.99f);
            }
            _lastMouseX = mouse.X;
            _lastMouseY = mouse.Y;
        }

        public void Render() {
            Console.WriteLine(_camera.Position);
            var viewProjLocation = GL.GetUniformLocation(Program.Resources.SolidBlockGS.ProgramID, "u_viewProj");
            var floats = new float[16];
            Helper.MatrixToFloats(_camera.CalculateViewProjectionMatrix(), floats);
            GL.ProgramUniformMatrix4(Program.Resources.SolidBlockGS.ProgramID, viewProjLocation, 1, true, floats);

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