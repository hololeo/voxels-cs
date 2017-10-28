using OpenTK;

namespace Voxels {
    public abstract class Block {
        public Vector3 PrimaryColor { get; protected set; } = Vector3.Zero;
        public Vector3 SecondaryColor { get; protected set; } = Vector3.Zero;
    }
}