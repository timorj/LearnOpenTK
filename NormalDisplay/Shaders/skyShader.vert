#version 330 core
layout(location = 0) in vec3 aPos;

out vec3 TexCoords;

uniform mat4 view;
uniform mat4 projection;

void main(void)
{
	TexCoords = aPos;
	vec4 pos = vec4(aPos.xyz, 1.0) * view * projection;
	gl_Position = pos.xyww;
}
