#version 460 core

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

uniform mat4 u_viewProj;

in gl_PerVertex {
    vec4 gl_Position;
} gl_in[];

out gl_PerVertex {
    vec4 gl_Position;
};

void main() {
    gl_Position = (gl_in[0].gl_Position + vec4(-0.1, -0.1, 0.0, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(0.1, -0.1, 0.0, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-0.1, 0.1, 0.0, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(0.1, 0.1, 0.0, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();
}
