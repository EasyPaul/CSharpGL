﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpShaderLanguage
{
    /*
     
     #version 150 core

in vec3 in_Position;
in vec2 in_UV;  
out vec2 pass_UV;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main(void) 
{
	gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(in_Position, 1.0);

	pass_UV = in_UV;
}

     */

    /// <summary>
    /// 这是一个用CSSL写的vertex shader的例子。
    /// </summary>
    class DemoVert : VertexShaderCode
    {
        [In]
        vec3 in_Position;
        [In]
        vec2 in_UV;
        [Out]
        vec2 pass_UV;

        [Uniform]
        mat4 projectionMatrix;
        [Uniform]
        mat4 viewMatrix;
        [Uniform]
        mat4 modelMatrix;

        public override void main()
        {
            gl_Position = vec4(in_Position, 1.0);//projectionMatrix*viewMatrix*modelMatrix*new vec4()
            pass_UV = in_UV;
        }

    }

    /// <summary>
    /// vertex shader共有的内容。
    /// 想写一个vertex shader，就继承此类型吧。
    /// </summary>
    public abstract class VertexShaderCode : ShaderCode
    {
        //public override sealed string ExtensionName
        //{
        //    get { return "vert"; }
        //}

        //public override SemanticShader Dump(string fullname)
        //{
        //    return new SemanticVertexShader(this, fullname);
        //}

        protected vec4 gl_Position;

    }
}