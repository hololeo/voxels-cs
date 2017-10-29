using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace Voxels {
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexAttribAttribute : Attribute {
        public int Index { get; private set; }
        public int Count { get; private set; }
        public int ComponentSize { get; private set; }
        public VertexAttribPointerType ComponentType { get; private set; }
        public int Offset { get; set; } = 0;

        public VertexAttribAttribute(int index, int count, Type type) {
            Index = index;
            Count = count;
            ComponentSize = Marshal.SizeOf(type);
            ComponentType = _lookup[type];
        }

        private static readonly IDictionary<Type, VertexAttribPointerType> _lookup
            = new Dictionary<Type, VertexAttribPointerType> {
            [typeof(float)] = VertexAttribPointerType.Float,
            [typeof(uint)] = VertexAttribPointerType.UnsignedInt
        };
    }
}