#version 330 core

in vec3 fsColor;

out vec4 FragColor;


void main(void)
{
	FragColor = vec4(fsColor.xyz, 1.0);
}
