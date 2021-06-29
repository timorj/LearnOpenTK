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

namespace UBO
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
            GL.ClearColor(0.2f,0.5f, 0.3f, 1.0f);
            

                    
           // floorMap = new Texture("../../Resource/Stone_Floor_002_COLOR.jpg");
           
            GL.GenBuffers(1, out objectVBO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, objectVertices.Length * sizeof(float), objectVertices, BufferUsageHint.StaticDraw);

            objectshader = new Shader("../../Shaders/objectshader.vert", "../../Shaders/objectshader.frag");
            objectshader.Use();

            objectScaleUpShader = new Shader("../../Shaders/objectshader.vert", "../../Shaders/shaderSingleColor.frag");
            objectScaleUpShader.Use();

            ScreenShader = new Shader("../../Shaders/screenShader.vert", "../../Shaders/screenShader.frag");
            ScreenShader.Use();

            reflectShader = new Shader("../../Shaders/reflectShader.vert", "../../Shaders/reflectShader.frag");
            reflectShader.Use();

            diffuseMap = new Texture("../../Resource/Wood_Herringbone_Tiles_001_basecolor.jpg");

            floorMap = new Texture("../../Resource/Stone_Floor_002_COLOR.jpg");

            grassMap = new Texture("../../Resource/blending_transparent_window.png");
            
            //立方体
            GL.GenVertexArrays(1, out objectVAO);
            GL.BindVertexArray(objectVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            var positionLocation = objectshader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            var texCoordsLocation = objectshader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(texCoordsLocation);
            GL.VertexAttribPointer(texCoordsLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //地板
            GL.GenBuffers(1, out floorVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, floorVertices.Length * sizeof(float), floorVertices, BufferUsageHint.StaticDraw);
            
            GL.GenVertexArrays(1, out floorVAO);
            GL.BindVertexArray(floorVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            positionLocation = objectshader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            texCoordsLocation = objectshader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(texCoordsLocation);
            GL.VertexAttribPointer(texCoordsLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
           
            //草
            GL.GenBuffers(1, out grassVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, grassVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, grassVertices.Length * sizeof(float), grassVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out grassVAO);
            GL.BindVertexArray(grassVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, grassVBO);
            positionLocation = objectshader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, grassVBO);
            texCoordsLocation = objectshader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(texCoordsLocation);
            GL.VertexAttribPointer(texCoordsLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //帧四边形
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
            texCoordsLocation = ScreenShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(texCoordsLocation);
            GL.VertexAttribPointer(texCoordsLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            
            
            //创建帧缓冲
            GL.GenFramebuffers(1, out FBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            //创建纹理附件缓冲
            GL.GenTextures(1, out TextureColorBuffer);
            GL.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 800, 600, 0, PixelFormat.Rgb, PixelType.Byte,(IntPtr)null);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureColorBuffer, 0);

            //如果不需要对深度缓冲和模板缓冲采样，则为深度缓冲附件和模板缓冲附件创建渲染缓冲附件以提高渲染效率
            //渲染缓冲附件
            GL.GenRenderbuffers(1, out RBO);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RBO);

            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, 800, 600);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, RBO);

            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception($"帧缓冲不完整{FBO},{FramebufferErrorCode.FramebufferComplete}");
            }
            skyShader = new Shader("../../Shaders/skyShader.vert", "../../Shaders/skyShader.frag");
            
            //天空盒
            GL.GenBuffers(1, out skyVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, skyVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, cubeVertices.Length * sizeof(float), cubeVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out skyVAO);
            GL.BindVertexArray(skyVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, skyVBO);
            positionLocation = skyShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //绑定立方体贴图
            cubemapTexture = LoadCubeMap(skyFaces);

            //反射贴图
            GL.GenBuffers(1, out reflectVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, reflectVBO);
           

            GL.BufferData(BufferTarget.ArrayBuffer, reflectBox.Length * sizeof(float), reflectBox, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out reflectVAO);
            GL.BindVertexArray(reflectVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, reflectVBO);
            positionLocation = reflectShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, reflectVBO);
            var normalLocation = reflectShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            _camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);

            

            //uniform缓冲对象
            //设置绑定点0
            Matrix4 projection = _camera.GetProjectionMatrix();
            Matrix4 view = _camera.GetViewMatrix();

            //在使用Uniform Block时，必须将矩阵转置后在放入内存中，否则会导致矩阵读取异常！
            projection = Matrix4.Transpose(projection);
            view = Matrix4.Transpose(view);
            
            Matrix4[] matrices = new Matrix4[] { view, projection };
                       
            int uniformBlockIndexObject = GL.GetUniformBlockIndex(objectshader.Handle, "Matrices");
            int uniformBlockIndexReflect = GL.GetUniformBlockIndex(reflectShader.Handle, "Matrices");

            //绑定至点
            GL.UniformBlockBinding(objectshader.Handle, uniformBlockIndexObject, 0);
            GL.UniformBlockBinding(reflectShader.Handle, uniformBlockIndexReflect, 0);

            GL.GenBuffers(1, out uboMatrices);
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);

            GL.BufferData(BufferTarget.UniformBuffer, 128,(IntPtr) null, BufferUsageHint.DynamicDraw);
                                
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer,0, uboMatrices, IntPtr.Zero, 128);
                                                          
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);

            //GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 128, matrices);//两种传入方式
            
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);//向内存中传入矩阵需要使用ref参数
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)64, 64, ref projection);
            
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            


            CursorVisible = false;
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Matrix4 model;
            Matrix4 view = _camera.GetViewMatrix();
            view = Matrix4.Transpose(view);
            Matrix4 projection;
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            //混合测试
            //启用混合测试
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //启用面剔除
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.FrontFace(FrontFaceDirection.Cw);
            //启用模板测试
          //  GL.Enable(EnableCap.StencilTest);
           // GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
           

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Enable(EnableCap.DepthTest);
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.Enable(EnableCap.DepthTest);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.1f, 0.2f, 0.1f, 1.0f);
            
          
            //渲染地板
            
            // GL.StencilMask(0x00);           
            GL.BindVertexArray(0);
            GL.BindVertexArray(floorVAO);
           
            objectshader.Use();
            model = Matrix4.CreateRotationX(0.0f);
            objectshader.SetMatrix4("model", model);
            floorMap.Use(TextureUnit.Texture0);
            objectshader.SetInt("diffuseMap", 0);

          
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            
            GL.BindVertexArray(0);
            //渲染立方体
            for (int i = 0; i < objectPosition.Length; i++)
            {
                objectshader.Use();
                diffuseMap.Use();
                GL.BindVertexArray(objectVAO);

                objectshader.SetInt("diffuseMap", 0);

                model = Matrix4.CreateTranslation(objectPosition[i]);
                //view = _camera.GetViewMatrix();
                //projection = _camera.GetProjectionMatrix();

                objectshader.SetMatrix4("model", model);


                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            }
            GL.BindVertexArray(0);


       
            //绘制天空盒
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            skyShader.Use();
            GL.BindVertexArray(skyVAO);

            view = new Matrix4(new Matrix3(_camera.GetViewMatrix()));
            //view = _camera.GetViewMatrix();
            projection = _camera.GetProjectionMatrix();

            skyShader.SetMatrix4("view", view);
            skyShader.SetMatrix4("projection", projection);

            skyShader.SetInt("cubeTexture", 0);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemapTexture);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            GL.DepthFunc(DepthFunction.Less);

            //绘制反射箱子
            GL.Disable(EnableCap.CullFace);
            GL.BindVertexArray(reflectVAO);
            reflectShader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, cubemapTexture);
            reflectShader.SetInt("skyBox", 0);
            model = Matrix4.CreateTranslation(new Vector3(1.0f, 0.5f, 0.0f));
            
            reflectShader.SetVector3("cameraPos", _camera.Position);
            reflectShader.SetMatrix4("model", model);    

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.Enable(EnableCap.CullFace);


            //渲染玻璃板
            GL.BindVertexArray(grassVAO);
            Dictionary<float, Vector3> sort = new Dictionary<float, Vector3>();
            for(int i = 0;i< grassPositions.Length; i++)
            {
                float distance = Vector3.Distance(_camera.Position, grassPositions[i]);
                sort[distance] = grassPositions[i];
            }
            
            sort = sort.OrderByDescending(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            GL.Disable(EnableCap.CullFace);
            objectshader.Use();
            grassMap.Use(TextureUnit.Texture0);
            foreach (KeyValuePair<float, Vector3> item in sort)
            {

                model = Matrix4.CreateTranslation(item.Value);
                view = _camera.GetViewMatrix();
                projection = _camera.GetProjectionMatrix();


                objectshader.SetMatrix4("model", model);
                objectshader.SetMatrix4("view", view);
                objectshader.SetMatrix4("projection", projection);

                objectshader.SetInt("diffuseMap", 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }

         

            GL.BindVertexArray(0);

            

            //还原默认帧缓冲
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Disable(EnableCap.DepthTest);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

            GL.BindVertexArray(quadVAO);
            ScreenShader.Use();
            ScreenShader.SetInt("screenTexture", 0);
            
            
            GL.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
          


            // GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            //GL.StencilMask(0xFF);



            /*
            GL.StencilMask(0x00);
            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);
           
            GL.Disable(EnableCap.DepthTest);
            for (int i = 0; i < objectPosition.Length; i++)
            {
                objectScaleUpShader.Use();
                
                GL.BindVertexArray(objectVAO);


                Matrix4 model = Matrix4.CreateTranslation(objectPosition[i]);
                model *= Matrix4.CreateScale(1.02f,1.05f,1.01f);
                Matrix4 view = _camera.GetViewMatrix();
                Matrix4 projection = _camera.GetProjectionMatrix();

                objectScaleUpShader.SetMatrix4("model", model);
                objectScaleUpShader.SetMatrix4("view", view);
                objectScaleUpShader.SetMatrix4("projection", projection);


                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            }
            */
            // GL.Enable(EnableCap.DepthTest);
            //GL.StencilMask(0xFF);



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
