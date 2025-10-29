using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Plugin.Timers.Settings;

namespace Plugin.Timers.UI
{
	public partial class ConfigCtrl : UserControl
	{
		private readonly Plugin _plugin;

		private TimerSettingsItem SelectedItem
			=> lvTimers.SelectedItems.Count == 0 ? null : (TimerSettingsItem)lvTimers.SelectedItems[0].Tag;
		 
		public ConfigCtrl(Plugin plugin)
		{
			this._plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));

			this.InitializeComponent();
			this.DataBind();
		}

		private void DataBind()
			=> this.AddListItem(this._plugin.Settings.TimerData);

		private void AddListItem(IEnumerable<TimerSettingsItem> proxyItems)
		{
			List<ListViewItem> itemsToAdd = new List<ListViewItem>();
			String[] subItems = Array.ConvertAll<String, String>(new String[lvTimers.Columns.Count], a => String.Empty);

			foreach(TimerSettingsItem item in proxyItems)
			{
				ListViewItem listItem = new ListViewItem();
				listItem.SubItems.AddRange(subItems);
				listItem.SubItems[colTimerName.Index].Text = item.TimerName;
				listItem.SubItems[colTimerInterval.Index].Text = item.Interval.ToString();
				listItem.SubItems[colTimerCount.Index].Text = this._plugin.Timers.GetTimers(item.TimerName).Count().ToString("N0");
				listItem.Tag = item;
				itemsToAdd.Add(listItem);
			}
			lvTimers.Items.AddRange(itemsToAdd.ToArray());

			ColumnHeaderAutoResizeStyle headerAutoResize = itemsToAdd.Count == 0
				? ColumnHeaderAutoResizeStyle.HeaderSize
				: ColumnHeaderAutoResizeStyle.ColumnContent;
			lvTimers.AutoResizeColumns(headerAutoResize);
		}

		private void tsbnAdd_Click(Object sender, EventArgs e)
		{
			TimerSettingsItem newItem = new TimerSettingsItem(this._plugin.Settings.Default);
			this._plugin.Settings.TimerData.AddWithCheck(newItem);
			this._plugin.Settings.SaveSettings();

			this.AddListItem(new TimerSettingsItem[] { newItem });
		}

		private void tsbnRemove_Click(Object sender, EventArgs e)
		{
			TimerSettingsItem item = this.SelectedItem;
			if(item != null)
			{
				this._plugin.Settings.TimerData.Remove(item);
				this._plugin.Settings.SaveSettings();
				lvTimers.SelectedItems[0].Remove();
			}
		}

		private void lvTimers_SelectedIndexChanged(Object sender, EventArgs e)
		{
			pgData.SelectedObject = this.SelectedItem;
			splitMain.Panel2Collapsed = pgData.SelectedObject == null;
			tsbnRemove.Enabled = pgData.SelectedObject != null;
		}

		private void lvTimers_AfterLabelEdit(Object sender, LabelEditEventArgs e)
		{
			ListViewItem listItem = lvTimers.Items[e.Item];
			TimerSettingsItem item = (TimerSettingsItem)listItem.Tag;
			if(e.Label != null && e.Label != item.TimerName)
			{//Cancel editing
				if(this._plugin.Settings.TimerData.ChangeTimerName(item, e.Label))
					this._plugin.Settings.SaveSettings();
				else
				{
					MessageBox.Show("Timer name with the same name already exists", e.Label, MessageBoxButtons.OK, MessageBoxIcon.Stop);
					e.CancelEdit = true;
				}
			}
		}

		private void lvTimers_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
			case Keys.Delete:
			case Keys.Back:
				this.tsbnRemove_Click(sender, e);
				e.Handled = true;
				break;
			case Keys.F2:
				if(lvTimers.SelectedItems.Count == 1)
				{
					lvTimers.SelectedItems[0].BeginEdit();
					e.Handled = true;
				}
				break;
			}
		}

		private void cmsTimers_Opening(Object sender, CancelEventArgs e)
		{
			ListViewItem item = lvTimers.SelectedItems.Count == 0 ? null : lvTimers.SelectedItems[0];
			e.Cancel = item == null;
			if(item != null)
				tsmiTimersStop.Enabled = tsmiTimersInvoke.Enabled = item.SubItems[colTimerCount.Index].Text != "0";
		}

		private void cmsTimers_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			ListViewItem selectedItem = lvTimers.SelectedItems.Count == 0 ? null : lvTimers.SelectedItems[0];
			TimerSettingsItem settings = selectedItem == null ? null : (TimerSettingsItem)selectedItem.Tag;

			if(e.ClickedItem == tsmiTimersInvoke)
				foreach(var timer in this._plugin.Timers.GetTimers(settings.TimerName))
					timer.InvokeCallback();//Manually invoke callback method for all started timers
			else if(e.ClickedItem == tsmiTimersStop)
			{
				//I make a separate copy because timers are deleted from Factory when stopped
				List<ITimerItem> timers = new List<ITimerItem>(this._plugin.Timers.GetTimers(settings.TimerName));
				foreach(ITimerItem timer in timers)
					timer.Stop();//Stop timer
				selectedItem.SubItems[colTimerCount.Index].Text = this._plugin.Timers.GetTimers(settings.TimerName).Count().ToString("N0");
			}
		}

		private void pgData_PropertyValueChanged(Object s, PropertyValueChangedEventArgs e)
		{
			ListViewItem listItem = lvTimers.SelectedItems[0];
			String oldTimerName = listItem.SubItems[colTimerName.Index].Text;
			TimerSettingsItem item = this.SelectedItem;

			if(item.TimerName != oldTimerName)
			{
				String newTimerName = item.TimerName;
				item.TimerName = oldTimerName;
				this._plugin.Settings.TimerData.ChangeTimerName(item, newTimerName);
			}

			listItem.SubItems[colTimerInterval.Index].Text = item.Interval.ToString();
			listItem.SubItems[colTimerName.Index].Text = item.TimerName;
			
			//this._plugin.Settings.TimerData[oldTimerName] = item;
			this._plugin.Settings.SaveSettings();
		}
	}
}