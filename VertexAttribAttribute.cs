using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenGL;

namespace Voxels {
    [AttributeUsage(AttributeTargets.Field)]
    public class VertexAttribAttribute : Attribute {
        public uint Index { get; set; }
        public int Count { get; set; }
        public uint ComponentSize { get; set; }
        public VertexAttribType ComponentType { get; set;}
        public int Offset { get; set; } = 0;

        public VertexAttribAttribute(uint index, int count, Type type) {
            Index = index;
            Count = count;

            ComponentSize = (uint) Marshal.SizeOf(type);
            ComponentType = Lookup[type];
        }

        private static readonly IDictionary<Type, VertexAttribType> Lookup = new Dictionary<Type, VertexAttribType> {
            [typeof(float)] = VertexAttribType.Float
        };
    }
}