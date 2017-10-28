#version 460

out vec4 out_color;

in vec3 gtf_color;

void main() {
    out_color = vec4(gtf_color, 1.0);
}