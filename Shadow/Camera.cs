using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Shadow
{
    class Camera
    {//旋转变量
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        // 欧拉角 俯仰角
        private float _pitch;

        // 偏航角
        private float _yaw = -MathHelper.PiOver2; // 没有这个会导致90度的偏置

        // 相机视角（弧度值）
        private float _fov = MathHelper.PiOver2;

        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }

        // 相机位置
        public Vector3 Position { get; set; }

        // 投影矩阵的角度
        public float AspectRatio { private get; set; }

        //映射
        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;

        // 一旦俯仰角的属性值改变，就将角度值转换为弧度值
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // 我们限制摄像机的俯仰角为负90度到90度
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // 同样一旦偏航角的属性值改变，就将角度值转换为弧度值
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        // 相机的视场，同样也限制角度
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        // 使用LookAt来获取观察矩阵
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        // 获取投影矩阵
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }

        // 更新属性值
        private void UpdateVectors()
        {
            //计算分量值
            _front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
            _front.Y = (float)Math.Sin(_pitch);
            _front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);

            //标准化
            _front = Vector3.Normalize(_front);

            // 标准化叉乘后的值
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}
