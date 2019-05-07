﻿/*
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
using System.Text;

namespace AntiDupl.NET
{
    public class CoreVersion
    {
        public int major;
        public int minor;
        public int release;
        public int revision;

        public CoreVersion(sbyte[] buffer)
        {
            var versions = Parse(buffer).Split('.');
            major = versions.Length > 0 ? Convert.ToInt32(versions[0]) : -1;
            minor = versions.Length > 1 ? Convert.ToInt32(versions[1]) : -1;
            release = versions.Length > 2 ? Convert.ToInt32(versions[2]) : -1;
            revision = versions.Length > 3 ? Convert.ToInt32(versions[3]) : -1;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            var already = false;
            if (major >= 0)
            {
                builder.Append(major.ToString());
                already = true;
            }
            if (minor >= 0)
            {
                if(already)
                    builder.Append(".");
                builder.Append(minor.ToString());
                already = true;
            }
            if (release >= 0)
            {
                if (already)
                    builder.Append(".");
                builder.Append(release.ToString());
                already = true;
            }
            if(revision >= 0)
            {
                if (already)
                    builder.Append(".");
                builder.Append(revision.ToString());
            }
            return builder.ToString();
        }

        private string Parse(sbyte[] buffer)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < buffer.Length; ++i)
            {
                if(buffer[i] == 0)
                    break;
                builder.Append(Convert.ToChar(buffer[i]));
            }
            return builder.ToString();
        }
    }
}
