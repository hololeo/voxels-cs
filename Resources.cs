using System;
using System.IO;
using System.Text;
using OpenGL;

namespace Voxels {
    public class Resources : IDisposable {
        public uint VoxelProgram => _voxelProgram;

        private uint _voxelProgram;

        public void Load() {
            _voxelProgram = CreateProgram(
                CreateShader("Assets/Voxel.vert", ShaderType.VertexShader),
                CreateShader("Assets/Voxel.frag", ShaderType.FragmentShader)
            );
        }

        private static uint CreateShader(string path, ShaderType type) {
            var id = Gl.CreateShader(type);
            Gl.ShaderSource(id, new[] { File.ReadAllText(path) });
            Gl.CompileShader(id);
            Gl.GetShader(id, ShaderParameterName.CompileStatus, out var status);
            if (status == 1) return id;
            var stringBuilder = new StringBuilder(1024);
            Gl.GetShaderInfoLog(id, 1024, out var _, stringBuilder);
            throw new Exception($"Shader compile error for '{path}':\n{stringBuilder}");
        }

        private static uint CreateProgram(params uint[] shaders) {
            var pid = Gl.CreateProgram();
            foreach (var sid in shaders) Gl.AttachShader(pid, sid);
            Gl.LinkProgram(pid);
            foreach (var sid in shaders) Gl.DeleteShader(sid);
            Gl.GetProgram(pid, ProgramProperty.LinkStatus, out var status);
            return status == 1 ? pid : throw new Exception("Program linking failed");
        }

        public void Dispose() {
            if (Gl.IsProgram(_voxelProgram)) Gl.DeleteProgram(_voxelProgram);
        }
    }
}