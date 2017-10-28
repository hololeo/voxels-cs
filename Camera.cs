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

        public float Fov {
            get { return _fov; }
            set {
                _fov = value * MathHelper.Pi / 180f;
                _isProjMatrixDirty = true;
            }
        }

        public float AspectRatio {
            get { return _aspectRatio; }
            set {
                _aspectRatio = value;
                _isProjMatrixDirty = true;
            }
        }

        public Matrix4 View => _isViewMatrixDirty ? CalculateViewMatrix() : _view;

        public Matrix4 Projection => _isProjMatrixDirty ? CalculateProjectionMatrix() : _proj;

        private Vector3 _pos = Vector3.Zero;
        private Vector3 _dir = Vector3.Zero;
        private float _fov, _aspectRatio;
        private Matrix4 _view;
        private Matrix4 _proj;
        private bool _isViewMatrixDirty = true, _isProjMatrixDirty = true;

        private Matrix4 CalculateViewMatrix() {
            var right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _dir));
            var up = Vector3.Cross(_dir, right);
            _isViewMatrixDirty = false;
            return _view = Matrix4.LookAt(_pos, _pos - _dir, up);
        }

        private Matrix4 CalculateProjectionMatrix() {
            _isProjMatrixDirty = false;
            return _proj = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 1000f);
        }
    }
}