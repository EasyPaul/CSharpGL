﻿using GLM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGL.Models
{
    /// <summary>
    /// 链条。若干个点用直线连接起来。
    /// </summary>
    class ChainModel : PCIModel
    {
        private Random random = new Random();

        /// <summary>
        /// 链条。若干个点用直线连接起来。
        /// </summary>
        /// <param name="pointCount">有多少个点</param>
        /// <param name="length">点的范围（长度）</param>
        /// <param name="width">点的范围（宽度）</param>
        /// <param name="height">点的范围（高度）</param>
        public ChainModel(int pointCount = 10, int length = 5, int width = 5, int height = 5)
        {
            var positions = new vec3[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                var point = new vec3();
                point.x = (float)random.NextDouble() * length;
                point.y = (float)random.NextDouble() * width;
                point.z = (float)random.NextDouble() * height;
                positions[i] = point;
            }
            this.Positions = positions.Move2Center();

            this.Colors = new vec3[pointCount];
            {
                for (int i = 0; i < pointCount; i++)
                {
                    uint p = (uint)((256 * 256 * 256) / pointCount * (i + 1));
                    var color = new vec3();
                    color.x = ((p >> 0) & 0xFF) / 255.0f;
                    color.y = ((p >> 8) & 0xFF) / 255.0f;
                    color.z = ((p >> 16) & 0xFF) / 255.0f;
                    this.Colors[i] = color;
                }
            }
        }


    }
}