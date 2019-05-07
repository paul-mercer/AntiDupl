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
using System.Drawing;

namespace AntiDupl.NET
{
    /// <summary>
    /// Панель содержит панели изображений одной группы.
    /// </summary>
    public class ThumbnailGroupPanel : RaisedPanel
    {
        private readonly CoreLib m_core;
        private CoreGroup m_group;
        private readonly Options m_options;
        public ThumbnailPanel[] ThumbnailPanels { get; private set; }
        public ThumbnailGroupTable Table { get; }

        public ThumbnailGroupPanel(CoreLib core, Options options, CoreGroup group, ThumbnailGroupTable thumbnailGroupTable)
        {
            m_core = core;
            m_options = options;
            m_group = group;
            Table = thumbnailGroupTable;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;

            var width = 0;
            var height = 0;
            ThumbnailPanels = new ThumbnailPanel[m_group.images.Length];
            for (var i = 0; i < m_group.images.Length; ++i)
            {
                ThumbnailPanels[i] = new ThumbnailPanel(m_core, m_options, m_group, i, this);
                ThumbnailPanels[i].Location = new Point(Padding.Left + ThumbnailPanels[i].Margin.Left + (ThumbnailPanels[i].Width + ThumbnailPanels[i].Margin.Horizontal)*i,
                    Padding.Top + ThumbnailPanels[i].Margin.Top);

                width += ThumbnailPanels[i].Width + ThumbnailPanels[i].Padding.Horizontal + Margin.Horizontal;
                height = Math.Max(height, ThumbnailPanels[i].Height + ThumbnailPanels[i].Padding.Vertical + Margin.Vertical);
            }
            ClientSize = new Size(width, height);

            Controls.AddRange(ThumbnailPanels);
        }
    }
}
