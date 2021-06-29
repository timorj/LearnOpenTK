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

namespace NormalDisplay
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

        private Model robot;

        private int matrices;

        private float explodeTime = 0;

        private Shader normalDisplayShader;
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
            _camera = new Camera(new Vector3(Vector3.Zero.X + 2.0f, 10.0f, Vector3.UnitZ.Z * 12) , Width / (float)Height);

            GeoTestShader = new Shader("../../Shaders/geoShader.vert", "../../Shaders/geoShader.frag");
            GeoTestShader.Use();

            robot = new Model("../../Data/Robot/nanosuit.obj");

            //法向量可视化着色器
            normalDisplayShader = new Shader("../../Shaders/normalDisplayShader.vert", "../../Shaders/normalDisplayShader.frag", "../../Shaders/normalDisplayShader.geo");
            normalDisplayShader.Use();
            
            //设置帧四边形
            ScreenShader = new Shader("../../Shaders/screenShader.vert", "../../Shaders/screenShader.frag");
            ScreenShader.Use();
            
            GL.GenBuffers(1, out quadVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * quadVertices.Length, quadVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out quadVAO);
            GL.BindVertexArray(quadVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            var positionLocation = ScreenShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            var textureLocation = ScreenShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //天空盒
            skyShader = new Shader("../../Shaders/skyShader.vert", "../../Shaders/skyShader.frag");
            skyShader.Use();

            GL.GenBuffers(1, out skyVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, skyVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, cubeVertices.Length * sizeof(float), cubeVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out skyVAO);
            GL.BindVertexArray(skyVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, skyVBO);
            positionLocation = skyShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            cubemapTexture = LoadCubeMap(skyFaces);
            
            //创建帧缓冲
            GL.GenFramebuffers(1, out FBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            //创建纹理缓冲
            GL.GenTextures(1, out TextureColorBuffer);
            GL.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 800, 600, 0, PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            //将纹理缓冲附加至当前绑定的帧缓冲对象
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureColorBuffer, 0);


            //当不需要对深度缓冲和模板缓冲采样时，创建渲染缓冲对象
            GL.GenRenderbuffers(1, out RBO);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);

            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, 800, 600);
           

            //将渲染缓冲对象附加到帧缓冲上面
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete) 
            {
                throw new Exception($"帧缓冲不完整，请检查帧缓冲！{FBO}");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            
            // 设置uniform缓冲
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();
            view = Matrix4.Transpose(view);
            projection = Matrix4.Transpose(projection);

            int uniformIndexObject = GL.GetUniformBlockIndex(GeoTestShader.Handle, "Matrices");
            int uniformIndexNormal = GL.GetUniformBlockIndex(normalDisplayShader.Handle, "Matrices");

            GL.UniformBlockBinding(GeoTestShader.Handle, uniformIndexObject, 0);
            GL.UniformBlockBinding(normalDisplayShader.Handle, uniformIndexNormal, 0);

            GL.GenBuffers(1, out matrices);

            GL.BindBuffer(BufferTarget.UniformBuffer, matrices);
            GL.BufferData(BufferTarget.UniformBuffer, 128, IntPtr.Zero, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 0, matrices, IntPtr.Zero, 128);

            GL.BindBuffer(BufferTarget.UniformBuffer, matrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);

            GL.BindBuffer(BufferTarget.UniformBuffer, matrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)64, 64, ref projection);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

           


            CursorVisible = false;
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //更新view矩阵
            Matrix4 view = _camera.GetViewMatrix();
            view = Matrix4.Transpose(view);
            GL.BindBuffer(BufferTarget.UniformBuffer, matrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            GL.Enable(EnableCap.Texture2D);
            GL.ClearColor(0.1f, 0.2f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit|ClearBufferMask.StencilBufferBit);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

          

            GeoTestShader.Use();
            Matrix4 model = Matrix4.CreateRotationX(0.0f);
            GeoTestShader.SetMatrix4("model", model);

            GeoTestShader.SetVector3("viewPos", _camera.Position);

            //explodeTime += (float)e.Time;
            
            //GeoTestShader.SetFloat("time", explodeTime);
            //物体材质
            GeoTestShader.SetInt("material.diffuse", 0);
            GeoTestShader.SetInt("material.specular", 1);
            GeoTestShader.SetFloat("material.shininess", 32.0f);

            //光源
            //平行光
            GeoTestShader.SetVector3("dirlight.direction", new Vector3(-2.0f));
            GeoTestShader.SetVector3("dirlight.ambient", new Vector3(0.7f));
            GeoTestShader.SetVector3("dirlight.diffuse", new Vector3(0.5f));
            GeoTestShader.SetVector3("dirlight.specular", new Vector3(0.5f));

            robot.Draw(GeoTestShader);

            GL.BindVertexArray(0);

            //法向量可视化
            normalDisplayShader.Use();

            model = Matrix4.CreateRotationX(0.0f);
            normalDisplayShader.SetMatrix4("model", model);

            robot.Draw(normalDisplayShader);

            GL.BindVertexArray(0);

            //天空盒
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            skyShader.Use();

            view = new Matrix4(new Matrix3(_camera.GetViewMatrix()));
            skyShader.SetMatrix4("view", view);

            Matrix4 projection = _camera.GetProjectionMatrix();

            skyShader.SetMatrix4("projection", projection);

            skyShader.SetInt("cubeTexture", 0);

            GL.BindVertexArray(skyVAO);

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.BindTexture(TextureTarget.TextureCubeMap, cubemapTexture);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            GL.DepthFunc(DepthFunction.Less);

            GL.BindVertexArray(0);


            //恢复默认帧缓冲
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        

            GL.BindVertexArray(quadVAO);

            ScreenShader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);

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
