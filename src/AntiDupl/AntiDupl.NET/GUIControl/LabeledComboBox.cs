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

namespace AntiDupl.NET
{
    public class LabeledComboBox : TableLayoutPanel
    {
        private Label m_label;

        public ComboBox comboBox { get; }
        public override string Text { get { return m_label.Text; } set { m_label.Text = value; } }

        public LabeledComboBox(int comboBoxWidth, int comboBoxHeight, EventHandler selectedIndexChanged)
        {
            Location = new System.Drawing.Point(0, 0);
            AutoSize = true;
            ColumnCount = 2;
            RowCount = 1;

            comboBox = new ComboBox
            {
                Size = new System.Drawing.Size(comboBoxWidth, comboBoxHeight),
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.SelectedIndexChanged += new EventHandler(selectedIndexChanged);
            Controls.Add(comboBox, 0, 0);

            m_label = InitFactory.Label.Create();
            m_label.Padding = new Padding(0, 5, 5, 5);
            Controls.Add(m_label, 1, 0);
        }

        public class Value : object
        {
            public int value;
            public string description;

            public Value(int val, string desc)
            {
                value = val;
                description = desc;
            }

            public override string ToString()
            {
                return description;
            }
        }

        public int SelectedValue
        {
            get
            {
                var val = (Value)comboBox.SelectedItem;
                return val.value;
            }
            set
            {
                var index = -1;
                var difference = int.MaxValue;
                for (var i = 0; i < comboBox.Items.Count; i++)
                {
                    var current = (Value)comboBox.Items[i];
                    if (Math.Abs(current.value - value) < difference)
                    {
                        difference = Math.Abs(current.value - value);
                        index = i;
                    }
                }
                comboBox.SelectedIndex = index;
            }
        }

        public void SetDescription(int index, string description)
        {
            var value = (Value)comboBox.Items[index];
            value.description = description;
        }
    }
}
