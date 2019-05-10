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
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

namespace AntiDupl.NET
{
    public class DynamicModule : IDisposable
    {
        public class DynamicModuleApiAttribute : Attribute
        {
        }

        public DynamicModule(string fileName)
        {
            FileName = fileName;

            if (string.IsNullOrEmpty(FileName))
                throw new Exception(string.Format("Bad library file name '{0}'!", FileName));

            try
            {
                m_module = LoadLibrary(FileName);
                if (m_module == IntPtr.Zero)
                    throw new Exception(string.Format("Can't load {0} dynamic library!", FileName));

                FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                for (var i = 0; i < fields.Length; ++i)
                {
                    FieldInfo field = fields[i];
                    try
                    {
                        var attributes = field.GetCustomAttributes(typeof(DynamicModuleApiAttribute), false);
                        if (attributes.Length > 0)
                        {
                            IntPtr address = GetProcAddress(m_module, field.Name);
                            Delegate delegate_ = Marshal.GetDelegateForFunctionPointer(address, field.FieldType);
                            field.SetValue(this, delegate_);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.ToString());
                    }
                }
            }
            catch
            {
                throw new Exception(string.Format("Can't load {0} dynamic library!", FileName));
            }
        }

        ~DynamicModule()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (m_module != IntPtr.Zero)
            {
                FreeLibrary(m_module);
                m_module = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        public string FileName { get; }

        /************************************ Private Members: ************************************/

        private IntPtr m_module = IntPtr.Zero;

        [DllImport("kernel32.dll",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Winapi,
            EntryPoint = "LoadLibraryA")]
        private static extern IntPtr LoadLibrary(string moduleName);

        [DllImport("kernel32.dll",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Winapi,
            EntryPoint = "FreeLibrary")]
        private static extern int FreeLibrary(IntPtr module);


        [DllImport("kernel32.dll",
            CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Winapi,
            EntryPoint = "GetProcAddress")]
        private static extern IntPtr GetProcAddress(IntPtr module, string functionName);
    }
}
