#version 330 core
out vec4 FragColor;

in VS_OUT
{
	vec3 FragPos;
	vec2 TexCoord;
	vec3 Normal;
}fs_in;

uniform vec3 viewPos;

struct Material
{
	sampler2D diffuse;
	sampler2D specular;

	float shininess;
};
uniform Material material;
//shadow map
uniform samplerCube shadowMap;
uniform float far_plane;

struct DirLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirlight;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);

struct PointLight
{
	vec3 position;
	
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float linear;
	float quadratic;
	float constant;
};
uniform PointLight pointlight;

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

uniform int blinn;

float ShadowCalculation(vec3 FragPos);
void main(void)
{
	vec3 normal = normalize(fs_in.Normal);

	vec3 viewDir = normalize(viewPos - normal);

	vec3 result = CalcDirLight(dirlight, normal, viewDir);

	result += CalcPointLight(pointlight, normal, fs_in.FragPos, viewDir);

	FragColor = vec4(result, 1.0);
	
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	float diff = max(dot(normal, lightDir), 0.0);
		
	vec3 reflectDir = reflect(-lightDir, normal);

	float spec = pow(max(dot(normal, reflectDir), 0.0), material.shininess);

	vec3 ambient = light.ambient * vec3(texture(material.diffuse, fs_in.TexCoord));
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, fs_in.TexCoord));
	vec3 specular = light.specular * spec * vec3(texture(material.specular, fs_in.TexCoord));

	return ambient + diffuse + specular;		
};

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fs_in.FragPos);

	float diff = max(dot(normal, lightDir), 0.0);

	vec3 reflectDir = reflect(-lightDir, normal);

	viewDir = normalize(viewDir);
	float spec = 0.0;
	if(blinn == 1)
	{
	//blinn phong
	vec3 halfwayDir = normalize(lightDir + viewDir); 

	 spec = pow(max(dot(normal, halfwayDir) ,0.0) ,material.shininess);
	}
	else
	{
	//phong
	 spec = pow(max(dot(viewDir, reflectDir) ,0.0) ,material.shininess);
	}
	float distance = length(light.position - fragPos);
	float attenuation = 1.0/ (light.constant + light.linear * distance + light.quadratic * pow(distance, 2.0));

	vec3 ambient = light.ambient * vec3(texture(material.diffuse, fs_in.TexCoord));
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, fs_in.TexCoord));
	vec3 specular = light.specular * spec * vec3(texture(material.specular, fs_in.TexCoord));

	ambient *= attenuation;
	diffuse *= attenuation; 
	specular *= attenuation;

	//Calculate shadow
	float shadow = ShadowCalculation(fs_in.FragPos);
	vec3 lighting = (ambient + (1.0 - shadow) * diffuse + specular);
	
	return lighting;

}
float ShadowCalculation(vec3 FragPos)
{
	vec3 fragToLight = FragPos - pointlight.position;
	float closetDepth = texture(shadowMap, fragToLight).r;

	closetDepth *= far_plane;

	float currentDepth = length(fragToLight);

	float bias = 0.05;

	/*
	without PCF
	float shadow = (currentDepth - bias) > closetDepth ? 1.0 : 0.0; 
	*/
	/* 
	
	//PCF(more soft)
	float shadow = 0.0;

	float samples = 4.0;
	
	float offset = 0.1;

	for(float x = -offset; x < offset; x += offset / (samples * 0.5))
	{
		for(float y = -offset; y < offset; y += offset / (samples * 0.5))
		{
			for(float z = -offset; z < offset; z += offset / (samples * 0.5))
			{
				float closetDepth = texture(shadowMap, fragToLight + vec3(x, y, z)).r;

				closetDepth *= far_plane;

				if(currentDepth - bias > closetDepth)
				{
					shadow += 1.0;
				}
			}
		}
	}
	shadow /= (samples * samples * samples);
	*/
	//another way about PCF(less power cost)
	vec3 sampleOffsetDirections[20] = vec3[]
	(
	   vec3( 1,  1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1,  1,  1), 
	   vec3( 1,  1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1,  1, -1),
	   vec3( 1,  1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1,  1,  0),
	   vec3( 1,  0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1,  0, -1),
	   vec3( 0,  1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0,  1, -1)
	);
	float shadow = 0.0;
	int samples = 20;
	float viewDistance = length(viewPos - FragPos);
	float diskRadius = 0.05;
	for(int i = 0; i < samples; ++i)
	{
		float closestDepth = texture(shadowMap, fragToLight + sampleOffsetDirections[i] * diskRadius).r;
		closestDepth *= far_plane;   // Undo mapping [0;1]
		if(currentDepth - bias > closestDepth)
			shadow += 1.0;
	}
	shadow /= float(samples);
	return shadow;
};
