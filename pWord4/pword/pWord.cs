using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using LL;
using System.IO;
using System.Runtime.InteropServices;
using Win32;
using UserActivityHook;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using LeftRight;
using pWordLib;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;

namespace myPword
{
	/// <summary> 
	/// pWord main form
	/// </summary>
    /// 
    public class pWord : System.Windows.Forms.Form
	{
        private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ContextMenu contextMenuNotify;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemShow;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ImageList imgToolbar1;
		private System.Windows.Forms.ToolBarButton toolBarTac;
		private System.Windows.Forms.ContextMenu cmTree;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.ImageList imageTree1;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItemBlank;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolBarButton toolBarView;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.NotifyIcon notifyIcon1;

        pNode p = new pNode();
        
			/// <summary>
			///   Add my VARIABLES HERE
			/// </summary>
			//
		bool VIS = false;
		
		// The master list for all views
		LL.List masterList = new LL.List();
			
		// flag_file is false if it's a new file, or upon opening program.
		// If flag_file is true... it's because the current tree was saved or opened.
		bool flag_file = false;
		string filename = "";
		string filenameHTML = "";
		pNode tmpNode = new pNode();  // used to store a tree node temporarily
		pNode getNode = new pNode();  // used for put op
		pNode moveNode = new pNode(); // moved pNode
		pNode xmlNode = new pNode();  // used for xml
		ArrayList xml = new ArrayList();
		int nodeIndex = 0;
		int xmlIndex = 0;



		public enum nodeMode
		{
			addto = 1,
			insert = 2,
			edit = 3
		}

		nodeMode mode = nodeMode.addto;  // see above
		int modeIndex = 0;  // this represents the index of interest depending on the mode

		public enum exportMode
		{
			treeview = 1,
			pNode = 2,
            treexml = 3
		}

		exportMode exportmode = exportMode.treeview;
	

		System.Drawing.Point StartPt; // Used for selecting multi nodes
		System.Drawing.Point StopPt;


		// TODO: Add TREE Stuff here
		//			pNode rootNode = new pNode("Master"

		private Image img;
		private ArrayList Nodes = new ArrayList();

        // TODO: decide if drag_flag needs to be used or not.
        private bool drag_flag = false;
		private bool autoHide_flag = false;
		private Point mousePT;
		private System.Windows.Forms.TextBox txtValue;
		private LeftRight.LeftRight userControl11;
		private System.Windows.Forms.ContextMenu cmMasters;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TextBox txtObject;
		private System.Windows.Forms.Label lblValue;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.CheckBox chkClear;
		private pView treeView1;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItem30;
		private System.Windows.Forms.MenuItem menuItem31;
		private System.Windows.Forms.MenuItem menuItem32;
		private System.Windows.Forms.SaveFileDialog saveFileDialogHTML;
		private System.Windows.Forms.MenuItem menuItem33;
		private System.Windows.Forms.MenuItem menuItem34;
		private System.Windows.Forms.MenuItem menuItem35;
        private ToolBarButton toolBarXML;
        private NotifyIcon notifyIcon2;
        private MenuItem menuItem2;

		// Import interop services

		pWordLib.UserActivityHook.UserActivityHook actHook;

		public pWord()
		{
            
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            // always check registry when starting pWord
            pWordLib.mgr.registryMgr rm = new pWordLib.mgr.registryMgr(pWordSettings.Default.version.ToString());


			//
			// DONE: Add any constructor code after InitializeComponent call
			//
            this.components = new System.ComponentModel.Container();

			// Create the NotifyIcon
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

			// the icon property sets the icon that will appear
			// in the systray for this application.
			notifyIcon1.Icon = new Icon(".\\icons\\PW.ico");

			// The context menu property sets the menu that will 
			// Appear when the systray icon is right clicked.
			this.notifyIcon1.ContextMenu = this.contextMenuNotify;

			// the text property sets the text that will be displayed 
			// in a tooltip, when the mouse hovers over the systray icon
			notifyIcon1.Text = "pWord (Notify Icon)";
			notifyIcon1.Visible = false;  // the notify icon should only be visible when it is minimized

			// Handle the double click event to activate the form.
			//notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            notifyIcon1.DoubleClick += new EventHandler(notifyIcon1_DoubleClick);

            actHook = new pWordLib.UserActivityHook.UserActivityHook();  // create an instance
			// hang on events
			actHook.OnMouseActivity+=new MouseEventHandler(MouseMoved);

            
			
		}

        void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // 
            this.WindowState = FormWindowState.Normal;
            this.autoHide_flag = false;
            VIS = true;

            this.autoHide_flag = false;
            this.statusBar1.Text = "AutoHide Inactive";
            this.toolBarTac.ImageIndex = 1;
            this.Visible = true;

            this.WindowState = FormWindowState.Normal;
            this.DockRight(sender, e);
            this.actHook.Stop();
            notifyIcon1.Visible = false;
            notifyIcon2.Visible = true;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				this.actHook.Stop();
				if (components != null) 
				{
					components.Dispose();
                    notifyIcon1.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pWord));
            this.contextMenuNotify = new System.Windows.Forms.ContextMenu();
            this.menuItemShow = new System.Windows.Forms.MenuItem();
            this.menuItemBlank = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarTac = new System.Windows.Forms.ToolBarButton();
            this.toolBarView = new System.Windows.Forms.ToolBarButton();
            this.toolBarXML = new System.Windows.Forms.ToolBarButton();
            this.imgToolbar1 = new System.Windows.Forms.ImageList(this.components);
            this.cmTree = new System.Windows.Forms.ContextMenu();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem30 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem32 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem31 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem34 = new System.Windows.Forms.MenuItem();
            this.menuItem35 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.imageTree1 = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.menuItem33 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cmMasters = new System.Windows.Forms.ContextMenu();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtObject = new System.Windows.Forms.TextBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.saveFileDialogHTML = new System.Windows.Forms.SaveFileDialog();
            this.notifyIcon2 = new System.Windows.Forms.NotifyIcon(this.components);
            this.userControl11 = new LeftRight.LeftRight();
            this.treeView1 = new myPword.pView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuNotify
            // 
            this.contextMenuNotify.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemShow,
            this.menuItemBlank,
            this.menuItemExit});
            this.contextMenuNotify.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuItemShow
            // 
            this.menuItemShow.Index = 0;
            this.menuItemShow.Text = "&Show";
            this.menuItemShow.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItemBlank
            // 
            this.menuItemBlank.Index = 1;
            this.menuItemBlank.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 2;
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 453);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(336, 22);
            this.statusBar1.TabIndex = 0;
            this.statusBar1.Text = "statusBar1";
            // 
            // toolBar1
            // 
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarTac,
            this.toolBarView,
            this.toolBarXML});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imgToolbar1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(336, 48);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarTac
            // 
            this.toolBarTac.Enabled = ((bool)(configurationAppSettings.GetValue("toolBarButton1.Enabled", typeof(bool))));
            this.toolBarTac.ImageIndex = ((int)(configurationAppSettings.GetValue("toolBarButton1.ImageIndex", typeof(int))));
            this.toolBarTac.Name = "toolBarTac";
            this.toolBarTac.Pushed = ((bool)(configurationAppSettings.GetValue("toolBarButton1.Pushed", typeof(bool))));
            this.toolBarTac.ToolTipText = "Move mouse to right for form to reappear.";
            // 
            // toolBarView
            // 
            this.toolBarView.Enabled = false;
            this.toolBarView.ImageIndex = 2;
            this.toolBarView.Name = "toolBarView";
            // 
            // toolBarXML
            // 
            this.toolBarXML.ImageIndex = 4;
            this.toolBarXML.Name = "toolBarXML";
            // 
            // imgToolbar1
            // 
            this.imgToolbar1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgToolbar1.ImageStream")));
            this.imgToolbar1.TransparentColor = System.Drawing.Color.Transparent;
            this.imgToolbar1.Images.SetKeyName(0, "");
            this.imgToolbar1.Images.SetKeyName(1, "");
            this.imgToolbar1.Images.SetKeyName(2, "");
            this.imgToolbar1.Images.SetKeyName(3, "");
            this.imgToolbar1.Images.SetKeyName(4, "XML.png");
            this.imgToolbar1.Images.SetKeyName(5, "Bin.png");
            // 
            // cmTree
            // 
            this.cmTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4,
            this.menuItem13,
            this.menuItem29,
            this.menuItem3,
            this.menuItem2,
            this.menuItem30,
            this.menuItem22,
            this.menuItem32,
            this.menuItem21,
            this.menuItem31,
            this.menuItem1,
            this.menuItem14,
            this.menuItem34,
            this.menuItem24,
            this.menuItem11});
            this.cmTree.Popup += new System.EventHandler(this.cmTree_Popup);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
            this.menuItem4.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.menuItem4.Text = "Add To";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.menuItem13.Text = "&Edit";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 2;
            this.menuItem29.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.menuItem29.Text = "&Insert";
            this.menuItem29.Click += new System.EventHandler(this.menuItem29_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.menuItem3.Text = "Copy";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click_1);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.Text = "A&ttributes";
            this.menuItem2.Click += new System.EventHandler(this.menuItemAttribute_Click);
            // 
            // menuItem30
            // 
            this.menuItem30.Index = 5;
            this.menuItem30.Text = "-";
            // 
            // menuItem22
            // 
            this.menuItem22.Index = 6;
            this.menuItem22.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
            this.menuItem22.Text = "Get Node";
            this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
            // 
            // menuItem32
            // 
            this.menuItem32.Index = 7;
            this.menuItem32.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuItem32.Text = "Cut Node";
            this.menuItem32.Click += new System.EventHandler(this.menuCutNode_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Enabled = false;
            this.menuItem21.Index = 8;
            this.menuItem21.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.menuItem21.Text = "Put Node In";
            this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
            // 
            // menuItem31
            // 
            this.menuItem31.Enabled = false;
            this.menuItem31.Index = 9;
            this.menuItem31.Shortcut = System.Windows.Forms.Shortcut.Ins;
            this.menuItem31.Text = "Insert Node";
            this.menuItem31.Click += new System.EventHandler(this.menuItem31_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Enabled = false;
            this.menuItem1.Index = 10;
            this.menuItem1.Text = "Encrypt Node";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 11;
            this.menuItem14.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.menuItem14.Text = "Delete Node";
            this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // menuItem34
            // 
            this.menuItem34.Index = 12;
            this.menuItem34.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem35});
            this.menuItem34.Text = "Export Node";
            // 
            // menuItem35
            // 
            this.menuItem35.Index = 0;
            this.menuItem35.Shortcut = System.Windows.Forms.Shortcut.F11;
            this.menuItem35.Text = "to XML/HTML";
            this.menuItem35.Click += new System.EventHandler(this.menuItem35_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 13;
            this.menuItem24.Text = "-";
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 14;
            this.menuItem11.Text = "Open Link [Dbl Lft Click]";
            this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
            // 
            // imageTree1
            // 
            this.imageTree1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageTree1.ImageStream")));
            this.imageTree1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageTree1.Images.SetKeyName(0, "");
            this.imageTree1.Images.SetKeyName(1, "");
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7,
            this.menuItem26,
            this.menuItem33,
            this.menuItem17});
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem8,
            this.menuItem9,
            this.menuItem10,
            this.menuItem15,
            this.menuItem23,
            this.menuItem25,
            this.menuItem16});
            this.menuItem7.Text = "&File";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 0;
            this.menuItem8.Text = "&New";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 1;
            this.menuItem9.Text = "&Open";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.Text = "&Save";
            this.menuItem10.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 3;
            this.menuItem15.Text = "-";
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 4;
            this.menuItem23.Text = "&Export XML";
            this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 5;
            this.menuItem25.Text = "-";
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 6;
            this.menuItem16.Text = "E&xit";
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 1;
            this.menuItem26.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem27,
            this.menuItem28});
            this.menuItem26.Text = "&View";
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 0;
            this.menuItem27.Text = "&Alphabetize a-Z";
            this.menuItem27.Click += new System.EventHandler(this.menuItem27_Click);
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 1;
            this.menuItem28.Text = "Alphabetize &Off";
            this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
            // 
            // menuItem33
            // 
            this.menuItem33.Index = 2;
            this.menuItem33.Text = "&Undo";
            this.menuItem33.Visible = false;
            this.menuItem33.Click += new System.EventHandler(this.menuItem33_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 3;
            this.menuItem17.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem12,
            this.menuItem18});
            this.menuItem17.MergeType = System.Windows.Forms.MenuMerge.Remove;
            this.menuItem17.Text = "&About";
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Enabled = false;
            this.menuItem12.Index = 0;
            this.menuItem12.Text = " &Help";
            this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click_1);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 1;
            this.menuItem18.Text = "&About pWord";
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "pWord files|*.pwd";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "pWord files|*.pwd";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // txtValue
            // 
            this.txtValue.BackColor = System.Drawing.SystemColors.Info;
            this.txtValue.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtValue.Location = new System.Drawing.Point(0, 419);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtValue.Size = new System.Drawing.Size(336, 34);
            this.txtValue.TabIndex = 3;
            this.txtValue.TabStop = false;
            // 
            // cmMasters
            // 
            this.cmMasters.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem19,
            this.menuItem20});
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 0;
            this.menuItem19.Text = "Add Master";
            this.menuItem19.Click += new System.EventHandler(this.menuItem19_Click);
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 1;
            this.menuItem20.Text = "Delete Master";
            this.menuItem20.Click += new System.EventHandler(this.menuItem20_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 411);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(336, 8);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 339);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(336, 104);
            this.panel2.TabIndex = 3;
            this.panel2.VisibleChanged += new System.EventHandler(this.panel2_VisibleChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtObject);
            this.panel4.Controls.Add(this.lblValue);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 24);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(332, 40);
            this.panel4.TabIndex = 2;
            // 
            // txtObject
            // 
            this.txtObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObject.Location = new System.Drawing.Point(56, 0);
            this.txtObject.Multiline = true;
            this.txtObject.Name = "txtObject";
            this.txtObject.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObject.Size = new System.Drawing.Size(276, 40);
            this.txtObject.TabIndex = 1;
            // 
            // lblValue
            // 
            this.lblValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblValue.Location = new System.Drawing.Point(0, 0);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(56, 40);
            this.lblValue.TabIndex = 0;
            this.lblValue.Text = "Value:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtName);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(332, 24);
            this.panel3.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtName.Location = new System.Drawing.Point(56, 0);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(276, 20);
            this.txtName.TabIndex = 1;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.chkClear);
            this.panel5.Controls.Add(this.btnCancel);
            this.panel5.Controls.Add(this.btnAdd);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 68);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(332, 32);
            this.panel5.TabIndex = 4;
            // 
            // chkClear
            // 
            this.chkClear.Checked = true;
            this.chkClear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClear.Location = new System.Drawing.Point(88, 0);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(96, 24);
            this.chkClear.TabIndex = 2;
            this.chkClear.TabStop = false;
            this.chkClear.Text = "Clear Text?";
            this.chkClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkClear.ThreeState = true;
            this.chkClear.CheckedChanged += new System.EventHandler(this.chkClear_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(197, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 24);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Controls.Add(this.treeView1);
            this.panel6.Location = new System.Drawing.Point(0, 104);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(336, 235);
            this.panel6.TabIndex = 5;
            // 
            // saveFileDialogHTML
            // 
            this.saveFileDialogHTML.Filter = "XML format|*.xml|HTML format|*.html|All Files|*.*";
            this.saveFileDialogHTML.Title = "Save As XML//HTML";
            this.saveFileDialogHTML.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogHTML_FileOk);
            // 
            // notifyIcon2
            // 
            this.notifyIcon2.Text = "notifyIcon2";
            this.notifyIcon2.Visible = true;
            this.notifyIcon2.DoubleClick += new System.EventHandler(this.notifyIcon2_DoubleClick);
            this.notifyIcon2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon2_MouseClick);
            this.notifyIcon2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon2_MouseDoubleClick);
            // 
            // userControl11
            // 
            this.userControl11.ContextMenu = this.cmMasters;
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl11.Location = new System.Drawing.Point(0, 48);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(336, 24);
            this.userControl11.TabIndex = 4;
            this.userControl11.TabStop = false;
            this.userControl11.LeftClicked += new System.EventHandler(this.userControl11_LeftClicked);
            this.userControl11.RightClicked += new System.EventHandler(this.userControl11_RightClicked);
            this.userControl11.Load += new System.EventHandler(this.userControl11_Load);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.ContextMenu = this.cmTree;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.HotTracking = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageTree1;
            this.treeView1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Scrollable = ((bool)(configurationAppSettings.GetValue("treeView1.Scrollable", typeof(bool))));
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(336, 235);
            this.treeView1.TabIndex = 3;
            this.treeView1.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCollapse_1);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand_1);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect_1);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop_1);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
            this.treeView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treeView1_KeyPress);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown_1);
            this.treeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseMove_1);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp_1);
            // 
            // pWord
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(336, 475);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.statusBar1);
            this.Enabled = ((bool)(configurationAppSettings.GetValue("pWord.Enabled", typeof(bool))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "pWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "pWord";
            this.TopMost = ((bool)(configurationAppSettings.GetValue("pWord.TopMost", typeof(bool))));
            this.Deactivate += new System.EventHandler(this.pWord_Deactivate);
            this.Load += new System.EventHandler(this.pWord_Load);
            this.VisibleChanged += new System.EventHandler(this.pWord_VisibleChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pWord_MouseDown);
            this.MouseLeave += new System.EventHandler(this.pWord_MouseLeave);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new pWord());
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			// EXIT
			this.Dispose(true);
			this.Close();
		}

		private void pWord_Load(object sender, System.EventArgs e)
		{
			this.Dock = System.Windows.Forms.DockStyle.Bottom;
			System.Drawing.Rectangle a  = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
			this.DockRight(sender,e);
			this.UpdateTree();
			
			this.autoHide_flag = false;
			VIS = true;

			//			this.WindowState = FormWindowState.Normal;
			
			this.autoHide_flag = false;
			this.statusBar1.Text = "AutoHide Inactive";
			this.toolBarTac.ImageIndex = 1;
			this.Visible = true;

			this.WindowState = FormWindowState.Normal;
			this.DockRight(sender,e);
			this.actHook.Stop();

		}

		struct Masters
		{
			string Name;
			object Value;
		}

		

		public void MouseMoved(object sender, MouseEventArgs e)
		{
            try
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    notifyIcon2.Visible = true;
                    if ((this.autoHide_flag == true) && (this.Visible == false))
                    {
                        if (e.X >= Screen.PrimaryScreen.Bounds.Right - 1)
                        {
                            if ((e.Y >= (Screen.PrimaryScreen.Bounds.Top + 64)) && (e.Y <= (Screen.PrimaryScreen.Bounds.Bottom - 80)))
                            {
                                this.VIS = true;
                                this.Visible = true;

                            }
                        }
                        else if (e.X < (Screen.PrimaryScreen.Bounds.Right - this.Width))
                        {
                            this.VIS = false;
                            this.Visible = false;

                        }
                    }
                    else if ((this.autoHide_flag == true) && (this.Visible == true))
                    {
                        if (e.X < (Screen.PrimaryScreen.Bounds.Right - this.Width))
                        {
                            this.VIS = false;
                            this.Visible = false;

                        }

                    }
                }
                else 
                {
                    //Console.WriteLine("How did I get here? Windows State: {0}", this.WindowState.ToString());
                    if (e.X < (Screen.PrimaryScreen.Bounds.Right - this.Width))
                    {
                        this.VIS = false;
                        this.Visible = false;
                        notifyIcon2.Visible = false;
                        notifyIcon1.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

				
		}


		private void pWord_VisibleChanged(object sender, System.EventArgs e)
		{
			
			invisible(VIS);
			this.Activate();
			this.treeView1.Focus();
			this.DockRight(sender,e);
			
		}
		
		private void invisible(bool vis)
		{
			if (VIS == true)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
		}

		protected Screen HostingScreen
		{
			get { return Screen.FromRectangle( this.Bounds ); }
		}

		private void DockCenter(object sender, EventArgs e)
		{
			this.Size = this.MinimumSize; // shrink to min size

			int centerX = HostingScreen.WorkingArea.Width / 2;
			int centerY = HostingScreen.WorkingArea.Height / 2;

			this.Location = new Point(
				centerX - this.Width / 2, 
				centerY - this.Height / 2
				);
		}

		private void DockLeft( object sender, EventArgs e )
		{
			this.Height = HostingScreen.WorkingArea.Height;
			this.Location = new Point( 0, 0 );
		}

		private void DockRight( object sender, EventArgs e )
		{
			this.Height = HostingScreen.WorkingArea.Height;
			this.Location = new Point( 
				HostingScreen.WorkingArea.Width - this.Width,
				0
				);
		}

		private void DockBottom( object sender, EventArgs e )
		{
			this.Height = this.MinimumSize.Height;
			this.Width = HostingScreen.WorkingArea.Width;

			this.Location = new Point( 
				0,
				HostingScreen.WorkingArea.Height - this.Height
				);
		}

		private void DockTop(object sender, EventArgs e)
		{
			this.Height = this.MinimumSize.Height;
			this.Width = HostingScreen.WorkingArea.Width;

			this.Location = new Point(0,0);
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			VIS = true;
			this.Visible = true;
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{

		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			VIS = true;
			this.Visible = true;
		}

		private void UpdateTree()
		{
			// Update Treeview




		}

		private void NewTree()
		{
			// Use NewTree when Loading a new TREEVIEW
			//masterNode = new pNode();
			//	subNode = new pNode();

			img = new Image();  // contains number's representing the items contained
			// in the image list.

		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			flag_file = false;  // notice... I don't want you saving new stuff over your old refined work.

			AYS ays = new AYS();
			// ARE YOU SURE? This operation will delete all of your work unless you have saved.
			// (Yes / No)

			ays.ShowDialog();
			if (ays.DialogResult == DialogResult.OK)
			{

				userControl11.Masters.Clear();
				userControl11.MastersValue.Clear();
				treeView1.Nodes.Clear();
				pNode masterNode;
				masterNode = new pNode("MASTER");
				TreePics apic = new TreePics("Master",img.GroupUp,img.GroupDown);
				masterNode.Tag = "MASTER";

				this.treeView1.Nodes.Add(masterNode);

				userControl11.index = 0;  // For some reason it loses track of index?
				userControl11.Masters.Add("MASTER");
				userControl11.MastersValue.Add(masterNode);
				userControl11.txtMaster.Text = (string)userControl11.Masters[userControl11.index];	
				this.tmpNode = (pNode)treeView1.Nodes[0];
			}

		}


		private void autosave()
		{
			// autosave
			try
			{
				//	Nodes[0] = userControl11;
				int count = userControl11.Masters.Count;
				Nodes.Clear();
				Nodes.Add(count);

				for (int i = 0;i < count; i++)
				{
					Nodes.Add((string)userControl11.Masters[i]);
					Nodes.Add((pNode)userControl11.MastersValue[i]);
				}

				//this.saveFileDialog1.ShowDialog();
				//string filename = this.saveFileDialog1.FileName;
				if (filename != null)
				{
					IFormatter formatter = new BinaryFormatter();
					Stream stream = new FileStream(filename,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
					formatter.Serialize(stream, Nodes);
					stream.Close();
				}
				flag_file = true;
			}
			catch(Exception f)
			{
				MessageBox.Show("You had an error while saving. " + f.Message,"SAVE ERROR",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}

		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			
			mode = nodeMode.addto;
			try
			{
//				AddItem dlg = new AddItem();
//				dlg.ShowDialog();
//				LL.List l = dlg.group;
//
//				if (dlg.DialogResult == DialogResult.OK)
//				{
//					// Add the master node to Nodes
//					pNode masterNode = treeView1.Nodes[0];
//					pNode aNode;
//					aNode = new pNode((string)l.RemoveFromFront());
//					TreePics apic = new TreePics("aNode",img.GroupUp,img.GroupDown);
//					aNode.Tag = l.RemoveFromFront();
//
//					treeView1.SelectedNode.Nodes.Add(aNode);
//
//					//Nodes[0] = masterNode;
//					userControl11.MastersValue[userControl11.index] = masterNode;

				// Change from Add Dialog to local members for adding name and value

//					if (flag_file == true)
//					{
//						autosave();
//					}
			//	this.tmpNode.Nodes.Clear();
				this.tmpNode = (pNode)treeView1.SelectedNode;

				this.txtName.Focus();
				this.statusBar1.Text = "Add to Mode";
				if (this.chkClear.Checked == true)
				{
					this.txtName.Clear();
					this.txtObject.Clear();
				}
//				}
				}
				
			catch (Exception f)
			{
				MessageBox.Show(f.Message);
			}
		
			// master node should be called by Nodes[0]...

			//			this.treeView1.Nodes.Add(Nodes[0]);
			//			Nodes[0].Nodes.Add(apic.PicNode);
	
        }

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
			this.autoHide_flag = false;
			VIS = true;

					this.autoHide_flag = false;
					this.statusBar1.Text = "AutoHide Inactive";
					this.toolBarTac.ImageIndex = 1;
			this.Visible = true;

			this.WindowState = FormWindowState.Normal;
						this.DockRight(sender,e);
			this.actHook.Stop();

		}

		private void treeView1_DragLeave(object sender, System.EventArgs e)
		{
			

		}

		private void treeView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			pNode a = new pNode("test",img.GroupUp,img.GroupDown);
			a.Tag = sender;

			this.treeView1.SelectedNode.Nodes.Add(a);
		}

		private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			if(e.Clicks == 1)
			{

				try
				{
					// if left mouse is clicked then do this
					treeView1.DoDragDrop(treeView1.SelectedNode.Tag,DragDropEffects.Copy);
				}
				catch(Exception f)
				{

				}
				this.drag_flag = true; // ok I'm down
			}
		}

		private void treeView1_LocationChanged(object sender, System.EventArgs e)
		{

		}

		private void treeView1_CursorChanged(object sender, System.EventArgs e)
		{
			int a = 1;
			int b = a;


		}

		private void menuSave_Click(object sender, System.EventArgs e)
		{
			
//			AYS ays = new AYS();
//			ays.label1.Text="Saving may delete your previous work.  Are you sure?";
//			ays.ShowDialog();
//			
//			if (ays.DialogResult == DialogResult.OK)
//			{

				try
				{
					//	Nodes[0] = userControl11;
					int count = userControl11.Masters.Count;
					Nodes.Clear();
					Nodes.Add(count);

					for (int i = 0;i < count; i++)
					{
						Nodes.Add((string)userControl11.Masters[i]);
						Nodes.Add((pNode)userControl11.MastersValue[i]);
					}
					this.saveFileDialog1.FileName = filename;
					this.saveFileDialog1.ShowDialog();

				}
				catch(Exception f)
				{
					MessageBox.Show("You had an error while saving. " + f.Message,"SAVE ERROR",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
				}
//			}
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{

			try
			{
				this.Nodes.Clear();
				this.userControl11.MastersValue.Clear();
				this.userControl11.Masters.Clear();

				this.openFileDialog1.FileName = filename;
				this.openFileDialog1.ShowDialog();
			}
			catch (Exception f)
			{
				MessageBox.Show("You had an error while loading. Please select the proper file. " + f.Message,"OPEN ERROR",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}
		}

		private void menuItem14_Click(object sender, System.EventArgs e)
		{
			try
			{
				// Add the master node to Nodes
				pNode masterNode;
				if (treeView1.SelectedNode.Parent != null)
				{
					treeView1.SelectedNode.Remove();
					masterNode = (pNode)treeView1.Nodes[0];
//					Nodes[0] = masterNode;
					userControl11.MastersValue[userControl11.index] = masterNode;

					if (flag_file == true)
					{
						autosave();
					}
				}
				else 
					MessageBox.Show("You must not delete the master node.");
			}
			catch (Exception f)
			{
				MessageBox.Show(f.Message);
			}
		}

		private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			this.drag_flag = false;
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			try
			{
				if (e.Button == this.toolBarTac)
				{
				
					if (this.autoHide_flag == true)
					{
						this.autoHide_flag = false;
						this.statusBar1.Text = "AutoHide Inactive";
						this.toolBarTac.ImageIndex = 1;
						this.actHook.Stop();
					}
					else
					{
						this.autoHide_flag = true;
						this.statusBar1.Text = "AutoHide Active";
						this.toolBarTac.ImageIndex = 0;
						this.actHook.Start();
					}
				}
				else if (e.Button == this.toolBarView)
				{
					if (filenameHTML != null)
					{
						if (exportmode == exportMode.treeview)
						{
							xml.Clear();  // clear out contents first.
							CallRecursive(treeView1);
						}
						else if (exportmode == exportMode.pNode)
						{
							xml.Clear(); // clear out contents first.
							CallRecursive(xmlNode);
						}
																  
					
						StreamWriter swFromFile = new StreamWriter(filenameHTML);

						swFromFile.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>"); 
						for (int i=0; i<xml.Count;i++)
						{
							swFromFile.Write(xml[i]);
						}
						swFromFile.Flush();
						swFromFile.Close();
						System.Diagnostics.Process.Start(filenameHTML);
					}

                    else
                    {
                        MessageBox.Show("You must first export a NODE to XML or HTML.");
                    }
				}
                else if (e.Button == this.toolBarXML)
                {

                    //XmlSerializer xs = new XmlSerializer(typeof(pNode));
                    //StringWriter sw = new StringWriter();
                    //xs.Serialize(sw, treeView1.Nodes[0]);
                }
			}
			catch(Exception f)
			{
				MessageBox.Show("You must first Export a NODE to XML or HTML format." + f.Message);
			}

		}

		private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
		{
		
		}

		private void pWord_MouseLeave(object sender, System.EventArgs e)
		{

		}

		private void pWord_Deactivate(object sender, System.EventArgs e)
		{
			if (this.autoHide_flag == true)
			{
				this.VIS = false;
				this.Visible = false;
			}
		}

		private void pWord_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void menuItem3_Click_1(object sender, System.EventArgs e)
		{


			Clipboard.SetDataObject(this.treeView1.SelectedNode.Tag,true);
			this.statusBar1.Text = "Copy Value Text Mode";
		}

		private void treeView1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			pNode a = (pNode)treeView1.GetNodeAt(e.X,e.Y);

			if (a != null)
			{
				treeView1.SelectedNode = a;
			this.txtValue.Text = (string)a.Tag;
			}

		}

		private void menuItem17_Click(object sender, System.EventArgs e)
		{
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog();

		}

		private void menuItem12_Click(object sender, System.EventArgs e)
		{


		}

		private void treeView1_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{

		}

		private void treeView1_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{

		}

		private void treeView1_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (treeView1.SelectedNode != null)
				treeView1.SelectedNode.SelectedImageIndex = 1;
		}

		private void treeView1_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (treeView1.SelectedNode != null)
									treeView1.SelectedNode.SelectedImageIndex = 0;
		}

		private void treeView1_ChangeUICues(object sender, System.Windows.Forms.UICuesEventArgs e)
		{
		
		}

		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			treeView1.SelectedNode.SelectedImageIndex = img.GroupDown;
		}

		private void menuItem18_Click(object sender, System.EventArgs e)
		{
			// Help add support documentation
			// this will access the documentation portion of this project
			frmAbout dlg = new frmAbout();
			dlg.ShowDialog();
		}

		private void userControl11_Load(object sender, System.EventArgs e)
		{
			pNode masterNode;
			masterNode = new pNode("MASTER");
			TreePics apic = new TreePics("Master",img.GroupUp,img.GroupDown);
			masterNode.Tag = "MASTER";

			this.treeView1.Nodes.Add(masterNode);

			userControl11.Masters.Add("MASTER");
			userControl11.MastersValue.Add(masterNode);
			userControl11.txtMaster.Text = (string)userControl11.Masters[userControl11.index];
			this.tmpNode = (pNode)treeView1.Nodes[0];
			// master node should be called by Nodes[0]...
		}			// Add the master node to Nodes


		private void menuItem19_Click(object sender, System.EventArgs e)
		{
			// Add master
			AddMaster dlg = new AddMaster();
			dlg.ShowDialog();
			
			if (dlg.DialogResult == DialogResult.OK)
			{
				this.treeView1.Nodes.Clear();
				pNode masterNode;
				masterNode = new pNode("MASTER");
				TreePics apic = new TreePics("masterNode",img.GroupUp,img.GroupDown);
				masterNode.Tag = "MASTER";

				this.treeView1.Nodes.Add(masterNode);
				
				userControl11.Masters.Add(dlg.txtMaster.Text);
				userControl11.MastersValue.Add(masterNode);
				userControl11.index++;				
				userControl11.txtMaster.Text = (string)userControl11.Masters[userControl11.index];
                this.tmpNode = (pNode)treeView1.Nodes[0];  // Always start with master
			}
		}
	
		private void menuItem20_Click(object sender, System.EventArgs e)
		{

		}

		private void userControl11_LeftClicked(object sender, System.EventArgs e)
		{

			// This is the hardest of all
			this.treeView1.Nodes.Clear();
			pNode masterNode = (pNode)userControl11.MastersValue[userControl11.index];
			TreePics apic = new TreePics("masterNode",img.GroupUp,img.GroupDown);
			this.treeView1.Nodes.Add(masterNode);


		}

		private void userControl11_RightClicked(object sender, System.EventArgs e)
		{
			// This is the hardest of all
			this.treeView1.Nodes.Clear();
			pNode masterNode = (pNode)userControl11.MastersValue[userControl11.index];
			TreePics apic = new TreePics("masterNode",img.GroupUp,img.GroupDown);
			this.treeView1.Nodes.Add(masterNode);
		}

		private void menuItem16_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(this.treeView1.SelectedNode.Tag.ToString());
			}
			catch(Exception f)
			{
				//MessageBox.Show("You must use an acceptable link contained in the value field!","DANGER",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtValue.Text = "Value Field is Empty.";
			}
		}

		private void menuItem12_Click_1(object sender, System.EventArgs e)
		{
			// help

		}

		private void panel2_VisibleChanged(object sender, System.EventArgs e)
		{
		
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if (mode == nodeMode.addto)
			{
				try
				{
					// Add the master node to Nodes
                    pNode masterNode = (pNode)treeView1.Nodes[0];
					pNode aNode;
					aNode = new pNode(this.txtName.Text);
					TreePics apic = new TreePics("aNode",img.GroupUp,img.GroupDown);
					aNode.Tag = this.txtObject.Text;
                    aNode.Text = this.txtName.Text;
					treeView1.SelectedNode = tmpNode;
					treeView1.SelectedNode.Nodes.Add(aNode);

					//tmpNode.Nodes.Add(aNode);

					//Nodes[0] = masterNode;
					userControl11.MastersValue[userControl11.index] = masterNode;

					// Change from Add Dialog to local members for adding name and value

					// check box
					if (this.chkClear.Checked == true)
					{
						this.txtName.Clear();
						this.txtObject.Clear();
					}

					if (flag_file == true)
					{
						autosave();
					}


					this.txtName.Focus();
				}
				catch (Exception f)
				{
					MessageBox.Show(f.Message);
				}
			}
			else if (mode == nodeMode.edit)
			{
				try
				{
					// EDIT MODE
					// Only edit the current node
                    pNode masterNode = (pNode)treeView1.Nodes[0];
					pNode aNode;
					aNode = new pNode(this.txtName.Text);
					TreePics apic = new TreePics("aNode",img.GroupUp,img.GroupDown);
					aNode.Tag = this.txtObject.Text;
					treeView1.SelectedNode = this.tmpNode;
					treeView1.SelectedNode.Text = aNode.Text;
					treeView1.SelectedNode.Tag = aNode.Tag;
					//treeView1.SelectedNode.Nodes[this.modeIndex] = aNode;
					//tmpNode.Nodes.Add(aNode);

					//Nodes[0] = masterNode;
					userControl11.MastersValue[userControl11.index] = masterNode;

					// Change from Add Dialog to local members for adding name and value

					// check box
					if (this.chkClear.Checked == true)
					{
						this.txtName.Clear();
						this.txtObject.Clear();
					}

					if (flag_file == true)
					{
						autosave();
					}
				}
				catch(Exception f)
				{
					MessageBox.Show(f.Message);
				}
			}
			else if (mode == nodeMode.insert)
			{
				try
				{
					// Insert Mode
					// Only edit the current node
                    pNode masterNode = (pNode)treeView1.Nodes[0];
					pNode aNode;
					aNode = new pNode(this.txtName.Text);
					TreePics apic = new TreePics("aNode",img.GroupUp,img.GroupDown);
					aNode.Tag = this.txtObject.Text;
					treeView1.SelectedNode = tmpNode;
					treeView1.SelectedNode.Nodes.Insert(modeIndex,aNode);
					//tmpNode.Nodes.Add(aNode);

					//Nodes[0] = masterNode;
					userControl11.MastersValue[userControl11.index] = masterNode;

					// Change from Add Dialog to local members for adding name and value

					// check box
					if (this.chkClear.Checked == true)
					{
						this.txtName.Clear();
						this.txtObject.Clear();
					}

					if (flag_file == true)
					{
						autosave();
					}
				}
				catch(Exception f)
				{
					MessageBox.Show(f.Message);
				}
			}

		}


	

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			panel2.Visible = false;
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
		
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void treeView1_MouseMove_1(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            pNode a = (pNode)treeView1.GetNodeAt(e.X, e.Y);

			if (a != null)
			{
				treeView1.SelectedNode = a;

				if (a.Text == "password")
				{
					this.txtValue.Text="password";	
				}
				else
				this.txtValue.Text = (string)a.Tag;
			}
		}

		private void treeView1_AfterCollapse_1(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (treeView1.SelectedNode != null)
				treeView1.SelectedNode.SelectedImageIndex = 0;

		}

		private void treeView1_AfterExpand_1(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (treeView1.SelectedNode != null)
				treeView1.SelectedNode.SelectedImageIndex = 1;

		}

		private void treeView1_AfterSelect_1(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			treeView1.SelectedNode.SelectedImageIndex = img.GroupDown;

		}

		private void treeView1_DragDrop_1(object sender, System.Windows.Forms.DragEventArgs e)
		{
			pNode a = new pNode("test",img.GroupUp,img.GroupDown);
			a.Tag = sender;

			this.treeView1.SelectedNode.Nodes.Add(a);
		}


		private void treeView1_MouseDown_1(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.Clicks == 1)
                {

                    try
                    {

                        this.StartPt.X = e.X;
                        this.StartPt.Y = e.Y;

                        ///
                        /// TODO:Fix Multiple Select Behavior in treeView1
                        /// 
                        /// 

                        // if left mouse is clicked then do this
                        //					    tmpNode = treeView1.SelectedNode;

                        /// TODO: Get rid of DoDragDrop behavior is possible
                        /// Try and save 
                        treeView1.DoDragDrop(treeView1.SelectedNode.Tag, DragDropEffects.Copy);
                        moveNode = (pNode)this.treeView1.SelectedNode;
                    }
                    catch (Exception f)
                    {

                    }


                    this.drag_flag = true; // ok I'm down
                }
                else if (e.Clicks == 2)
                {
                    System.EventArgs generic = new System.EventArgs();
                    menuItem11_Click(sender, generic);
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // the context menu is being called... 
                // now record the node
                object s = sender;
                Console.WriteLine("Duh!");
            }

		}
		private void treeView1_MouseUp_1(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			/// TODO: Use StopPt and StartPt to select multiple nodes
			/// Place these nodes in an ArrayList and when the PutNode is used
			/// place All the nodes
			/// also behavior... start from StartPt and always end with end point so 
			/// that if things need to get flipped around it will be possible.
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
				this.drag_flag = false;
		}

		private void menuItem13_Click(object sender, System.EventArgs e)
		{

			mode = nodeMode.edit;
			
			try
			{
				this.modeIndex=treeView1.SelectedNode.Index;
//				this.txtName.Text = treeView1.SelectedNode.Nodes[modeIndex].Text;
//				this.txtObject.Text = treeView1.SelectedNode.Nodes[modeIndex].Text;
                this.tmpNode = (pNode)treeView1.SelectedNode;
				this.txtName.Text = tmpNode.Text;
				this.txtObject.Text = (string)tmpNode.Tag;
				this.statusBar1.Text = "Edit Mode";
				this.txtName.Focus();
			}
			catch(Exception f)
			{
				MessageBox.Show(f.Message);
			}
//			this.tmpNode = treeView1.SelectedNode;
//
//			this.txtName.Focus();
//			This about adding a flag called edit mode  same to Add In
		}

		private void menuItem21_Click(object sender, System.EventArgs e)
		{
			// put in
			try
			{
				treeView1.SelectedNode.Nodes.Insert(treeView1.SelectedNode.Nodes.Count,this.getNode);
				this.getNode = (pNode)this.getNode.Clone();

				if (flag_file == true)
				{
					autosave();
				}

			}
			catch(Exception f)
			{
				MessageBox.Show(f.Message);
			}

		}

		private void menuItem22_Click(object sender, System.EventArgs e)
		{
			
			// get
			try
			{

				this.getNode = (pNode)treeView1.SelectedNode.Clone();
				this.menuItem21.Enabled = true;
				this.menuItem31.Enabled = true;
				this.nodeIndex = treeView1.SelectedNode.Index;
				this.statusBar1.Text = "Get Node Mode";
			}
			catch (Exception f)
			{
				MessageBox.Show(f.Message);
			}
		}

		private void menuItem23_Click(object sender, System.EventArgs e)
		{
		
							xml.Clear();  // clear out contents first.
			CallRecursive(treeView1);
			try
			{
				exportmode = exportMode.treeview;

				this.saveFileDialogHTML.FileName = filenameHTML;
				this.saveFileDialogHTML.Title = "Save View to XML/HTML";
				this.saveFileDialogHTML.ShowDialog();
				filenameHTML = this.saveFileDialogHTML.FileName;
				if (filenameHTML != null)
				{
				


//					IFormatter formatter = new BinaryFormatter();
//					Stream stream = new FileStream(this.saveFileDialog1.FileName,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
//					for (int i=0; i<xml.Count;i++)
//					{
//						formatter.Serialize(stream, xml[i].ToString());
//					}
//					this.filename = this.saveFileDialog1.FileName;
//					stream.Close();
//					FileStream fs = new FileStream(filename,
//						FileMode.CreateNew, FileAccess.Write, FileShare.None);
    



				this.toolBarView.Enabled = true;
				}







			}
			catch(Exception f)
			{
				MessageBox.Show("You had an error while exporting to XML. " + f.Message,"SAVE ERROR",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}


		}

		private void PrintRecursive(pNode pNode)
		{
			string [] split = null;
			string delimStr = " ";
			char [] delimiter = delimStr.ToCharArray();

			// Print the node.
		//	System.Diagnostics.Debug.WriteLine(pNode.Text);
		//	MessageBox.Show(pNode.Text);
			// Print each node recursively.
			foreach (pNode tn in pNode.Nodes)
			{
				split = null;
				xml.Add("<"); xml.Add(tn.Text); xml.Add(">");
				xml.Add("\n");
				xml.Add((string)tn.Tag);
				PrintRecursive(tn);
				split = tn.Text.Split(delimiter,2);
				xml.Add("</"); xml.Add(split[0]); xml.Add(">"); xml.Add("\n");
			}
		}


		// Call the procedure using the TreeView.
		private void CallRecursive(pView treeView)
		{
			string [] split = null;
			string delimStr = " ";
			char [] delimiter = delimStr.ToCharArray();


			// Print each node recursively.
			TreeNodeCollection nodes = treeView.Nodes;
			foreach (pNode n in nodes)
			{
				split =null;
				xml.Add("<"); xml.Add(n.Text); xml.Add(">");
				xml.Add("\n");
				xml.Add((string)n.Tag);
				PrintRecursive(n);
				split = n.Text.Split(delimiter,2);
				xml.Add("</"); xml.Add(split[0]); xml.Add(">"); xml.Add("\n");
			}
		}

		private void CallRecursive(pNode pNode)
		{
			string [] split = null;
			string delimStr = " ";
			char [] delimiter = delimStr.ToCharArray();


			// Print each node recursively.
			TreeNodeCollection nodes = pNode.Nodes;
			foreach (pNode n in nodes)
			{
				split =null;
				xml.Add("<"); xml.Add(n.Text); xml.Add(">");
				xml.Add("\n");
				xml.Add((string)n.Tag);
				PrintRecursive(n);
				split = n.Text.Split(delimiter,2);
				xml.Add("</"); xml.Add(split[0]); xml.Add(">"); xml.Add("\n");
			}
		}

		private void menuItem27_Click(object sender, System.EventArgs e)
		{
			AYS dlg = new AYS();
			dlg.label1.Text="ARE YOU SURE???\nSorting may change your structure.";
			dlg.ShowDialog();
			if (dlg.DialogResult == DialogResult.OK)
			{
				this.treeView1.Sorted = true;
			}
				

		}

		private void menuItem28_Click(object sender, System.EventArgs e)
		{
			this.treeView1.Sorted = false;
		}

		private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//this.treeView1.Cursor = 
		}

		private void menuItem29_Click(object sender, System.EventArgs e)
		{
			mode = nodeMode.insert;
			if (this.chkClear.Checked == true)
			{
				this.txtName.Clear();
				this.txtObject.Clear();
			}
			try
			{
				this.tmpNode = (pNode)this.treeView1.SelectedNode.Parent;
				this.modeIndex = treeView1.SelectedNode.Index;
				this.txtName.Focus();

			}

			catch (Exception f)
			{
				MessageBox.Show(f.Message);

			}
			this.statusBar1.Text = "Insert Mode";


		}

		private void menuItem31_Click(object sender, System.EventArgs e)
		{
			// Insert next to
			try
			{
				treeView1.SelectedNode.Parent.Nodes.Insert(treeView1.SelectedNode.Index,getNode);
					
					
					//Insert(treeView1.SelectedNode.Nodes.Count,this.getNode);
				this.getNode = (pNode)this.getNode.Clone();

				if (flag_file == true)
				{
					autosave();
				}

			}
			catch(Exception f)
			{
				MessageBox.Show(f.Message);
			}
		}

		private void treeView1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
		}

		private void treeView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Delete)
			{

				System.EventArgs f = new System.EventArgs();
				menuItem14_Click(sender, f);
			}
			else if (e.KeyCode == System.Windows.Forms.Keys.Insert)
			{
				EventArgs f = new EventArgs();
				if (menuItem29.Enabled == true)
				{
					menuItem29_Click(sender,f);
				}
			}
		}

		private void chkClear_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			filename = this.openFileDialog1.FileName;
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(filename,FileMode.Open,FileAccess.Read,FileShare.Read);
			Nodes = (ArrayList) formatter.Deserialize(stream);
			stream.Close();

			this.treeView1.Nodes.Clear();
			//				this.treeView1.Nodes.Add((pNode)Nodes[0]);

			//	Nodes[0] = userControl11;
			int count = (int)Nodes[0];
			int i = 1;
			while (i < Nodes.Count)
			{
				userControl11.Masters.Add((string)Nodes[i]);
                // check Nodes type
                ++i;
                if (Nodes[i].GetType() == typeof(pNode))
                {
                    userControl11.MastersValue.Add((pNode)Nodes[i]);
                }
                else if (Nodes[i].GetType() == typeof(TreeNode))
                {
                    // Compatibility with old version 6A
                    // convert TreeNode to a pNode
                    TreeNode a = (TreeNode)Nodes[i];
                    
                    // p only gets the top node for a master... it doesn't delve in and get everything else
                    // Todo: get all other nodes in a proper TreeNode to pNode conversion
                    // I want to perform the conversion indie of the pNode class itself
                    try
                    {
                        pNode p = pNode.TreeNode2pNode(a);
                        //                    userControl11.MastersValue.Add((TreeNode)Nodes[i]);
                        userControl11.MastersValue.Add(p);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
				i++;
			}
			userControl11.index = 0;
			this.treeView1.Nodes.Add((pNode)userControl11.MastersValue[userControl11.index]);
			flag_file = true;

		}

		private void menuItem33_Click(object sender, System.EventArgs e)
		{
			AYS ays = new AYS();
			ays.label1.Text = "This will load the file to the last insert, or addition only if you have saved or opened a file previously.";
			
			ays.ShowDialog();
			if (ays.DialogResult== DialogResult.OK)
			{
				try
				{
					if (this.flag_file == true)
					{
						this.Nodes.Clear();
						this.userControl11.MastersValue.Clear();
						this.userControl11.Masters.Clear();

	
						IFormatter formatter = new BinaryFormatter();
						Stream stream = new FileStream(filename,FileMode.Open,FileAccess.Read,FileShare.Read);
						Nodes = (ArrayList) formatter.Deserialize(stream);
						stream.Close();

						this.treeView1.Nodes.Clear();
						//				this.treeView1.Nodes.Add((pNode)Nodes[0]);
	
						//	Nodes[0] = userControl11;
						int count = (int)Nodes[0];
						int i = 1;
						while (i < Nodes.Count)
						{
							userControl11.Masters.Add((string)Nodes[i]);
							userControl11.MastersValue.Add((pNode)Nodes[++i]);
							i++;
						}
						userControl11.index = 0;
						this.treeView1.Nodes.Add((pNode)userControl11.MastersValue[userControl11.index]);
						flag_file = true;
					}
					else
						MessageBox.Show("UNDO: Failed");

				}
				catch (Exception f)
				{
					MessageBox.Show("You had an error while loading. Please select the proper file. " + f.Message,"OPEN ERROR",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);

				}
			}

		}

        /// <summary>
        /// Cuts a node only if node is not master node ie... has no parent node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void menuCutNode_Click(object sender, System.EventArgs e)
		{
			// get
			try
			{

				this.getNode = (pNode)treeView1.SelectedNode.Clone();
                if (this.getNode.Parent != null)
                {
                    this.menuItem21.Enabled = true;
                    this.menuItem31.Enabled = true;
                    this.nodeIndex = treeView1.SelectedNode.Index;
                    if (this.treeView1.SelectedNode.Text != "MASTER")
                    {
                        this.treeView1.SelectedNode.Remove();
                    }
                    this.statusBar1.Text = "CUT Node Mode";
                }
                else
                    MessageBox.Show("Cutting master node not allowed.", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception f)
			{
				MessageBox.Show(f.Message);
			}
		}
		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{

			filename = this.saveFileDialog1.FileName;
			if (filename != null)
			{
				IFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(this.saveFileDialog1.FileName,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
				formatter.Serialize(stream, Nodes);
				this.filename = this.saveFileDialog1.FileName;
				stream.Close();
			}
			flag_file = true;
		}

		private void saveFileDialogHTML_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string filenameHTML = this.saveFileDialogHTML.FileName;
			if (filenameHTML != null)
			{
				StreamWriter swFromFile = new StreamWriter(filenameHTML);

				swFromFile.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>"); 
				for (int i=0; i<xml.Count;i++)
				{
					swFromFile.Write(xml[i]);
					//						formatter.Serialize(stream, xml[i].ToString());
				}

				swFromFile.Flush();
				swFromFile.Close();
			}
		}

		private void menuItem35_Click(object sender, System.EventArgs e)
		{	

			try
			{
				exportmode = exportMode.pNode;

				xml.Clear();  // clear out contents first.
			
				this.xmlNode = (pNode)treeView1.SelectedNode;
				this.xmlIndex = treeView1.SelectedNode.Index;
				this.menuItem21.Enabled = true;
				this.menuItem31.Enabled = true;
				this.nodeIndex = treeView1.SelectedNode.Index;
				this.statusBar1.Text = "Export Node XML Mode";
				CallRecursive(xmlNode);

				this.saveFileDialogHTML.FileName = filenameHTML;
				this.saveFileDialogHTML.Title="Save the NODE to XML/HTML";
				this.saveFileDialogHTML.ShowDialog();
				filenameHTML = this.saveFileDialogHTML.FileName;
				if (filenameHTML != null)
				{

					//					IFormatter formatter = new BinaryFormatter();
					//					Stream stream = new FileStream(this.saveFileDialog1.FileName,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
					//					for (int i=0; i<xml.Count;i++)
					//					{
					//						formatter.Serialize(stream, xml[i].ToString());
					//					}
					//					this.filename = this.saveFileDialog1.FileName;
					//					stream.Close();
					//					FileStream fs = new FileStream(filename,
					//						FileMode.CreateNew, FileAccess.Write, FileShare.None);
    
					StreamWriter swFromFile = new StreamWriter(filenameHTML);

					swFromFile.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>"); 
					for (int i=0; i<xml.Count;i++)					
					{
						swFromFile.Write(xml[i]);
						//						formatter.Serialize(stream, xml[i].ToString());
					}
					swFromFile.Flush();
					swFromFile.Close();
				}

				this.toolBarView.Enabled = true;
			}
			catch(Exception f)
			{
				MessageBox.Show("You had an error while exporting to XML. " + f.Message,"SAVE ERROR",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}

		}

		private void txtName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Return)
			{
				this.txtObject.Focus();
				e.Handled = true;
			}
		}

		private void menuItem36_Click(object sender, System.EventArgs e)
		{
		
		}

        private void cmTree_Popup(object sender, EventArgs e)
        {

        }


        // Notify Icon2 is is the one that goes on the task bar not the notification area
        private void notifyIcon2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Notify icon mouse double click
            Debug.WriteLine("Notify icon mouse double click.");
        }

        private void notifyIcon2_DoubleClick(object sender, EventArgs e)
        {
            // comment this doesn't really work.  When you click on this icon on the 
            // windows bar task bar, it basically does nothing when you double click it
            // notify icon double click
            // this moves it into the notification area.  Does exactly what the 
            // autohide + minimize does
            this.autoHide_flag = true;
            this.statusBar1.Text = "AutoHide Active";
            this.toolBarTac.ImageIndex = 0;
            this.actHook.Start();
            this.VIS = false;
            this.Visible = false;
            notifyIcon2.Visible = false;
            notifyIcon1.Visible = true;
        }

        private void notifyIcon2_MouseClick(object sender, MouseEventArgs e)
        {
            // notify icon mouse click
            // this event does nothing.  All the logic for controlling a window opening back
            // up appears to be coming from the windows taskbar and not this event

        }

        private void menuItemAttribute_Click(object sender, EventArgs e)
        {
            // add an attribute to a node
            Debug.WriteLine(e.ToString());
        }



	}
}