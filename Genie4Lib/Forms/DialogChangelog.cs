using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Emulators;

namespace GenieClient
{
    public partial class DialogChangelog
    {
        private void OK_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public DialogChangelog()
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            // Add any initialization after the InitializeComponent() call.
            TextBoxInfo.Text = Genie4Lib.MyResources.GetApplicationName() + System.Environment.NewLine + 
                string.Format("Version {0}", Genie4Lib.MyResources.GetApplicationVersion()) + System.Environment.NewLine + 
                string.Format("Build Date {0}", Utility.AssemblyBuildDate(assembly)) + System.Environment.NewLine + 
                Utility.AssemblyCopyright(assembly) + System.Environment.NewLine +
                Utility.AssemblyCompanyName() + System.Environment.NewLine + 
                Utility.AssemblyDescription(assembly) + System.Environment.NewLine;





            var o = Assembly.GetExecutingAssembly().GetManifestResourceStream("GenieClient.Changelog.txt");
            if (!Information.IsNothing(o))
            {
                var s = new StreamReader(o);
                if (!Information.IsNothing(s))
                {
                    TextBoxInfo.AppendText(s.ReadToEnd());
                }
            }

            // For Each sText As String In GetListOfEmbeddedResources()
            // Me.TextBoxInfo.AppendText(sText & vbNewLine)
            // Next
        }

        public Array GetListOfEmbeddedResources()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }
    }
}