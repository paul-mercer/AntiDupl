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
using System.Windows.Forms;
using System.Drawing;

namespace AntiDupl.NET
{
    /// <summary>
    /// Панель превью изображения.
    /// </summary>
    public class ThumbnailPreview : Panel
    {
        private readonly CoreLib m_core;
        private readonly Options m_options;
        private readonly MainSplitContainer m_mainSplitContainer;
        public CoreGroup Group { get; private set; } = null;
        public int Index { get; private set; } = 0;
        public CoreImageInfo ImageInfo { get { return Group.images[Index]; } }

        private PictureBoxPanel m_pictureBoxPanel;
        
        public ThumbnailPreview(CoreLib core, Options options, MainSplitContainer mainSplitContainer)
        {
            m_core = core;
            m_options = options;
            m_mainSplitContainer = mainSplitContainer;
            InnitializeComponents();
            //InnitializeTestButton();
        }
        
        private void InnitializeComponents()
        {
            Location = new Point(0, 0);
            Dock = DockStyle.Fill;

            m_pictureBoxPanel = new PictureBoxPanel(m_core, m_options);

            Controls.Add(m_pictureBoxPanel);
        }

        private void InnitializeTestButton()
        {
            var testButton = new Button
            {
                Text = "Test",
                Location = new Point(10, 10),
                AutoSize = true
            };
            testButton.Click += (sender, e) =>
            {
                m_mainSplitContainer.UpdateResults();
            };
            Controls.Add(testButton);
        }

        public void SetThumbnail(CoreGroup group, int index)
        {
            Group = group;
            Index = index;
            m_pictureBoxPanel.UpdateImage(ImageInfo);
            m_pictureBoxPanel.UpdateImagePadding(Group.sizeMax);
            m_pictureBoxPanel.Refresh();
        }
    }
}
