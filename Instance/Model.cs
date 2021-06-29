using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Assimp;
using System.Linq;
using static Instance.Mesh;
using System.Drawing;
using System.IO;

namespace Instance
{
    class Model
    {
        public Model(string path)
        {
            loadModel(path);
        }
        public void Draw(Shader shader)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].Draw(shader);
            }
        }
        private List<Mesh.Texture> textures_loaded = new List<Mesh.Texture>();//纹理动态数组：用以储存加载后的纹理
        private List<Mesh> meshes = new List<Mesh>();
        string directory;
        #region loadModel
        /// <summary>
        /// 加载Assimp模型
        /// </summary>
        /// <param name="path"></param>
        private void loadModel(string path)
        {
            var importer = new AssimpContext();
            Scene scene = importer.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs);
            if (scene == null || scene.RootNode == null)
            {
                throw new Exception("Assimp加载错误!");
            }
            directory = path.Substring(0, path.LastIndexOf('/'));

            processNode(scene.RootNode, scene);
        }
        #endregion
        #region processNode
        /// <summary>
        /// 递归处理节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="scene"></param>
        private void processNode(Node node, Scene scene)
        {
            //处理节点所有网格
            for (int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh mesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(processMesh(mesh, scene));
            }
            //对子节点重复此过程
            for (int i = 0; i < node.ChildCount; i++)
            {
                processNode(node.Children[i], scene);
            }
        }
        #endregion
        #region processMesh
        /// <summary>
        /// 将Assimp.Mesh类转换为定义的Mesh类
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        private Mesh processMesh(Assimp.Mesh mesh, Scene scene)
        {
            Vertex[] vertices = new Vertex[mesh.Vertices.Count];
            int[] indices = new int[mesh.FaceCount * 3];
            List<Instance.Mesh.Texture> textures = new List<Instance.Mesh.Texture>();

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vertex vertex;
                //顶点
                Vector3 tempPosition;
                tempPosition.X = mesh.Vertices[i].X;
                tempPosition.Y = mesh.Vertices[i].Y;
                tempPosition.Z = mesh.Vertices[i].Z;
                vertex.Position = tempPosition;

                // 法线
                Vector3 tempNormal;
                tempNormal.X = mesh.Normals[i].X;
                tempNormal.Y = mesh.Normals[i].Y;
                tempNormal.Z = mesh.Normals[i].Z;
                vertex.Normal = tempNormal;

                //纹理坐标
                if (mesh.TextureCoordinateChannels[0].Count > 1)
                {
                    Vector2 tempTextureCoord;
                    tempTextureCoord.X = mesh.TextureCoordinateChannels[0][i].X;
                    tempTextureCoord.Y = mesh.TextureCoordinateChannels[0][i].Y;
                    vertex.TexCoords = tempTextureCoord;
                }
                else vertex.TexCoords = new Vector2(0.0f, 0.0f);

                vertices[i] = vertex;

            }

            //处理索引
            for (int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];
                for (int j = 0; j < face.IndexCount; j++)
                {
                    indices[3 * i + j] = face.Indices[j];
                }
            }
            //处理网格的材质
            if (mesh.MaterialIndex > 0)
            {
                Material material = scene.Materials[mesh.MaterialIndex];
                List<Instance.Mesh.Texture> diffuseMaps = loadMatericalTextures(material, TextureType.Diffuse, "diffuse");
                for (int i = 0; i < diffuseMaps.Count; i++)
                {
                    textures.Insert(textures.Count, diffuseMaps[i]);
                }
                List<Instance.Mesh.Texture> specularMaps = loadMatericalTextures(material, TextureType.Specular, "specular");
                for (int i = 0; i < specularMaps.Count; i++)
                {
                    textures.Insert(textures.Count, specularMaps[i]);
                }

            }
            Mesh tempMesh = new Mesh(vertices, indices, textures);
            return tempMesh;

        }
        #endregion
        #region loadMateicalTextures
        /// <summary>
        /// 加载材质纹理
        /// 如果材质纹理已被加载，则会跳过加载
        /// </summary>
        /// <param name="material"></param>
        /// <param name="type"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        private List<Instance.Mesh.Texture> loadMatericalTextures(Material material, TextureType type, string typename)
        {
            List<Mesh.Texture> textures = new List<Mesh.Texture>();
            for (int i = 0; i < material.GetMaterialTextureCount(type); i++)
            {
                bool isSkip = false; //判断是否跳过加载
                TextureSlot foundTexture;
                material.GetMaterialTexture(type, i, out foundTexture);
                for (int j = 0; j < textures_loaded.Count; i++)
                {

                    if (textures_loaded[j].path.Equals(foundTexture.FilePath))
                    {
                        textures.Add(textures_loaded[j]);
                        isSkip = true;
                        break;
                    }
                }
                if (!isSkip)
                {
                    Mesh.Texture texture;
                    texture.id = TextureFromFile(foundTexture.FilePath, directory);
                    texture.type = typename;
                    texture.path = foundTexture.FilePath;
                    textures.Add(texture);
                }
            }
            return textures;

        }
        #endregion
        #region TextureFromFile
        /// <summary>
        /// 从文件中加载纹理
        /// </summary>
        /// <param name="path"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private int TextureFromFile(string path, string directory)
        {
            try
            {
                string filename = path;
                filename = directory + '/' + filename;

                int textureID;
                GL.GenTextures(1, out textureID);

                var bitmap = new Bitmap(filename);

                var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                if (data != null)
                {
                    GL.BindTexture(TextureTarget.Texture2D, textureID);
                    GL.TexImage2D(TextureTarget.Texture2D, 0,
                        PixelInternalFormat.Rgba,
                        bitmap.Width,
                        bitmap.Height,
                        0,
                        PixelFormat.Bgra,
                        PixelType.UnsignedByte,
                        data.Scan0);

                    bitmap.UnlockBits(data);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


                }
                return textureID;
            }
            catch (FileNotFoundException)
            {
                return 0;
            }


        }
        #endregion
    }
}
