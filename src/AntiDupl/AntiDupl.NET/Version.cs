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
using System.IO;
using System.Xml.Serialization;

namespace AntiDupl.NET
{
    public class Version
    {
        public int major;
        public int minor;
        public int release;

        public Version()
        {
            var versions = External.Version.Split('.');
            major = Convert.ToInt32(versions[0]);
            minor = Convert.ToInt32(versions[1]);
            release = Convert.ToInt32(versions[2]);
        }

        static public Version LoadXml(Stream stream)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(Version));
                var version = (Version)xmlSerializer.Deserialize(stream);
                return version;
            }
            catch
            {
                return null;
            }
        }

        public void SaveXml(string fileName)
        {
            try
            {
                TextWriter writer = new StreamWriter(fileName);
                var xmlSerializer = new XmlSerializer(typeof(Version));
                xmlSerializer.Serialize(writer, this);
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(major.ToString());
            builder.Append(".");
            builder.Append(minor.ToString());
            builder.Append(".");
            builder.Append(release.ToString());
            return builder.ToString();
        }

        static public int Compare(Version v1, Version v2)
        {
            if (v1.major == v2.major)
            {
                if (v1.minor == v2.minor)
                {
                    return v1.release - v2.release;
                }
                else
                    return v1.minor - v2.minor;
            }
            else
                return v1.major - v2.major;
        }

        static public bool Compatible(CoreVersion coreVersion)
        {
            var version = new Version();
            return
                version.major == coreVersion.major &&
                version.minor == coreVersion.minor &&
                version.release == coreVersion.release;
        }
    }
}
