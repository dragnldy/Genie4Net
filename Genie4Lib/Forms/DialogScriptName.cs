using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GenieClient
{
    public partial class DialogScriptName
    {
        public DialogScriptName()
        {
            InitializeComponent();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ScriptName
        {
            get
            {
                    return TextBoxName.Text;
            }

            set
            {
                TextBoxName.Text = value;
            }
        }
    }
}