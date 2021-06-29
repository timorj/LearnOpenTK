#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 2) in vec2 aTexture;

uniform mat4 model;

layout(std140) uniform Matrices
{
    mat4 view;
    mat4 projection;
};

out vec2 TexCoords;

void main(void)
{
 gl_Position = vec4(aPos.xyz,1.0)* model * view * projection;
 TexCoords = aTexture;

}
