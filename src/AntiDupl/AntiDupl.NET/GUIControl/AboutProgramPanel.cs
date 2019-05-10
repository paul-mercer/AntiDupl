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
    public class AboutProgramPanel : Panel
    {
        private const int LOGO_SIZE = 32;

        private readonly CoreLib m_core;

        public AboutProgramPanel(CoreLib core)
        {
            m_core = core;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            TableLayoutPanel layout = InitFactory.Layout.Create(1, 4, 5, 0);
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.AutoSize = true;

            layout.Controls.Add(CreateLogotype(new Font(Font.FontFamily, Font.Size * 2.0f)), 0, 0);

            layout.Controls.Add(GetCoopyrightLabel(0), 0, 1);

            layout.Controls.Add(GetCoopyrightLabel(1), 0, 2);

            layout.Controls.Add(CreateInfoTable(Font), 0, 3);

            Controls.Add(layout);
        }

        private Label GetCoopyrightLabel(int index)
        {
            string text = null;
            switch(index)
            {
                case 0: text = Resources.Strings.Current.AboutProgramPanel_CopyrightLabel0_Text; break;
                case 1: text = Resources.Strings.Current.AboutProgramPanel_CopyrightLabel1_Text; break;
            }
            Label label = CreateLabel(text, new Font(Font.FontFamily, Font.Size * 1.2f));
            label.Margin = new Padding(0, (index == 0 ? 10 : 0), 0, (index == 1 ? 10 : 0));
            return label;
        }

        private TableLayoutPanel CreateLogotype(Font font)
        {
            TableLayoutPanel layout = InitFactory.Layout.Create(4, 1, 0, 0);
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.AutoSize = true;

            var bitmap = Resources.Icons.Get(new Size(LOGO_SIZE, LOGO_SIZE)).ToBitmap();
            var pictureBox = new PictureBox
            {
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                Margin = new Padding(0),
                ClientSize = bitmap.Size,
                Image = bitmap
            };
            layout.Controls.Add(pictureBox, 1, 0);

            layout.Controls.Add(CreateLinkLabel(Application.ProductName, Resources.WebLinks.GithubComAntidupl, font), 2, 0);

            return layout;
        }

        private Control CreateInfoTable(Font font)
        {
            TableLayoutPanel table = InitFactory.Layout.Create(2, 4, 0, 0);
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            table.Dock = DockStyle.Top;
            table.AutoSize = true;

            table.Controls.Add(CreateLabel(Resources.Strings.Current.AboutProgramPanel_ComponentLabel_Text, 
                new Font(font, FontStyle.Bold)), 0, 0);
            table.Controls.Add(CreateLabel(Resources.Strings.Current.AboutProgramPanel_VersionLabel_Text, 
                new Font(font, FontStyle.Bold)), 1, 0);

            table.Controls.Add(CreateLinkLabel(Application.ProductName, Resources.WebLinks.GithubComAntidupl, font), 0, 1);
            table.Controls.Add(CreateLabel(m_core.GetVersion(CoreDll.VersionType.AntiDupl).ToString(), font), 1, 1);

            table.Controls.Add(CreateLinkLabel("Simd", Resources.WebLinks.GithubComSimd, font), 0, 2);
            table.Controls.Add(CreateLabel(m_core.GetVersion(CoreDll.VersionType.Simd).ToString(), font), 1, 2);

            table.Controls.Add(CreateLinkLabel("OpenJPEG", Resources.WebLinks.OpenjpegOrg, font), 0, 3);
            table.Controls.Add(CreateLabel(m_core.GetVersion(CoreDll.VersionType.OpenJpeg).ToString(), font), 1, 3);

            return table;
        }

        private Label CreateLabel(string text, Font font)
        {
            var label = new Label
            {
                AutoSize = true,
                Padding = new Padding(2),
                Font = font,
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            return label;
        }

        private LinkLabel CreateLinkLabel(string text, string link, Font font)
        {
            var linkLabel = new LinkLabel
            {
                AutoSize = true,
                Font = font,
                Text = text,
                Dock = DockStyle.Fill,
                LinkBehavior = LinkBehavior.HoverUnderline,
                Padding = new Padding(2)
            };
            linkLabel.Links.Add(new LinkLabel.Link(0, text.Length, link));
            linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(OnLinkLabelLinkClicked);
            linkLabel.TextAlign = ContentAlignment.MiddleCenter;

            var toolTip = new ToolTip
            {
                ShowAlways = true
            };
            toolTip.SetToolTip(linkLabel, link);

            return linkLabel;
        }

        private void OnLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var linkLabel = (LinkLabel)sender;
            var linkIndex = linkLabel.Links.IndexOf(e.Link);
            LinkLabel.Link link = linkLabel.Links[linkIndex];
            link.Visited = true;
            System.Diagnostics.Process.Start(link.LinkData.ToString());
        }
    }
}
