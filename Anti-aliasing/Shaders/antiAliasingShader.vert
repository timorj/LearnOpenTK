#version 330 core
layout(location = 0) in vec3 aPos;

layout(std140) uniform Matrices
{
	mat4 view;
	mat4 projection;
};
uniform mat4 model;
void main(void)
{
	gl_Position = vec4(aPos, 1.0) * model * view * projection;

}