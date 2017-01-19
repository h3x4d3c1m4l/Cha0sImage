/*
    This file is part of the Cha0sImage library and image manipulator tool.

    h3xmonitor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    h3xmonitor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.IO;
using ImageSharp;
using ImageSharp.Formats;

namespace Cha0sImageLib
{
    public static class ImageManipulators
    {
        public static void Intensifier(Stream pImageStream, Stream pOutputStream)
        {
            const double CONST_MAX_OFFSET_PERCENTAGE = 1;
            const int CONST_FRAMES = 10;
            const int CONST_FRAMEDELAY = 2;

            var inputImg = new Image(pImageStream);

            var h = inputImg.Height;
            var hMaxOffset = (int) (CONST_MAX_OFFSET_PERCENTAGE / 100 * h);
            var w = inputImg.Width;
            var wMaxOffset = (int) (CONST_MAX_OFFSET_PERCENTAGE / 100 * w);

            var rng = new Random();
            var outputImg = new Image(inputImg).Crop(w - wMaxOffset, h - hMaxOffset);
            for (var i = 0; i < CONST_FRAMES; i++)
            {
                var clone = new Image(inputImg);

                // offset
                var xshift = rng.Next(0, hMaxOffset);
                var yshift = rng.Next(0, wMaxOffset);
                clone.Crop(new Rectangle(xshift, yshift, w - wMaxOffset, h - hMaxOffset));

                // color overlay
                var randomColor = Color.Transparent;
                randomColor.R = (byte)rng.Next(0, 255);
                randomColor.G = (byte)rng.Next(0, 255);
                randomColor.B = (byte)rng.Next(0, 255);
                randomColor.A = 150;
                clone.Fill(randomColor);

                // add to animation
                outputImg.Frames.Add(new ImageFrame(clone) { FrameDelay = CONST_FRAMEDELAY });
            }
            outputImg.FrameDelay = CONST_FRAMEDELAY;

            outputImg.Save(pOutputStream, new GifFormat());
        }

        public static void MoreJPEG(Stream pImageStream, Stream pOutputStream)
        {
            const int CONST_QUALITY = 2;

            var inputImg = new Image(pImageStream);
            inputImg.Save(pOutputStream, new JpegEncoder { Quality = CONST_QUALITY });
        }
    }
}
