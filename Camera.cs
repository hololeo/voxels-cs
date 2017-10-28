using OpenTK;

namespace Voxels {
    public class Camera {
        public Vector3 Position {
            get { return _pos; }
            set {
                _pos = value;
                _isViewMatrixDirty = true;
            }
        }

        public Vector3 Direction {
            get { return _dir; }
            set {
                _dir = value;
                _isViewMatrixDirty = true;
            }
        }

        public Matrix4 View => _isViewMatrixDirty ? CalculateViewMatrix() : _vpMatrix;

        private Vector3 _pos = Vector3.Zero;
        private Vector3 _dir = Vector3.Zero;
        private Matrix4 _vpMatrix = Matrix4.Identity;
        private bool _isViewMatrixDirty;

        private Matrix4 CalculateViewMatrix() {
            var right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _dir));
            var up = Vector3.Cross(_dir, right);
            return _vpMatrix = Matrix4.LookAt(_pos, _pos - _dir, up);
        }
    }
}