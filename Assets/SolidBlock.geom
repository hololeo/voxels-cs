#version 460 core

layout (points) in;
layout (triangle_strip, max_vertices = 24) out;

uniform mat4 u_viewProj;
uniform vec3 u_primaryColor;
uniform vec3 u_secondaryColor;

in gl_PerVertex {
    vec4 gl_Position;
} gl_in[];

out gl_PerVertex {
    vec4 gl_Position;
};

out vec3 gtf_color;

void main() {
    float size = 4.99f;

    // Left.
    gtf_color = u_primaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-size, -size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, -size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Right.
    gtf_color = u_secondaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-size, -size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, -size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, size, size, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Front.
    gtf_color = u_primaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(size, -size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, -size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, size, size, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Back.
    gtf_color = u_secondaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-size, -size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, -size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, size, size, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Top.
    gtf_color = u_primaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-size, size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, size, size, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();

    // Bottom.
    gtf_color = u_secondaryColor;
    gl_Position = (gl_in[0].gl_Position + vec4(-size, -size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, -size, -size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(-size, -size, size, 0.0)) * u_viewProj;
    EmitVertex();
    gl_Position = (gl_in[0].gl_Position + vec4(size, -size, size, 0.0)) * u_viewProj;
    EmitVertex();
    EndPrimitive();
}
