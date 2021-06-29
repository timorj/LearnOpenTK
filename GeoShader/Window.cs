using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using System.Runtime.InteropServices;
using GlmNet;

namespace GeoShader
{
    class Window : GameWindow
    {
        #region 变量
        #region 顶点
        private readonly float[] objectVertices =
        {
        //背面
         // positions          // texture Coords
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        //正面
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        //左面
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        //右面
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

         //底面
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        //顶面
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,         
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };
        #endregion
        private int objectVAO;
        private int objectVBO;

        private Shader objectshader;

        private Shader objectScaleUpShader;

        private Shader ScreenShader;

        private Texture diffuseMap;

        private float[] floorVertices =
        {
         // positions          // texture Coords 
         20.0f, -0.5f,  20.0f,  2.0f, 0.0f,
         -20.0f, -0.5f, -20.0f,  0.0f, 2.0f,
         -20.0f, -0.5f,  20.0f,  0.0f, 0.0f,

         20.0f, -0.5f,  20.0f,  2.0f, 0.0f,
         20.0f, -0.5f, -20.0f,  2.0f, 2.0f,                
         -20.0f, -0.5f, -20.0f,  0.0f, 2.0f,         
        };

        private Vector3[] objectPosition =
        {
        new Vector3(2.0f, 0.1f, 1.0f),
        new Vector3(1.0f, 0.1f, -3.5f),
        new Vector3(4.0f, 0.1f, -2.5f),
        new Vector3(0.0f, 0.1f, -3.0f)

        };

        private float[] grassVertices =
        {
              // positions          // texture Coords
        0.0f,  0.5f,  0.0f,  0.0f,  0.0f,
        0.0f, -0.5f,  0.0f,  0.0f,  1.0f,
        1.0f, -0.5f,  0.0f,  1.0f,  1.0f,

        0.0f,  0.5f,  0.0f,  0.0f,  0.0f,
        1.0f, -0.5f,  0.0f,  1.0f,  1.0f,
        1.0f,  0.5f,  0.0f,  1.0f,  0.0f
        };

        private Vector3[] grassPositions =
        {
            new Vector3(-1.5f,  0.2f, -0.48f),
            new Vector3(1.5f,  0.2f,  0.51f),
            new Vector3(0.0f,  0.2f,  0.7f),
            new Vector3(-0.3f,  0.2f, -2.3f),
            new Vector3(0.5f,  0.2f, -0.6f),
        };

        private float[] quadVertices = {
        // vertex attributes for a quad that fills the entire screen in Normalized Device Coordinates.
        // positions   // texCoords
        -1.0f,  1.0f,  0.0f, 1.0f,
        -1.0f, -1.0f,  0.0f, 0.0f,
         1.0f, -1.0f,  1.0f, 0.0f,

        -1.0f,  1.0f,  0.0f, 1.0f,
         1.0f, -1.0f,  1.0f, 0.0f,
         1.0f,  1.0f,  1.0f, 1.0f
          };

        private int quadVAO;
        private int quadVBO;

        private int grassVAO;
        private int grassVBO;

        private Texture grassMap;

        private int floorVAO;
        private int floorVBO;

        private Texture floorMap;

        private Camera _camera;

        //帧缓冲

        private int FBO;

        private int TextureColorBuffer;

        private int RBO;

        //天空盒
        private Shader skyShader;

        private int skyVAO;

        private int skyVBO;

        private string[] skyFaces =
        {
            "right.jpg",
            "left.jpg",
            "top.jpg",
            "bottom.jpg",
            "front.jpg",
            "back.jpg"
        };

        private float[] cubeVertices =
        {
                // positions  
            -20.0f, -20.0f,  20.0f,
            -20.0f,  20.0f,  20.0f,
             20.0f,  20.0f,  20.0f,
             20.0f,  20.0f,  20.0f,
             20.0f, -20.0f,  20.0f,
            -20.0f, -20.0f,  20.0f,

            -20.0f,  20.0f, -20.0f,
            -20.0f, -20.0f, -20.0f,
             20.0f, -20.0f, -20.0f,
             20.0f, -20.0f, -20.0f,
             20.0f,  20.0f, -20.0f,
            -20.0f,  20.0f, -20.0f,

            -20.0f, -20.0f,  20.0f,
            -20.0f, -20.0f, -20.0f,
            -20.0f,  20.0f, -20.0f,
            -20.0f,  20.0f, -20.0f,
            -20.0f,  20.0f,  20.0f,
            -20.0f, -20.0f,  20.0f,

             20.0f, -20.0f, -20.0f,
             20.0f, -20.0f,  20.0f,
             20.0f,  20.0f,  20.0f,
             20.0f,  20.0f,  20.0f,
             20.0f,  20.0f, -20.0f,
             20.0f, -20.0f, -20.0f,

           

            -20.0f,  20.0f, -20.0f,
             20.0f,  20.0f, -20.0f,
             20.0f,  20.0f,  20.0f,
             20.0f,  20.0f,  20.0f,
            -20.0f,  20.0f,  20.0f,
            -20.0f,  20.0f, -20.0f,

            -20.0f, -20.0f, -20.0f,
            -20.0f, -20.0f,  20.0f,
             20.0f, -20.0f, -20.0f,
             20.0f, -20.0f, -20.0f,
            -20.0f, -20.0f,  20.0f,
             20.0f, -20.0f,  20.0f
        };

        private int cubemapTexture;

        // 反射映射
        private Shader reflectShader;

        private int reflectVAO;

        private int reflectVBO;

        private float[] reflectBox =
        {
             -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };
        
        private bool _firstMove = true;

        private Vector2 _lastPos;

        private int uboMatrices;

        //学习几何着色器
        private float[] points =
        {
            -0.5f,  0.5f, 1.0f, 0.0f, 0.0f,
            -0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
             0.5f,  0.5f, 0.0f, 0.0f, 1.0f,
             0.5f, -0.5f, 0.2f, 0.3f, 0.4f
        };

        private int GeoVBO;

        private int GeoVAO;

        private Shader GeoTestShader;

        
        #endregion

        public struct UniformBlock
        {
            public Matrix4 view;
            public Matrix4 projection;
            public static readonly int size = BlittableValueType<UniformBlock>.Stride;

            public UniformBlock(Matrix4 view, Matrix4 projection)
            {
                this.view = view;
                this.projection = projection;
            }
        }
        //声明窗口主体，注意开启模板测试
        public Window(int width, int height, string title) : base(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 16, 8), title) { }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f,0.0f, 0.3f, 1.0f);

            GeoTestShader = new Shader("../../Shaders/geoShader.vert", "../../Shaders/geoShader.frag", "../../Shaders/geoShader.geo");
            GeoTestShader.Use();

            GL.GenBuffers(1, out GeoVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, GeoVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, points.Length * sizeof(float), points, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.GenVertexArrays(1, out GeoVAO);
            GL.BindVertexArray(GeoVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, GeoVBO);
            var positionLocation = GeoTestShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, GeoVBO);
            var colorLocation = GeoTestShader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                      
            _camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);

 
            CursorVisible = false;
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GeoTestShader.Use();
            
            GL.BindVertexArray(GeoVAO);

            GL.DrawArrays(PrimitiveType.Points, 0, 4);


            SwapBuffers();
            base.OnRenderFrame(e);
        }
        /// <summary>
        /// 加载立方体贴图
        /// </summary>
        /// <param name="skyBoxVertices">立方体贴图路径</param>
        /// <returns></returns>
        private int LoadCubeMap(string[] skyBoxVertices)
        {
            int textureID;
            //加载立方体贴图
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.TextureCubeMap, textureID);

            for(int i = 0; i< skyFaces.Length; i++)
            {

                var image = new Bitmap("../../Resource/skybox/" + skyFaces[i].ToString());            
                var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (data.Scan0 != null)
                {
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                }
                else
                {
                    throw new Exception("纹理加载失败!");
                }               
            }
            GL.TextureParameter(textureID, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(textureID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TextureParameter(textureID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TextureParameter(textureID, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TextureParameter(textureID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            return textureID;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            const float cameraSpeed = 5f;
            const float sensitivity = 0.2f;


            if (input.IsKeyDown(Key.Up))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Key.Down))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Key.Left))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Key.Right))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Key.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Key.LShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }
            if (input.IsKeyDown(Key.R))
            {
                _camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);
            }
            var mouse = Mouse.GetState();

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }

            base.OnUpdateFrame(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused)
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _camera.Fov -= e.DeltaPrecise;
            base.OnMouseWheel(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }


        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            //GL.DeleteBuffer(_lightVAO);
            //GL.DeleteBuffer(_elementBufferObject);
            //GL.DeleteVertexArray(_objectVAO);
            GL.DeleteBuffer(floorVBO);
            GL.DeleteVertexArray(floorVAO);
            GL.DeleteBuffer(objectVBO);
            GL.DeleteVertexArray(objectVAO);
            GL.DeleteBuffer(grassVBO);
            GL.DeleteVertexArray(grassVAO);
            GL.DeleteBuffer(quadVBO);
            GL.DeleteVertexArray(quadVAO);
            GL.DeleteBuffer(skyVBO);
            GL.DeleteVertexArray(skyVAO);

            base.OnUnload(e);
        }
    }
}
