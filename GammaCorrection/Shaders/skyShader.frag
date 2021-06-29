﻿#version 330 core
in vec3 TexCoords;

out vec4 FragColor;

uniform samplerCube cubeTexture;

void main(void)
{
	FragColor = texture(cubeTexture, TexCoords);
}