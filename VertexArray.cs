using System;
using System.Collections.Generic;
using OpenGL;

namespace Voxels {
    public class VertexArray : IDisposable {
        public uint Vao => _vao;

        private uint _vao;
        private IEnumerable<ArrayBuffer> _buffers;

        public VertexArray Create(Func<IEnumerable<ArrayBuffer>> bufferCreationCallback) {
            _vao = Gl.GenVertexArray();
            Gl.BindVertexArray(_vao);
            _buffers = bufferCreationCallback();
            Gl.BindVertexArray(0);
            return this;
        }

        public void Dispose() {
            foreach (var buffer in _buffers) buffer?.Dispose();
            if (Gl.IsVertexArray(_vao)) Gl.DeleteVertexArrays(_vao);
        }
    }
}