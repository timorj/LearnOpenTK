#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;

out vec3 Position;
out vec3 Normal;


layout(std140) uniform Matrices
{
    mat4 view;
    mat4 projection;
};

uniform mat4 model;


void main(void)
{
	Position = vec3(vec4(aPos, 1.0) * model);
	Normal = mat3(transpose(inverse(model))) * aNormal;

	gl_Position = vec4(aPos,1.0) * model * view * projection;

}