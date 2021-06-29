#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D diffuseMap;



void main(void)
{
  vec4 TexColor = texture(diffuseMap, TexCoords);
 
  FragColor = TexColor;

 
}