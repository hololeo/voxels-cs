using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class Voxel : IDisposable {
        public const int Size = 16;
        public const int BlockCount = 16 * 16 * 16;

        public VertexArray Vao => _vao;
        public Block[,,] Blocks { get; } = new Block[Size, Size, Size];

        private VertexArray _vao;

        public Voxel() {
            _vao = VertexArray.Create(() => {
                var random = new Random();
                var vertices = new VoxelVertex[Voxel.BlockCount];
                foreach (var x in Enumerable.Range(0, Voxel.Size))
                foreach (var y in Enumerable.Range(0, Voxel.Size))
                foreach (var z in Enumerable.Range(0, Voxel.Size))
                    vertices[z * Voxel.Size * Voxel.Size + y * Voxel.Size + x] = new VoxelVertex {
                        Position = new Vector3(x, y, z),
                        BlockType = (uint) random.Next(0, 6)
                    };
                return new[] {
                    ArrayBuffer.CreateAsVertices(vertices, BufferUsageHint.StaticDraw)
                };
            });
        }

        public void Dispose() {
            _vao?.Dispose();
        }

        public void Fill(Block block) {
            Blocks.AsParallel().Cast<Block>().ForAll(b => b = block);
        }
    }
}