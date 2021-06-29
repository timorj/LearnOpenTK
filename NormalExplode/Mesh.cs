using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace NormalExplode
{
    class Mesh
    {
        /// <summary>
        /// 结构体：顶点
        /// 储存顶点的位置坐标、法线坐标和纹理坐标
        /// </summary>
        public struct Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoords;
        }

        /// <summary>
        /// 结构体：纹理
        /// 储存纹理的结构体：纹理id、纹理类型、纹理路径
        /// </summary>
        public struct Texture
        {
            public int id;
            public string type;
            public string path;
        }

        public Vertex[] vertices;
        public int[] indices;
        public List<Texture> textures;
        //public float[] position;
        //public float[] normal;
        //public float[] texCoords;

        public Mesh(Vertex[] vertices, int[] indices, List<Texture> textures)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.textures = textures;
            /*
            position = new float[vertices.Length * 3];
            normal = new float[vertices.Length * 3];
            texCoords = new float[vertices.Length * 2];
            for(int i = 0; i< vertices.Length; i++)
            {
                position[3 * i] = vertices[i].Position.X;
                position[3 * i + 1] = vertices[i].Position.Y;
                position[3 * i + 2] = vertices[i].Position.Z;
                normal[3 * i] = vertices[i].Normal.X;
                normal[3 * i + 1] = vertices[i].Normal.Y;
                normal[3 * i + 2] = vertices[i].Normal.Z;
                texCoords[2 * i] = vertices[i].TexCoords.X;
                texCoords[2 * i + 1] = vertices[i].TexCoords.Y;
            }
            */
            setupMesh();
        }

        private int VAO, VBO, EBO;
        //private int positionVBO, normalVBO, texCoordsVBO;

        private void setupMesh()
        {
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);
            /*
            GL.GenBuffers(1, out positionVBO);
            GL.GenBuffers(1, out normalVBO);
            GL.GenBuffers(1, out texCoordsVBO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, position.Length * sizeof(float), position, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, normal.Length * sizeof(float), normal, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordsVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);
            */

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * (2 * Vector3.SizeInBytes + Vector2.SizeInBytes), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);
            /*
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVBO);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float,false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVBO);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordsVBO);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            */
            //顶点
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 2 * Vector3.SizeInBytes + Vector2.SizeInBytes, 0);
            //法线
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 2 * Vector3.SizeInBytes + Vector2.SizeInBytes, Vector3.SizeInBytes);
            //纹理坐标
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * Vector3.SizeInBytes + Vector2.SizeInBytes, 2 * Vector3.SizeInBytes);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BindVertexArray(0);

        }
        /// <summary>
        /// 绘制模型
        /// </summary>
        /// <param name="shader"></param>
        public void Draw(Shader shader)
        {
            int diffuseNr = 1;
            int specularNr = 1;
            for (int i = 0; i < textures.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);

                //如果有多种材质的话，可以从这里更改
                string number = "";
                string name = textures[i].type;
                if (name == "diffuse")
                {
                    number = (diffuseNr++).ToString();
                }
                else if (name == "specular")
                {
                    number = (specularNr++).ToString();
                }
                shader.SetFloat("material." + name, i);
                GL.BindTexture(TextureTarget.Texture2D, textures[i].id);
            }
            GL.ActiveTexture(0);

            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }



    }
}
