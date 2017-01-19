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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Cha0sImageLib;
using CommandLine;

namespace Cha0sImageManipulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // parse arguments
            Parser.Default.ParseArguments<Options>(args).WithParsed(pOptions =>
            {
                // succesfully parsed, print header
                var verInf = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                Console.Error.WriteLine($"{verInf.ProductName} {verInf.ProductVersion}\r\n{verInf.LegalCopyright}\r\n");

                // get input stream
                Stream inputStream;
                if (!string.IsNullOrWhiteSpace(pOptions.InputFile))
                    inputStream = File.OpenRead(pOptions.InputFile);
                else
                    inputStream = Console.OpenStandardInput();

                // get output stream
                Stream outputStream;
                if (!string.IsNullOrWhiteSpace(pOptions.OutputFile))
                    outputStream = File.OpenWrite(pOptions.OutputFile);
                else
                    outputStream = Console.OpenStandardOutput();

                // do image manipulation
                if (pOptions.FilterIntensify)
                    ImageManipulators.Intensifier(inputStream, outputStream);
                else if (pOptions.FilterNeedsMoreJPEG)
                    ImageManipulators.MoreJPEG(inputStream, outputStream);
                else
                    Console.Out.WriteLine("No manipulation option specified. Please use the '--help' parameter for help.");
            }).WithNotParsed(pOptions =>
            {
                // error during parsing
                Environment.Exit(1);
            });
        }
    }
}
