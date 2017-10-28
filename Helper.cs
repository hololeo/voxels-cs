using OpenTK;

namespace Voxels {
    public static class Helper {
        public static void MatrixToFloats(Matrix4 matrix, float[] output) {
            output[0] = matrix[0, 0];
            output[1] = matrix[0, 1];
            output[2] = matrix[0, 2];
            output[3] = matrix[0, 3];
            output[4] = matrix[1, 0];
            output[5] = matrix[1, 1];
            output[6] = matrix[1, 2];
            output[7] = matrix[1, 3];
            output[8] = matrix[2, 0];
            output[9] = matrix[2, 1];
            output[10] = matrix[2, 2];
            output[11] = matrix[2, 3];
            output[12] = matrix[3, 0];
            output[13] = matrix[3, 1];
            output[14] = matrix[3, 2];
            output[15] = matrix[3, 3];
        }
    }
}