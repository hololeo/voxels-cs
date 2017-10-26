using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class ShaderProgram : IDisposable {
        public int ProgramID => _prog;

        private int _prog;

        public ShaderProgram Create(ShaderType type, string path) {
            _prog = GL.CreateShaderProgram(type, 1, new[] {File.ReadAllText(path)});
            var infoLog = GL.GetProgramInfoLog(_prog);
            if (string.IsNullOrWhiteSpace(infoLog)) return this;
            throw new Exception($"Could not make shader program '{path}':\n{infoLog}");
        }

        public void Dispose() {
            if (GL.IsProgram(_prog)) GL.DeleteProgram(_prog);
        }
    }
}