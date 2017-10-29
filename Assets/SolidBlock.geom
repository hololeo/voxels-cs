#version 460 core

#define SIZE 0.499999

layout (points) in;
layout (triangle_strip, max_vertices = 24) out;

uniform mat4 u_viewProj;
uniform vec3 u_primaryColor;
uniform vec3 u_secondaryColor;
uniform uint u_blockID;

in gl_PerVertex {
    vec4 gl_Position;
} gl_in[];

out gl_PerVertex {
    vec4 gl_Position;
};

in uint vtg_btype[];

out vec3 gtf_color;

void main() {
    if (vtg_btype[0] != u_blockID) return;
    
    // Left.
    gtf_color = u_primaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, -SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, -SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Right.
    gtf_color = u_secondaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, -SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, -SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Front.
    gtf_color = u_primaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, -SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, -SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Back.
    gtf_color = u_secondaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, -SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, -SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Top.
    gtf_color = u_primaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Bottom.
    gtf_color = u_secondaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, -SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, -SIZE, -SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-SIZE, -SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(SIZE, -SIZE, SIZE, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();
}
