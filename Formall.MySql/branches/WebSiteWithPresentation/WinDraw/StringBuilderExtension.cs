﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Custom.Presentation
{
    using Custom.Algebra;

    internal static class StringBuilderExtension
    {
        internal static StringBuilder AppendHeader(this StringBuilder sb)
        {
            return sb.Append("<?xml version=\"1.0\" standalone=\"no\"?>")
                .Append(@"<!-- Created with Qrcode.Net (http://qrcodenet.codeplex.com/) -->")
                .Append("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">");
        }

        internal static StringBuilder AppendSVGTag(this StringBuilder sb, MatrixPoint displaysize, MatrixPoint viewboxSize, EPSColor background, EPSColor fill)
        {

            if (displaysize.X <= 0 || displaysize.Y <= 0)
                return sb.Append(string.Format("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.2\" baseProfile=\"tiny\" viewBox=\"0 0 {0} {1}\" viewport-fill=\"rgb({2}, {3}, {4})\" viewport-fill-opacity=\"{5}\" fill=\"rgb({6}, {7}, {8})\" fill-opacity=\"{9}\" {10}>",
                    viewboxSize.X,
                    viewboxSize.Y,
                    background.R,
                    background.G,
                    background.B,
                    ConvertAlpha(background.A),
                    fill.R,
                    fill.G,
                    fill.B,
                    ConvertAlpha(fill.A),
                    BackgroundStyle(background)
                    ));
            else
                return sb.Append(string.Format("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.2\" baseProfile=\"tiny\" viewBox=\"0 0 {0} {1}\" viewport-fill=\"rgb({2}, {3}, {4})\" viewport-fill-opacity=\"{5}\" fill=\"rgb({6}, {7}, {8})\" fill-opacity=\"{9}\" {10} width=\"{11}\" height=\"{12}\">",
                    viewboxSize.X,
                    viewboxSize.Y,
                    background.R,
                    background.G,
                    background.B,
                    ConvertAlpha(background.A),
                    fill.R,
                    fill.G,
                    fill.B,
                    ConvertAlpha(fill.A),
                    BackgroundStyle(background),
                    displaysize.X,
                    displaysize.Y));
        }

        internal static StringBuilder AppendRec(this StringBuilder sb, MatrixPoint position, MatrixPoint size)
        {
            return sb.Append(string.Format("<rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\"/>",
                position.X,
                position.Y,
                size.X,
                size.Y + 0.03));
        }

        internal static StringBuilder AppendSVGTagEnd(this StringBuilder sb)
        {
            return sb.Append("</svg>");
        }

        internal static double ConvertAlpha(byte alpha)
        {
            return Math.Round((((double)alpha) / (double)255), 2);
        }

        internal static string BackgroundStyle(EPSColor color)
        {
            double alpha = ConvertAlpha(color.A);
            return string.Format("style=\"background-color:rgb({0},{1},{2});background-color:rgba({0},{1},{2},{3});\"",
                color.R,
                color.G,
                color.B,
                alpha);
        }

    }
}
