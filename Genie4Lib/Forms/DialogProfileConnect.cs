using Emulators;
using GenieClient.Genie;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GenieClient
{
    public partial class DialogProfileConnect
    {
        public DialogProfileConnect()
        {
            InitializeComponent();
            EditNote.Enabled = false;
            _OK_Button.Enabled = false;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
                OK_Close();
        }

        private bool Ok2Close()
        {
            if (ListBoxProfiles.SelectedItem is null)
            {
                Interaction.Beep();
                MessageBox.Show("Please select a profile to connect.", "No Profile Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void OK_Close()
        {
            if (Ok2Close())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            Cancel_Close();
        }

        private void Cancel_Close()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string ProfileName
        {
            get
            {
                if (!Information.IsNothing(ListBoxProfiles.SelectedItem))
                {
                    return ListBoxProfiles.SelectedItem.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)] 
        public string ControlWithFocus { get; set; }
        private void ListBoxProfiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
                OK_Close();
        }
        private void ListBoxProfiles_MouseClick(object sender, MouseEventArgs e)
        {
            _OK_Button.Enabled = ListBoxProfiles.SelectedItem is not null;
        }

        private void ListBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ControlWithFocus  == ListBoxProfiles.Name &&
                ListBoxProfiles.SelectedItem is not null)
            {
                string selectedProfile = ListBoxProfiles.SelectedItem.ToString();
                // Find the corresponding TreeNode in the TreeView
                bool found = false;
                foreach (TreeNode node in _profiles.Nodes)
                {
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        TreeNode foundNode = childNode.Nodes
                            .Cast<TreeNode>()
                            .FirstOrDefault(n => n.Name == selectedProfile);
                        if (foundNode != null)
                        {
                            _profiles.SelectedNode = foundNode;
                            _profiles.SelectedNode.EnsureVisible();
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                EditNote.Enabled = false;
            }
            _OK_Button.Enabled = ListBoxProfiles.SelectedItem is not null;
        }
        private string m_sConfigDir = string.Empty;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)] 
        public bool ClassicConnect { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ConfigDir
        {
            get
            {
                return m_sConfigDir;
            }

            set
            {
                m_sConfigDir = value;
            }
        }
        private void DialogProfileConnect_VisibleChanged(object sender, EventArgs e)
        {
            string sFile = string.Empty;
            if (Visible == true)
            {
                _profiles.Visible = !ClassicConnect;
                ControlWithFocus = _profiles.Visible ? _profiles.Name : ListBoxProfiles.Name;

                string[] profiles = Directory.GetFiles(m_sConfigDir + @"\Profiles\", "*.xml");
                ListBoxProfiles.Items.Clear();
                _profiles.Nodes.Clear();
                foreach (string profile in profiles)
                {
                    string fileContents = "";
                    using (StreamReader reader = new StreamReader(profile))
                    {
                        fileContents = reader.ReadToEnd();
                    }
                    if (fileContents.Length > 0)
                    {
                        Genie.XMLConfig xml = new Genie.XMLConfig();
                        xml.LoadXml(fileContents);
                        string game = xml.GetValue("Genie/Profile", "Game", "");
                        string account = xml.GetValue("Genie/Profile", "Account", "");
                        if (string.IsNullOrWhiteSpace(account)) account = "ACCOUNT_UNKNOWN";
                        string note = xml.GetValue("Genie/Profile", "Note", "");
                        if (!_profiles.Nodes.ContainsKey(game)) _profiles.Nodes.Add(game, game);
                        if (!_profiles.Nodes[game].Nodes.ContainsKey(account)) _profiles.Nodes[game].Nodes.Add(account, account);
                        string profileName = Path.GetFileNameWithoutExtension(profile);
                        string profileText = profileName;
                        if (!string.IsNullOrWhiteSpace(note)) profileText += $" - {note}";
                        TreeNode profileNode = new TreeNode();
                        profileNode.Name = profileName;
                        profileNode.Text = profileText;
                        profileNode.Tag = profile;
                        _profiles.Nodes[game].Nodes[account].Nodes.Add(profileNode);
                        ListBoxProfiles.Items.Add(profileName);
                    }
                }
                _profiles.ExpandAll();
                if(_profiles.Nodes.Count > 0) _profiles.Nodes[0].EnsureVisible(); //this will scroll the window to the top of the list

            }
        }
        private void ListBoxProfiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OK_Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Cancel_Close();
            }
            else
            {
                // Handle other key events if necessary
                e.Handled = true; // signal that the key event has been handled
                Interaction.Beep();
            }
        }

        private void ToggleView_Click(object sender, EventArgs e)
        {
            _profiles.Visible = !_profiles.Visible;
            if (_profiles.Visible)
            {
                _OK_Button.Enabled = _profiles.SelectedNode is not null && _profiles.SelectedNode.Level == 2;
                EditNote.Enabled = _OK_Button.Enabled;
                _profiles.Focus();
                ControlWithFocus = _profiles.Name;
            }
            else
            {
                _OK_Button.Enabled = ListBoxProfiles.SelectedItem is not null;
                EditNote.Enabled = false;
                ControlWithFocus = ListBoxProfiles.Name;
            }
        }

        private void _profiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 2)
            {
                ListBoxProfiles.SelectedItem = e.Node.Name;
                EditNote.Enabled = true;
                _OK_Button.Enabled = true;
            }
            else
            {
                EditNote.Enabled = false;
                _OK_Button.Enabled = false;
                ListBoxProfiles.SelectedItem = null; // Clear the selection in ListBoxProfiles
            }
        }

        private void _profiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OK_Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Cancel_Close();
            }
        }
        private void _profiles_DoubleClick(object sender, EventArgs e)
        {
            // We will check to see if profile is selected before closing
            OK_Close();
        }

        private void EditNote_Click(object sender, EventArgs e)
        {
            if (!_profiles.Visible)
            {
                MessageBox.Show("Please toggle the view to edit and display notes.", "Wrong View", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_profiles.SelectedNode is not null && _profiles.SelectedNode.Level == 2)
            {
                int noteStart = _profiles.SelectedNode.Text.IndexOf(" - ");
                string noteText = "";
                if (noteStart > 0) noteText = _profiles.SelectedNode.Text.Substring(noteStart + 3).Trim();
                DialogProfileNote dProfile = Genie4Lib.MyResources.FormResource("DialogProfileNote", true) as DialogProfileNote;
                if (dProfile is null)
                {
                    MessageBox.Show("Unable to open the note dialog. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dProfile.NoteText = noteText;
                if (dProfile.ShowDialog(this) == DialogResult.OK)
                {
                    string note = dProfile.NoteText;
                    string profileText = _profiles.SelectedNode.Name;
                    if (!string.IsNullOrWhiteSpace(note)) profileText += $" - {note}";
                    _profiles.SelectedNode.Text = profileText;
                    Genie.XMLConfig xml = new XMLConfig();
                    xml.LoadFile(_profiles.SelectedNode.Tag.ToString());
                    xml.SetValue("Genie/Profile", "Note", note);
                    xml.SaveToFile(_profiles.SelectedNode.Tag.ToString());
                }
            }
        }
    }
}