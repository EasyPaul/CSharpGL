﻿using CSharpGL.Objects.Texts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Font2Bmps
{
    class FontTextureXmlPrinter
    {
        private CSharpGL.Objects.Texts.FontTexture ttfTexture;

        public FontTextureXmlPrinter(CSharpGL.Objects.Texts.FontTexture ttfTexture)
        {
            // TODO: Complete member initialization
            this.ttfTexture = ttfTexture;
        }


        public void Print(string fontFullname)
        {
            XElement xElement = FontTextureHelper.ToXElement(this.ttfTexture);
            xElement.Save(fontFullname + ".xml");
        }

    }
}