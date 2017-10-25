using System;
using System.IO;
using OpenGL;
using OpenGL.Objects;

namespace Voxels {
    public class Resources : IDisposable {
        public readonly ShaderProgram VoxelProgram = new ShaderProgram("Voxel");

        public void Load() {
            CreateVoxelProgram();
        }

        private void CreateVoxelProgram() {
            var vs = new Shader(ShaderType.VertexShader);
            using (var stream = new FileStream("Assets/Voxel.vert", FileMode.Open))
                vs.LoadSource(stream);
            vs.Create(Program.Context);
            var fs = new Shader(ShaderType.FragmentShader);
            using (var stream = new FileStream("Assets/Voxel.frag", FileMode.Open))
                fs.LoadSource(stream);
            fs.Create(Program.Context);

            VoxelProgram.AttachShader(vs);
            VoxelProgram.AttachShader(fs);
            VoxelProgram.Create(Program.Context);
        }

        public void Dispose() {
            VoxelProgram?.Dispose();
        }
    }
}