namespace Logai
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleBar = new System.Windows.Forms.Panel();
            this.titleLayout = new System.Windows.Forms.TableLayoutPanel();
            this.appTitle = new System.Windows.Forms.Label();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.maximizeButton = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.navList = new System.Windows.Forms.FlowLayoutPanel();
            this.overviewButton = new System.Windows.Forms.Button();
            this.cleanerButton = new System.Windows.Forms.Button();
            this.performanceButton = new System.Windows.Forms.Button();
            this.securityButton = new System.Windows.Forms.Button();
            this.toolsButton = new System.Windows.Forms.Button();
            this.infoButton = new System.Windows.Forms.Button();
            this.sectionTitle = new System.Windows.Forms.Label();
            this.brandPanel = new System.Windows.Forms.Panel();
            this.brandStatus = new System.Windows.Forms.Label();
            this.brandTitle = new System.Windows.Forms.Label();
            this.brandAccent = new System.Windows.Forms.Panel();
            this.pageHost = new Logai.PageHostTabControl();
            this.overviewTabPage = new System.Windows.Forms.TabPage();
            this.cleanerTabPage = new System.Windows.Forms.TabPage();
            this.performanceTabPage = new System.Windows.Forms.TabPage();
            this.securityTabPage = new System.Windows.Forms.TabPage();
            this.toolsTabPage = new System.Windows.Forms.TabPage();
            this.infoTabPage = new System.Windows.Forms.TabPage();
            this.overviewPageLabel = new System.Windows.Forms.Label();
            this.cleanerPageLabel = new System.Windows.Forms.Label();
            this.performancePageLabel = new System.Windows.Forms.Label();
            this.securityPageLabel = new System.Windows.Forms.Label();
            this.toolsPageLabel = new System.Windows.Forms.Label();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.infoTitleLabel = new System.Windows.Forms.Label();
            this.infoSubtitleLabel = new System.Windows.Forms.Label();
            this.specsTextBox = new System.Windows.Forms.TextBox();
            this.liveCpuLabel = new System.Windows.Forms.Label();
            this.liveCpuClockLabel = new System.Windows.Forms.Label();
            this.liveRamLabel = new System.Windows.Forms.Label();
            this.liveGpuLabel = new System.Windows.Forms.Label();
            this.liveGpuClockLabel = new System.Windows.Forms.Label();
            this.liveTempLabel = new System.Windows.Forms.Label();
            this.processTitleLabel = new System.Windows.Forms.Label();
            this.topProcessesListView = new System.Windows.Forms.ListView();
            this.processNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.processPidColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.processCpuColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.processRamColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.processGpuColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.processGpuMemoryColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titleBar.SuspendLayout();
            this.titleLayout.SuspendLayout();
            this.sidebarPanel.SuspendLayout();
            this.navList.SuspendLayout();
            this.brandPanel.SuspendLayout();
            this.pageHost.SuspendLayout();
            this.overviewTabPage.SuspendLayout();
            this.cleanerTabPage.SuspendLayout();
            this.performanceTabPage.SuspendLayout();
            this.securityTabPage.SuspendLayout();
            this.toolsTabPage.SuspendLayout();
            this.infoTabPage.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // titleBar
            //
            this.titleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.titleBar.Controls.Add(this.titleLayout);
            this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleBar.Location = new System.Drawing.Point(0, 0);
            this.titleBar.Name = "titleBar";
            this.titleBar.Size = new System.Drawing.Size(1000, 44);
            this.titleBar.TabIndex = 0;
            this.titleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            //
            // titleLayout
            //
            this.titleLayout.ColumnCount = 4;
            this.titleLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.titleLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.titleLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.titleLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.titleLayout.Controls.Add(this.appTitle, 0, 0);
            this.titleLayout.Controls.Add(this.minimizeButton, 1, 0);
            this.titleLayout.Controls.Add(this.maximizeButton, 2, 0);
            this.titleLayout.Controls.Add(this.exit, 3, 0);
            this.titleLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLayout.Location = new System.Drawing.Point(0, 0);
            this.titleLayout.Name = "titleLayout";
            this.titleLayout.Padding = new System.Windows.Forms.Padding(16, 0, 8, 0);
            this.titleLayout.RowCount = 1;
            this.titleLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.titleLayout.Size = new System.Drawing.Size(1000, 44);
            this.titleLayout.TabIndex = 0;
            this.titleLayout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            //
            // appTitle
            //
            this.appTitle.AutoEllipsis = true;
            this.appTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.appTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(156)))), ((int)(((byte)(255)))));
            this.appTitle.Location = new System.Drawing.Point(19, 0);
            this.appTitle.Name = "appTitle";
            this.appTitle.Size = new System.Drawing.Size(844, 44);
            this.appTitle.TabIndex = 0;
            this.appTitle.Text = "Logai";
            this.appTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.appTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            //
            // minimizeButton
            //
            this.minimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.minimizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.minimizeButton.FlatAppearance.BorderSize = 0;
            this.minimizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.minimizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.minimizeButton.ForeColor = System.Drawing.Color.White;
            this.minimizeButton.Location = new System.Drawing.Point(866, 0);
            this.minimizeButton.Margin = new System.Windows.Forms.Padding(0);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(42, 44);
            this.minimizeButton.TabIndex = 1;
            this.minimizeButton.TabStop = false;
            this.minimizeButton.Text = "-";
            this.minimizeButton.UseVisualStyleBackColor = false;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            //
            // maximizeButton
            //
            this.maximizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.maximizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maximizeButton.FlatAppearance.BorderSize = 0;
            this.maximizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.maximizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.maximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maximizeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.maximizeButton.ForeColor = System.Drawing.Color.White;
            this.maximizeButton.Location = new System.Drawing.Point(908, 0);
            this.maximizeButton.Margin = new System.Windows.Forms.Padding(0);
            this.maximizeButton.Name = "maximizeButton";
            this.maximizeButton.Size = new System.Drawing.Size(42, 44);
            this.maximizeButton.TabIndex = 2;
            this.maximizeButton.TabStop = false;
            this.maximizeButton.Text = "[]";
            this.maximizeButton.UseVisualStyleBackColor = false;
            this.maximizeButton.Click += new System.EventHandler(this.maximizeButton_Click);
            //
            // exit
            //
            this.exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exit.FlatAppearance.BorderSize = 0;
            this.exit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(34)))), ((int)(((byte)(28)))));
            this.exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(43)))), ((int)(((byte)(28)))));
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.exit.ForeColor = System.Drawing.Color.White;
            this.exit.Location = new System.Drawing.Point(950, 0);
            this.exit.Margin = new System.Windows.Forms.Padding(0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(42, 44);
            this.exit.TabIndex = 3;
            this.exit.TabStop = false;
            this.exit.Text = "X";
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            //
            // sidebarPanel
            //
            this.sidebarPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.sidebarPanel.Controls.Add(this.navList);
            this.sidebarPanel.Controls.Add(this.sectionTitle);
            this.sidebarPanel.Controls.Add(this.brandPanel);
            this.sidebarPanel.Location = new System.Drawing.Point(0, 44);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Padding = new System.Windows.Forms.Padding(14, 18, 14, 14);
            this.sidebarPanel.Size = new System.Drawing.Size(180, 576);
            this.sidebarPanel.TabIndex = 1;
            //
            // navList
            //
            this.navList.Controls.Add(this.overviewButton);
            this.navList.Controls.Add(this.cleanerButton);
            this.navList.Controls.Add(this.performanceButton);
            this.navList.Controls.Add(this.securityButton);
            this.navList.Controls.Add(this.toolsButton);
            this.navList.Controls.Add(this.infoButton);
            this.navList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.navList.Location = new System.Drawing.Point(14, 122);
            this.navList.Margin = new System.Windows.Forms.Padding(0);
            this.navList.Name = "navList";
            this.navList.Size = new System.Drawing.Size(152, 276);
            this.navList.TabIndex = 2;
            this.navList.WrapContents = false;
            //
            // overviewButton
            //
            this.overviewButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(49)))), ((int)(((byte)(69)))));
            this.overviewButton.FlatAppearance.BorderSize = 0;
            this.overviewButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.overviewButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(49)))), ((int)(((byte)(69)))));
            this.overviewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.overviewButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.overviewButton.ForeColor = System.Drawing.Color.White;
            this.overviewButton.Location = new System.Drawing.Point(0, 0);
            this.overviewButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.overviewButton.Name = "overviewButton";
            this.overviewButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.overviewButton.Size = new System.Drawing.Size(152, 38);
            this.overviewButton.TabIndex = 0;
            this.overviewButton.Text = "Overview";
            this.overviewButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.overviewButton.UseVisualStyleBackColor = false;
            this.overviewButton.Click += new System.EventHandler(this.navButton_Click);
            //
            // cleanerButton
            //
            this.cleanerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.cleanerButton.FlatAppearance.BorderSize = 0;
            this.cleanerButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.cleanerButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(59)))), ((int)(((byte)(45)))));
            this.cleanerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cleanerButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.cleanerButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.cleanerButton.Location = new System.Drawing.Point(0, 46);
            this.cleanerButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.cleanerButton.Name = "cleanerButton";
            this.cleanerButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.cleanerButton.Size = new System.Drawing.Size(152, 38);
            this.cleanerButton.TabIndex = 1;
            this.cleanerButton.Text = "Cleaner";
            this.cleanerButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cleanerButton.UseVisualStyleBackColor = false;
            this.cleanerButton.Click += new System.EventHandler(this.navButton_Click);
            //
            // performanceButton
            //
            this.performanceButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.performanceButton.FlatAppearance.BorderSize = 0;
            this.performanceButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.performanceButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(41)))), ((int)(((byte)(69)))));
            this.performanceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.performanceButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.performanceButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.performanceButton.Location = new System.Drawing.Point(0, 92);
            this.performanceButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.performanceButton.Name = "performanceButton";
            this.performanceButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.performanceButton.Size = new System.Drawing.Size(152, 38);
            this.performanceButton.TabIndex = 2;
            this.performanceButton.Text = "Performance";
            this.performanceButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.performanceButton.UseVisualStyleBackColor = false;
            this.performanceButton.Click += new System.EventHandler(this.navButton_Click);
            //
            // securityButton
            //
            this.securityButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.securityButton.FlatAppearance.BorderSize = 0;
            this.securityButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.securityButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.securityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.securityButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.securityButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.securityButton.Location = new System.Drawing.Point(0, 138);
            this.securityButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.securityButton.Name = "securityButton";
            this.securityButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.securityButton.Size = new System.Drawing.Size(152, 38);
            this.securityButton.TabIndex = 3;
            this.securityButton.Text = "Security";
            this.securityButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.securityButton.UseVisualStyleBackColor = false;
            this.securityButton.Click += new System.EventHandler(this.navButton_Click);
            //
            // toolsButton
            //
            this.toolsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.toolsButton.FlatAppearance.BorderSize = 0;
            this.toolsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.toolsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(53)))), ((int)(((byte)(33)))));
            this.toolsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toolsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.toolsButton.Location = new System.Drawing.Point(0, 184);
            this.toolsButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.toolsButton.Name = "toolsButton";
            this.toolsButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolsButton.Size = new System.Drawing.Size(152, 38);
            this.toolsButton.TabIndex = 4;
            this.toolsButton.Text = "Tools";
            this.toolsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolsButton.UseVisualStyleBackColor = false;
            this.toolsButton.Click += new System.EventHandler(this.navButton_Click);
            //
            // infoButton
            //
            this.infoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.infoButton.FlatAppearance.BorderSize = 0;
            this.infoButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.infoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(55)))), ((int)(((byte)(60)))));
            this.infoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.infoButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.infoButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.infoButton.Location = new System.Drawing.Point(0, 230);
            this.infoButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.infoButton.Name = "infoButton";
            this.infoButton.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.infoButton.Size = new System.Drawing.Size(152, 38);
            this.infoButton.TabIndex = 5;
            this.infoButton.Text = "Info";
            this.infoButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.infoButton.UseVisualStyleBackColor = false;
            this.infoButton.Click += new System.EventHandler(this.navButton_Click);
            //
            // sectionTitle
            //
            this.sectionTitle.AutoEllipsis = true;
            this.sectionTitle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.sectionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.sectionTitle.Location = new System.Drawing.Point(14, 96);
            this.sectionTitle.Name = "sectionTitle";
            this.sectionTitle.Size = new System.Drawing.Size(152, 26);
            this.sectionTitle.TabIndex = 1;
            this.sectionTitle.Text = "CONTROL CENTER";
            this.sectionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // brandPanel
            //
            this.brandPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.brandPanel.Controls.Add(this.brandStatus);
            this.brandPanel.Controls.Add(this.brandTitle);
            this.brandPanel.Controls.Add(this.brandAccent);
            this.brandPanel.Location = new System.Drawing.Point(14, 18);
            this.brandPanel.Name = "brandPanel";
            this.brandPanel.Size = new System.Drawing.Size(152, 64);
            this.brandPanel.TabIndex = 0;
            //
            // brandStatus
            //
            this.brandStatus.AutoEllipsis = true;
            this.brandStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.brandStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(209)))), ((int)(((byte)(139)))));
            this.brandStatus.Location = new System.Drawing.Point(16, 34);
            this.brandStatus.Name = "brandStatus";
            this.brandStatus.Size = new System.Drawing.Size(124, 20);
            this.brandStatus.TabIndex = 2;
            this.brandStatus.Text = "Efficiency First";
            this.brandStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // brandTitle
            //
            this.brandTitle.AutoEllipsis = true;
            this.brandTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.brandTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.brandTitle.Location = new System.Drawing.Point(16, 10);
            this.brandTitle.Name = "brandTitle";
            this.brandTitle.Size = new System.Drawing.Size(124, 24);
            this.brandTitle.TabIndex = 1;
            this.brandTitle.Text = "Logai";
            this.brandTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // brandAccent
            //
            this.brandAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(156)))), ((int)(((byte)(255)))));
            this.brandAccent.Dock = System.Windows.Forms.DockStyle.Left;
            this.brandAccent.Location = new System.Drawing.Point(0, 0);
            this.brandAccent.Name = "brandAccent";
            this.brandAccent.Size = new System.Drawing.Size(4, 64);
            this.brandAccent.TabIndex = 0;
            //
            // pageHost
            //
            this.pageHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageHost.Controls.Add(this.overviewTabPage);
            this.pageHost.Controls.Add(this.cleanerTabPage);
            this.pageHost.Controls.Add(this.performanceTabPage);
            this.pageHost.Controls.Add(this.securityTabPage);
            this.pageHost.Controls.Add(this.toolsTabPage);
            this.pageHost.Controls.Add(this.infoTabPage);
            this.pageHost.Location = new System.Drawing.Point(180, 44);
            this.pageHost.Name = "pageHost";
            this.pageHost.SelectedIndex = 0;
            this.pageHost.Size = new System.Drawing.Size(820, 576);
            this.pageHost.TabIndex = 2;
            //
            // overviewTabPage
            //
            this.overviewTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.overviewTabPage.Controls.Add(this.overviewPageLabel);
            this.overviewTabPage.Location = new System.Drawing.Point(4, 22);
            this.overviewTabPage.Name = "overviewTabPage";
            this.overviewTabPage.Padding = new System.Windows.Forms.Padding(24);
            this.overviewTabPage.Size = new System.Drawing.Size(812, 550);
            this.overviewTabPage.TabIndex = 0;
            this.overviewTabPage.Text = "Overview";
            this.overviewTabPage.UseVisualStyleBackColor = false;
            //
            // cleanerTabPage
            //
            this.cleanerTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.cleanerTabPage.Controls.Add(this.cleanerPageLabel);
            this.cleanerTabPage.Location = new System.Drawing.Point(4, 22);
            this.cleanerTabPage.Name = "cleanerTabPage";
            this.cleanerTabPage.Padding = new System.Windows.Forms.Padding(24);
            this.cleanerTabPage.Size = new System.Drawing.Size(812, 550);
            this.cleanerTabPage.TabIndex = 1;
            this.cleanerTabPage.Text = "Cleaner";
            this.cleanerTabPage.UseVisualStyleBackColor = false;
            //
            // performanceTabPage
            //
            this.performanceTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.performanceTabPage.Controls.Add(this.performancePageLabel);
            this.performanceTabPage.Location = new System.Drawing.Point(4, 22);
            this.performanceTabPage.Name = "performanceTabPage";
            this.performanceTabPage.Padding = new System.Windows.Forms.Padding(24);
            this.performanceTabPage.Size = new System.Drawing.Size(812, 550);
            this.performanceTabPage.TabIndex = 2;
            this.performanceTabPage.Text = "Performance";
            this.performanceTabPage.UseVisualStyleBackColor = false;
            //
            // securityTabPage
            //
            this.securityTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.securityTabPage.Controls.Add(this.securityPageLabel);
            this.securityTabPage.Location = new System.Drawing.Point(4, 22);
            this.securityTabPage.Name = "securityTabPage";
            this.securityTabPage.Padding = new System.Windows.Forms.Padding(24);
            this.securityTabPage.Size = new System.Drawing.Size(812, 550);
            this.securityTabPage.TabIndex = 3;
            this.securityTabPage.Text = "Security";
            this.securityTabPage.UseVisualStyleBackColor = false;
            //
            // toolsTabPage
            //
            this.toolsTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.toolsTabPage.Controls.Add(this.toolsPageLabel);
            this.toolsTabPage.Location = new System.Drawing.Point(4, 22);
            this.toolsTabPage.Name = "toolsTabPage";
            this.toolsTabPage.Padding = new System.Windows.Forms.Padding(24);
            this.toolsTabPage.Size = new System.Drawing.Size(812, 550);
            this.toolsTabPage.TabIndex = 4;
            this.toolsTabPage.Text = "Tools";
            this.toolsTabPage.UseVisualStyleBackColor = false;
            //
            // infoTabPage
            //
            this.infoTabPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.infoTabPage.Controls.Add(this.infoPanel);
            this.infoTabPage.Location = new System.Drawing.Point(4, 22);
            this.infoTabPage.Name = "infoTabPage";
            this.infoTabPage.Size = new System.Drawing.Size(812, 550);
            this.infoTabPage.TabIndex = 5;
            this.infoTabPage.Text = "Info";
            this.infoTabPage.UseVisualStyleBackColor = false;
            //
            // overviewPageLabel
            //
            this.overviewPageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewPageLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.overviewPageLabel.ForeColor = System.Drawing.Color.White;
            this.overviewPageLabel.Location = new System.Drawing.Point(24, 24);
            this.overviewPageLabel.Name = "overviewPageLabel";
            this.overviewPageLabel.Size = new System.Drawing.Size(764, 502);
            this.overviewPageLabel.TabIndex = 0;
            this.overviewPageLabel.Text = "Overview";
            this.overviewPageLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // cleanerPageLabel
            //
            this.cleanerPageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cleanerPageLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.cleanerPageLabel.ForeColor = System.Drawing.Color.White;
            this.cleanerPageLabel.Location = new System.Drawing.Point(24, 24);
            this.cleanerPageLabel.Name = "cleanerPageLabel";
            this.cleanerPageLabel.Size = new System.Drawing.Size(764, 502);
            this.cleanerPageLabel.TabIndex = 0;
            this.cleanerPageLabel.Text = "Cleaner";
            this.cleanerPageLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // performancePageLabel
            //
            this.performancePageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.performancePageLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.performancePageLabel.ForeColor = System.Drawing.Color.White;
            this.performancePageLabel.Location = new System.Drawing.Point(24, 24);
            this.performancePageLabel.Name = "performancePageLabel";
            this.performancePageLabel.Size = new System.Drawing.Size(764, 502);
            this.performancePageLabel.TabIndex = 0;
            this.performancePageLabel.Text = "Performance";
            this.performancePageLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // securityPageLabel
            //
            this.securityPageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.securityPageLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.securityPageLabel.ForeColor = System.Drawing.Color.White;
            this.securityPageLabel.Location = new System.Drawing.Point(24, 24);
            this.securityPageLabel.Name = "securityPageLabel";
            this.securityPageLabel.Size = new System.Drawing.Size(764, 502);
            this.securityPageLabel.TabIndex = 0;
            this.securityPageLabel.Text = "Security";
            this.securityPageLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // toolsPageLabel
            //
            this.toolsPageLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolsPageLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.toolsPageLabel.ForeColor = System.Drawing.Color.White;
            this.toolsPageLabel.Location = new System.Drawing.Point(24, 24);
            this.toolsPageLabel.Name = "toolsPageLabel";
            this.toolsPageLabel.Size = new System.Drawing.Size(764, 502);
            this.toolsPageLabel.TabIndex = 0;
            this.toolsPageLabel.Text = "Tools";
            this.toolsPageLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // infoPanel
            //
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.infoPanel.Controls.Add(this.topProcessesListView);
            this.infoPanel.Controls.Add(this.processTitleLabel);
            this.infoPanel.Controls.Add(this.liveTempLabel);
            this.infoPanel.Controls.Add(this.liveGpuClockLabel);
            this.infoPanel.Controls.Add(this.liveGpuLabel);
            this.infoPanel.Controls.Add(this.liveRamLabel);
            this.infoPanel.Controls.Add(this.liveCpuClockLabel);
            this.infoPanel.Controls.Add(this.liveCpuLabel);
            this.infoPanel.Controls.Add(this.specsTextBox);
            this.infoPanel.Controls.Add(this.infoSubtitleLabel);
            this.infoPanel.Controls.Add(this.infoTitleLabel);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoPanel.Location = new System.Drawing.Point(0, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(812, 550);
            this.infoPanel.TabIndex = 2;
            //
            // infoTitleLabel
            //
            this.infoTitleLabel.AutoEllipsis = true;
            this.infoTitleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.infoTitleLabel.ForeColor = System.Drawing.Color.White;
            this.infoTitleLabel.Location = new System.Drawing.Point(24, 16);
            this.infoTitleLabel.Name = "infoTitleLabel";
            this.infoTitleLabel.Size = new System.Drawing.Size(260, 34);
            this.infoTitleLabel.TabIndex = 0;
            this.infoTitleLabel.Text = "Info";
            this.infoTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // infoSubtitleLabel
            //
            this.infoSubtitleLabel.AutoEllipsis = true;
            this.infoSubtitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.infoSubtitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.infoSubtitleLabel.Location = new System.Drawing.Point(26, 50);
            this.infoSubtitleLabel.Name = "infoSubtitleLabel";
            this.infoSubtitleLabel.Size = new System.Drawing.Size(720, 22);
            this.infoSubtitleLabel.TabIndex = 1;
            this.infoSubtitleLabel.Text = "Live device specs, performance, and top resource usage.";
            this.infoSubtitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // specsTextBox
            //
            this.specsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.specsTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.specsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.specsTextBox.Font = new System.Drawing.Font("Consolas", 8.75F);
            this.specsTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.specsTextBox.Location = new System.Drawing.Point(28, 78);
            this.specsTextBox.Multiline = true;
            this.specsTextBox.Name = "specsTextBox";
            this.specsTextBox.ReadOnly = true;
            this.specsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.specsTextBox.Size = new System.Drawing.Size(768, 166);
            this.specsTextBox.TabIndex = 2;
            this.specsTextBox.Text = "Select Info to load detailed system specs.";
            //
            // liveCpuLabel
            //
            this.liveCpuLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.liveCpuLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(156)))), ((int)(((byte)(255)))));
            this.liveCpuLabel.Location = new System.Drawing.Point(28, 254);
            this.liveCpuLabel.Name = "liveCpuLabel";
            this.liveCpuLabel.Size = new System.Drawing.Size(260, 22);
            this.liveCpuLabel.TabIndex = 3;
            this.liveCpuLabel.Text = "CPU Usage: --";
            this.liveCpuLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // liveCpuClockLabel
            //
            this.liveCpuClockLabel.AutoEllipsis = true;
            this.liveCpuClockLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.liveCpuClockLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(156)))), ((int)(((byte)(255)))));
            this.liveCpuClockLabel.Location = new System.Drawing.Point(28, 278);
            this.liveCpuClockLabel.Name = "liveCpuClockLabel";
            this.liveCpuClockLabel.Size = new System.Drawing.Size(260, 22);
            this.liveCpuClockLabel.TabIndex = 4;
            this.liveCpuClockLabel.Text = "CPU Live GHz: --";
            this.liveCpuClockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // liveRamLabel
            //
            this.liveRamLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.liveRamLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(209)))), ((int)(((byte)(139)))));
            this.liveRamLabel.Location = new System.Drawing.Point(28, 302);
            this.liveRamLabel.Name = "liveRamLabel";
            this.liveRamLabel.Size = new System.Drawing.Size(260, 22);
            this.liveRamLabel.TabIndex = 5;
            this.liveRamLabel.Text = "RAM: --";
            this.liveRamLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // liveGpuLabel
            //
            this.liveGpuLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liveGpuLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.liveGpuLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(115)))), ((int)(((byte)(255)))));
            this.liveGpuLabel.Location = new System.Drawing.Point(310, 254);
            this.liveGpuLabel.Name = "liveGpuLabel";
            this.liveGpuLabel.Size = new System.Drawing.Size(486, 22);
            this.liveGpuLabel.TabIndex = 6;
            this.liveGpuLabel.Text = "GPU Usage: --";
            this.liveGpuLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // liveGpuClockLabel
            //
            this.liveGpuClockLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liveGpuClockLabel.AutoEllipsis = true;
            this.liveGpuClockLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.liveGpuClockLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(115)))), ((int)(((byte)(255)))));
            this.liveGpuClockLabel.Location = new System.Drawing.Point(310, 278);
            this.liveGpuClockLabel.Name = "liveGpuClockLabel";
            this.liveGpuClockLabel.Size = new System.Drawing.Size(486, 22);
            this.liveGpuClockLabel.TabIndex = 7;
            this.liveGpuClockLabel.Text = "GPU Live GHz: --";
            this.liveGpuClockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // liveTempLabel
            //
            this.liveTempLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liveTempLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.liveTempLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(178)))), ((int)(((byte)(77)))));
            this.liveTempLabel.Location = new System.Drawing.Point(310, 302);
            this.liveTempLabel.Name = "liveTempLabel";
            this.liveTempLabel.Size = new System.Drawing.Size(486, 22);
            this.liveTempLabel.TabIndex = 8;
            this.liveTempLabel.Text = "Temps: --";
            this.liveTempLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // processTitleLabel
            //
            this.processTitleLabel.AutoEllipsis = true;
            this.processTitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.processTitleLabel.ForeColor = System.Drawing.Color.White;
            this.processTitleLabel.Location = new System.Drawing.Point(26, 338);
            this.processTitleLabel.Name = "processTitleLabel";
            this.processTitleLabel.Size = new System.Drawing.Size(260, 22);
            this.processTitleLabel.TabIndex = 9;
            this.processTitleLabel.Text = "Top resource processes";
            this.processTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // topProcessesListView
            //
            this.topProcessesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topProcessesListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.topProcessesListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topProcessesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.processNameColumn,
            this.processPidColumn,
            this.processCpuColumn,
            this.processRamColumn,
            this.processGpuColumn,
            this.processGpuMemoryColumn});
            this.topProcessesListView.ForeColor = System.Drawing.Color.White;
            this.topProcessesListView.FullRowSelect = true;
            this.topProcessesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.topProcessesListView.HideSelection = false;
            this.topProcessesListView.Location = new System.Drawing.Point(28, 364);
            this.topProcessesListView.Name = "topProcessesListView";
            this.topProcessesListView.Size = new System.Drawing.Size(768, 182);
            this.topProcessesListView.TabIndex = 10;
            this.topProcessesListView.UseCompatibleStateImageBehavior = false;
            this.topProcessesListView.View = System.Windows.Forms.View.Details;
            //
            // processNameColumn
            //
            this.processNameColumn.Text = "Process";
            this.processNameColumn.Width = 190;
            //
            // processPidColumn
            //
            this.processPidColumn.Text = "PID";
            this.processPidColumn.Width = 50;
            //
            // processCpuColumn
            //
            this.processCpuColumn.Text = "CPU";
            this.processCpuColumn.Width = 70;
            //
            // processRamColumn
            //
            this.processRamColumn.Text = "RAM";
            this.processRamColumn.Width = 90;
            //
            // processGpuColumn
            //
            this.processGpuColumn.Text = "GPU";
            this.processGpuColumn.Width = 70;
            //
            // processGpuMemoryColumn
            //
            this.processGpuMemoryColumn.Text = "GPU RAM";
            this.processGpuMemoryColumn.Width = 100;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Controls.Add(this.pageHost);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.titleBar);
            this.Name = "Form1";
            this.Text = "Logai";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.titleBar.ResumeLayout(false);
            this.titleLayout.ResumeLayout(false);
            this.sidebarPanel.ResumeLayout(false);
            this.navList.ResumeLayout(false);
            this.brandPanel.ResumeLayout(false);
            this.pageHost.ResumeLayout(false);
            this.overviewTabPage.ResumeLayout(false);
            this.cleanerTabPage.ResumeLayout(false);
            this.performanceTabPage.ResumeLayout(false);
            this.securityTabPage.ResumeLayout(false);
            this.toolsTabPage.ResumeLayout(false);
            this.infoTabPage.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel titleBar;
        private System.Windows.Forms.TableLayoutPanel titleLayout;
        private System.Windows.Forms.Label appTitle;
        private System.Windows.Forms.Button minimizeButton;
        private System.Windows.Forms.Button maximizeButton;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel brandPanel;
        private System.Windows.Forms.Panel brandAccent;
        private System.Windows.Forms.Label brandTitle;
        private System.Windows.Forms.Label brandStatus;
        private System.Windows.Forms.Label sectionTitle;
        private System.Windows.Forms.FlowLayoutPanel navList;
        private System.Windows.Forms.Button overviewButton;
        private System.Windows.Forms.Button cleanerButton;
        private System.Windows.Forms.Button performanceButton;
        private System.Windows.Forms.Button securityButton;
        private System.Windows.Forms.Button toolsButton;
        private System.Windows.Forms.Button infoButton;
        private Logai.PageHostTabControl pageHost;
        private System.Windows.Forms.TabPage overviewTabPage;
        private System.Windows.Forms.TabPage cleanerTabPage;
        private System.Windows.Forms.TabPage performanceTabPage;
        private System.Windows.Forms.TabPage securityTabPage;
        private System.Windows.Forms.TabPage toolsTabPage;
        private System.Windows.Forms.TabPage infoTabPage;
        private System.Windows.Forms.Label overviewPageLabel;
        private System.Windows.Forms.Label cleanerPageLabel;
        private System.Windows.Forms.Label performancePageLabel;
        private System.Windows.Forms.Label securityPageLabel;
        private System.Windows.Forms.Label toolsPageLabel;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Label infoTitleLabel;
        private System.Windows.Forms.Label infoSubtitleLabel;
        private System.Windows.Forms.TextBox specsTextBox;
        private System.Windows.Forms.Label liveCpuLabel;
        private System.Windows.Forms.Label liveCpuClockLabel;
        private System.Windows.Forms.Label liveRamLabel;
        private System.Windows.Forms.Label liveGpuLabel;
        private System.Windows.Forms.Label liveGpuClockLabel;
        private System.Windows.Forms.Label liveTempLabel;
        private System.Windows.Forms.Label processTitleLabel;
        private System.Windows.Forms.ListView topProcessesListView;
        private System.Windows.Forms.ColumnHeader processNameColumn;
        private System.Windows.Forms.ColumnHeader processPidColumn;
        private System.Windows.Forms.ColumnHeader processCpuColumn;
        private System.Windows.Forms.ColumnHeader processRamColumn;
        private System.Windows.Forms.ColumnHeader processGpuColumn;
        private System.Windows.Forms.ColumnHeader processGpuMemoryColumn;
    }
}
