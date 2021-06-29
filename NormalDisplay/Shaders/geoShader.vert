#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 2) in vec3 aNormal;
layout(location = 1) in vec2 aTexCoord;

uniform mat4 model;
layout(std140) uniform Matrices
{	
	mat4 view;
	mat4 projection;
};
out VS_OUT
{	
	vec3 Normal;
	vec2 TexCoord;
	vec3 FragPos;
}vs_out;

void main(void)
{
	gl_Position = vec4(aPos, 1.0) * model * view * projection;
	vs_out.Normal = aNormal * mat3(transpose(inverse(model)));
	vs_out.TexCoord = aTexCoord;
	vs_out.FragPos = vec3(vec4(aPos, 1.0) * model);
}