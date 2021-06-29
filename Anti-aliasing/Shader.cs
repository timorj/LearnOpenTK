using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Anti_aliasing
{
    class Shader
    {
        public int Handle;
        private readonly Dictionary<string, int> _uniformLocation;
        public Shader(string vertShaderPath, string fragShaderPath)
        {
            //创建顶点着色器
            var shaderSource = LoadSource(vertShaderPath);
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            //绑定着色器代码
            GL.ShaderSource(vertexShader, shaderSource);
            //检测是否着色器代码是否编译错误,错误则抛出异常
            CompileShader(vertexShader);

            //创建片段着色器
            shaderSource = LoadSource(fragShaderPath);
            var fragShader = GL.CreateShader(ShaderType.FragmentShader);

            //绑定着色器代码 
            GL.ShaderSource(fragShader, shaderSource);
            //检验是否编译错误，错误则抛出异常
            CompileShader(fragShader);

            //创建着色器程序
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragShader);

            //获取着色器语言的uniform
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int NumberOfUniforms);
            _uniformLocation = new Dictionary<string, int>();
            for(int i = 0; i < NumberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                _uniformLocation.Add(key, location);
            }
          
        }
        private static void LinkProgram(int program)
        {
            //链接程序
            GL.LinkProgram(program);
            //检测程序是否链接错误,错误则抛出异常
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if(code != (int)All.True)
            {
                var InfoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Exception occured whilst linking program({program}).\n\n{InfoLog}");
            }
        }
        private static string LoadSource(string vertShaderPath)
        {
            using(var sr = new StreamReader(vertShaderPath, UTF8Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var InfoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Exception occurred whilst compiling Shader({shader}).\n\n{InfoLog}");

            }
        }
        public void Use()
        {
            GL.UseProgram(Handle);
        }
        /// <summary>
        /// 获取GLSL中变量的索引
        /// </summary>
        /// <param name="atrribName"></param>
        /// <returns></returns>
        public int GetAttribLocation(string atrribName)
        {
            return GL.GetAttribLocation(Handle, atrribName);
        }
        /// <summary>
        /// 设置GLSL中的Int类型的uniform变量
        /// </summary>
        /// <param name="name">uniform名称</param>
        /// <param name="data">目标值</param>
        public void SetInt(string name ,int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocation[name], data);
        }
        /// <summary>
        /// 设置GLSL中的Float类型的uniform变量
        /// </summary>
        /// <param name="name">uniform名称</param>
        /// <param name="data">目标值</param>
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocation[name], data);
        }
        /// <summary>
        /// 设置GLSL中的Matrix4类型的uniform变量
        /// </summary>
        /// <param name="name">uniform名称</param>
        /// <param name="data">目标值</param>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocation[name],true, ref data);
        }
        /// <summary>
        /// 设置GLSL中的vector3类型的uniform变量
        /// </summary>
        /// <param name="name">uniform名称</param>
        /// <param name="data">目标值</param>
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocation[name], data);
        }
        
    }
}
