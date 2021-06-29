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


namespace RenderShadow
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

        private Shader objectShader;

        private Shader objectScaleUpShader;

        private Shader screenShader;

        private Texture diffuseMap;

        private float[] floorVertices =
        {
     // Positions          // Normals         // Texture Coords
        25.0f, -0.5f, 25.0f, 0.0f, 1.0f, 0.0f, 25.0f, 0.0f,
        -25.0f, -0.5f, -25.0f, 0.0f, 1.0f, 0.0f, 0.0f, 25.0f,
        -25.0f, -0.5f, 25.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,

        25.0f, -0.5f, 25.0f, 0.0f, 1.0f, 0.0f, 25.0f, 0.0f,
        25.0f, -0.5f, -25.0f, 0.0f, 1.0f, 0.0f, 25.0f, 25.0f,
        - 25.0f, -0.5f, -25.0f, 0.0f, 1.0f, 0.0f, 0.0f, 25.0f
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
        -1.0f,  1.0f, 0.0f,  0.0f, 1.0f,
        -1.0f, -1.0f, 0.0f,  0.0f, 0.0f,
         1.0f, -1.0f, 0.0f, 1.0f, 0.0f,

        -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
         1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
         1.0f,  1.0f, 0.0f, 1.0f, 1.0f
          };


        private int quadVAO;
        private int quadVBO;

        private int grassVAO;
        private int grassVBO;

        private Texture grassMap;

        private int floorVAO;
        private int floorVBO;

        private Texture floorDiffuseMap;

        private Texture floorSpecularMap;


        private Shader floorShader;

        private Camera _camera;

        //帧缓冲

        private int FBO;

        private int textureColorBuffer;

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

        // 阴影
        private int depthMapFBO;

        private int depthMap;

        private const int SHADOW_WIDTH = 1024, SHADOW_HEIGHT = 1024;

        private Shader depthMapShader;
        #endregion
        #region 物体顶点
        private readonly float[] _objectVertices = {

         -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,
         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  0.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,

        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  1.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,

        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,
         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  1.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f
        };
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

           // floorShader = new Shader("../../Shaders/floorShader.vert", "../../Shaders/floorShader.frag");
            //floorShader.Use();

            floorDiffuseMap = new Texture("../../Resource/WoodFloor032_2K-PNG/WoodFloor032_2K_Color.png");

            floorSpecularMap = new Texture("../../Resource/WoodFloor032_2K-PNG/WoodFloor032_2K_Displacement.png");

            objectShader = new Shader("../../Shaders/objectShader.vert", "../../Shaders/objectShader.frag");
            objectShader.Use();

            depthMapShader = new Shader("../../Shaders/simpleDepthShader.vert", "../../Shaders/simpleDepthShader.frag");
            depthMapShader.Use();

            screenShader = new Shader("../../Shaders/screenShader.vert", "../../Shaders/screenShader.frag");
            screenShader.Use();


            //地板
            GL.GenBuffers(1, out floorVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, floorVertices.Length * sizeof(float), floorVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out floorVAO);
            GL.BindVertexArray(floorVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            //var positionLocation = floorShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            //var normalLocation = floorShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
            //var textureLocation = floorShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //物块
            GL.GenBuffers(1, out objectVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _objectVertices.Length * sizeof(float), _objectVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out objectVAO);
            GL.BindVertexArray(objectVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            //positionLocation = floorShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            //normalLocation = floorShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, objectVBO);
            //textureLocation = floorShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            _camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);
            
            //设置UBO
            Matrix4 view = _camera.GetViewMatrix();
            view = Matrix4.Transpose(view);

            Matrix4 projection = _camera.GetProjectionMatrix();
            projection = Matrix4.Transpose(projection);

            //int uniformBolckIndexFloor = GL.GetUniformBlockIndex(floorShader.Handle, "Matrices");
            int uniformBolckIndexObject = GL.GetUniformBlockIndex(objectShader.Handle, "Matrices");

           //GL.UniformBlockBinding(floorShader.Handle, uniformBolckIndexFloor, 0);
            GL.UniformBlockBinding(objectShader.Handle, uniformBolckIndexObject, 0);

            GL.GenBuffers(1, out uboMatrices);
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferData(BufferTarget.UniformBuffer, 128, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, uniformBolckIndexObject, uboMatrices, IntPtr.Zero, 128);

            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);

            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)64, 64, ref projection);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            
            //深度贴图
            GL.GenFramebuffers(1, out depthMapFBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);

            GL.GenTextures(1, out depthMap);
            GL.BindTexture(TextureTarget.Texture2D, depthMap);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, SHADOW_WIDTH, SHADOW_HEIGHT, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            float[] borderColor = { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthMap, 0);

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            //帧四边形
            GL.GenBuffers(1, out quadVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out quadVAO);
            GL.BindVertexArray(quadVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            var positionLocation = screenShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            var textureLocation = screenShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            

            CursorVisible = false;
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            
            Matrix4 view = _camera.GetViewMatrix();
            view = Matrix4.Transpose(view);
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
            //开启gramme校正
            GL.Enable(EnableCap.FramebufferSrgb);

            
           
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //创建光源方向的模型矩阵
            Matrix4 lightProjection;
            lightProjection = Matrix4.CreateOrthographicOffCenter(-10.0f, 10.0f, -10.0f, 10.0f, 1.0f, 7.5f);
            
            Matrix4 lightView = Matrix4.LookAt(new Vector3(-2.0f, 4.0f, -1.0f), new Vector3(0.0f), new Vector3(0.0f, 1.0f, 0.0f));

            Matrix4 lightSpaceMatrix = lightView * lightProjection ;

            depthMapShader.Use();
            depthMapShader.SetMatrix4("lightSpaceMatrix", lightSpaceMatrix);

            //渲染深度贴图
            GL.Viewport(0, 0, SHADOW_WIDTH, SHADOW_HEIGHT);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
            
           // GL.CullFace(CullFaceMode.Back);
            GL.Clear(ClearBufferMask.DepthBufferBit);
           
            renderScene(depthMapShader);



            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
           // GL.CullFace(CullFaceMode.Front);
            GL.Viewport(0, 0, Width, Height);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            objectShader.Use();
            objectShader.SetMatrix4("lightSpaceMatrix", lightSpaceMatrix);
            setUpUniforms(objectShader);

            objectShader.SetInt("shadowMap", 2);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, depthMap);

            renderScene(objectShader);

            GL.BindVertexArray(quadVAO);
            screenShader.Use();
            screenShader.SetInt("screenTexture", 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, depthMap);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            



            

            SwapBuffers();
            base.OnRenderFrame(e);
        }
        
        /// <summary>
        /// 场景绘制
        /// </summary>
        /// <param name="shader"></param>
        private void renderScene(Shader shader) 
        {
            
            //绘制地板
            Matrix4 model = Matrix4.CreateRotationX(0.0f);
            GL.BindVertexArray(floorVAO);
            
            //地板
            shader.Use();
            shader.SetMatrix4("model", model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            
            GL.BindVertexArray(0);
            //物块
            for (int i = 0; i < objectPosition.Length; i++)
            {
                GL.BindVertexArray(objectVAO);

                model =Matrix4.CreateTranslation(objectPosition[i]);

                shader.SetMatrix4("model", model);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
               
            }
            GL.BindVertexArray(0);


        }

        private void setUpUniforms(Shader shader) 
        {
            floorDiffuseMap.Use();
            floorSpecularMap.Use(TextureUnit.Texture1);
          
            shader.SetVector3("viewPos", _camera.Position);

            shader.SetInt("material.diffuse", 0);
            shader.SetInt("material.specular", 1);
            shader.SetInt("blinn", 1);
            shader.SetFloat("material.shininess", 64.0f);

            shader.SetVector3("dirlight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            shader.SetVector3("dirlight.ambient", new Vector3(0.1f));
            shader.SetVector3("dirlight.diffuse", new Vector3(0.2f));
            shader.SetVector3("dirlight.specular", new Vector3(0.2f));


            //点光源
            shader.SetVector3("pointlight.position", new Vector3(-2.0f, 4.0f, -1.0f));
            shader.SetVector3("pointlight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
            shader.SetVector3("pointlight.diffuse", new Vector3(1.0f));
            shader.SetVector3("pointlight.specular", new Vector3(1.0f));
            shader.SetFloat("pointlight.constant", 1.0f);
            shader.SetFloat("pointlight.linear", 0.09f);
            shader.SetFloat("pointlight.quadratic", 0.032f);

        }
        /*
        private void renderCube()
        {

          

           GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
           
            // 物块
            for (int i = 0; i < objectPosition.Length; i++)
            {
                objectShader.Use();

                GL.BindVertexArray(objectVAO);

                Matrix4 model = Matrix4.CreateTranslation(objectPosition[i]);
                 shader.SetMatrix4("model", model);
                shader.SetVector3("viewPos", _camera.Position);

                shader.SetInt("material.diffuse", 0);
                shader.SetInt("material.specular", 1);
                shader.SetInt("blinn", 0);
                shader.SetFloat("material.shininess", 32.0f);

                shader.SetVector3("dirlight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
                shader.SetVector3("dirlight.ambient", new Vector3(0.1f));
                shader.SetVector3("dirlight.diffuse", new Vector3(0.1f));
                shader.SetVector3("dirlight.specular", new Vector3(0.1f));

                //点光源
                shader.SetVector3("pointlight.position", new Vector3(0.0f, 0.5f, 0.0f));
                shader.SetVector3("pointlight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
                shader.SetVector3("pointlight.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
                shader.SetVector3("pointlight.specular", new Vector3(1.0f, 1.0f, 1.0f));
                shader.SetFloat("pointlight.constant", 1.0f);
                shader.SetFloat("pointlight.linear", 0.09f);
                shader.SetFloat("pointlight.quadratic", 0.032f);
                
                objectShader.SetMatrix4("model", model);

               
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


            }

        }
*/

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
