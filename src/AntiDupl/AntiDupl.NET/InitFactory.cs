/*
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
using System;
using System.Windows.Forms;

namespace AntiDupl.NET
{
    /// <summary>
    /// ������� ������� �������� GUI
    /// </summary>
    public static class InitFactory
    {
        public static class Layout
        {
            public static TableLayoutPanel Create(int columCount, int rowCount)
            {
                var layout = new TableLayoutPanel
                {
                    Location = new System.Drawing.Point(0, 0),
                    Dock = DockStyle.Fill,
                    ColumnCount = columCount,
                    RowCount = rowCount
                };
                return layout;
            }

            public static TableLayoutPanel Create(int columCount, int rowCount, int padding)
            {
                TableLayoutPanel layout = Create(columCount, rowCount);
                layout.Padding = new Padding(padding);
                return layout;
            }

            public static TableLayoutPanel Create(int columCount, int rowCount, int padding, int margin)
            {
                TableLayoutPanel layout = Create(columCount, rowCount, padding);
                layout.Margin = new Padding(margin);
                return layout;
            }

            public static TableLayoutPanel CreateVerticalCenterAlign(int padding, int margin)
            {
                TableLayoutPanel layout = Create(1, 3, padding, margin);
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                return layout;
            }

            public static TableLayoutPanel CreateHorizontalCenterAlign(int padding, int margin)
            {
                TableLayoutPanel layout = Create(3, 1, padding, margin);
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                return layout;
            }

            public static TableLayoutPanel CreateVerticalCompensatedCenterAlign(int first, int second)
            {
                TableLayoutPanel layout = Create(1, 5, 0, 0);
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, first));
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, second));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                return layout;
            }

            public static TableLayoutPanel CreateHorizontalCompensatedCenterAlign(int first, int second)
            {
                TableLayoutPanel layout = Create(5, 1, 0, 0);
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, first));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, second));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                return layout;
            }
        };

        public static class Font
        {
            public static System.Drawing.Font Create(float size)
            {
                var font = new System.Drawing.Font("Microsoft Sans Serif", size,
                  System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                return font;
            }
        };

        public static class Label
        {
            public static System.Windows.Forms.Label Create()
            {
                var label = new System.Windows.Forms.Label
                {
                    AutoSize = true
                };
                return label;
            }

            public static System.Windows.Forms.Label Create(float fontSize)
            {
                System.Windows.Forms.Label label = Create();
                label.Font = Font.Create(fontSize);
                return label;
            }

            public static System.Windows.Forms.Label Create(string text, System.Drawing.Font font)
            {
                var label = new System.Windows.Forms.Label
                {
                    AutoSize = true,
                    Font = font,
                    Text = text
                };
                return label;
            }
        };

        public static class CheckBox
        {
            public static System.Windows.Forms.CheckBox Create(EventHandler checkedChanged)
            {
                var checkBox = new System.Windows.Forms.CheckBox();
                checkBox.CheckedChanged += new EventHandler(checkedChanged);
                checkBox.AutoSize = true;
                return checkBox;
            }
        };

        public static class ListBox
        {
            public static System.Windows.Forms.ListBox Create(EventHandler selectedIndexChanged, EventHandler doubleClick)
            {
                var listBox = new System.Windows.Forms.ListBox
                {
                    Location = new System.Drawing.Point(0, 0),
                    Dock = DockStyle.Fill
                };
                listBox.SelectedIndexChanged += new EventHandler(selectedIndexChanged);
                listBox.DoubleClick += new EventHandler(doubleClick);
                return listBox;
            }
        };

        public static class CheckedListBox
        {
            public static System.Windows.Forms.CheckedListBox Create(EventHandler selectedIndexChanged, EventHandler doubleClick, ItemCheckEventHandler itemCheck)
            {
                var checkBox = new System.Windows.Forms.CheckedListBox
                {
                    Location = new System.Drawing.Point(0, 0),
                    Dock = DockStyle.Fill
                };
                checkBox.SelectedIndexChanged += new EventHandler(selectedIndexChanged);
                checkBox.DoubleClick += new EventHandler(doubleClick);
                checkBox.ItemCheck += new ItemCheckEventHandler(itemCheck);
                return checkBox;
            }
        };

        public static class MenuItem
        {
            public static ToolStripMenuItem Create(string image, object tag, EventHandler handler)
            {
                var menuItem = new ToolStripMenuItem();
                if (image != null)
                {
                    menuItem.Image = Resources.Images.Get(image);
                    menuItem.ImageScaling = ToolStripItemImageScaling.None;
                }
                menuItem.Tag = tag;
                menuItem.Click += new EventHandler(handler);
                return menuItem;
            }

            public static ToolStripMenuItem Create(string image, object tag, EventHandler handler, bool checkedValue)
            {
                ToolStripMenuItem menuItem = Create(image, tag, handler);
                menuItem.CheckOnClick = true;
                menuItem.Checked = checkedValue;
                return menuItem;
            }
        };

        public static class ToolButton
        {
            public static ToolStripButton Create(string image, object tag, EventHandler handler)
            {
                var toolButton = new ToolStripButton();
                if (image != null)
                {
                    toolButton.Image = Resources.Images.Get(image);
                    toolButton.ImageScaling = ToolStripItemImageScaling.None;
                }
                toolButton.Overflow = ToolStripItemOverflow.Never;
                toolButton.Margin = new Padding(1);
                toolButton.Padding = new Padding(1);
                toolButton.Tag = tag;
                toolButton.Click += new EventHandler(handler);
                return toolButton;
            }
        };
    };
}
