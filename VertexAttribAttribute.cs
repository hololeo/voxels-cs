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
        public VertexAttribIntegerType IComponentType { get; private set; }
        public int Offset { get; set; }
        public bool IsInteger { get; private set; }

        public VertexAttribAttribute(int index, int count, Type type) {
            Index = index;
            Count = count;
            ComponentSize = Marshal.SizeOf(type);
            _lookup.TryGetValue(type, out var vtype);
            ComponentType = vtype;
            IsInteger = _ilookup.TryGetValue(type, out var itype);
            IComponentType = itype;
        }

        private static readonly IDictionary<Type, VertexAttribPointerType> _lookup
            = new Dictionary<Type, VertexAttribPointerType> {
            [typeof(float)] = VertexAttribPointerType.Float
        };

        private static readonly IDictionary<Type, VertexAttribIntegerType> _ilookup
            = new Dictionary<Type, VertexAttribIntegerType> {
                [typeof(int)] = VertexAttribIntegerType.Int,
                [typeof(uint)] = VertexAttribIntegerType.UnsignedInt
            };
    }
}