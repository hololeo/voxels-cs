using System;
using System.Collections.Generic;
using OpenGL;

namespace Voxels {
    public class ArrayBuffer<T> : IDisposable where T : struct {
        public uint Vbo => _vbo;

        private uint _vbo;

        public ArrayBuffer(IEnumerable<T> data, BufferUsage usage) {
            _vbo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            Gl.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6, data, usage);
            Gl.EnableVertexAttribArray(0);
            Gl.VertexAttribPointer(0, 2, VertexAttribType.Float, false, sizeof(float) * 2, IntPtr.Zero);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose() {
            if (Gl.IsBuffer(_vbo)) Gl.DeleteBuffers(_vbo);
        }
    }
}