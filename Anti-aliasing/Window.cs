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

namespace Anti_aliasing
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

        private int textureColorBuffer;

        private int textureColorBufferMultiSampled;

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

        private Shader objectShader;

        private int drawFBO;
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
        public Window(int width, int height, string title) : base(width, height, new GraphicsMode(new ColorFormat(16, 16, 16, 16), 16, 8, 4), title) { }

        protected override void OnLoad(EventArgs e)
        {
            
            GL.ClearColor(0.2f,0.5f, 0.3f, 1.0f);

            objectShader = new Shader("../../Shaders/antiAliasingShader.vert", "../../Shaders/antiAliasingShader.frag");
            objectShader.Use();

            GL.GenBuffers(1, out objectVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, objectVertices.Length * sizeof(float), objectVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out objectVAO);
            GL.BindVertexArray(objectVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            var positionLocation = objectShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            _camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);


            ScreenShader = new Shader("../../Shaders/screenShader.vert", "../../Shaders/screenShader.frag");
            ScreenShader.Use();
            //设置帧四边形
            GL.GenBuffers(1, out quadVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out quadVAO);
            GL.BindVertexArray(quadVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            positionLocation = ScreenShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            var textureLocation = ScreenShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();
            view = Matrix4.Transpose(view);
            projection = Matrix4.Transpose(projection);
            //设置UBO
            int uniformBlockIndexObject = GL.GetUniformBlockIndex(objectShader.Handle, "Matrices");
            GL.UniformBlockBinding(objectShader.Handle, uniformBlockIndexObject, 0);

            GL.GenBuffers(1, out uboMatrices);
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferData(BufferTarget.UniformBuffer, 128, IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 0, uboMatrices, IntPtr.Zero, 128);

            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)64, 64, ref projection);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            //离屏MSAA
            GL.GenFramebuffers(1, out FBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            GL.GenTextures(1, out textureColorBufferMultiSampled);
            GL.BindTexture(TextureTarget.Texture2DMultisample, textureColorBufferMultiSampled);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 800, 600, 0, PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);
            GL.TexImage2DMultisample(TextureTargetMultisample.Texture2DMultisample, 4, PixelInternalFormat.Rgb, 800, 600, true);
            //GL.TextureParameter(TextureColorBuffer, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //GL.TextureParameter(TextureColorBuffer, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.BindTexture(TextureTarget.Texture2DMultisample, 0);

            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureColorBuffer, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,  TextureTarget.Texture2DMultisample, textureColorBufferMultiSampled, 0);

            GL.GenRenderbuffers(1, out RBO);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);

            //GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, 800, 600);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.Depth24Stencil8, 800, 600);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception($"帧缓冲不完整，请检查帧缓冲！{FBO}");
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.GenFramebuffers(1, out drawFBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, drawFBO);

            GL.GenTextures(1, out textureColorBuffer);
            GL.BindTexture(TextureTarget.Texture2D, textureColorBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 800, 600, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TextureParameter(textureColorBuffer, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(textureColorBuffer, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureColorBuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception($"帧缓冲不完整，请检查帧缓冲！{FBO}");
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);



            CursorVisible = false;
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            Matrix4 view = _camera.GetViewMatrix();
            view = Matrix4.Transpose(view);
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Enable(EnableCap.Multisample);

           

            objectShader.Use();      
            GL.BindVertexArray(objectVAO);
            Matrix4 model = Matrix4.CreateRotationZ(0.0f);

            objectShader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            GL.BindVertexArray(0);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, FBO);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, drawFBO);

            GL.BlitFramebuffer(0, 0, 800, 600, 0, 0, 800, 600, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(1.0f, 0.0f, 0.0f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.DepthTest);



            ScreenShader.Use();
            GL.BindVertexArray(quadVAO);
            ScreenShader.SetInt("screenTexture", 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureColorBuffer);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.BindVertexArray(0);
      

          

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
