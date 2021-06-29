#version 330 core
layout(triangles) in;
layout(line_strip, max_vertices = 2) out;

in VS_OUT
{
	vec3 Normal;
	vec2 TexCoord;
	vec3 FragPos;
}gs_in[];

out GS_OUT
{
	vec2 TexCoords;
	vec3 GsNormal;
	vec3 GsFragPos;
}gs_out;

void GenerateLine(int index);

const float Magnitude = 0.05;
void main()
{
	GenerateLine(0);
	GenerateLine(1);
	GenerateLine(2);
}

void GenerateLine(int index)
{
	gl_Position = gl_in[index].gl_Position;
	gs_out.TexCoords = gs_in[index].TexCoord;
	gs_out.GsNormal = gs_in[index].Normal;
	gs_out.GsFragPos = gs_in[index].FragPos;
	EmitVertex();
	gl_Position = gl_in[index].gl_Position + vec4(gs_in[index].Normal, 0.0) * Magnitude;
	EmitVertex();

	EndPrimitive();
}