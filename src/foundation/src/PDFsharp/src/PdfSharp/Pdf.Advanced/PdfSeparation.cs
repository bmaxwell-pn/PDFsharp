using PdfSharp.Drawing;
//using PdfSharp.Pdf.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSharp.Pdf.Advanced
{
    /// <summary>
    /// Represents a spot color separation
    /// </summary>
    public class PdfSeparation : PdfArray
    {
        /// <summary>
        /// Spot color separations
        /// </summary>
        public PdfSeparation(PdfDocument document, XColor color)
            : base(document)
        {
            if (color.IsEmpty)
                throw new ArgumentNullException("color");
            if (color.ColorSpace != XColorSpace.Spot)
                throw new InvalidOperationException("Color must be a spot color.");

            this.Elements.Add(new PdfName("/Separation"));
            this.Elements.Add(new PdfName("/" + color.SpotName));
            this.Elements.Add(new PdfName(document.Options.ColorMode == PdfColorMode.Rgb ? "/DeviceRGB" : "/DeviceCMYK"));
            this.Elements.Add(buildParams(color));
        }

        private PdfDictionary buildParams(XColor color)
        {
            PdfDictionary dict = new PdfDictionary();

            if (this.Owner.Options.ColorMode != PdfColorMode.Rgb)
            {
                // cmyk
                dict.Elements.Add("/C0", buildColors(0, 0, 0, 0));
                dict.Elements.Add("/C1", buildColors(color.C, color.M, color.Y, color.K));
                dict.Elements.Add("/Range", buildRange(8));
            }
            else
            {
                // rgb
                dict.Elements.Add("/C0", buildColors(255, 255, 255));
                dict.Elements.Add("/C1", buildColors(color.R, color.G, color.B));
                dict.Elements.Add("/Range", buildRange(6));
            }


            dict.Elements.Add("/Domain", buildRange(2));
            dict.Elements.Add("/FunctionType", new PdfInteger(2));
            dict.Elements.Add("/N", new PdfReal(1));


            return dict;
        }

        private PdfArray buildColors(double cyan, double magenta, double yellow, double black)
        {
            PdfArray colors = new PdfArray();

            colors.Elements.Add(new PdfReal(cyan));
            colors.Elements.Add(new PdfReal(magenta));
            colors.Elements.Add(new PdfReal(yellow));
            colors.Elements.Add(new PdfReal(black));

            return colors;
        }

        private PdfArray buildColors(double red, double green, double blue)
        {
            PdfArray colors = new PdfArray();

            colors.Elements.Add(new PdfReal(red / 255));
            colors.Elements.Add(new PdfReal(green / 255));
            colors.Elements.Add(new PdfReal(blue / 255));

            return colors;
        }

        private PdfArray buildRange(int limit)
        {
            PdfArray arr = new PdfArray();

            for (int i = 0; i < limit; i++)
                arr.Elements.Add(new PdfReal(i % 2));

            return arr;
        }
    }
}
