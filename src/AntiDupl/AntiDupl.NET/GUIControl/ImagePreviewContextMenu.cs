﻿/*
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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;

namespace AntiDupl.NET
{
    public class ImagePreviewContextMenu : ContextMenuStrip
    {
        private CoreLib m_core;
        private Options m_options;
        private CoreOptions m_coreOptions;
        private ImagePreviewPanel m_imagePreviewPanel;
        private ResultsListView m_resultsListView;

        private ToolStripMenuItem m_copyPathItem;
        private ToolStripMenuItem m_copyFileNameItem;
        private ToolStripMenuItem m_openImageItem;
        private ToolStripMenuItem m_openFolderItem;
        private ToolStripMenuItem m_addToIgnore;
        private ToolStripMenuItem m_addToIgnoreDirectory;
        private ToolStripMenuItem m_renameImageItem;
        private ToolStripMenuItem m_renameImageLikeNeighbourItem;
        private ToolStripMenuItem m_moveImageToNeighbourItem;
        private ToolStripMenuItem m_moveImageAndRenameToNeighbourItem;
        private ToolStripMenuItem m_moveGroupToNeighbourItem;
        private ToolStripMenuItem m_renameGroupAsNeighbourItem;

        
        public ImagePreviewContextMenu(CoreLib core, Options options, CoreOptions coreOptions, ImagePreviewPanel imagePreviewPanel, ResultsListView resultsListView)
        {
            m_core = core;
            m_options = options;
            m_coreOptions = coreOptions;
            m_imagePreviewPanel = imagePreviewPanel;
            m_resultsListView = resultsListView;
            InitializeComponents();
            UpdateStrings();
            Resources.Strings.OnCurrentChange += new Resources.Strings.CurrentChangeHandler(UpdateStrings);
            Opening += new CancelEventHandler(OnOpening);
        }

        private void InitializeComponents()
        {
            RenderMode = ToolStripRenderMode.System;

            m_copyPathItem = InitFactory.MenuItem.Create(null, null, CopyPath);
            m_copyFileNameItem = InitFactory.MenuItem.Create(null, null, new EventHandler(this.CopyFileName));
            m_openImageItem = InitFactory.MenuItem.Create(null, null, OpenImage);
            m_openFolderItem = InitFactory.MenuItem.Create(null, null, OpenFolder);
            m_addToIgnore = InitFactory.MenuItem.Create(null, null, AddToIgnore);
            m_addToIgnoreDirectory = InitFactory.MenuItem.Create(null, null, AddToIgnoreDirectory);
            m_renameImageItem = InitFactory.MenuItem.Create(null, null, m_imagePreviewPanel.RenameImage);
            m_renameImageLikeNeighbourItem = InitFactory.MenuItem.Create(null, null, new EventHandler(RenameImageLikeNeighbour));
            m_moveImageToNeighbourItem = InitFactory.MenuItem.Create(null, null, new EventHandler(MoveImageToNeighbour));
            m_moveImageAndRenameToNeighbourItem = InitFactory.MenuItem.Create(null, null, new EventHandler(MoveAndRenameToNeighbour));
            m_moveGroupToNeighbourItem = InitFactory.MenuItem.Create(null, null, MoveGroupToNeighbour);
            m_renameGroupAsNeighbourItem = InitFactory.MenuItem.Create(null, null, RenameCurrentGroupAsNeighbour);
            
            Items.Add(new ToolStripSeparator());
        }
        
        private void OnOpening(object sender, EventArgs e)
        {
            Items.Clear();
            
            Items.Add(m_copyPathItem);
            Items.Add(m_copyFileNameItem);
            Items.Add(new ToolStripSeparator());
            Items.Add(m_openImageItem);
            Items.Add(m_openFolderItem);
            Items.Add(new ToolStripSeparator());
            Items.Add(m_addToIgnore);
            Items.Add(m_addToIgnoreDirectory);
            Items.Add(new ToolStripSeparator());
            Items.Add(m_renameImageItem);
            if (RenameImageLikeNeighbourEnable())
            {
                m_renameImageLikeNeighbourItem.Image = m_imagePreviewPanel.RenameCurrentType == CoreDll.RenameCurrentType.First ? Resources.Images.Get("RenameFirstLikeSecondVerticalMenu") : Resources.Images.Get("RenameSecondLikeFirstVerticalMenu");
                Items.Add(m_renameImageLikeNeighbourItem);
            }
            if (MoveToNeighbourEnable())
            {
                m_moveImageToNeighbourItem.Image = m_imagePreviewPanel.RenameCurrentType == CoreDll.RenameCurrentType.First ? Resources.Images.Get("MoveFirstToSecondVerticalMenu") : Resources.Images.Get("MoveSecondToFirstVerticalMenu");
                Items.Add(m_moveImageToNeighbourItem);
                Items.Add(m_moveImageAndRenameToNeighbourItem);
            }
            if (MoveGroupEnable())
            {
                Items.Add(new ToolStripSeparator());
                Items.Add(m_moveGroupToNeighbourItem);
                Items.Add(m_renameGroupAsNeighbourItem);
            }
        }

        private void UpdateStrings()
        {
            Strings s = Resources.Strings.Current;

            m_copyPathItem.Text = s.ImagePreviewContextMenu_CopyPathItem_Text;
            m_copyFileNameItem.Text = s.ImagePreviewContextMenu_CopyFileNameItem_Text;
            m_openImageItem.Text = s.ImagePreviewContextMenu_OpenImageItem_Text;
            m_openFolderItem.Text = s.ImagePreviewContextMenu_OpenFolderItem_Text;
            m_addToIgnore.Text = s.ImagePreviewContextMenu_AddToIgnore_Text;
            m_addToIgnoreDirectory.Text = s.ImagePreviewContextMenu_AddToIgnoreDirectory_Text;
            m_renameImageItem.Text = s.ImagePreviewContextMenu_RenameImageItem_Text;
            m_renameImageLikeNeighbourItem.Text = s.ImagePreviewContextMenu_RenameImageLikeNeighbour_Text;
            m_moveImageToNeighbourItem.Text = s.ImagePreviewContextMenu_MoveImageToNeighbourItem_Text;
            m_moveImageAndRenameToNeighbourItem.Text = s.ImagePreviewContextMenu_MoveAndRenameImageToNeighbourItem_Text;
            m_moveGroupToNeighbourItem.Text = s.ImagePreviewContextMenu_MoveGroupToNeighbourItem_Text;
            m_renameGroupAsNeighbourItem.Text = s.ImagePreviewContextMenu_RenameGroupAsNeighbourItem_Text;
        }

        private void OpenImage(object sender, EventArgs e)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = m_imagePreviewPanel.CurrentImageInfo.path
            };
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
        }

        private void OpenFolder(object sender, EventArgs e)
        {
            FolderOpener.OpenContainingFolder(m_imagePreviewPanel.CurrentImageInfo);
        }

        private void AddToIgnore(object sender, EventArgs e)
        {
            if (m_imagePreviewPanel.CurrentImageInfo != null)
            {
                Array.Resize(ref m_coreOptions.ignorePath, m_coreOptions.ignorePath.Length + 1);
                m_coreOptions.ignorePath[m_coreOptions.ignorePath.Length - 1] = new CorePathWithSubFolder(m_imagePreviewPanel.CurrentImageInfo.path, false);
                m_coreOptions.Validate(m_core, m_options.onePath);
                m_resultsListView.RefreshResults();
            }
        }

        private void AddToIgnoreDirectory(object sender, EventArgs e)
        {
            if (m_imagePreviewPanel.CurrentImageInfo != null)
            {
                Array.Resize(ref m_coreOptions.ignorePath, m_coreOptions.ignorePath.Length + 1);
                m_coreOptions.ignorePath[m_coreOptions.ignorePath.Length - 1] = new CorePathWithSubFolder(m_imagePreviewPanel.CurrentImageInfo.GetDirectoryString(), true);
                m_coreOptions.Validate(m_core, m_options.onePath);
                m_resultsListView.RefreshResults();
            }
        }

        private void CopyPath(object sender, EventArgs e)
        {
            Clipboard.SetText(m_imagePreviewPanel.CurrentImageInfo.path);
        }

        private void CopyFileName(object sender, EventArgs e)
        {
            Clipboard.SetText(Path.GetFileNameWithoutExtension(m_imagePreviewPanel.CurrentImageInfo.path));
        }

        /// <summary>
        /// Проверка на то, что имена разные.
        /// </summary>
        /// <returns></returns>
        private bool RenameImageLikeNeighbourEnable()
        {
            if (m_imagePreviewPanel.NeighbourImageInfo != null && m_imagePreviewPanel.CurrentImageInfo != null)
            {
                return m_imagePreviewPanel.NeighbourImageInfo.GetFileNameWithoutExtensionString() != m_imagePreviewPanel.CurrentImageInfo.GetFileNameWithoutExtensionString();
            }
            return false;
        }

        private void RenameImageLikeNeighbour(object sender, EventArgs e)
        {
            if (m_imagePreviewPanel.RenameCurrentType == CoreDll.RenameCurrentType.First)
                m_resultsListView.MakeAction(CoreDll.LocalActionType.RenameFirstLikeSecond, CoreDll.TargetType.Current);
            else
                m_resultsListView.MakeAction(CoreDll.LocalActionType.RenameSecondLikeFirst, CoreDll.TargetType.Current);
        }

        /// <summary>
        /// Проверка на то, что директории у картинок разные.
        /// </summary>
        private bool MoveToNeighbourEnable()
        {
            if (m_imagePreviewPanel.NeighbourImageInfo != null && m_imagePreviewPanel.CurrentImageInfo != null)
            {
                return m_imagePreviewPanel.NeighbourImageInfo.GetDirectoryString() != m_imagePreviewPanel.CurrentImageInfo.GetDirectoryString();
            }
            return false;
        }

        private void MoveImageToNeighbour(object sender, EventArgs e)
        {
            if (m_imagePreviewPanel.RenameCurrentType == CoreDll.RenameCurrentType.First)
                m_resultsListView.MakeAction(CoreDll.LocalActionType.MoveFirstToSecond, CoreDll.TargetType.Current);
            else
                m_resultsListView.MakeAction(CoreDll.LocalActionType.MoveSecondToFirst, CoreDll.TargetType.Current);
        }

        private void MoveAndRenameToNeighbour(object sender, EventArgs e)
        {
            if (m_imagePreviewPanel.RenameCurrentType == CoreDll.RenameCurrentType.First)
                m_resultsListView.MakeAction(CoreDll.LocalActionType.MoveAndRenameFirstToSecond, CoreDll.TargetType.Current);
            else
                m_resultsListView.MakeAction(CoreDll.LocalActionType.MoveAndRenameSecondToFirst, CoreDll.TargetType.Current);
        }

        /// <summary>
        /// Возврашает истину, если в групе больше 2 файлов, тоесть есть смысл перемащать группами.
        /// </summary>
        /// <returns></returns>
        private bool MoveGroupEnable()
        {
            if (m_core.GetImageInfoSize(m_imagePreviewPanel.Group) > 2)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Перенести группу в папку к соседней.
        /// </summary>
        private void MoveGroupToNeighbour(object sender, EventArgs e)
        {
            m_resultsListView.MoveCurrentGroupToDirectory(m_imagePreviewPanel.NeighbourImageInfo.GetDirectoryString());
        }

        private void RenameCurrentGroupAsNeighbour(object sender, EventArgs e)
        {
            m_resultsListView.RenameCurrentGroupAs(m_imagePreviewPanel.NeighbourImageInfo.GetFileNameWithoutExtensionString());
        }
    }
}
