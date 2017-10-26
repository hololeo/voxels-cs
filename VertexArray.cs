using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class VertexArray : IDisposable {
        public int Vao => _vao;

        private int _vao;
        private IEnumerable<ArrayBuffer> _buffers;

        public VertexArray Create(Func<IEnumerable<ArrayBuffer>> bufferCreationCallback) {
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            _buffers = bufferCreationCallback();
            GL.BindVertexArray(0);
            return this;
        }

        public void Dispose() {
            foreach (var buffer in _buffers) buffer?.Dispose();
            if (GL.IsVertexArray(_vao)) GL.DeleteVertexArray(_vao);
        }
    }
}