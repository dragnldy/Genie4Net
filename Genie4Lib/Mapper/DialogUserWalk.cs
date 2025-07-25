using GenieClient.Genie;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GenieClient
{
    public partial class DialogUserWalk
    {
        public DialogUserWalk()
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
        public string Action
        {
            get
            {
                return _TextboxAction.Text;
            }
            set
            {
                _TextboxAction.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Success
        {
            get
            {
                return _TextboxSuccess.Text;
            }
            set
            {
                _TextboxSuccess.Text = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Retry
        {
            get
            {
                return _TextboxRetry.Text;
            }
            set
            {
                _TextboxRetry.Text = value;
            }
        }

        public void Recolor(Globals.Presets.Preset window, Globals.Presets.Preset textbox, Globals.Presets.Preset button)
        {
            BackColor = window.BgColor;
            ForeColor = window.FgColor;
            _TextboxAction.ForeColor = textbox.FgColor;
            _TextboxAction.BackColor = textbox.BgColor;
            _TextboxSuccess.ForeColor = textbox.FgColor;
            _TextboxSuccess.BackColor = textbox.BgColor;
            _TextboxRetry.ForeColor = textbox.FgColor;
            _TextboxRetry.BackColor = textbox.BgColor;
            OK_Button.ForeColor = button.FgColor;
            OK_Button.BackColor = button.BgColor;
            Cancel_Button.ForeColor = button.FgColor;
            Cancel_Button.BackColor = button.BgColor;
        }
    }
}