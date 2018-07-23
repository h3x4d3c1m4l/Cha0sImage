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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.Primitives;

namespace Cha0sImageLib
{
    public static class ImageManipulators
    {
        public static void Intensifier(Stream pImageStream, Stream pOutputStream)
        {
            const double CONST_MAX_OFFSET_PERCENTAGE = 1;
            const int CONST_FRAMES = 10;
            const int CONST_FRAMEDELAY = 2;

            var inputImg = Image.Load(pImageStream);

            var h = inputImg.Height;
            var hMaxOffset = (int) (CONST_MAX_OFFSET_PERCENTAGE / 100 * h);
            var w = inputImg.Width;
            var wMaxOffset = (int) (CONST_MAX_OFFSET_PERCENTAGE / 100 * w);

            var rng = new Random();

            var outputImg = inputImg.Clone();
            outputImg.Mutate(x => x.Crop(w - wMaxOffset, h - hMaxOffset));
            for (var i = 0; i < CONST_FRAMES; i++)
            {
                var clone = inputImg.Clone();

                // offset
                var xshift = rng.Next(0, hMaxOffset);
                var yshift = rng.Next(0, wMaxOffset);
                clone.Mutate(x => x.Crop(new Rectangle(xshift, yshift, w - wMaxOffset, h - hMaxOffset)));

                // color overlay
                var randomColor = Rgba32.Transparent;
                randomColor.R = (byte)rng.Next(0, 255);
                randomColor.G = (byte)rng.Next(0, 255);
                randomColor.B = (byte)rng.Next(0, 255);
                randomColor.A = 150;
                clone.Mutate(x => x.Fill(randomColor));
                
                // add to animation
                var frame = outputImg.Frames.AddFrame(clone.Frames.RootFrame);
                frame.MetaData.FrameDelay = CONST_FRAMEDELAY;
            }

            outputImg.Frames.RootFrame.MetaData.FrameDelay = CONST_FRAMEDELAY;
            outputImg.SaveAsGif(pOutputStream, new GifEncoder());
        }

        public static void MoreJPEG(Stream pImageStream, Stream pOutputStream)
        {
            const int CONST_QUALITY = 2;

            var inputImg = Image.Load(pImageStream);
            inputImg.SaveAsJpeg(pOutputStream, new JpegEncoder { Quality = CONST_QUALITY });
        }
    }
}
