using System;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;

namespace Voxels {
    public class Voxel {
        public const int Size = 16;
        public const int BlockCount = 16 * 16 * 16;

        public Block[,,] Blocks { get; } = new Block[Size, Size, Size];

        public void Fill<T>() where T : Block {
            Parallel.ForEach(Blocks.Cast<Block>(), b => Activator.CreateInstance<T>());
        }
    }
}