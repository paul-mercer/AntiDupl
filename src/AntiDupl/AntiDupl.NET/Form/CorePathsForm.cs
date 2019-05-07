/*
* AntiDupl.NET Program (http://ermig1979.github.io/AntiDupl).
*
* Copyright (c) 2002-2018 Yermalayeu Ihar, 2014 Borisov Dmitry.
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
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;


namespace AntiDupl.NET
{
    public class CorePathsForm : Form
    {
        static public int FORM_WIDTH = 450;
        static public int FORM_HEIGHT = 350;

        private readonly CoreLib m_core;
        private Options m_options;
        private CoreOptions m_oldCoreOptions; //����� �� ���������
        private CoreOptions m_newCoreOptions; //����� ����� ���������

        private Button m_okButton;
        private Button m_cancelButton;
        private Button m_addFilesButton;
        private Button m_addFolderButton;
        private Button m_changeButton;
        private Button m_removeButton;

        private TabControl m_tabControl;
        private TabPage m_searchTabPage;
        private CheckedListBox m_searchCheckedList;
        private TabPage m_ignoreTabPage;
        private ListBox m_ignoreListBox;
        private TabPage m_validTabPage;
        private ListBox m_validListBox;
        private TabPage m_deleteTabPage;
        private ListBox m_deleteListBox;
        private ToolTip m_toolTip;

        public CorePathsForm(CoreLib core, Options options, CoreOptions coreOptions)
        {
            m_core = core;
            m_options = options;
            m_oldCoreOptions = coreOptions;
            m_newCoreOptions = m_oldCoreOptions.Clone();
            InitializeComponent();
            UpdateStrings();
            UpdatePath();
            UpdateButtonEnabling();
        }

        void InitializeComponent()
        {
            ClientSize = new System.Drawing.Size(FORM_WIDTH, FORM_HEIGHT);
            FormBorderStyle = FormBorderStyle.Sizable;
            Icon = Resources.Icons.Get(Icon.Size);
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            MaximizeBox = false;
            MinimizeBox = false;
            KeyPreview = true;

            Resources.Help.Bind(this, Resources.Help.Paths);

            TableLayoutPanel mainTableLayoutPanel = InitFactory.Layout.Create(1, 2, 5);
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            Controls.Add(mainTableLayoutPanel);

            TableLayoutPanel pathTableLayoutPanel = InitFactory.Layout.Create(1, 2, 0);
            pathTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            pathTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            pathTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            mainTableLayoutPanel.Controls.Add(pathTableLayoutPanel, 0, 0);

            m_tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(0, 0),
                Margin = new Padding(0)
            };
            m_tabControl.Selected += new TabControlEventHandler(OnTabControlSelected);
            pathTableLayoutPanel.Controls.Add(m_tabControl, 0, 0);

            m_searchTabPage = new TabPage
            {
                Tag = CoreDll.PathType.Search
            };
            m_tabControl.Controls.Add(m_searchTabPage);

            m_toolTip = new ToolTip
            {
                ShowAlways = true
            };

            m_searchCheckedList = InitFactory.CheckedListBox.Create(OnSelectedIndexChanged, OnListBoxDoubleClick, OnItemCheck);
            m_searchTabPage.Controls.Add(m_searchCheckedList);

            m_ignoreTabPage = new TabPage
            {
                Tag = CoreDll.PathType.Ignore
            };
            m_tabControl.Controls.Add(m_ignoreTabPage);

            m_ignoreListBox = InitFactory.ListBox.Create(OnSelectedIndexChanged, OnListBoxDoubleClick);
            m_ignoreTabPage.Controls.Add(m_ignoreListBox);

            m_validTabPage = new TabPage
            {
                Tag = CoreDll.PathType.Valid
            };
            m_tabControl.Controls.Add(m_validTabPage);

            m_validListBox = InitFactory.ListBox.Create(OnSelectedIndexChanged, OnListBoxDoubleClick);
            m_validTabPage.Controls.Add(m_validListBox);

            m_deleteTabPage = new TabPage
            {
                Tag = CoreDll.PathType.Delete
            };
            m_tabControl.Controls.Add(m_deleteTabPage);

            m_deleteListBox = InitFactory.ListBox.Create(OnSelectedIndexChanged, OnListBoxDoubleClick);
            m_deleteTabPage.Controls.Add(m_deleteListBox);

            TableLayoutPanel pathButtonsTableLayoutPanel = InitFactory.Layout.Create(4, 1);
            pathButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            pathButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            pathButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pathButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pathTableLayoutPanel.Controls.Add(pathButtonsTableLayoutPanel, 0, 1);

            m_addFilesButton = new Button
            {
                AutoSize = true
            };
            m_addFilesButton.Click += new EventHandler(OnAddFilesButtonClick);
            pathButtonsTableLayoutPanel.Controls.Add(m_addFilesButton, 0, 0);

            m_addFolderButton = new Button
            {
                AutoSize = true
            };
            m_addFolderButton.Click += new EventHandler(OnAddFolderButtonClick);
            pathButtonsTableLayoutPanel.Controls.Add(m_addFolderButton, 1, 0);

            m_changeButton = new Button
            {
                AutoSize = true
            };
            m_changeButton.Click += new EventHandler(OnChangeButtonClick);
            pathButtonsTableLayoutPanel.Controls.Add(m_changeButton, 2, 0);

            m_removeButton = new Button
            {
                AutoSize = true
            };
            m_removeButton.Click += new EventHandler(OnRemoveButtonClick);
            pathButtonsTableLayoutPanel.Controls.Add(m_removeButton, 3, 0);

            TableLayoutPanel mainButtonsTableLayoutPanel = InitFactory.Layout.Create(3, 1);
            mainButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            mainButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            mainButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            mainTableLayoutPanel.Controls.Add(mainButtonsTableLayoutPanel, 0, 1);

            m_okButton = new Button();
            m_okButton.Click += new EventHandler(OnOkButtonClick);
            mainButtonsTableLayoutPanel.Controls.Add(m_okButton, 1, 0);

            m_cancelButton = new Button();
            m_cancelButton.Click += new EventHandler(OnCancelButtonClick);
            mainButtonsTableLayoutPanel.Controls.Add(m_cancelButton, 2, 0);

            AllowDrop = true;
            DragDrop += new DragEventHandler(OnDragDrop);
            DragEnter += new DragEventHandler(OnDragEnter);
            KeyDown += new KeyEventHandler(OnKeyDown);
        }

        private void UpdateStrings()
        {
            Strings s = Resources.Strings.Current;

            Text = s.CorePathsForm_Text;

            m_toolTip.SetToolTip(m_searchCheckedList, s.CorePathsForm_SearchCheckedListBox_ToolTip_Text);

            m_searchTabPage.Text = s.CorePathsForm_SearchTabPage_Text;
            m_ignoreTabPage.Text = s.CorePathsForm_IgnoreTabPage_Text;
            m_validTabPage.Text = s.CorePathsForm_ValidTabPage_Text;
            m_deleteTabPage.Text = s.CorePathsForm_DeleteTabPage_Text;

            m_okButton.Text = s.OkButton_Text;
            m_cancelButton.Text = s.CancelButton_Text;
            m_addFilesButton.Text = s.CorePathsForm_AddFilesButton_Text;
            m_addFolderButton.Text = s.CorePathsForm_AddFolderButton_Text;
            m_changeButton.Text = s.CorePathsForm_ChangeButton_Text;
            m_removeButton.Text = s.CorePathsForm_RemoveButton_Text;
        }

        /// <summary>
        /// Set new CoreOptions to passed path.
        /// </summary>
        private CorePathWithSubFolder[] GetCurrentPath()
        {
            switch ((CoreDll.PathType)m_tabControl.SelectedTab.Tag)
            {
                case CoreDll.PathType.Search:
                    return m_newCoreOptions.searchPath;
                case CoreDll.PathType.Ignore:
                    return m_newCoreOptions.ignorePath;
                case CoreDll.PathType.Valid:
                    return m_newCoreOptions.validPath;
                case CoreDll.PathType.Delete:
                    return m_newCoreOptions.deletePath;
                default:
                    return null;
            }
        }

        private ListBox GetCurrentListBox()
        {
            switch ((CoreDll.PathType)m_tabControl.SelectedTab.Tag)
            {
                case CoreDll.PathType.Search:
                    return m_searchCheckedList;
                case CoreDll.PathType.Ignore:
                    return m_ignoreListBox;
                case CoreDll.PathType.Valid:
                    return m_validListBox;
                case CoreDll.PathType.Delete:
                    return m_deleteListBox;
                default:
                    return null;
            }
        }

        private CheckedListBox GetCurrentCheckedListBox()
        {
            switch ((CoreDll.PathType)m_tabControl.SelectedTab.Tag)
            {
                case CoreDll.PathType.Search:
                    return m_searchCheckedList;
                default:
                    return null;
            }
        }

        private void SetCurrentPath(CorePathWithSubFolder[] path)
        {
            switch ((CoreDll.PathType)m_tabControl.SelectedTab.Tag)
            {
                case CoreDll.PathType.Search:
                    m_newCoreOptions.searchPath = path;
                    break;
                case CoreDll.PathType.Ignore:
                    m_newCoreOptions.ignorePath = path;
                    break;
                case CoreDll.PathType.Valid:
                    m_newCoreOptions.validPath = path;
                    break;
                case CoreDll.PathType.Delete:
                    m_newCoreOptions.deletePath = path;
                    break;
                default:
                    return;
            }
        }

        private static string GetInitialPath(CorePathWithSubFolder[] paths)
        {
            string existedPath = null;
            for (var i = 0; i < paths.Length; ++i)
                if (Directory.Exists(paths[i].path))
                    existedPath = paths[i].path;
            if (existedPath != null)
                return existedPath;
            else
                return Application.StartupPath;
        }

        private string GetFilter()
        {
            var builder = new StringBuilder();
            builder.Append("Image files: ");
            if (m_newCoreOptions.searchOptions.JPEG)
                builder.Append("JPEG; ");
            if (m_newCoreOptions.searchOptions.GIF)
                builder.Append("GIF; ");
            if (m_newCoreOptions.searchOptions.PNG)
                builder.Append("PNG; ");
            if (m_newCoreOptions.searchOptions.BMP)
                builder.Append("BMP; ");
            if (m_newCoreOptions.searchOptions.TIFF)
                builder.Append("TIFF; ");
            if (m_newCoreOptions.searchOptions.EMF)
                builder.Append("EMF; ");
            if (m_newCoreOptions.searchOptions.WMF)
                builder.Append("WMF; ");
            if (m_newCoreOptions.searchOptions.EXIF)
                builder.Append("EXIF; ");
            if (m_newCoreOptions.searchOptions.ICON)
                builder.Append("ICON; ");
            if (m_newCoreOptions.searchOptions.JP2)
                builder.Append("JP2; ");
            if (m_newCoreOptions.searchOptions.PSD)
                builder.Append("PSD; ");
            builder.Append("|");
            if (m_newCoreOptions.searchOptions.JPEG)
                builder.Append("*.jpeg;*.jfif;*.jpg;*.jpe;*.jiff;*.jif;*.j;*.jng;*.jff;");
            if (m_newCoreOptions.searchOptions.GIF)
                builder.Append("*.gif;");
            if (m_newCoreOptions.searchOptions.PNG)
                builder.Append("*.png;");
            if (m_newCoreOptions.searchOptions.BMP)
                builder.Append("*.bmp;*.dib;*.rle;");
            if (m_newCoreOptions.searchOptions.TIFF)
                builder.Append("*.tif;*.tiff;");
            if (m_newCoreOptions.searchOptions.EMF)
                builder.Append("*.emf;*.emz;");
            if (m_newCoreOptions.searchOptions.WMF)
                builder.Append("*.wmf");
            if (m_newCoreOptions.searchOptions.EXIF)
                builder.Append("*.exif;");
            if (m_newCoreOptions.searchOptions.ICON)
                builder.Append("*.icon;*.ico;*.icn;");
            if (m_newCoreOptions.searchOptions.JP2)
                builder.Append("*.jp2;*.j2k;*.j2c;*.jpc;*.jpf;*.jpx;");
            if (m_newCoreOptions.searchOptions.PSD)
                builder.Append("*.psd;");
            return builder.ToString();
        }

        /// <summary>
        /// ��������� ���������� ListBox
        /// </summary>
        /// <param name="path">���� ��� ����������</param>
        /// <param name="box">����������� ListBox</param>
        static private void UpdatePath(string[] path, ListBox box)
        {
            var selectedIndex = box.SelectedIndex;
            box.Items.Clear();
            for (var i = 0; i < path.Length; ++i)
                box.Items.Add(path[i]);
            if (selectedIndex >= 0 && selectedIndex < path.Length)
                box.SelectedIndex = selectedIndex;
        }

        static private void UpdatePath(CorePathWithSubFolder[] paths, CheckedListBox box)
        {
            var selectedIndex = box.SelectedIndex;
            box.Items.Clear();
            for (var i = 0; i < paths.Length; ++i)
                box.Items.Add(paths[i].path, paths[i].enableSubFolder);
            if (selectedIndex >= 0 && selectedIndex < paths.Length)
                box.SelectedIndex = selectedIndex;
        }

        static private void UpdatePath(CorePathWithSubFolder[] paths, ListBox box)
        {
            var selectedIndex = box.SelectedIndex;
            box.Items.Clear();
            for (var i = 0; i < paths.Length; ++i)
                box.Items.Add(paths[i].path);
            if (selectedIndex >= 0 && selectedIndex < paths.Length)
                box.SelectedIndex = selectedIndex;
        }


        private void UpdatePath()
        {
            UpdatePath(m_newCoreOptions.searchPath, m_searchCheckedList); 
            UpdatePath(m_newCoreOptions.ignorePath, m_ignoreListBox);
            UpdatePath(m_newCoreOptions.validPath, m_validListBox);
            UpdatePath(m_newCoreOptions.deletePath, m_deleteListBox);
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            m_newCoreOptions.CopyTo(ref m_oldCoreOptions);
            Close();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnAddFolderButtonClick(object sender, EventArgs e)
        {
            CorePathWithSubFolder[] path = GetCurrentPath();
            if (path == null)
                return;
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                SelectedPath = GetInitialPath(path)
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Array.Resize(ref path, path.Length + 1);
                path[path.Length - 1] = new CorePathWithSubFolder();
                if (dialog.SelectedPath[dialog.SelectedPath.Length - 1] == Path.DirectorySeparatorChar)
                    path[path.Length - 1].path = dialog.SelectedPath.Remove(dialog.SelectedPath.Length - 1);
                else
                    path[path.Length - 1].path = dialog.SelectedPath;
                path[path.Length - 1].enableSubFolder = true;
                SetCurrentPath(path);
                m_newCoreOptions.Validate(m_core, m_options.onePath);
                UpdatePath();
                UpdateButtonEnabling();
            }
        }

        private void OnAddFilesButtonClick(object sender, EventArgs e)
        {
            CorePathWithSubFolder[] path = GetCurrentPath();
            if (path == null)
                return;
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                RestoreDirectory = true,
                InitialDirectory = GetInitialPath(path),
                Filter = GetFilter()
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Array.Resize(ref path, path.Length + dialog.FileNames.Length);
                for (var i = 0; i < dialog.FileNames.Length; ++i)
                {
                    if (path[path.Length - 1 - i] == null)
                        path[path.Length - 1 - i] = new CorePathWithSubFolder();
                    path[path.Length - 1 - i].path = dialog.FileNames[dialog.FileNames.Length - 1 - i];
                    path[path.Length - 1 - i].enableSubFolder = false;
                }
                SetCurrentPath(path);
                m_newCoreOptions.Validate(m_core, m_options.onePath);
                UpdatePath();
                UpdateButtonEnabling();
            }
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            RemovePath(false);
        }

        private void OnTabControlSelected(object sender, TabControlEventArgs e)
        {
            UpdateButtonEnabling();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonEnabling();
        }

        private void OnItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateSubFolderOptions(e);
        }

        private void UpdateSubFolderOptions(ItemCheckEventArgs e)
        {
            if (m_tabControl.SelectedTab == null)
                m_tabControl.SelectedIndex = 0;

            var pathType = (CoreDll.PathType)m_tabControl.SelectedTab.Tag;
            CheckedListBox checkedListBox = GetCurrentCheckedListBox();
            CorePathWithSubFolder[] path = GetCurrentPath();

            if (checkedListBox != null)
                if (checkedListBox.SelectedIndex != -1)
                {
                    if (checkedListBox == m_searchCheckedList)
                    {
                        if (e.NewValue == CheckState.Unchecked)
                            path[checkedListBox.SelectedIndex].enableSubFolder = false;
                        else
                            path[checkedListBox.SelectedIndex].enableSubFolder = true;
                    }
                }
        }

        private void OnListBoxDoubleClick(object sender, EventArgs e)
        {
            ChangePath();
        }

        private void OnChangeButtonClick(object sender, EventArgs e)
        {
            ChangePath();
        }

        private void ChangePath()
        {
            ListBox listBox = GetCurrentListBox();
            CorePathWithSubFolder[] path = GetCurrentPath();
            if (listBox == null || path == null)
                return;
            if (listBox.SelectedIndex < 0 || listBox.SelectedIndex >= path.Length)
                return;
            if (Directory.Exists(path[listBox.SelectedIndex].path))
            {
                var dialog = new FolderBrowserDialog
                {
                    ShowNewFolderButton = false,
                    SelectedPath = path[listBox.SelectedIndex].path
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath[dialog.SelectedPath.Length - 1] == Path.DirectorySeparatorChar)
                        path[listBox.SelectedIndex].path = dialog.SelectedPath.Remove(dialog.SelectedPath.Length - 1);
                    else
                        path[listBox.SelectedIndex].path = dialog.SelectedPath;
                    SetCurrentPath(path);
                    m_newCoreOptions.Validate(m_core, m_options.onePath);
                    UpdatePath();
                    UpdateButtonEnabling();
                }
            }
            else
            {
                var dialog = new OpenFileDialog
                {
                    RestoreDirectory = true,
                    FileName = path[listBox.SelectedIndex].path,
                    Filter = GetFilter()
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path[listBox.SelectedIndex].path = dialog.FileName;
                    SetCurrentPath(path);
                    m_newCoreOptions.Validate(m_core, m_options.onePath);
                    UpdatePath();
                    UpdateButtonEnabling();
                }
            }
        }

        private void UpdateButtonEnabling()
        {
            if (m_tabControl.SelectedTab == null)
                m_tabControl.SelectedIndex = 0;

            var pathType = (CoreDll.PathType)m_tabControl.SelectedTab.Tag;
            ListBox listBox = GetCurrentListBox();
            CorePathWithSubFolder[] path = GetCurrentPath();

            if (listBox == null || path == null ||
              listBox.SelectedIndex < 0 || listBox.SelectedIndex >= path.Length)
            {
                m_changeButton.Enabled = false;
                m_removeButton.Enabled = false;
            }
            else
            {
                m_changeButton.Enabled = true;
                if (listBox == m_searchCheckedList && path.Length == 1)
                    m_removeButton.Enabled = false;
                else
                    m_removeButton.Enabled = true;
            }

            m_okButton.Enabled = !m_oldCoreOptions.Equals(m_newCoreOptions);
        }

        private CorePathWithSubFolder[] GetActualPath(string[] path)
        {
            var actualPath = new List<CorePathWithSubFolder>();
            var actualExtensions = m_oldCoreOptions.searchOptions.GetActualExtensions(); //������ �������������� ����������
            for (var i = 0; i < path.Length; ++i)
            {
                if (Directory.Exists(path[i]))
                {
                    var sfPath = new CorePathWithSubFolder
                    {
                        path = path[i],
                        enableSubFolder = true
                    };
                    actualPath.Add(sfPath);
                }
                else
                {
                    var fileInfo = new FileInfo(path[i]);
                    if (fileInfo.Extension.Length > 1)
                    {
                        var extension = fileInfo.Extension.ToUpper().Substring(1);
                        for (var j = 0; j < actualExtensions.Length; ++j)
                        {
                            if (extension == actualExtensions[j]) //���� ���������� �� ������ ��������������
                            {
                                var sfPath = new CorePathWithSubFolder
                                {
                                    path = path[i],
                                    enableSubFolder = false
                                };
                                actualPath.Add(sfPath);
                                break;
                            }
                        }
                    }
                }
            }
            return (CorePathWithSubFolder[])actualPath.ToArray();
        }

        private void AddPath(string[] additional)
        {
            if (additional.Length > 0)
            {
                var path = new List<CorePathWithSubFolder>(GetCurrentPath());
                CorePathWithSubFolder[] current = GetCurrentPath();
                CorePathWithSubFolder[] actual = GetActualPath(additional);

                if (current != null && actual.Length > 0)
                {
                    path.AddRange(current);
                    path.AddRange(actual);
                    SetCurrentPath((CorePathWithSubFolder[])path.ToArray());
                    m_newCoreOptions.Validate(m_core, m_options.onePath);
                    UpdatePath();
                    UpdateButtonEnabling();
                }
            }
        }

        private void RemovePath(bool copyToClipboard)
        {
            ListBox listBox = GetCurrentListBox();
            CorePathWithSubFolder[] path = GetCurrentPath();

            if (listBox == null || path == null)
                return;
            if (listBox.SelectedIndex < 0 || listBox.SelectedIndex >= path.Length)
                return;
            if (listBox == m_searchCheckedList && path.Length == 1)
                return;

            if (copyToClipboard)
            {
                Clipboard.SetDataObject(path[listBox.SelectedIndex]);
            }
            path[listBox.SelectedIndex].path = "";
            SetCurrentPath(path);
            m_newCoreOptions.Validate(m_core, m_options.onePath);
            UpdatePath();
            UpdateButtonEnabling();
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            var path = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddPath(path);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                var path = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (GetActualPath(path).Length > 0)
                {
                    e.Effect = DragDropEffects.All;
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.C | Keys.Control))
            {
                ListBox listBox = GetCurrentListBox();
                CorePathWithSubFolder[] path = GetCurrentPath();
                if (listBox != null && path != null && listBox.SelectedIndex >= 0 && listBox.SelectedIndex < path.Length)
                {
                    Clipboard.SetDataObject(path[listBox.SelectedIndex]);
                }
            }
            else if (e.KeyData == (Keys.V | Keys.Control))
            {
                ListBox listBox = GetCurrentListBox();
                if(listBox != null)
                {
                    if (Clipboard.ContainsFileDropList())
                    {
                        StringCollection fileDropList = Clipboard.GetFileDropList();
                        var path = new string[fileDropList.Count];
                        fileDropList.CopyTo(path, 0);
                        AddPath(path);
                    }
                    else if (Clipboard.ContainsText())
                    {
                        var path = new string[1];
                        path[0] = Clipboard.GetText();
                        AddPath(path);
                    }
                }
            }
            else if (e.KeyData == (Keys.X | Keys.Control))
            {
                RemovePath(true);
            }
            else if (e.KeyData == Keys.Delete)
            {
                RemovePath(false);
            }
        }
    }
}
