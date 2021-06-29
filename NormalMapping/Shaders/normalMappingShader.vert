#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexture;

uniform mat4 model;

layout(std140) uniform Matrices
{
	mat4 view;
	mat4 projection;
};

out VS_OUT
{
	vec3 FragPos;
	vec2 TexCoord;
}vs_out;

void main(void)
{
	gl_Position = vec4(aPos.xyz, 1.0) * model * view * projection;
	
	//输出代码块
	vs_out.TexCoord = aTexture;
	vs_out.FragPos = vec3(vec4(aPos, 1.0) * model);
}
