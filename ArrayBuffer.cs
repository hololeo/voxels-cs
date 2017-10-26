using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenGL;

namespace Voxels {
    public class ArrayBuffer<T> : IDisposable where T : struct {
        public uint Vbo => _vbo;

        private uint _vbo;

        public ArrayBuffer(IEnumerable<T> positions, int components, VertexAttribType type, BufferUsage usage) {
            var typeSize = Marshal.SizeOf<T>();
            var bufferSize = typeSize * components;
            _vbo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint) bufferSize, positions, usage);
            Gl.EnableVertexAttribArray(0);
            Gl.VertexAttribPointer(0, components, type, false, typeSize, IntPtr.Zero);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose() {
            if (Gl.IsBuffer(_vbo)) Gl.DeleteBuffers(_vbo);
        }
    }
}