using OpenTK;

namespace Voxels {
    public class Camera {
        public Vector3 Position;
        public Vector3 Direction;
        public float Fov;
        public float AspectRatio = 1f;

        public Matrix4 CalculateViewProjectionMatrix() {
            var right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Direction));
            var up = Vector3.Cross(Direction, right);
            return Matrix4.LookAt(Position, Position - Direction, up)
                * Matrix4.CreatePerspectiveFieldOfView(Fov * MathHelper.Pi / 180f, AspectRatio, 0.01f, 1000f);
        }
    }
}