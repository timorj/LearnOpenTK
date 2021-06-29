#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexture;

layout(std140) uniform Matrices
{
	mat4 view;
	mat4 projection;
};
uniform mat4 model;
uniform mat4 lightSpaceMatrix;

out VS_OUT
{
	vec3 FragPos;
	vec2 TexCoord;
	vec3 Normal;
	vec4 FragPosLightSpace;
}vs_out;

void main(void)
{
	gl_Position = vec4(aPos, 1.0) * model * view * projection;

	vs_out.TexCoord = aTexture;
	vs_out.Normal = mat3(transpose(inverse(model))) * aNormal;
	vs_out.FragPos = vec3(vec4(aPos, 1.0) * model);
	vs_out.FragPosLightSpace = vec4(vs_out.FragPos, 1.0) * lightSpaceMatrix;
}