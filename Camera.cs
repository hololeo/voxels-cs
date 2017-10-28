using OpenTK;

namespace Voxels {
    public class Camera {
        public Vector3 Position;
        public Vector3 Front;
        public float Fov;
        public float AspectRatio = 1f;

        public Matrix4 CalculateViewProjectionMatrix() {
            var right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Front));
            var up = Vector3.Cross(Front, right);
            return Matrix4.LookAt(Position, Position + Front, up)
                * Matrix4.CreatePerspectiveFieldOfView(Fov * MathHelper.Pi / 180f, AspectRatio, 0.01f, 1000f);
        }
    }
}