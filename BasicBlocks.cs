using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class StoneBlock : Block {
        public StoneBlock(Vector3 baseColor) {
            PrimaryColor = baseColor;
            SecondaryColor = PrimaryColor * 0.75f;
            GeometryShader = Program.Resources.SolidBlockGS;
        }
    }
}