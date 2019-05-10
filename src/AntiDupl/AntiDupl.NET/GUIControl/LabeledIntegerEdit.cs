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
using System.Windows.Forms;

namespace AntiDupl.NET
{
    public class LabeledIntegerEdit : TableLayoutPanel
    {
        private readonly TextBox m_textBox;
        private readonly Label m_label;
        private int m_value = 0;
        private int m_min = int.MinValue;
        private int m_max = int.MaxValue;
        private int m_default = 0;
        private readonly EventHandler m_valueChangedHandler;

        public override string Text { get { return m_label.Text; } set { m_label.Text = value; } }

        public int Value
        {
            get { return m_value; }
            set
            {
                if (value > m_max || value < m_min)
                {
                    m_value = m_default;
                }
                else
                {
                    m_value = value;
                }
                m_textBox.Text = m_value.ToString();
            }
        }

        public int Min
        {
            get { return m_min; }
            set
            {
                m_min = value;
                m_max = Math.Max(m_max, m_min);
                m_default = Math.Max(m_default, m_min);
                m_value = Math.Max(m_value, m_min);
                m_textBox.Text = m_value.ToString();
            }
        }

        public int Max
        {
            get { return m_max; }
            set
            {
                m_max = value;
                m_min = Math.Min(m_min, m_max);
                m_default = Math.Min(m_default, m_max);
                m_value = Math.Min(m_value, m_max);
                m_textBox.Text = m_value.ToString();
            }
        }

        public int Default
        {
            get { return m_default; }
            set
            {
                m_default = value;
                m_min = Math.Min(m_min, m_default);
                m_max = Math.Max(m_default, m_max);
            }
        }

        public LabeledIntegerEdit(int textBoxWidth, int textBoxHeight, EventHandler valueChangedHandler)
        {
            m_valueChangedHandler = valueChangedHandler;

            Location = new System.Drawing.Point(0, 0);
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 1;

            m_textBox = new TextBox
            {
                Size = new System.Drawing.Size(textBoxWidth, textBoxHeight),
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0),
                Multiline = false
            };
            m_textBox.KeyUp += new KeyEventHandler(OnTextBoxKeyUp);
            Controls.Add(m_textBox, 0, 0);

            m_label = new Label
            {
                AutoSize = true,
                Padding = new Padding(0, 5, 5, 5)
            };
            Controls.Add(m_label, 1, 0);
        }

        private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            var old = m_value;

            try
            {
                m_value = Convert.ToInt32(m_textBox.Text);
                if (m_value > m_max || m_value < m_min)
                {
                    m_value = old;
                }
            }
            catch (Exception)
            {
                m_value = old;
            }

            if (old != m_value && m_valueChangedHandler != null)
            {
                m_valueChangedHandler(sender, e);
            }

            m_textBox.Text = m_value.ToString();
        }
    }
}
