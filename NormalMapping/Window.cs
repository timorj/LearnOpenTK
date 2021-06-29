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

namespace NormalMapping
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

        private int nmVBO;
        private int nmVAO;
        private Shader nmShader;
        private Shader screenShader;

        private Texture nmDiffuseMap;
        private Texture nmSpecularMap;
        private Texture nmNormalMap;


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

            nmShader = new Shader("../../Shaders/normalMappingShader.vert", "../../Shaders/normalMappingShader.frag");

            screenShader = new Shader("../../Shaders/screenShader.vert", "../../Shaders/screenShader.frag");

            nmDiffuseMap = new Texture("../../Resource/NormalMapping/brickwall.jpg");
            nmSpecularMap = new Texture("../../Resource/NormalMapping/brickwall.jpg");

            nmNormalMap = new Texture("../../Resource/NormalMapping/brickwall_normal.jpg");


            GL.GenBuffers(1, out nmVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, nmVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * floorVertices.Length, floorVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out nmVAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, nmVBO);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, nmVBO);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            _camera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);

            //帧四边形
            GL.GenBuffers(1, out quadVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out quadVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, quadVBO);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            

            //设置UBO
            Matrix4 view = _camera.GetViewMatrix();
            view = Matrix4.Transpose(view);

            Matrix4 projection = _camera.GetProjectionMatrix();
            projection = Matrix4.Transpose(projection);

            int uniformBolckIndexObject = GL.GetUniformBlockIndex(nmShader.Handle, "Matrices");
            GL.UniformBlockBinding(nmShader.Handle, uniformBolckIndexObject, 0);

            GL.GenBuffers(1, out uboMatrices);
            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferData(BufferTarget.UniformBuffer, 128, IntPtr.Zero, BufferUsageHint.StaticDraw);

            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, uniformBolckIndexObject, uboMatrices, IntPtr.Zero, 128);

            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, 64, ref view);

            GL.BindBuffer(BufferTarget.UniformBuffer, uboMatrices);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)64, 64, ref projection);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

         
            CursorVisible = false;
            base.OnLoad(e);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.Enable(EnableCap.DepthTest);
            //开启gramme校正
            GL.Enable(EnableCap.FramebufferSrgb);

            GL.Viewport(0, 0, 1024, 1024);

            RenderScene(nmShader);

            GL.BindVertexArray(quadVAO);
            screenShader.Use();
            screenShader.SetInt("screenTexture", 0);

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        private void RenderScene(Shader shader)
        {
            Matrix4 model = Matrix4.CreateRotationX(0.0f);
            GL.BindVertexArray(nmVAO);

            shader.Use();
            shader.SetMatrix4("model", model);

            shader.SetVector3("viewPos", _camera.Position);

            nmDiffuseMap.Use();
            nmSpecularMap.Use(TextureUnit.Texture1);

            nmNormalMap.Use(TextureUnit.Texture2);

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

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
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
