using OpenTK;

namespace Voxels {
    public class StoneBlock : Block {
        public StoneBlock() {
            PrimaryColor = new Vector3(1.0f, 0.5f, 0.2f);
            SecondaryColor = PrimaryColor * 0.75f;
            GeometryShader = Program.Resources.SolidBlockGS;
        }
    }
}