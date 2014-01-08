﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Custom.Presentation
{
    using Custom.Algebra;

    public class SVGRenderer
    {
        private ISizeCalculation m_iSize;

        private EPSColor m_DarkColor;
        private EPSColor m_LightColor;

        public SVGRenderer(ISizeCalculation isize, EPSColor darkcolor, EPSColor lightcolor)
        {
            m_iSize = isize;
            m_DarkColor = darkcolor;
            m_LightColor = lightcolor;
        }

        public void WriteToStream(BitMatrix matrix, Stream stream)
        {
            this.WriteToStream(matrix, stream, true);
        }

        public void WriteToStream(BitMatrix matrix, Stream stream, bool includeSize)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendHeader();
                this.AppendSVGQrCode(sb, matrix, includeSize, true);
                writer.Write(sb.ToString());
            }
        }

        public string WriteToString(BitMatrix matrix)
        {
            return this.WriteToString(matrix, true);
        }

        public string WriteToString(BitMatrix matrix, bool includeSize)
        {
            StringBuilder sb = new StringBuilder();
            this.AppendSVGQrCode(sb, matrix, includeSize, false);
            return sb.ToString();
        }

        private void AppendSVGQrCode(StringBuilder sb, BitMatrix matrix, bool includeSize, bool isStream)
        {
            DrawingSize dsize = m_iSize.GetSize(matrix.Width);
            int pixelwidth = m_iSize.GetSize(matrix.Width).CodeWidth;
            int quietZone = (int)dsize.QuietZoneModules;
            int width = matrix == null ? 21 : matrix.Width;
            sb.AppendSVGTag(includeSize ? new MatrixPoint(pixelwidth, pixelwidth) : new MatrixPoint(0, 0),
                new MatrixPoint(2 * quietZone + width, 2 * quietZone + width), m_LightColor, m_DarkColor);
            if (!isStream)
                sb.Append(@"<!-- Created with Qrcode.Net (http://qrcodenet.codeplex.com/) -->");
            AppendDarkCell(sb, matrix, quietZone, quietZone);
            sb.AppendSVGTagEnd();
        }

        private static void AppendDarkCell(StringBuilder sb, BitMatrix matrix, int offsetX, int offSetY)
        {
            int width = matrix == null ? 21 : matrix.Width;

            if (matrix == null)
                return;

            int preX = -1;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (matrix[x, y])
                    {
                        //Set start point if preX == -1
                        if (preX == -1)
                            preX = x;
                        //If this is last module in that row. Draw rectangle
                        if (x == width - 1)
                        {
                            sb.AppendRec(new MatrixPoint(preX + offsetX, y + offSetY), new MatrixPoint(x - preX + 1, 1));
                            preX = -1;
                        }
                    }
                    else if (!matrix[x, y] && preX != -1)
                    {
                        //Here will be first light module after sequence of dark module.
                        //Draw previews sequence of dark Module
                        sb.AppendRec(new MatrixPoint(preX + offsetX, y+offSetY), new MatrixPoint(x - preX, 1));
                        preX = -1;
                    }
                }
            }
        }
    }
}
