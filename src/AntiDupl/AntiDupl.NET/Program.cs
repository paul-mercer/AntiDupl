/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy 
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/
using System;
using System.Windows.Forms;
using System.IO;

namespace AntiDupl.NET
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string customSavePath = null;
            if (GetParameter(args, "-s", ref customSavePath))
            {
                var directoryInfo = new DirectoryInfo(customSavePath);
                if (!directoryInfo.Exists)
                    throw new Exception(string.Format("The directory '{0}' is not exists!", customSavePath));
                Resources.UserPath = customSavePath;
            }
            else
            {
                Resources.UserPath = Resources.GetDefaultUserPath();
            }
            Resources.Strings.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
               
        static bool GetParameter(string[] args, string name, ref string value)
        {
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (string.Compare(args[i], name) == 0)
                {
                    value = args[i + 1];
                    return true;
                }
            }
            return false;
        }
    }
}