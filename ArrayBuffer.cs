using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    public class ArrayBuffer : IDisposable {
        public int Vbo => _vbo;

        private int _vbo;

        public static ArrayBuffer CreateAsUnique<T>(T[] data, int components, VertexAttribPointerType type, BufferUsageHint usage)
            where T : struct {
            var typeSize = Marshal.SizeOf<T>();
            var bufferSize = typeSize * data.Length;
            var buffer = new ArrayBuffer {_vbo = GL.GenBuffer()};
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer._vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, data, usage);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, components, type, false, typeSize * components, IntPtr.Zero);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return buffer;
        }

        public static ArrayBuffer CreateAsVertices<T>(T[] vertices, BufferUsageHint usage)
            where T : struct {
            var typeSize = Marshal.SizeOf<T>();
            var bufferSize = typeSize * vertices.Length;
            var buffer = new ArrayBuffer {_vbo = GL.GenBuffer()};
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer._vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, vertices, usage);
            foreach (var field in typeof(T).GetFields()) {
                var attrib = field.GetCustomAttribute<VertexAttribAttribute>();
                if (attrib == null) continue;
                GL.EnableVertexAttribArray(attrib.Index);
                GL.VertexAttribPointer(attrib.Index, attrib.Count, attrib.ComponentType, false, typeSize,
                    new IntPtr(attrib.Offset));
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return buffer;
        }

        public void Dispose() {
            if (GL.IsBuffer(_vbo)) GL.DeleteBuffer(_vbo);
        }
    }
}