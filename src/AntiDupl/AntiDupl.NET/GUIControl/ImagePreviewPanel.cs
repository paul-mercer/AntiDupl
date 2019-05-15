/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar, 2013-2015 Borisov Dmitry.
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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.IO;

namespace AntiDupl.NET
{
    public class ImagePreviewPanel : TableLayoutPanel
    {
        private const int MAX_PATH = 260;

        public enum Position
        {
            Left,
            Top,
            Right,
            Bottom
        }
        private Position m_position;
        /// <summary>
        /// ��������������� ������ ��� ������ ��������.
        /// </summary>
        public CoreDll.RenameCurrentType RenameCurrentType { get; private set; }

        private const int IBW = 1;//Internal border width
        private const int EBW = 2;//External border width

        private readonly CoreLib m_core;
        private readonly Options m_options;
        private readonly ResultsListView m_resultsListView;
        /// <summary>
        /// ������ ����������.
        /// </summary>
        public int Group { get; private set; }

        private CoreImageInfo m_currentImageInfo;
        public CoreImageInfo CurrentImageInfo { get { return m_currentImageInfo; } }
        private CoreImageInfo m_neighbourImageInfo;
        public CoreImageInfo NeighbourImageInfo { get { return m_neighbourImageInfo; } }
        
        private PictureBoxPanel m_pictureBoxPanel;
        private Label m_fileSizeLabel;
        private Label m_imageSizeLabel;
        private Label m_imageTypeLabel;
        private Label m_imageBlocknessLabel;
        private Label m_imageBlurringLabel;
        private Label m_imageExifLabel;
        private Label m_pathLabel;
        private ToolTip m_toolTip;

        public ImagePreviewPanel(CoreLib core, Options options, ResultsListView resultsListView, Position position)
        {
            m_core = core;
            m_options = options;
            m_resultsListView = resultsListView;
            InitializeComponents();
            SetPosition(position);
        }
        
        // ����������� ������������� ���� ��� ��� �������� �����.
        private void InitializeComponents()
        {
            Strings s = Resources.Strings.Current;

            Location = new Point(0, 0);
            Margin = new Padding(0);
            Padding = new Padding(0);
            Dock = DockStyle.Fill;
            
            ColumnCount = 1;
            RowCount = 2;

            m_pictureBoxPanel = new PictureBoxPanel(m_core, m_options)
            {
                ContextMenuStrip = new ImagePreviewContextMenu(m_core, m_options, m_resultsListView.CoreOptions, this, m_resultsListView)
            };

            m_fileSizeLabel = new Label
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true
            };

            m_imageSizeLabel = new Label
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                Margin = new Padding(IBW, 0, 0, 0),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true
            };

            m_imageBlocknessLabel = new Label
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                Margin = new Padding(IBW, 0, 0, 0),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true
            };

            m_imageBlurringLabel = new Label
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                Margin = new Padding(IBW, 0, 0, 0),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true
            };

            m_imageTypeLabel = new Label
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                Margin = new Padding(IBW, 0, 0, 0),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true
            };

            m_imageExifLabel = new Label
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                Margin = new Padding(IBW, 0, 0, 0),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = true,
                Text = s.ImagePreviewPanel_EXIF_Text,
                Visible = false
            };

            m_pathLabel = new Label
            {
                Location = new Point(0, 0),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(1, 3, 1, 0),
                AutoEllipsis = true
            };
            m_pathLabel.DoubleClick += new EventHandler(RenameImage);

            m_toolTip = new ToolTip
            {
                ShowAlways = true
            };
            m_toolTip.SetToolTip(m_imageBlocknessLabel, s.ResultsListView_Blockiness_Column_Text);
            m_toolTip.SetToolTip(m_imageBlurringLabel, s.ResultsListView_Blurring_Column_Text);
            // �������� AutomaticDelay ��������� ���������� ���� �������� ��������, ������� ����� ������������ ��� ��������� �������� �������AutoPopDelay, InitialDelay � ReshowDelay. ������ ��� ��� ��������� �������� AutomaticDelay ��������������� ��������� �������� �� ���������.
            //m_toolTip.AutomaticDelay = 500;
            // �������� �������, � �������������, � ������� �������� ��������� ���� ������ ���������� � �������� �������� ����������, ������ ��� �������� ���� ����������� ���������.
            // ����� �������� �������� AutomaticDelay. 
            m_toolTip.InitialDelay = 500;
            // �������� ��� ������ �������� �������, ������� ������ ������ ����� ���������� ���� ��������� ����������� ��������� ��� ����������� ��������� ���� � ������ �������� ���������� �� ������.
            // ���� ����� �������� �������� AutomaticDelay. 
            m_toolTip.ReshowDelay = 1;
            // ������ �������, � �������������, ToolTip �������� ��������, ����� ��������� ��������� �� �������� ����������. �������� �� ��������� - 5000. 
            // � ������ ��� ������, ��� �������� �������� AutomaticDelay. 
            // you cannot set the AutoPopDelay time higher than an Int16.MaxValue (i.e. 32767) and have it working. Using the tooltip Show() method leads to the same result. Any value higher than 32767 leads the timer to be reset to 5000ms.
            m_toolTip.AutoPopDelay = short.MaxValue;
        }

        /// <summary>
        /// Set information in image panel.
        /// ��������� ���������� � ������ �����������.
        /// </summary>
        private void SetImageInfo(CoreImageInfo currentImageInfo, CoreImageInfo neighbourImageInfo)
        {
            /*bool updateCurrent = UpdateImageInfo(ref m_currentImageInfo, currentImageInfo);
            bool updateNeighbour = UpdateImageInfo(ref m_neighbourImageInfo, neighbourImageInfo);*/
            var updateCurrent = true;
            var updateNeighbour = true;
            if (!m_options.resultsOptions.ShowNeighboursImages)
            {
                m_currentImageInfo = currentImageInfo;
                m_neighbourImageInfo = neighbourImageInfo;
            }
            else
            {
                updateCurrent = UpdateImageInfo(ref m_currentImageInfo, currentImageInfo);
                updateNeighbour = UpdateImageInfo(ref m_neighbourImageInfo, neighbourImageInfo);
            }
            if (updateCurrent)
            {
                m_pictureBoxPanel.UpdateImage(currentImageInfo);
                m_fileSizeLabel.Text = m_currentImageInfo.GetFileSizeString();
                m_imageSizeLabel.Text = m_currentImageInfo.GetImageSizeString();
                m_imageBlocknessLabel.Text = m_currentImageInfo.GetBlockinessString();
                m_imageBlurringLabel.Text = m_currentImageInfo.GetBlurringString();
                m_imageTypeLabel.Text = m_currentImageInfo.type == CoreDll.ImageType.None ? "   " : m_currentImageInfo.GetImageTypeString();
                if (currentImageInfo.exifInfo.isEmpty == CoreDll.FALSE)
                {
                    m_imageExifLabel.Visible = true;
                    SetExifTooltip(currentImageInfo);
                }
                else
                    m_imageExifLabel.Visible = false;
                m_pathLabel.Text = m_currentImageInfo.path;
                if (m_neighbourImageInfo != null) //��������� highlight
                {
                    m_imageSizeLabel.ForeColor =
                            m_currentImageInfo.height * m_currentImageInfo.width < m_neighbourImageInfo.height * m_neighbourImageInfo.width ?
                            Color.Red : DefaultForeColor;
                    m_imageTypeLabel.ForeColor = m_currentImageInfo.type != m_neighbourImageInfo.type ?
                            Color.Red : DefaultForeColor;
                    m_fileSizeLabel.ForeColor = m_currentImageInfo.size < m_neighbourImageInfo.size ?
                            Color.Red : DefaultForeColor;
                    m_imageBlocknessLabel.ForeColor = m_currentImageInfo.blockiness > m_neighbourImageInfo.blockiness ?
                            Color.Red : DefaultForeColor;
                    m_imageBlurringLabel.ForeColor = m_currentImageInfo.blurring > m_neighbourImageInfo.blurring ?
                            Color.Red : DefaultForeColor;
                    m_imageExifLabel.ForeColor = ExifEqual(m_currentImageInfo.exifInfo, m_neighbourImageInfo.exifInfo) ?
                        DefaultForeColor : Color.Red;
                }
            }
            else if (m_neighbourImageInfo != null)
            {
                m_imageSizeLabel.ForeColor = m_currentImageInfo.height * m_currentImageInfo.width < m_neighbourImageInfo.height * m_neighbourImageInfo.width ?
                        Color.Red : DefaultForeColor;
                m_imageTypeLabel.ForeColor = m_currentImageInfo.type != m_neighbourImageInfo.type ?
                        Color.Red : DefaultForeColor;
                m_fileSizeLabel.ForeColor = m_currentImageInfo.size < m_neighbourImageInfo.size ?
                        Color.Red : DefaultForeColor;
                m_imageBlocknessLabel.ForeColor = m_currentImageInfo.blockiness > m_neighbourImageInfo.blockiness ?
                        Color.Red : DefaultForeColor;
                m_imageBlurringLabel.ForeColor = m_currentImageInfo.blurring > m_neighbourImageInfo.blurring ?
                        Color.Red : DefaultForeColor;
                m_imageExifLabel.ForeColor = ExifEqual(m_currentImageInfo.exifInfo, m_neighbourImageInfo.exifInfo) ?
                    DefaultForeColor : Color.Red;
            }
            if (updateCurrent || updateNeighbour)
            {
                var neighbourSizeMax = new Size(0, 0);
                if(m_neighbourImageInfo != null)
                    neighbourSizeMax = new Size((int)m_neighbourImageInfo.width, (int)m_neighbourImageInfo.height);
                m_pictureBoxPanel.UpdateImagePadding(neighbourSizeMax);
                Refresh();
            }
        }

        /// <summary>
        /// ���������, ����� �� ���������� ������� ���������� �� �����������.
        /// </summary>
        static private bool UpdateImageInfo(ref CoreImageInfo oldImageInfo, CoreImageInfo newImageInfo)
        {
            if (oldImageInfo == null || 
                oldImageInfo.path.CompareTo(newImageInfo.path) != 0 ||
                oldImageInfo.size != newImageInfo.size || 
                oldImageInfo.time != newImageInfo.time)
            {
                oldImageInfo = newImageInfo;
                return true;
            }
            return false;
        }

        public void SetResult(CoreResult result)
        {
            if(result.type == CoreDll.ResultType.None)
                throw new Exception("Bad result type!");

            Group = result.group;

            switch(m_position)
            {
            case Position.Left:
            case Position.Top:
                if (result.type == CoreDll.ResultType.DuplImagePair)
                    SetImageInfo(result.first, result.second);
                else
                    SetImageInfo(result.first, null);

                break;
            case Position.Right:
            case Position.Bottom:
                if (result.type == CoreDll.ResultType.DuplImagePair)
                    SetImageInfo(result.second, result.first);
                else
                    SetImageInfo(result.second, null);
                break;
            }
        }
        
        /// <summary>
        /// Adding controls in panel
        /// ���������� ����������� �� ������
        /// </summary>
        public void SetPosition(Position position)
        {
            m_position = position;
            switch (m_position)
            {
                case Position.Left:
                case Position.Top:
                    RenameCurrentType = CoreDll.RenameCurrentType.First;
                    break;
                case Position.Right:
                case Position.Bottom:
                    RenameCurrentType = CoreDll.RenameCurrentType.Second;
                    break;
            }

            m_pictureBoxPanel.Position = m_position;
            
            TableLayoutPanel infoLayout = InitFactory.Layout.Create(7, 1); //number of controls in panel
            infoLayout.Height = m_imageSizeLabel.Height;
            if (m_position != Position.Left)
            {
                m_pathLabel.TextAlign = ContentAlignment.TopLeft;
            
                m_fileSizeLabel.Margin = new Padding(EBW, 0, 0, 0);
                m_pathLabel.Margin = new Padding(IBW, 0, EBW, 0);

                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//fileSizeLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageSizeLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageBlocknessLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageBlurringLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageTypeLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageExifLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));//pathLabel

                infoLayout.Controls.Add(m_fileSizeLabel, 0, 0);
                infoLayout.Controls.Add(m_imageSizeLabel, 1, 0);
                infoLayout.Controls.Add(m_imageBlocknessLabel, 2, 0);
                infoLayout.Controls.Add(m_imageBlurringLabel, 3, 0);
                infoLayout.Controls.Add(m_imageTypeLabel, 4, 0);
                infoLayout.Controls.Add(m_imageExifLabel, 5, 0);
                infoLayout.Controls.Add(m_pathLabel, 6, 0);
            }
            else
            {
                m_pathLabel.TextAlign = ContentAlignment.TopRight;
                
                m_pathLabel.Margin = new Padding(EBW, 0, 0, 0);
                m_fileSizeLabel.Margin = new Padding(IBW, 0, EBW, 0);

                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));//pathLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageExifLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageTypeLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageBlurringLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageBlocknessLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//imageSizeLabel
                infoLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));//fileSizeLabel

                infoLayout.Controls.Add(m_pathLabel, 0, 0);
                infoLayout.Controls.Add(m_imageTypeLabel, 1, 0);
                infoLayout.Controls.Add(m_imageBlurringLabel, 2, 0);
                infoLayout.Controls.Add(m_imageBlocknessLabel, 3, 0); 
                infoLayout.Controls.Add(m_imageSizeLabel, 4, 0);
                infoLayout.Controls.Add(m_imageExifLabel, 5, 0);
                infoLayout.Controls.Add(m_fileSizeLabel, 6, 0);
            }

            Controls.Clear();
            RowStyles.Clear();
            if(m_position == Position.Bottom)
            {
                m_pictureBoxPanel.Margin = new Padding(EBW, IBW, EBW, EBW);
                infoLayout.Margin = new Padding(0, EBW, 0, 0);
                
                RowStyles.Add(new RowStyle(SizeType.AutoSize));
                RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                Controls.Add(infoLayout, 0, 0);
                Controls.Add(m_pictureBoxPanel, 0, 1);
            }
            else
            {
                m_pictureBoxPanel.Margin = new Padding(EBW, EBW, EBW, IBW);
                infoLayout.Margin = new Padding(0, 0, 0, EBW);
                
                RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                RowStyles.Add(new RowStyle(SizeType.AutoSize));
                Controls.Add(m_pictureBoxPanel, 0, 0);
                Controls.Add(infoLayout, 0, 1);
            }
        }

        public void RenameImage(object sender, EventArgs e)
        {
            var fileInfo = new FileInfo(m_currentImageInfo.path);
            var dialog = new SaveFileDialog
            {
                FileName = fileInfo.FullName,
                OverwritePrompt = false,
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = fileInfo.Extension
            };
            dialog.FileOk += new CancelEventHandler(OnRenameImageDialogFileOk);
            dialog.Title = Resources.Strings.Current.ImagePreviewContextMenu_RenameImageItem_Text;
            dialog.InitialDirectory = fileInfo.Directory.ToString();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_resultsListView.RenameCurrent(RenameCurrentType, dialog.FileName);
            }
        }

        private void OnRenameImageDialogFileOk(object sender, CancelEventArgs e)
        {
            var dialog = (SaveFileDialog)sender;
            var oldFileInfo = new FileInfo(m_currentImageInfo.path);
            var newFileInfo = new FileInfo(dialog.FileName);
            if (newFileInfo.FullName != oldFileInfo.FullName && newFileInfo.Exists)
            {
                MessageBox.Show(Resources.Strings.Current.ErrorMessage_FileAlreadyExists,
                    dialog.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            else if (newFileInfo.Extension != oldFileInfo.Extension && newFileInfo.Extension.Length > 0)
            {
                e.Cancel = MessageBox.Show(Resources.Strings.Current.WarningMessage_ChangeFileExtension, 
                    dialog.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel;
            }
        }

        private List<string> GetExifList(CoreImageInfo currentImageInfo, Strings s)
        {
            var exifList = new List<string>();
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.imageDescription))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_ImageDescription + currentImageInfo.exifInfo.imageDescription);
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.equipMake))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_EquipMake + currentImageInfo.exifInfo.equipMake);
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.equipModel))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_EquipModel + currentImageInfo.exifInfo.equipModel);
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.softwareUsed))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_SoftwareUsed + currentImageInfo.exifInfo.softwareUsed);
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.dateTime))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_DateTime + currentImageInfo.exifInfo.dateTime);
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.artist))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_Artist + currentImageInfo.exifInfo.artist);
            if (!string.IsNullOrEmpty(currentImageInfo.exifInfo.userComment))
                exifList.Add(s.ImagePreviewPanel_EXIF_Tooltip_UserComment + currentImageInfo.exifInfo.userComment);
            return exifList;
        }

        /// <summary>
        /// ������������� �������� ��������� tooltip ��� ������� EXIF.
        /// </summary>
        private void SetExifTooltip(CoreImageInfo currentImageInfo)
        {
            Strings s = Resources.Strings.Current;
            var exifSting = string.Empty;

            List<string> exifList = GetExifList(currentImageInfo, s);

            if (exifList.Count > 0)
            {
                for (var i = 0; i < exifList.Count - 1; i++)
                {
                    exifSting += exifList[i];
                    exifSting += Environment.NewLine;
                }
                exifSting += exifList[exifList.Count - 1];

                m_toolTip.SetToolTip(m_imageExifLabel, exifSting);
            }
        }

        /// <summary>
        /// ��������� ��������� EXIF ��� ����� �����.
        /// </summary>
        /// <param name="result"></param>
        public void UpdateExifTooltip(CoreResult result)
        {
            if (result.type == CoreDll.ResultType.None)
                throw new Exception("Bad result type!");

            switch (m_position)
            {
                case Position.Left:
                case Position.Top:
                    if (result.first.exifInfo.isEmpty == CoreDll.FALSE)
                        SetExifTooltip(result.first);
                    break;
                case Position.Right:
                case Position.Bottom:
                    if (result.second.exifInfo.isEmpty == CoreDll.FALSE)
                        SetExifTooltip(result.second);
                    break;
            }
        }

        /// <summary>
        /// �������� ����� �� Exif.
        /// </summary>
        private bool ExifEqual(CoreDll.AdImageExifW imageExif1, CoreDll.AdImageExifW imageExif2)
        {
            if (imageExif1.isEmpty == imageExif2.isEmpty &&
                imageExif1.artist.CompareTo(imageExif2.artist) == 0 &&
                imageExif1.dateTime.CompareTo(imageExif2.dateTime) == 0 &&
                imageExif1.equipMake.CompareTo(imageExif2.equipMake) == 0 &&
                imageExif1.equipModel.CompareTo(imageExif2.equipModel) == 0 &&
                imageExif1.imageDescription.CompareTo(imageExif2.imageDescription) == 0 &&
                imageExif1.softwareUsed.CompareTo(imageExif2.softwareUsed) == 0 &&
                imageExif1.userComment.CompareTo(imageExif2.userComment) == 0)
                return true;

            return false;
        }

        public ComparableBitmap[] GetImageFragments()
        {
            var amountOfFragments = m_options.resultsOptions.AmountOfFragmentsOnX * m_options.resultsOptions.AmountOfFragmentsOnY;

            Bitmap bitmap = m_pictureBoxPanel.Bitmap;
            if (bitmap != null && m_options.resultsOptions.NormalizedSizeOfImage > 16)
            {
                var smallSize = new Size(m_options.resultsOptions.NormalizedSizeOfImage, m_options.resultsOptions.NormalizedSizeOfImage);
                bitmap = ResizeImage(bitmap, smallSize);

                if (bitmap != null)
                {
                    var fragments = new ComparableBitmap[amountOfFragments];
                    var widthOfFragment = bitmap.Width / m_options.resultsOptions.AmountOfFragmentsOnX;
                    var heightOfFragment = bitmap.Height / m_options.resultsOptions.AmountOfFragmentsOnX;

                    for (int i = 0, x = 0, y = 0; i < amountOfFragments; i++)
                    {
                        var rectangle = new Rectangle(x, y, widthOfFragment, heightOfFragment);
                        fragments[i] = new ComparableBitmap(bitmap, rectangle);

                        x += widthOfFragment;
                        if (x >= bitmap.Width)
                        {
                            x = 0;
                            y += heightOfFragment;
                        }
                    }

                    return fragments;
                }
            }

            return null;
        }

        public void SetDifference(List<Rectangle> rectangles)
        {
            m_pictureBoxPanel.SetDifference(rectangles);
        }

        public void ClearDifference()
        {
            m_pictureBoxPanel.ClearDifference();
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                var b = new Bitmap(size.Width, size.Height);
                using (var g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, new Rectangle(0, 0, size.Width, size.Height),
                       0, 0, imgToResize.Width, imgToResize.Height, GraphicsUnit.Pixel);
                }
                return b;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

    }
}
