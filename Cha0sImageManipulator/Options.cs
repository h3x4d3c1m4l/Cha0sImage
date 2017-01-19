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
using CommandLine;

namespace Cha0sImageManipulator
{
    class Options
    {
        [Option('i', "input", HelpText = "Input image to be processed (if omitted standard input will be used).")]
        public string InputFile { get; set; }

        [Option('o', "output", HelpText = "Output image (if omitted standard output will be used).")]
        public string OutputFile { get; set; }

        [Option("fi", HelpText = "Intensify the image. Will return a GIF image.", SetName = "Filters", Required = false)]
        public bool FilterIntensify { get; set; }

        [Option("fj", HelpText = "Use this if your image needs more JPEG. Will return a JPEG image.", SetName = "Filters", Required = false)]
        public bool FilterNeedsMoreJPEG { get; set; }
    }
}