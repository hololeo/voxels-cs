using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class ShaderProgram : IDisposable {
        public int ProgramID => _prog;

        private int _prog;

        public ShaderProgram Create(ShaderType type, string path) {
            _prog = GL.CreateShaderProgram(type, 1, new[] {File.ReadAllText(path)});
            GL.GetProgram(_prog, GetProgramParameterName.InfoLogLength, out var length);
            if (length == 0) return this;
            throw new Exception($"Could not make shader program '{path}':\n{GL.GetShaderInfoLog(_prog)}");
        }

        public void Dispose() {
            if (GL.IsProgram(_prog)) GL.DeleteProgram(_prog);
        }
    }
}