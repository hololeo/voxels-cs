using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public struct VoxelVertex {
        [VertexAttrib(0, 3, typeof(float))] public Vector3 Position;
    }

    public class World : IDisposable {
        public IReadOnlyDictionary<(int, int, int), Voxel> Voxels => _voxels;
        private Dictionary<(int, int, int), Voxel> _voxels = new Dictionary<(int, int, int), Voxel>();
        private VertexArray _vao;
        private int _ppo;

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

            var color = GL.GetUniformLocation(Program.Resources.VoxelFS.ProgramID, "u_color");
            GL.ProgramUniform3(Program.Resources.VoxelFS.ProgramID, color, 0.2f, 0.5f, 1.0f);
        }

        public void Dispose() {
            _vao?.Dispose();
        }

        public void Render() {
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