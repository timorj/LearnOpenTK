#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexture;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in mat4 instanceMatrix;

layout(std140) uniform Matrices
{
	mat4 view;
	mat4 projection;
};

out vec2 TexCoords;
out vec3 Normal;
out vec3 FragPos;

void main(void)
{
	gl_Position =  vec4(aPos.xyz,1.0) * instanceMatrix * view * projection;
	TexCoords = aTexture;
	Normal = aNormal * mat3(transpose(inverse(instanceMatrix)));
	FragPos = vec3(vec4(aPos,1.0) * instanceMatrix);
}
