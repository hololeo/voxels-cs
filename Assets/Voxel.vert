#version 460

layout (location = 0) in vec3 in_pos;
layout (location = 1) in uint in_btype;

out gl_PerVertex {
    vec4 gl_Position;
};

out uint vtg_btype;

void main() {
    gl_Position = vec4(in_pos, 1.0);
    vtg_btype = in_btype;
}
