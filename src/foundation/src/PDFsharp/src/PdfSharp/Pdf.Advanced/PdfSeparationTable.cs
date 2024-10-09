using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfSeparationTable : PdfResourceTable
    {
        readonly Dictionary<string, PdfSeparation> separations = new Dictionary<string, PdfSeparation>();

        public PdfSeparationTable(PdfDocument document)
            : base(document)
        {
        }

        public PdfSeparation GetSeparation(XColor color)
        {
            string spotName = color.SpotName;

            PdfSeparation pdfSeparation;

            if (!this.separations.TryGetValue(spotName, out pdfSeparation))
            {
                pdfSeparation = new PdfSeparation(Owner, color);

                Debug.Assert(pdfSeparation.Owner == Owner);

                this.separations[spotName] = pdfSeparation;
            }

            return pdfSeparation;
        }
    }
}
