﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MagicMongoDBTool.Module;
using MongoDB.Bson;

namespace MagicMongoDBTool.UserController
{
    public partial class ctlDataView : UserControl
    {

        /// <summary>
        /// 这里需要控制3中不同的数据类型，普通的Collection，GFS，USER。
        /// 图标复用方式来处理不同的类型。
        /// </summary>

        #region"Main"
        public ctlDataView(MongoDBHelper.DataViewInfo _DataViewInfo)
        {
            InitializeComponent();
            mDataViewInfo = _DataViewInfo;
        }
        /// <summary>
        /// Control for show Data
        /// </summary>
        public List<Control> _dataShower = new List<Control>();
        /// <summary>
        /// DataView信息
        /// </summary>
        public MongoDBHelper.DataViewInfo mDataViewInfo;
        /// <summary>
        /// 关闭Tab事件
        /// </summary>
        public event EventHandler CloseTab;
        /// <summary>
        /// 是否为Admin数据库
        /// </summary>
        private Boolean IsAdminDB;
        /// <summary>
        /// 是否为系统数据集
        /// </summary>
        private Boolean IsSystemCollection;
        /// <summary>
        /// 节点类型
        /// </summary>
        private String strNodeType;
        /// <summary>
        /// 节点路径
        /// </summary>
        private String strNodeData;
        /// <summary>
        /// 加载数据集控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctlDataView_Load(object sender, EventArgs e)
        {
            this.cmbRecPerPage.SelectedIndex = 1;
            mDataViewInfo.LimitCnt = 100;
            strNodeType = mDataViewInfo.strDBTag.Split(":".ToCharArray())[0];
            strNodeData = mDataViewInfo.strDBTag.Split(":".ToCharArray())[1];

            String[] DataList = strNodeData.Split("/".ToCharArray());
            if (DataList[(int)MongoDBHelper.PathLv.DatabaseLV] == MongoDBHelper.DATABASE_NAME_ADMIN)
            {
                IsAdminDB = true;
            }
            IsSystemCollection = MongoDBHelper.IsSystemCollection(DataList[(int)MongoDBHelper.PathLv.DatabaseLV],
                                                                  DataList[(int)MongoDBHelper.PathLv.CollectionLV]);

            if (strNodeType == MongoDBHelper.COLLECTION_TAG)
            {
                _dataShower.Add(lstData);
                _dataShower.Add(trvData);
                _dataShower.Add(txtData);
            }
            else
            {
                _dataShower.Add(lstData);
                this.tabDataShower.Controls.Remove(tabTreeView);
                this.tabDataShower.Controls.Remove(tabTextView);
            }

            this.lstData.MouseClick += new MouseEventHandler(lstData_MouseClick);
            this.lstData.MouseDoubleClick += new MouseEventHandler(lstData_MouseDoubleClick);
            this.lstData.SelectedIndexChanged += new EventHandler(lstData_SelectedIndexChanged);
            this.trvData.MouseClick += new MouseEventHandler(trvData_MouseClick_Top);
            this.trvData.AfterSelect += new TreeViewEventHandler(trvData_AfterSelect_Top);
            this.trvData.KeyDown += new KeyEventHandler(trvData_KeyDown);
            this.trvData.AfterExpand += new TreeViewEventHandler(trvData_AfterExpand);
            this.trvData.AfterCollapse += new TreeViewEventHandler(trvData_AfterCollapse);


            this.tabDataShower.SelectedIndexChanged += new EventHandler(
                //If tabpage changed,the selected data in dataview will disappear,set delete selected record to false
                (x, y) =>
                {
                    this.DelSelectRecordToolStripMenuItem.Enabled = false;
                    if (IsNeedRefresh)
                    {
                        RefreshStripButton_Click(sender, e);
                    }
                }
            );
            if (!SystemManager.IsUseDefaultLanguage())
            {
                //数据显示区
                this.tabTreeView.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Tab_Tree);
                this.tabTableView.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Tab_Table);
                this.tabTextView.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Tab_Text);

                this.AddDocumentToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataCollection_AddDocument);
                this.EditDocStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.OpenInNativeEditor);
                this.DelSelectRecordToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataCollection_DropDocument);

                this.PrePageStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView_Previous);
                this.NextPageStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView_Next);
                this.FirstPageStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView_First);
                this.LastPageStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView_Last);
                this.QueryStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView_Query);
                this.FilterStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView_DataFilter);

                this.DataDocumentToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument);
                this.AddElementToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument_AddElement);
                this.DropElementToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument_DropElement);
                this.ModifyElementToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument_ModifyElement);
                this.CopyElementToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument_CopyElement);
                this.CutElementToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument_CutElement);
                this.PasteElementToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_DataDocument_PasteElement);

                this.DelFileToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_DelFile);
                this.UploadFileToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_Upload);
                this.DownloadFileToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_Download);
                this.OpenFileToolStripMenuItem.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_OpenFile);

            }


            //Change Icons Enable to Disable at first load
            this.EditDocStripButton.Enabled = false;
            this.QueryStripButton.Enabled = false;
            this.FilterStripButton.Enabled = false;
            this.FilterStripButton.Checked = false;
            this.DelSelectRecordToolStripButton.Enabled = false;
            this.CutStripButton.Enabled = false;
            this.CopyStripButton.Enabled = false;
            this.PasteStripButton.Enabled = false;

            //Change Icons Visible by DataType
            OpenFileStripButton.Visible = false;
            ExpandAllStripButton.Visible = false;
            CollapseAllStripButton.Visible = false;
            CutStripButton.Visible = false;
            CopyStripButton.Visible = false;
            PasteStripButton.Visible = false;

            switch (strNodeType)
            {
                case MongoDBHelper.COLLECTION_TAG:
                    ExpandAllStripButton.Visible = true;
                    CollapseAllStripButton.Visible = true;
                    EditDocStripButton.Enabled = true;
                    CutStripButton.Visible = true;
                    CopyStripButton.Visible = true;
                    PasteStripButton.Visible = true;
                    break;
                case MongoDBHelper.GRID_FILE_SYSTEM_TAG:
                    OpenFileStripButton.Image = MagicMongoDBTool.Properties.Resources.Open.ToBitmap();
                    OpenFileStripButton.Visible = true;

                    if (!SystemManager.IsUseDefaultLanguage())
                    {
                        NewDocumentStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_Upload);
                        EditDocStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_Download);
                        DelSelectRecordToolStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_FileSystem_DelFile);
                    }
                    else
                    {
                        NewDocumentStripButton.Text = "Upload File";
                        EditDocStripButton.Text = "Download File";
                        DelSelectRecordToolStripButton.Text = "Delete File";
                    }
                    break;
                case MongoDBHelper.USER_LIST_TAG:
                    NewDocumentStripButton.Image = MagicMongoDBTool.Properties.Resources.AddUserToDB;
                    EditDocStripButton.Image = MagicMongoDBTool.Properties.Resources.DBkey;
                    DelSelectRecordToolStripButton.Image = MagicMongoDBTool.Properties.Resources.DelUser;
                    if (!SystemManager.IsUseDefaultLanguage())
                    {
                        NewDocumentStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_Database_AddUser);
                        EditDocStripButton.Text = "Change User Config";
                        DelSelectRecordToolStripButton.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_Operation_Database_DelUser);
                    }
                    else
                    {
                        NewDocumentStripButton.Text = "New User";
                        EditDocStripButton.Text = "Change User Config";
                        DelSelectRecordToolStripButton.Text = "Delete User";
                    }
                    break;
                default:
                    break;
            }

            //加载数据
            MongoDBHelper.FillDataToControl(ref mDataViewInfo, _dataShower);
            //数据导航
            SetDataNav();
        }

        /// <summary>
        /// Is Need Refresh after the element is modify
        /// </summary>
        public Boolean IsNeedRefresh = false;
        /// <summary>
        /// 添加新文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewDocumentStripButton_Click(object sender, EventArgs e)
        {
            switch (strNodeType)
            {
                case MongoDBHelper.COLLECTION_TAG:
                    NewDocument();
                    break;
                case MongoDBHelper.GRID_FILE_SYSTEM_TAG:
                    UploadFile();
                    break;
                case MongoDBHelper.USER_LIST_TAG:
                    if (mDataViewInfo.strDBTag.EndsWith(MongoDBHelper.DATABASE_NAME_ADMIN + "/" + MongoDBHelper.COLLECTION_NAME_USER))
                    {
                        SystemManager.OpenForm(new frmUser(true));
                    }
                    else
                    {
                        SystemManager.OpenForm(new frmUser(false));
                    }
                    RefreshStripButton_Click(sender, e);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 在默认编辑器中打开TextView内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDocStripButton_Click(object sender, EventArgs e)
        {
            switch (strNodeType)
            {
                case MongoDBHelper.COLLECTION_TAG:
                    MongoDBHelper.SaveAndOpenStringAsFile(txtData.Text);
                    break;
                case MongoDBHelper.GRID_FILE_SYSTEM_TAG:
                    DownloadFile();
                    break;
                case MongoDBHelper.USER_LIST_TAG:
                    changePasswordToolStripMenuItem_Click(sender, e);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelSelectedRecord_Click(object sender, EventArgs e)
        {
            switch (strNodeType)
            {
                case MongoDBHelper.COLLECTION_TAG:
                    DeleteSelectedRecords();
                    break;
                case MongoDBHelper.GRID_FILE_SYSTEM_TAG:
                    DelFile();
                    break;
                case MongoDBHelper.USER_LIST_TAG:
                    if (IsAdminDB)
                    {
                        RemoveUserFromAdminToolStripMenuItem_Click(sender, e);
                    }
                    else
                    {
                        RemoveUserToolStripMenuItem_Click(sender, e);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 关闭本Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseStripButton_Click(object sender, EventArgs e)
        {
            if (CloseTab != null)
            {
                CloseTab(sender, e);
            }
        }

        #endregion

        #region"数据展示区操作"
        /// <summary>
        /// 数据列表选中索引变换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SystemManager.GetCurrentCollection().Name)
            {
                case MongoDBHelper.COLLECTION_NAME_GFS_FILES:
                    //文件系统
                    UploadFileToolStripMenuItem.Enabled = true;
                    switch (lstData.SelectedItems.Count)
                    {
                        case 0:
                            //禁止所有操作
                            this.EditDocStripButton.Enabled = false;
                            this.OpenFileStripButton.Enabled = false;
                            this.DelSelectRecordToolStripButton.Enabled = false;
                            this.DelFileToolStripMenuItem.Enabled = false;
                            lstData.ContextMenuStrip = null;
                            break;
                        case 1:
                            //可以进行所有操作
                            if (!mDataViewInfo.IsReadOnly)
                            {
                                DelSelectRecordToolStripButton.Enabled = true;
                                this.DelFileToolStripMenuItem.Enabled = true;
                            }
                            EditDocStripButton.Enabled = true;
                            OpenFileStripButton.Enabled = true;
                            break;
                        default:
                            //可以删除多个文件
                            this.EditDocStripButton.Enabled = false;
                            this.OpenFileStripButton.Enabled = false;
                            if (!mDataViewInfo.IsReadOnly)
                            {
                                this.DelSelectRecordToolStripButton.Enabled = true;
                            }
                            break;
                    }
                    break;
                case MongoDBHelper.COLLECTION_NAME_USER:
                    //用户数据库
                    if (lstData.SelectedItems.Count > 0 && !mDataViewInfo.IsReadOnly)
                    {
                        this.DelSelectRecordToolStripButton.Enabled = true;
                        this.RemoveUserFromAdminToolStripMenuItem.Enabled = true;
                        this.RemoveUserToolStripMenuItem.Enabled = true;
                        if (this.lstData.SelectedItems.Count == 1)
                        {
                            this.EditDocStripButton.Enabled = true;
                        }
                    }
                    break;
                default:
                    //数据系统
                    DelSelectRecordToolStripButton.Enabled = false;
                    if (lstData.SelectedItems.Count > 0 && !IsSystemCollection && !mDataViewInfo.IsReadOnly)
                    {
                        DelSelectRecordToolStripMenuItem.Enabled = true;
                        this.DelSelectRecordToolStripButton.Enabled = true;
                    }
                    if (this.lstData.SelectedItems.Count == 1)
                    {
                        this.EditDocStripButton.Enabled = true;
                    }
                    break;
            }
        }
        /// <summary>
        /// 双击列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (SystemManager.GetCurrentCollection().Name == MongoDBHelper.COLLECTION_NAME_GFS_FILES)
            {
                String strFileName = lstData.SelectedItems[0].Text;
                MongoDBHelper.OpenFile(strFileName);
            }
        }
        /// <summary>
        /// 数据列表右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstData_MouseClick(object sender, MouseEventArgs e)
        {
            SystemManager.SelectObjectTag = mDataViewInfo.strDBTag;

            if (lstData.SelectedItems.Count > 0)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    this.contextMenuStripMain = new ContextMenuStrip();

                    switch (SystemManager.GetCurrentCollection().Name)
                    {
                        case MongoDBHelper.COLLECTION_NAME_GFS_FILES:
                            //Grid File System
                            this.contextMenuStripMain.Items.Add(this.DownloadFileToolStripMenuItem.Clone());
                            this.contextMenuStripMain.Items.Add(this.OpenFileToolStripMenuItem.Clone());
                            this.contextMenuStripMain.Items.Add(this.DelFileToolStripMenuItem.Clone());
                            break;
                        case MongoDBHelper.COLLECTION_NAME_USER:
                            if (SystemManager.GetCurrentDataBase().Name == MongoDBHelper.DATABASE_NAME_ADMIN)
                            {
                                this.contextMenuStripMain.Items.Add(this.RemoveUserFromAdminToolStripMenuItem.Clone());
                            }
                            else
                            {
                                this.contextMenuStripMain.Items.Add(this.RemoveUserToolStripMenuItem.Clone());
                            }
                            if (lstData.SelectedItems.Count == 1)
                            {
                                this.contextMenuStripMain.Items.Add(this.changePasswordToolStripMenuItem.Clone());
                            }
                            break;
                        default:
                            this.contextMenuStripMain.Items.Add(this.DelSelectRecordToolStripMenuItem.Clone());
                            break;
                    }
                    lstData.ContextMenuStrip = this.contextMenuStripMain;
                    contextMenuStripMain.Show(lstData.PointToScreen(e.Location));
                }
            }
        }

        /// <summary>
        /// 数据树菜单的禁止
        /// </summary>
        public void DisableDataTreeOpr()
        {
            RemoveUserFromAdminToolStripMenuItem.Enabled = false;
            RemoveUserToolStripMenuItem.Enabled = false;
            DelSelectRecordToolStripMenuItem.Enabled = false;
            DelFileToolStripMenuItem.Enabled = false;
            AddElementToolStripMenuItem.Enabled = false;
            DropElementToolStripMenuItem.Enabled = false;
            ModifyElementToolStripMenuItem.Enabled = false;
            CopyElementToolStripMenuItem.Enabled = false;
            CutElementToolStripMenuItem.Enabled = false;
            PasteElementToolStripMenuItem.Enabled = false;

            this.CutStripButton.Enabled = false;
            this.CopyStripButton.Enabled = false;
            this.PasteStripButton.Enabled = false;
        }

        /// <summary>
        /// 数据树形被选择后(TOP)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvData_AfterSelect_Top(object sender, TreeViewEventArgs e)
        {
            DisableDataTreeOpr();
            SystemManager.SelectObjectTag = mDataViewInfo.strDBTag;
            if (trvData.SelectedNode.Level == 0)
            {
                //顶层可以删除的节点
                if (!mDataViewInfo.IsReadOnly)
                {
                    switch (SystemManager.GetCurrentCollection().Name)
                    {
                        case MongoDBHelper.COLLECTION_NAME_GFS_FILES:

                            DelFileToolStripMenuItem.Enabled = true;
                            break;
                        case MongoDBHelper.COLLECTION_NAME_USER:

                            if (SystemManager.GetCurrentDataBase().Name == MongoDBHelper.DATABASE_NAME_ADMIN)
                            {
                                RemoveUserFromAdminToolStripMenuItem.Enabled = true;
                            }
                            else
                            {
                                RemoveUserToolStripMenuItem.Enabled = true;
                            }
                            break;
                        default:
                            if (!MongoDBHelper.IsSystemCollection(SystemManager.GetCurrentCollection()) && !SystemManager.GetCurrentCollection().IsCapped())
                            {
                                //普通数据
                                //在顶层的时候，允许添加元素,不允许删除元素和修改元素(删除选中记录)
                                DelSelectRecordToolStripMenuItem.Enabled = true;
                                DelSelectRecordToolStripButton.Enabled = true;
                                AddElementToolStripMenuItem.Enabled = true;
                                if (MongoDBHelper.CanPasteAsElement)
                                {
                                    PasteElementToolStripMenuItem.Enabled = true;
                                    PasteStripButton.Enabled = true;
                                }
                            }
                            else
                            {
                                DelSelectRecordToolStripMenuItem.Enabled = false;
                                DelSelectRecordToolStripButton.Enabled = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                //非顶层元素
                trvData_AfterSelect_NotTop(sender, e);
            }
        }
        /// <summary>
        /// 数据树形被选择后(非TOP)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvData_AfterSelect_NotTop(object sender, TreeViewEventArgs e)
        {
            //非顶层可以删除的节点
            switch (SystemManager.GetCurrentCollection().Name)
            {
                case MongoDBHelper.COLLECTION_NAME_GFS_FILES:
                case MongoDBHelper.COLLECTION_NAME_USER:
                default:
                    if (!MongoDBHelper.IsSystemCollection(SystemManager.GetCurrentCollection()) &&
                        !mDataViewInfo.IsReadOnly &&
                        !SystemManager.GetCurrentCollection().IsCapped())
                    {
                        //普通数据:允许添加元素,不允许删除元素
                        DropElementToolStripMenuItem.Enabled = true;
                        CopyElementToolStripMenuItem.Enabled = true;
                        CopyStripButton.Enabled = true;
                        CutElementToolStripMenuItem.Enabled = true;
                        CutStripButton.Enabled = true;
                        if (trvData.SelectedNode.Nodes.Count != 0)
                        {
                            //父节点
                            //1. 以Array_Mark结尾的数组
                            //2. Document
                            if (trvData.SelectedNode.FullPath.EndsWith(MongoDBHelper.Array_Mark))
                            {
                                //列表的父节点
                                if (MongoDBHelper.CanPasteAsValue)
                                {
                                    PasteElementToolStripMenuItem.Enabled = true;
                                    PasteStripButton.Enabled = true;
                                }
                            }
                            else
                            {
                                //文档的父节点
                                if (MongoDBHelper.CanPasteAsElement)
                                {
                                    PasteElementToolStripMenuItem.Enabled = true;
                                    PasteStripButton.Enabled = true;
                                }
                            }
                            AddElementToolStripMenuItem.Enabled = true;
                            ModifyElementToolStripMenuItem.Enabled = false;
                        }
                        else
                        {
                            //子节点
                            //1.简单元素
                            //2.空的Array
                            //3.空的文档
                            //4.Array中的Value
                            BsonValue t;
                            if (trvData.SelectedNode.Tag is BsonElement)
                            {
                                //子节点是一个元素，获得子节点的Value
                                t = ((BsonElement)trvData.SelectedNode.Tag).Value;
                                if (t.IsBsonDocument || t.IsBsonArray)
                                {
                                    //2.空的Array
                                    //3.空的文档
                                    ModifyElementToolStripMenuItem.Enabled = false;
                                    AddElementToolStripMenuItem.Enabled = true;
                                    if (t.IsBsonDocument)
                                    {
                                        //3.空的文档
                                        if (MongoDBHelper.CanPasteAsElement)
                                        {
                                            PasteElementToolStripMenuItem.Enabled = true;
                                            PasteStripButton.Enabled = true;
                                        }

                                    }
                                    if (t.IsBsonArray)
                                    {
                                        //3.Array
                                        if (MongoDBHelper.CanPasteAsValue)
                                        {
                                            PasteElementToolStripMenuItem.Enabled = true;
                                            PasteStripButton.Enabled = true;
                                        }
                                    }
                                }
                                else
                                {
                                    //1.简单元素
                                    ModifyElementToolStripMenuItem.Enabled = true;
                                    AddElementToolStripMenuItem.Enabled = false;
                                }
                            }
                            else
                            {
                                //子节点是一个Array的Value，获得Value
                                //4.Array中的Value
                                t = (BsonValue)trvData.SelectedNode.Tag;
                                ModifyElementToolStripMenuItem.Enabled = true;
                                if (t.IsBsonArray || t.IsBsonDocument)
                                {
                                    //当这个值是一个数组或者文档时候，仍然允许其添加子元素
                                    AddElementToolStripMenuItem.Enabled = true;
                                }
                                else
                                {
                                    AddElementToolStripMenuItem.Enabled = false;
                                }
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 是否需要改变选中节点
        /// </summary>
        private Boolean IsNeedChangeNode = true;
        /// <summary>
        /// 展开节点后的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trvData_AfterExpand(object sender, TreeViewEventArgs e)
        {
            trvData.SelectedNode = e.Node;
            IsNeedChangeNode = false;
            SystemManager.SetCurrentDocument(e.Node);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trvData_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            trvData.SelectedNode = e.Node;
            IsNeedChangeNode = false;
            SystemManager.SetCurrentDocument(e.Node);
        }
        /// <summary>
        /// 鼠标动作（顶层）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvData_MouseClick_Top(object sender, MouseEventArgs e)
        {
            if (IsNeedChangeNode)
            {
                //在节点展开和关闭后，不能使用这个方法来重新设定SelectedNode
                trvData.SelectedNode = this.trvData.GetNodeAt(e.Location);
            }
            IsNeedChangeNode = true;
            if (trvData.SelectedNode == null)
            {
                return;
            }
            SystemManager.SetCurrentDocument(trvData.SelectedNode);
            if (trvData.SelectedNode.Level == 0)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    this.contextMenuStripMain = new ContextMenuStrip();

                    //顶层可以修改的节点
                    switch (SystemManager.GetCurrentCollection().Name)
                    {
                        case MongoDBHelper.COLLECTION_NAME_GFS_FILES:
                            this.contextMenuStripMain.Items.Add(this.DelFileToolStripMenuItem.Clone());
                            break;
                        case MongoDBHelper.COLLECTION_NAME_USER:
                            if (SystemManager.GetCurrentDataBase().Name == MongoDBHelper.DATABASE_NAME_ADMIN)
                            {
                                this.contextMenuStripMain.Items.Add(this.RemoveUserFromAdminToolStripMenuItem.Clone());
                            }
                            else
                            {
                                this.contextMenuStripMain.Items.Add(this.RemoveUserToolStripMenuItem.Clone());
                            }
                            break;
                        default:
                            ///允许删除
                            this.contextMenuStripMain.Items.Add(this.DelSelectRecordToolStripMenuItem.Clone());
                            ///允许添加
                            this.contextMenuStripMain.Items.Add(this.AddElementToolStripMenuItem.Clone());
                            ///允许粘贴
                            this.contextMenuStripMain.Items.Add(this.PasteElementToolStripMenuItem.Clone());
                            break;
                    }
                    trvData.ContextMenuStrip = this.contextMenuStripMain;
                    contextMenuStripMain.Show(trvData.PointToScreen(e.Location));
                }
            }
            else
            {
                //非顶层元素
                trvData_MouseClick_NotTop(sender, e);
            }
        }
        /// <summary>
        /// 鼠标动作（非顶层）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvData_MouseClick_NotTop(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.contextMenuStripMain = new ContextMenuStrip();

                //顶层可以删除的节点
                switch (SystemManager.GetCurrentCollection().Name)
                {
                    case MongoDBHelper.COLLECTION_NAME_GFS_FILES:
                    case MongoDBHelper.COLLECTION_NAME_USER:
                    default:
                        this.contextMenuStripMain.Items.Add(this.AddElementToolStripMenuItem.Clone());
                        this.contextMenuStripMain.Items.Add(this.ModifyElementToolStripMenuItem.Clone());
                        this.contextMenuStripMain.Items.Add(this.DropElementToolStripMenuItem.Clone());
                        this.contextMenuStripMain.Items.Add(this.CopyElementToolStripMenuItem.Clone());
                        this.contextMenuStripMain.Items.Add(this.CutElementToolStripMenuItem.Clone());
                        this.contextMenuStripMain.Items.Add(this.PasteElementToolStripMenuItem.Clone());
                        break;
                }
                trvData.ContextMenuStrip = this.contextMenuStripMain;
                contextMenuStripMain.Show(trvData.PointToScreen(e.Location));
            }
        }
        /// <summary>
        /// 键盘动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trvData_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (DelSelectRecordToolStripMenuItem.Enabled)
                    {
                        DelSelectedRecord_Click(sender, e);
                    }
                    else
                    {
                        if (this.DropElementToolStripMenuItem.Enabled)
                        {
                            DropElementToolStripMenuItem_Click(sender, e);
                        }
                    }
                    break;
                case Keys.F2:
                    if (this.ModifyElementToolStripMenuItem.Enabled)
                    {
                        ModifyElementToolStripMenuItem_Click(sender, e);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region"管理：元素操作"

        /// <summary>
        /// Add New Document
        /// </summary>
        private void NewDocument()
        {
            BsonValue id = MongoDBHelper.InsertEmptyDocument(SystemManager.GetCurrentCollection(), mDataViewInfo.IsSafeMode);
            if (id != BsonNull.Value)
            {
                TreeNode newDoc = new TreeNode(SystemManager.GetCurrentCollection().Name + "[" + (SystemManager.GetCurrentCollection().Count()).ToString() + "]");
                newDoc.Tag = id;
                TreeNode newid = new TreeNode("_id:" + id.ToString());
                newid.Tag = id;
                newDoc.Nodes.Add(newid);
                trvData.Nodes.Add(newDoc);
                tabDataShower.SelectedIndex = 0;
                trvData.SelectedNode = newid;
                IsNeedRefresh = true;
                RefreshStripButton_Click(null, null);
            }
            else
            {
                MyMessageBox.ShowMessage("Error", "New Document Error");
            }
        }
        /// <summary>
        /// Delete Selected Documents
        /// </summary>
        private void DeleteSelectedRecords()
        {
            String strTitle = "Delete Document";
            String strMessage = "Are you sure to delete selected document(s)?";
            if (!SystemManager.IsUseDefaultLanguage())
            {
                strTitle = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_Data);
                strMessage = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_Data_Confirm);
            }
            if (MyMessageBox.ShowConfirm(strTitle, strMessage))
            {
                if (tabDataShower.SelectedTab == tabTableView)
                {
                    //lstData
                    String strKey = lstData.Columns[0].Text;
                    foreach (ListViewItem item in lstData.SelectedItems)
                    {
                        MongoDBHelper.DropDocument(SystemManager.GetCurrentCollection(), item.Tag, strKey);
                    }
                    lstData.ContextMenuStrip = null;
                }
                else
                {
                    String strKey = trvData.SelectedNode.Nodes[0].Text.Split(":".ToCharArray())[0];
                    MongoDBHelper.DropDocument(SystemManager.GetCurrentCollection(), trvData.SelectedNode.Tag, strKey);
                    trvData.ContextMenuStrip = null;
                }
                DelSelectRecordToolStripMenuItem.Enabled = false;
                RefreshStripButton_Click(null, null);
            }
        }
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean IsElement = true;
            BsonValue t;
            if (trvData.SelectedNode.Tag is BsonElement)
            {
                t = ((BsonElement)trvData.SelectedNode.Tag).Value;
            }
            else
            {
                t = (BsonValue)trvData.SelectedNode.Tag;
            }
            if (t.IsBsonArray)
            {
                IsElement = false;
            }
            SystemManager.OpenForm(new frmElement(false, trvData.SelectedNode, IsElement));
            IsNeedRefresh = true;
        }
        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trvData.SelectedNode.Level == 1 & trvData.SelectedNode.PrevNode == null)
            {
                MyMessageBox.ShowMessage("Error", "_id Can't be delete");
                return;
            }
            if (trvData.SelectedNode.Parent.Text.EndsWith(MongoDBHelper.Array_Mark))
            {
                MongoDBHelper.DropArrayValue(trvData.SelectedNode.FullPath, trvData.SelectedNode.Index);
            }
            else
            {
                MongoDBHelper.DropElement(trvData.SelectedNode.FullPath, (BsonElement)trvData.SelectedNode.Tag);
            }
            trvData.Nodes.Remove(trvData.SelectedNode);
            IsNeedRefresh = true;
        }
        /// <summary>
        /// 修改元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trvData.SelectedNode.Level == 1 & trvData.SelectedNode.PrevNode == null)
            {
                MyMessageBox.ShowMessage("Error", "_id can't be modify");
                return;
            }
            if (trvData.SelectedNode.Parent.Text.EndsWith(MongoDBHelper.Array_Mark))
            {
                SystemManager.OpenForm(new frmElement(true, trvData.SelectedNode, false));
            }
            else
            {
                SystemManager.OpenForm(new frmElement(true, trvData.SelectedNode));
            }
            IsNeedRefresh = true;
        }
        /// <summary>
        /// Copy Element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MongoDBHelper._ClipElement = trvData.SelectedNode.Tag;
            if (trvData.SelectedNode.Parent.Text.EndsWith(MongoDBHelper.Array_Mark))
            {
                MongoDBHelper.CopyValue((BsonValue)trvData.SelectedNode.Tag);
            }
            else
            {
                MongoDBHelper.CopyElement((BsonElement)trvData.SelectedNode.Tag);
            }
        }
        /// <summary>
        /// Paste Element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trvData.SelectedNode.FullPath.EndsWith(MongoDBHelper.Array_Mark))
            {
                MongoDBHelper.PasteValue(trvData.SelectedNode.FullPath);
                TreeNode NewValue = new TreeNode(MongoDBHelper.ConvertToString((BsonValue)MongoDBHelper._ClipElement));
                NewValue.Tag = MongoDBHelper._ClipElement;
                trvData.SelectedNode.Nodes.Add(NewValue);
            }
            else
            {
                String PasteMessage = MongoDBHelper.PasteElement(trvData.SelectedNode.FullPath);
                if (String.IsNullOrEmpty(PasteMessage))
                {
                    //GetCurrentDocument()的第一个元素是ID
                    MongoDBHelper.AddBsonDocToTreeNode(trvData.SelectedNode,
                                                       new BsonDocument().Add((BsonElement)MongoDBHelper._ClipElement));
                }
                else
                {
                    MyMessageBox.ShowMessage("Exception", PasteMessage);
                }
            }
            IsNeedRefresh = true;
        }
        /// <summary>
        /// Cut Element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trvData.SelectedNode.Level == 1 & trvData.SelectedNode.PrevNode == null)
            {
                MyMessageBox.ShowMessage("Error", "_id can't be cut");
                return;
            }
            if (trvData.SelectedNode.Parent.Text.EndsWith(MongoDBHelper.Array_Mark))
            {
                MongoDBHelper.CutValue(trvData.SelectedNode.FullPath, trvData.SelectedNode.Index, (BsonValue)trvData.SelectedNode.Tag);
            }
            else
            {
                MongoDBHelper.CutElement(trvData.SelectedNode.FullPath, (BsonElement)trvData.SelectedNode.Tag);
            }
            trvData.Nodes.Remove(trvData.SelectedNode);
            IsNeedRefresh = true;
        }
        #endregion

        #region"管理：GFS"
        /// <summary>
        /// Upload File
        /// </summary>
        public void UploadFile()
        {
            OpenFileDialog upfile = new OpenFileDialog();
            if (upfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MongoDBHelper.UpLoadFile(upfile.FileName);
                RefreshStripButton_Click(null, null);
            }
        }
        /// <summary>
        /// DownLoad File
        /// </summary>
        public void DownloadFile()
        {
            SaveFileDialog downfile = new SaveFileDialog();
            String strFileName = lstData.SelectedItems[0].Text;
            //For Winodws,Linux user DirectorySeparatorChar Replace with @"\"
            downfile.FileName = strFileName.Split(System.IO.Path.DirectorySeparatorChar)[strFileName.Split(System.IO.Path.DirectorySeparatorChar).Length - 1];
            if (downfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MongoDBHelper.DownloadFile(downfile.FileName, strFileName);
            }
            RefreshStripButton_Click(null, null);
        }
        /// <summary>
        /// Open File
        /// </summary>
        private void OpenFileStripButton_Click(object sender, EventArgs e)
        {
            String strFileName = lstData.SelectedItems[0].Text;
            MongoDBHelper.OpenFile(strFileName);
        }
        /// <summary>
        /// Delete File
        /// </summary>
        public void DelFile()
        {
            String strTitle = "Delete Files";
            String strMessage = "Are you sure to delete selected File(s)?";
            if (!SystemManager.IsUseDefaultLanguage())
            {
                strTitle = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_Data);
                strMessage = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_Data_Confirm);
            }
            if (MyMessageBox.ShowConfirm(strTitle, strMessage))
            {
                if (tabDataShower.SelectedTab == tabTableView)
                {
                    String strFileName = lstData.SelectedItems[0].Text;
                    MongoDBHelper.DelFile(strFileName);
                    lstData.ContextMenuStrip = null;
                }
                else
                {
                    MongoDBHelper.DelFile(trvData.SelectedNode.Tag.ToString());
                    trvData.ContextMenuStrip = null;
                }
                RefreshStripButton_Click(null, null);
            }
        }
        #endregion

        #region"用户"
        /// <summary>
        /// Drop User from Admin Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveUserFromAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String strTitle = "Drop User";
            String strMessage = "Are you sure to delete user(s) from Admin Group?";
            if (!SystemManager.IsUseDefaultLanguage())
            {
                strTitle = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_User);
                strMessage = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_User_Confirm);
            }

            //这里也可以使用普通的删除数据的方法来删除用户。
            if (MyMessageBox.ShowConfirm(strTitle, strMessage))
            {
                if (tabDataShower.SelectedTab == tabTableView)
                {
                    //lstData
                    foreach (ListViewItem item in lstData.SelectedItems)
                    {
                        MongoDBHelper.RemoveUserFromSvr(item.SubItems[1].Text);
                    }
                    lstData.ContextMenuStrip = null;
                }
                else
                {
                    MongoDBHelper.RemoveUserFromSvr(trvData.SelectedNode.Tag.ToString());
                    trvData.ContextMenuStrip = null;
                }
                RemoveUserFromAdminToolStripMenuItem.Enabled = false;
                RefreshStripButton_Click(sender, e);
            }
        }
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String strTitle = "Drop User";
            String strMessage = "Are you sure to delete user(s) from this database";
            if (!SystemManager.IsUseDefaultLanguage())
            {
                strTitle = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_User);
                strMessage = SystemManager.mStringResource.GetText(StringResource.TextType.Drop_User_Confirm);
            }
            if (MyMessageBox.ShowConfirm(strTitle, strMessage))
            {
                if (tabDataShower.SelectedTab == tabTableView)
                {
                    //lstData
                    foreach (ListViewItem item in lstData.SelectedItems)
                    {
                        MongoDBHelper.RemoveUserFromDB(item.SubItems[1].Text);
                    }
                    lstData.ContextMenuStrip = null;
                }
                else
                {
                    MongoDBHelper.RemoveUserFromDB(trvData.SelectedNode.Tag.ToString());
                    trvData.ContextMenuStrip = null;
                }
                RemoveUserToolStripMenuItem.Enabled = false;
                RefreshStripButton_Click(sender, e);
            }
        }
        /// <summary>
        /// 密码变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mDataViewInfo.strDBTag.EndsWith(MongoDBHelper.DATABASE_NAME_ADMIN + "/" + MongoDBHelper.COLLECTION_NAME_USER))
            {
                SystemManager.OpenForm(new frmUser(true, lstData.SelectedItems[0].SubItems[1].Text));
            }
            else
            {
                SystemManager.OpenForm(new frmUser(false, lstData.SelectedItems[0].SubItems[1].Text));
            }
            RefreshStripButton_Click(sender, e);
        }
        #endregion

        #region"数据导航"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbRecPerPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (cmbRecPerPage.SelectedIndex)
            {
                case 0:
                    mDataViewInfo.LimitCnt = 50;
                    break;
                case 1:
                    mDataViewInfo.LimitCnt = 100;
                    break;
                case 2:
                    mDataViewInfo.LimitCnt = 200;
                    break;
                default:
                    mDataViewInfo.LimitCnt = 100;
                    break;
            }
            RefreshStripButton_Click(sender, e);
        }
        /// <summary>
        /// 第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstPage_Click(object sender, EventArgs e)
        {
            MongoDBHelper.PageChanged(MongoDBHelper.PageChangeOpr.FirstPage, ref mDataViewInfo, _dataShower);
            SetDataNav();
        }
        /// <summary>
        /// 前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrePage_Click(object sender, EventArgs e)
        {
            MongoDBHelper.PageChanged(MongoDBHelper.PageChangeOpr.PrePage, ref mDataViewInfo, _dataShower);
            SetDataNav();
        }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPage_Click(object sender, EventArgs e)
        {
            MongoDBHelper.PageChanged(MongoDBHelper.PageChangeOpr.NextPage, ref mDataViewInfo, _dataShower);
            SetDataNav();
        }
        /// <summary>
        /// 最后页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastPage_Click(object sender, EventArgs e)
        {
            MongoDBHelper.PageChanged(MongoDBHelper.PageChangeOpr.LastPage, ref mDataViewInfo, _dataShower);
            SetDataNav();
        }
        /// <summary>
        /// 展开所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandAll_Click(object sender, EventArgs e)
        {
            trvData.ExpandAll();
        }
        /// <summary>
        /// 折叠所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseAll_Click(object sender, EventArgs e)
        {
            trvData.CollapseAll();
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void clear()
        {
            lstData.Clear();
            txtData.Text = String.Empty;
            trvData.Nodes.Clear();
            lstData.ContextMenuStrip = null;
            trvData.ContextMenuStrip = null;
        }
        /// <summary>
        /// 设置导航可用性
        /// </summary>
        private void SetDataNav()
        {
            PrePageStripButton.Enabled = mDataViewInfo.HasPrePage;
            NextPageStripButton.Enabled = mDataViewInfo.HasNextPage;
            FirstPageStripButton.Enabled = mDataViewInfo.HasPrePage;
            LastPageStripButton.Enabled = mDataViewInfo.HasNextPage;
            this.FilterStripButton.Checked = mDataViewInfo.IsUseFilter;
            this.QueryStripButton.Enabled = true;
            String strTitle = "Records";
            if (!SystemManager.IsUseDefaultLanguage())
            {
                strTitle = SystemManager.mStringResource.GetText(StringResource.TextType.Main_Menu_DataView);
            }
            if (mDataViewInfo.CurrentCollectionTotalCnt == 0)
            {
                this.DataNaviToolStripLabel.Text = strTitle + "：0/0";
            }
            else
            {
                this.DataNaviToolStripLabel.Text = strTitle + "：" + (mDataViewInfo.SkipCnt + 1).ToString() + "/" + mDataViewInfo.CurrentCollectionTotalCnt.ToString();
            }
        }
        /// <summary>
        /// RefreshCtl
        /// </summary>
        public void RefreshCtl() {
            RefreshStripButton_Click(null, null);
        }
        /// <summary>
        /// Refresh Data
        /// </summary>
        private void RefreshStripButton_Click(object sender, EventArgs e)
        {
            this.clear();
            mDataViewInfo.SkipCnt = 0;
            MongoDBHelper.FillDataToControl(ref mDataViewInfo, _dataShower);
            SetDataNav();
            if (strNodeType != MongoDBHelper.COLLECTION_TAG)
            {
                this.EditDocStripButton.Enabled = false;
            }
            this.DelSelectRecordToolStripButton.Enabled = false;
            IsNeedRefresh = false;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueryStripButton_Click(object sender, EventArgs e)
        {
            SystemManager.OpenForm(new frmQuery(mDataViewInfo));
            Boolean HasContiditon = mDataViewInfo.mDataFilter.QueryConditionList.Count > 0 ? true : false;
            this.FilterStripButton.Enabled = HasContiditon;
            this.FilterStripButton.Checked = HasContiditon;
            mDataViewInfo.IsUseFilter = HasContiditon;
            //重新展示数据
            RefreshStripButton_Click(sender, e);
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterStripButton_Click(object sender, EventArgs e)
        {
            mDataViewInfo.IsUseFilter = !mDataViewInfo.IsUseFilter;
            this.FilterStripButton.Checked = mDataViewInfo.IsUseFilter;
            //过滤变更后，重新刷新
            RefreshStripButton_Click(sender, e);
        }
        #endregion

    }
}