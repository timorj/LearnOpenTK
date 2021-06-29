#version 330 core
in vec2 TexCoords;

uniform sampler2D screenTexture;
out vec4 FragColor;

float offset = 1.0 / 300.0;

void main(void)
{
	
	vec4 aColor = texture(screenTexture, TexCoords);
	/*
	use those method could realize some effcations. such as Inverted, average, bulrry, sharpen
	
	//inverted
	//FragClor = vec4(1- aColor.rgb, 1.0);

	//average(Used to weighted averages.)
	//float average = 0.2126 * aColor.r + 0.7152 * aColor.g + 0.0722 * aColor.b;
	//FragColor = vec4(average, average, average, 1.0);
	 */
	//kernel effcations
	//1.Sharpen kernel

	vec2 offsets[9] = vec2[](
	vec2(-offset, offset),
	vec2(0, offset),
	vec2(offset, offset),
	vec2(-offset, 0),
	vec2(0, 0),
	vec2(offset, 0),
	vec2(-offset, -offset),
	vec2(0, -offset),
	vec2(offset, -offset)
	);
	/*
	//sharpen kernel
	float kernel[9] = float[](
	-1, -1, -1,
	-1, 9, -1,
	-1, -1, -1
	);
	*/
	
	//blurry kernel
	float kernel[9] = float[](
	1.0 / 16, 2.0 / 16, 1.0 / 16,
	2.0 / 16, 4.0 / 16, 2.0 / 16,
	1.0 / 16, 2.0 / 16, 2.0 / 16
	);
	
	/*
	//edge-decetion kernel
	float kernel[9] = float[](
	1, 1, 1,
	1, -8, 1,
	1, 1, 1
	);
	*/

	vec3 sampleTex[9];

	for(int i = 0; i < 9; i++)
	{
		sampleTex[i] = vec3(texture(screenTexture, TexCoords.st + offsets[i]));
	}
	vec3 col = vec3(0.0);
	for(int i = 0; i < 9; i++)
	{
		col += sampleTex[i] * kernel[i];
	}

	FragColor = vec4(aColor);


	
}