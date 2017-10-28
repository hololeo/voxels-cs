using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class VertexArray : IDisposable {
        public int Vao => _vao;

        private int _vao;
        private IEnumerable<ArrayBuffer> _buffers;

        public static VertexArray Create(Func<IEnumerable<ArrayBuffer>> bufferCreationCallback) {
            var va = new VertexArray {_vao = GL.GenVertexArray()};
            GL.BindVertexArray(va._vao);
            va._buffers = bufferCreationCallback();
            GL.BindVertexArray(0);
            return va;
        }

        public void Dispose() {
            foreach (var buffer in _buffers) buffer?.Dispose();
            if (GL.IsVertexArray(_vao)) GL.DeleteVertexArray(_vao);
        }
    }
}