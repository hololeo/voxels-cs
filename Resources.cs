using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class Resources : IDisposable {
        public ShaderProgram VoxelVS { get; private set; } = new ShaderProgram();
        public ShaderProgram VoxelFS { get; private set; } = new ShaderProgram();

        public void Load() {
            VoxelVS.Create(ShaderType.VertexShader, "Assets/Voxel.vert");
            VoxelFS.Create(ShaderType.FragmentShader, "Assets/Voxel.frag");
        }

        public void Dispose() {
            VoxelVS?.Dispose();
            VoxelFS?.Dispose();
        }
    }
}