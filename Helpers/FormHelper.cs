using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CitRobots.Helpers
{
    public static class FormHelper
    {
        public static void ClearForm(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox) ((TextBox)c).Clear();
                else if (c is ComboBox) ((ComboBox)c).SelectedIndex = -1;
                else if (c is CheckBox) ((CheckBox)c).Checked = false;
                else if (c is RadioButton) ((RadioButton)c).Checked = false;
                else if (c.HasChildren) ClearForm(c);
            }
        }

        public static void DisableForm(Control parent, bool disable)
        {
            foreach (Control c in parent.Controls)
            {
                c.Enabled = !disable;
                if (c.HasChildren) DisableForm(c, disable);
            }
        }
    }
}
