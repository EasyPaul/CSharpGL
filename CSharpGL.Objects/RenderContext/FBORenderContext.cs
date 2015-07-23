﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGL.Objects
{
    /// <summary>
    /// A render context.
    /// </summary>
    public class FBORenderContext : HiddenWindowRenderContext
    {

        /// <summary>
        /// Creates the render context provider. Must also create the OpenGL extensions.
        /// </summary>
        /// <param name="openGLVersion">The desired OpenGL version.</param>
        /// <param name="gl">The OpenGL context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="parameter">The parameter</param>
        /// <returns></returns>
        public override bool Create(GLVersion openGLVersion, int width, int height, int bitDepth, object parameter)
        {
            //  Call the base class. 	        
            base.Create(openGLVersion, width, height, bitDepth, parameter);

            uint[] ids = new uint[1];

            //  First, create the frame buffer and bind it.
            ids = new uint[1];
            GL.GenFramebuffersEXT(1, ids);
            frameBufferID = ids[0];
            GL.BindFramebufferEXT(GL.GL_FRAMEBUFFER_EXT, frameBufferID);

            //	Create the colour render buffer and bind it, then allocate storage for it.
            GL.GenRenderbuffersEXT(1, ids);
            colourRenderBufferID = ids[0];
            GL.BindRenderbufferEXT(GL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            GL.RenderbufferStorageEXT(GL.GL_RENDERBUFFER_EXT, GL.GL_RGBA, width, height);

            //	Create the depth render buffer and bind it, then allocate storage for it.
            GL.GenRenderbuffersEXT(1, ids);
            depthRenderBufferID = ids[0];
            GL.BindRenderbufferEXT(GL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
            GL.RenderbufferStorageEXT(GL.GL_RENDERBUFFER_EXT, GL.GL_DEPTH_COMPONENT24, width, height);

            //  Set the render buffer for colour and depth.
            GL.FramebufferRenderbufferEXT(GL.GL_FRAMEBUFFER_EXT, GL.GL_COLOR_ATTACHMENT0_EXT,
                GL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            GL.FramebufferRenderbufferEXT(GL.GL_FRAMEBUFFER_EXT, GL.GL_DEPTH_ATTACHMENT_EXT,
                GL.GL_RENDERBUFFER_EXT, depthRenderBufferID);

            dibSectionDeviceContext = Win32.CreateCompatibleDC(DeviceContextHandle);

            //  Create the DIB section.
            dibSection.Create(dibSectionDeviceContext, width, height, bitDepth);

            return true;
        }

        private void DestroyFramebuffers()
        {
            //  Delete the render buffers.
            GL.DeleteRenderbuffersEXT(2, new uint[] { colourRenderBufferID, depthRenderBufferID });

            //	Delete the framebuffer.
            GL.DeleteFramebuffersEXT(1, new uint[] { frameBufferID });

            //  Reset the IDs.
            colourRenderBufferID = 0;
            depthRenderBufferID = 0;
            frameBufferID = 0;
        }

        public override void Destroy()
        {
            //  Delete the render buffers.
            DestroyFramebuffers();

            //  Destroy the internal dc.
            Win32.DeleteDC(dibSectionDeviceContext);

            //	Call the base, which will delete the render context handle and window.
            base.Destroy();
        }

        public override void SetDimensions(int width, int height)
        {
            //  Call the base.
            base.SetDimensions(width, height);

            //	Resize dib section.
            dibSection.Resize(width, height, BitDepth);

            DestroyFramebuffers();

            //  TODO: We should be able to just use the code below - however we 
            //  get invalid dimension issues at the moment, so recreate for now.

            /*
            //  Resize the render buffer storage.
            GL.BindRenderbufferEXT(GL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            GL.RenderbufferStorageEXT(GL.GL_RENDERBUFFER_EXT, GL.GL_RGBA, width, height);
            GL.BindRenderbufferEXT(GL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
            GL.RenderbufferStorageEXT(GL.GL_RENDERBUFFER_EXT, GL.GL_DEPTH_ATTACHMENT_EXT, width, height);
            var complete = GL.CheckFramebufferStatusEXT(GL.GL_FRAMEBUFFER_EXT);
            */

            uint[] ids = new uint[1];

            //  First, create the frame buffer and bind it.
            ids = new uint[1];
            GL.GenFramebuffersEXT(1, ids);
            frameBufferID = ids[0];
            GL.BindFramebufferEXT(GL.GL_FRAMEBUFFER_EXT, frameBufferID);

            //	Create the colour render buffer and bind it, then allocate storage for it.
            GL.GenRenderbuffersEXT(1, ids);
            colourRenderBufferID = ids[0];
            GL.BindRenderbufferEXT(GL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            GL.RenderbufferStorageEXT(GL.GL_RENDERBUFFER_EXT, GL.GL_RGBA, width, height);

            //	Create the depth render buffer and bind it, then allocate storage for it.
            GL.GenRenderbuffersEXT(1, ids);
            depthRenderBufferID = ids[0];
            GL.BindRenderbufferEXT(GL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
            GL.RenderbufferStorageEXT(GL.GL_RENDERBUFFER_EXT, GL.GL_DEPTH_COMPONENT24, width, height);

            //  Set the render buffer for colour and depth.
            GL.FramebufferRenderbufferEXT(GL.GL_FRAMEBUFFER_EXT, GL.GL_COLOR_ATTACHMENT0_EXT,
                GL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            GL.FramebufferRenderbufferEXT(GL.GL_FRAMEBUFFER_EXT, GL.GL_DEPTH_ATTACHMENT_EXT,
                GL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
        }

        public override void Blit(IntPtr hdc)
        {
            if (DeviceContextHandle != IntPtr.Zero)
            {
                //  Set the read buffer.
                GL.ReadBuffer(GL.GL_COLOR_ATTACHMENT0_EXT);

                //	Read the pixels into the DIB section.
                GL.ReadPixels(0, 0, Width, Height, GL.GL_BGRA,
                    GL.GL_UNSIGNED_BYTE, dibSection.Bits);

                //	Blit the DC (containing the DIB section) to the target DC.
                Win32.BitBlt(hdc, 0, 0, Width, Height,
                    dibSectionDeviceContext, 0, 0, Win32.SRCCOPY);
            }
        }

        protected uint colourRenderBufferID = 0;
        protected uint depthRenderBufferID = 0;
        protected uint frameBufferID = 0;
        protected IntPtr dibSectionDeviceContext = IntPtr.Zero;
        protected DIBSection dibSection = new DIBSection();

        /// <summary>
        /// Gets the internal DIB section.
        /// </summary>
        /// <value>The internal DIB section.</value>
        public DIBSection InternalDIBSection
        {
            get { return dibSection; }
        }
    }
}