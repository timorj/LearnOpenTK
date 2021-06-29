#version 330 core
out vec4 FragColor;

in vec3 Position;
in vec3 Normal;

uniform vec3 cameraPos;
uniform samplerCube  skyBox;

void main(void)
{
    /*
	//reflect effcation
	vec3 I = normalize(Position - cameraPos);
	vec3 R = reflect(I, normalize(Normal));
	*/
	//refract effcation
	float ratio = 1.0 / 1.52;
	vec3 I = normalize(Position - cameraPos);
	vec3 R = refract(I, normalize(Normal), ratio);
	FragColor = vec4(texture(skyBox, R).rgb, 1.0);
}