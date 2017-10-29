using System.Collections.Generic;
using OpenTK;

namespace Voxels {
    public abstract class Block {
        public static IReadOnlyDictionary<uint, Block> Blocks { get; private set; } = new Dictionary<uint, Block> {
            [1] = new StoneBlock()
        };

        public Vector3 PrimaryColor { get; protected set; } = Vector3.Zero;
        public Vector3 SecondaryColor { get; protected set; } = Vector3.Zero;
        public ShaderProgram GeometryShader { get; protected set; }
    }
}