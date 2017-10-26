#version 460 core

layout (points) in;
layout (triangles, max_vertices = 6) out;

void main() {
    gl_Position = gl_in[0].gl_Position + vec4(0.1, 0.1, 0.0, 0.0);
    EmitVertex();
    EndPrimitive();
}
