using System.Collections.Generic;

namespace Voxels {
    public class World {
        public IReadOnlyDictionary<(int, int, int), Voxel> Voxels => _voxels;
        private Dictionary<(int, int, int), Voxel> _voxels = new Dictionary<(int, int, int), Voxel>();

        public void GenerateChunk(int x, int y, int z) {
            if (_voxels.ContainsKey((x, y, z))) return;
            _voxels[(x, y, z)] = new Voxel();
        }
    }
}