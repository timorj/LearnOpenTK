#version 330 core
in vec2 TexCoords;
in vec3 Normal;
in vec3 FragPos;

uniform vec3 viewPos;

struct Material
{
	sampler2D diffuse;
	sampler2D specular;

	float shininess;
};
uniform Material material;

struct DirLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirlight;

out vec4 FragColor;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
void main(void)
{
	vec3 norm = normalize(Normal);
	
	vec3 viewDir = normalize(viewPos - FragPos);

	vec3 result = CalcDirLight(dirlight, norm, viewDir);

	FragColor = vec4(result, 1.0);
	
}
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	float diff = max(dot(normal, lightDir), 0.0);

	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(normal, reflectDir),0.0), material.shininess);

	vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
	vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
	
	return ambient + diffuse + specular;

};

