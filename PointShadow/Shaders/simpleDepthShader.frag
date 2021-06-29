#version 330 core
in vec4 FragPos;

uniform vec3 lightPos;
uniform float far_plane;

void main(void)
{
	
	float lightDistance = length(FragPos.xyz - lightPos);

	lightDistance = lightDistance / far_plane;

	gl_FragDepth = lightDistance;
}