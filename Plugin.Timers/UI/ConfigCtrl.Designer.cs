namespace Plugin.Timers.UI
{
	partial class ConfigCtrl
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
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStrip tsMain;
			this.tsbnAdd = new System.Windows.Forms.ToolStripButton();
			this.tsbnRemove = new System.Windows.Forms.ToolStripButton();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvTimers = new System.Windows.Forms.ListView();
			this.colTimerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colTimerInterval = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colTimerCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmsTimers = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiTimersInvoke = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiTimersStop = new System.Windows.Forms.ToolStripMenuItem();
			this.pgData = new System.Windows.Forms.PropertyGrid();
			tsMain = new System.Windows.Forms.ToolStrip();
			tsMain.SuspendLayout();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.cmsTimers.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnAdd,
            this.tsbnRemove});
			tsMain.Location = new System.Drawing.Point(0, 0);
			tsMain.Name = "tsMain";
			tsMain.Size = new System.Drawing.Size(200, 27);
			tsMain.TabIndex = 0;
			tsMain.Text = "toolStrip1";
			// 
			// tsbnAdd
			// 
			this.tsbnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnAdd.Image = global::Plugin.Timers.Properties.Resources.FileNew;
			this.tsbnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnAdd.Name = "tsbnAdd";
			this.tsbnAdd.Size = new System.Drawing.Size(24, 24);
			this.tsbnAdd.Text = "Add timer";
			this.tsbnAdd.Click += new System.EventHandler(this.tsbnAdd_Click);
			// 
			// tsbnRemove
			// 
			this.tsbnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnRemove.Image = global::Plugin.Timers.Properties.Resources.iconDelete;
			this.tsbnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnRemove.Name = "tsbnRemove";
			this.tsbnRemove.Size = new System.Drawing.Size(24, 24);
			this.tsbnRemove.Text = "Remove timer";
			this.tsbnRemove.Click += new System.EventHandler(this.tsbnRemove_Click);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 27);
			this.splitMain.Margin = new System.Windows.Forms.Padding(4);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.lvTimers);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.pgData);
			this.splitMain.Size = new System.Drawing.Size(200, 158);
			this.splitMain.SplitterDistance = 78;
			this.splitMain.TabIndex = 1;
			// 
			// lvTimers
			// 
			this.lvTimers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTimerName,
            this.colTimerInterval,
            this.colTimerCount});
			this.lvTimers.ContextMenuStrip = this.cmsTimers;
			this.lvTimers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvTimers.FullRowSelect = true;
			this.lvTimers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvTimers.HideSelection = false;
			this.lvTimers.LabelEdit = true;
			this.lvTimers.Location = new System.Drawing.Point(0, 0);
			this.lvTimers.Margin = new System.Windows.Forms.Padding(4);
			this.lvTimers.MultiSelect = false;
			this.lvTimers.Name = "lvTimers";
			this.lvTimers.Size = new System.Drawing.Size(200, 78);
			this.lvTimers.TabIndex = 0;
			this.lvTimers.UseCompatibleStateImageBehavior = false;
			this.lvTimers.View = System.Windows.Forms.View.Details;
			this.lvTimers.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lvTimers_AfterLabelEdit);
			this.lvTimers.SelectedIndexChanged += new System.EventHandler(this.lvTimers_SelectedIndexChanged);
			this.lvTimers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvTimers_KeyDown);
			// 
			// colTimerName
			// 
			this.colTimerName.Text = "TimerName";
			// 
			// colTimerInterval
			// 
			this.colTimerInterval.Text = "Interval";
			// 
			// colTimerCount
			// 
			this.colTimerCount.Text = "Count";
			// 
			// cmsTimers
			// 
			this.cmsTimers.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmsTimers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTimersInvoke,
            this.tsmiTimersStop});
			this.cmsTimers.Name = "cmsTimers";
			this.cmsTimers.Size = new System.Drawing.Size(122, 52);
			this.cmsTimers.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTimers_Opening);
			this.cmsTimers.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsTimers_ItemClicked);
			// 
			// tsmiTimersInvoke
			// 
			this.tsmiTimersInvoke.Name = "tsmiTimersInvoke";
			this.tsmiTimersInvoke.Size = new System.Drawing.Size(121, 24);
			this.tsmiTimersInvoke.Text = "&Invoke";
			// 
			// tsmiTimersStop
			// 
			this.tsmiTimersStop.Name = "tsmiTimersStop";
			this.tsmiTimersStop.Size = new System.Drawing.Size(121, 24);
			this.tsmiTimersStop.Text = "&Stop";
			// 
			// pgData
			// 
			this.pgData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgData.LineColor = System.Drawing.SystemColors.ControlDark;
			this.pgData.Location = new System.Drawing.Point(0, 0);
			this.pgData.Margin = new System.Windows.Forms.Padding(4);
			this.pgData.Name = "pgData";
			this.pgData.Size = new System.Drawing.Size(200, 76);
			this.pgData.TabIndex = 0;
			this.pgData.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgData_PropertyValueChanged);
			// 
			// ConfigCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Controls.Add(tsMain);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "ConfigCtrl";
			this.Size = new System.Drawing.Size(200, 185);
			tsMain.ResumeLayout(false);
			tsMain.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.cmsTimers.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ListView lvTimers;
		private System.Windows.Forms.ColumnHeader colTimerName;
		private System.Windows.Forms.ColumnHeader colTimerInterval;
		private System.Windows.Forms.ColumnHeader colTimerCount;
		private System.Windows.Forms.PropertyGrid pgData;
		private System.Windows.Forms.ToolStripButton tsbnRemove;
		private System.Windows.Forms.ToolStripButton tsbnAdd;
		private System.Windows.Forms.ContextMenuStrip cmsTimers;
		private System.Windows.Forms.ToolStripMenuItem tsmiTimersInvoke;
		private System.Windows.Forms.ToolStripMenuItem tsmiTimersStop;
	}
}
