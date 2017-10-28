using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class Resources : IDisposable {
        public ShaderProgram VoxelVS { get; private set; }
        public ShaderProgram VoxelFS { get; private set; }
        public ShaderProgram SolidBlockGS { get; private set; }

        public void Load() {
            VoxelVS = ShaderProgram.CompileFromFile(ShaderType.VertexShader, "Assets/Voxel.vert");
            VoxelFS = ShaderProgram.CompileFromFile(ShaderType.FragmentShader, "Assets/Voxel.frag");
            SolidBlockGS = ShaderProgram.CompileFromFile(ShaderType.GeometryShader, "Assets/SolidBlock.geom");
        }

        public void Dispose() {
            VoxelVS?.Dispose();
            VoxelFS?.Dispose();
        }
    }
}