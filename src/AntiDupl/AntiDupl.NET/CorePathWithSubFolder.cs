﻿/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar, 2013-2018 Borisov Dmitry.
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

namespace AntiDupl.NET
{
    public class CorePathWithSubFolder
    {
        public bool enableSubFolder;
        public string path;
         
        public CorePathWithSubFolder()
        {
        }

        public CorePathWithSubFolder(string path, bool enableSubFolder)
        {
            this.path = path;
            this.enableSubFolder = enableSubFolder;
        }

        public CorePathWithSubFolder(CorePathWithSubFolder pathWithSubFolder)
        {
            enableSubFolder = pathWithSubFolder.enableSubFolder;
            path = pathWithSubFolder.path;
        }

        public CorePathWithSubFolder(ref CoreDll.adPathWithSubFolderW pathWithSubFolder)
        {
            enableSubFolder = pathWithSubFolder.enableSubFolder != CoreDll.FALSE;
            path = pathWithSubFolder.path;
        }

        public void ConvertTo(ref CoreDll.adPathWithSubFolderW pathWithSubFolder)
        {
            pathWithSubFolder.enableSubFolder = enableSubFolder ? CoreDll.TRUE : CoreDll.FALSE;
            pathWithSubFolder.path = path;
        }

        public CorePathWithSubFolder Clone()
        {
            return new CorePathWithSubFolder(this);
        }

        public bool Equals(CorePathWithSubFolder pathWithSubFolder)
        {
            return
                enableSubFolder == pathWithSubFolder.enableSubFolder &&
                path == pathWithSubFolder.path;
        }
    }
}
