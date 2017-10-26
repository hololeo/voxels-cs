using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class Resources : IDisposable {
        public int VoxelProgram => _voxelProgram;

        private int _voxelProgram;

        public void Load() {
            _voxelProgram = CreateProgram(
                CreateShader("Assets/Voxel.vert", ShaderType.VertexShader),
                CreateShader("Assets/Voxel.frag", ShaderType.FragmentShader)
            );
        }

        private static int CreateShader(string path, ShaderType type) {
            var id = GL.CreateShader(type);
            GL.ShaderSource(id, File.ReadAllText(path));
            GL.CompileShader(id);
            GL.GetShader(id, ShaderParameter.CompileStatus, out var status);
            if (status == 1) return id;
            var stringBuilder = new StringBuilder(1024);
            GL.GetShaderInfoLog(id, 1024, out var _, stringBuilder);
            throw new Exception($"Shader compile error for '{path}':\n{stringBuilder}");
        }

        private static int CreateProgram(params int[] shaders) {
            var pid = GL.CreateProgram();
            foreach (var sid in shaders) GL.AttachShader(pid, sid);
            GL.LinkProgram(pid);
            foreach (var sid in shaders) GL.DeleteShader(sid);
            GL.GetProgram(pid, GetProgramParameterName.LinkStatus, out var status);
            return status == 1 ? pid : throw new Exception("Program linking failed");
        }

        public void Dispose() {
            if (GL.IsProgram(_voxelProgram)) GL.DeleteProgram(_voxelProgram);
        }
    }
}