using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenGL;

namespace Voxels {
    public class ArrayBuffer : IDisposable {
        public uint Vbo => _vbo;

        private uint _vbo;

        public ArrayBuffer CreateAsUnique<T>(IEnumerable<T> data, int components, VertexAttribType type, BufferUsage usage)
            where T : struct {
            var typeSize = Marshal.SizeOf<T>();
            var bufferSize = typeSize * data.Count();
            _vbo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint) bufferSize, data, usage);
            Gl.EnableVertexAttribArray(0);
            Gl.VertexAttribPointer(0, components, type, false, typeSize * components, IntPtr.Zero);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return this;
        }

        public ArrayBuffer CreateAsVertices<T>(IEnumerable<T> vertices, BufferUsage usage)
            where T : struct {
            var typeSize = Marshal.SizeOf<T>();
            var bufferSize = typeSize * vertices.Count();
            _vbo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint) bufferSize, vertices, usage);
            foreach (var field in typeof(T).GetFields()) {
                var attrib = field.GetCustomAttribute<VertexAttribAttribute>();
                if (attrib == null) continue;
                Gl.EnableVertexAttribArray(attrib.Index);
                Gl.VertexAttribPointer(attrib.Index, attrib.Count, attrib.ComponentType, false,
                    (int) (attrib.ComponentSize * attrib.Count), new IntPtr(attrib.Offset));
            }
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return this;
        }

        public void Dispose() {
            if (Gl.IsBuffer(_vbo)) Gl.DeleteBuffers(_vbo);
        }
    }
}