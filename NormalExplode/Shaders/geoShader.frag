#version 330 core
struct Material
{
	sampler2D diffuse;
	sampler2D specular;
	float shininess;
};
uniform Material material;

in GS_OUT
{
	vec2 TexCoords;
	vec3 GsNormal;
	vec3 GsFragPos;
}fs_in;

struct DirLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirlight;

uniform vec3 viewPos;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);


out vec4 FragColor;


void main(void)
{
	vec3 norm = fs_in.GsNormal;

	vec3 viewDir = normalize(viewPos - fs_in.GsFragPos);

	vec3 result = CalcDirLight(dirlight, norm, viewDir);

	FragColor = vec4(result.xyz, 1.0);
	
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	float diff = max(dot(normal, lightDir), 0.0);

	vec3 reflectDir = reflect(-lightDir, normal);

	float spec = pow(max(dot(normal, reflectDir),0.0), material.shininess);

	vec3 ambient = light.ambient * vec3(texture(material.diffuse, fs_in.TexCoords));

	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, fs_in.TexCoords));

	vec3 specular = light.specular * spec * vec3(texture(material.specular, fs_in.TexCoords));
	return ambient + diffuse + specular;
};

