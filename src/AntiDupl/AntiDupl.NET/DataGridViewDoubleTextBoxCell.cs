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
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AntiDupl.NET
{
    public class DataGridViewDoubleTextBoxCell : DataGridViewTextBoxCell
    {
        const int PADDING = 3;
        const int SEPARATOR_WIDTH = 1;
        const int LEFT_INTEND = 1;
        const int TOP_INTEND = 2;
        const int RIGHT_INTEND = 1;

        public Color separatorColor { get; set; } = Color.LightGray;
        public Color markerColor { get; set; } = Color.Red;

        public enum MarkType
        {
            None,
            First,
            Second,
            Both
        }
        public MarkType markType { get; set; } = MarkType.None;


        private object m_first;
        private object m_second;

        public DataGridViewDoubleTextBoxCell(object first, object second)
        {
            m_first = first;
            m_second = second;
        }

        override protected void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
          int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue,
          string errorText, DataGridViewCellStyle cellStyle,
          DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,
              value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            Color ordinaryColor, markColor, firstColor, secondColor;

            if ((cellState & DataGridViewElementStates.Selected) != 0)
            {
                ordinaryColor = cellStyle.SelectionForeColor;
                markColor = markerColor;
            }
            else
            {
                ordinaryColor = cellStyle.ForeColor;
                markColor = markerColor;
            }

            switch (markType)
            {
                case MarkType.None:
                    firstColor = ordinaryColor;
                    secondColor = ordinaryColor;
                    break;
                case MarkType.First:
                    firstColor = markColor;
                    secondColor = ordinaryColor;
                    break;
                case MarkType.Second:
                    firstColor = ordinaryColor;
                    secondColor = markColor;
                    break;
                case MarkType.Both:
                    firstColor = markColor;
                    secondColor = markColor;
                    break;
                default:
                    firstColor = ordinaryColor;
                    secondColor = ordinaryColor;
                    break;
            }

            var separatorPen = new Pen(separatorColor);
            var separatorX = (cellBounds.Top + cellBounds.Bottom) / 2;
            var format = new StringFormat
            {
                Trimming = StringTrimming.EllipsisCharacter
            };
            format.FormatFlags |= StringFormatFlags.NoWrap;

            switch (cellStyle.Alignment)
            {
                case DataGridViewContentAlignment.BottomCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case DataGridViewContentAlignment.BottomLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case DataGridViewContentAlignment.BottomRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    break;
                case DataGridViewContentAlignment.MiddleCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case DataGridViewContentAlignment.MiddleLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case DataGridViewContentAlignment.MiddleRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case DataGridViewContentAlignment.NotSet:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case DataGridViewContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case DataGridViewContentAlignment.TopLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case DataGridViewContentAlignment.TopRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    break;
            }

            var firstBounds = new Rectangle(
              cellBounds.Left + LEFT_INTEND, cellBounds.Top + TOP_INTEND,
              cellBounds.Width - LEFT_INTEND - RIGHT_INTEND, cellBounds.Height / 2 - TOP_INTEND);
            graphics.DrawString(m_first.ToString(), cellStyle.Font, new SolidBrush(firstColor),
              firstBounds, format);

            var secondBounds = new Rectangle(
              cellBounds.Left + LEFT_INTEND, separatorX + SEPARATOR_WIDTH + TOP_INTEND,
              cellBounds.Width - LEFT_INTEND - RIGHT_INTEND, cellBounds.Height / 2 - TOP_INTEND);
            graphics.DrawString(m_second.ToString(), cellStyle.Font, new SolidBrush(secondColor),
              secondBounds, format);

            graphics.DrawLine(separatorPen, cellBounds.Left, separatorX, cellBounds.Right - 2, separatorX);

            if (GetPreferredSize(graphics, cellStyle, rowIndex, new Size(0, 0)).Width > cellBounds.Width)
            {
                var builder = new StringBuilder();
                builder.AppendLine(m_first.ToString());
                builder.Append(m_second.ToString());
                ToolTipText = builder.ToString();
            }
            else
            {
                ToolTipText = "";
            }
        }

        override protected Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle,
            int rowIndex, Size constraintSize)
        {
            Size preferedSize = new Size(0, 0), firstSize = new Size(0, 0), secondSize = new Size(0, 0);
            if (m_first != null)
            {
                firstSize.Height += cellStyle.Padding.Top;
                firstSize = MeasureTextSize(graphics, m_first.ToString(), cellStyle.Font,
                  TextFormatFlags.TextBoxControl | TextFormatFlags.HorizontalCenter);
                firstSize.Height += cellStyle.Padding.Bottom;
            }
            if (m_second != null)
            {
                secondSize.Height += cellStyle.Padding.Top;
                secondSize += MeasureTextSize(graphics, m_second.ToString(), cellStyle.Font,
                  TextFormatFlags.TextBoxControl | TextFormatFlags.HorizontalCenter);
                secondSize.Height += cellStyle.Padding.Bottom;
            }
            preferedSize.Height = firstSize.Height + secondSize.Height + SEPARATOR_WIDTH + 4 * PADDING;
            preferedSize.Width = firstSize.Width > secondSize.Width ? firstSize.Width : secondSize.Width + 2 * PADDING;
            return preferedSize;
        }
    }
}
