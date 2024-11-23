using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Plugin.Timers
{
	internal class TimeSpanEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			=> UITypeEditorEditStyle.DropDown;

		public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
		{
			IWindowsFormsEditorService frmsvr = null;
			frmsvr = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			if(frmsvr != null)
			{
				DateTimePicker picker = new DateTimePicker()
				{
					ShowUpDown = true,
					Value = DateTime.Today.Add((TimeSpan)value),
					Format = DateTimePickerFormat.Time,
				};

				frmsvr.DropDownControl(picker);
				value = picker.Value.TimeOfDay;
			}

			return value;
		}
	}
}