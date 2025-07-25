﻿using System;
using System.Windows.Forms;
using GenieClient.Genie;

namespace GenieClient.Forms
{
    public partial class FormMapperSettings : Form
    {
        private Globals _globals;
        private static string[] _globalVariables = ["caravan", "mapwalk", "searchwalk", "drag", "broom_carpet", "verbose", "automapper.iceroadcollect", "automapper.cyclic", "automapper.sigilwalk", "automapper.seekhealing", "automapper.userwalk"];

        public event EventVariableChangedEventHandler EventVariableChanged;
        public delegate void EventVariableChangedEventHandler(string sVariable);
        public event EventClassChangeEventHandler EventClassChange;
        public delegate void EventClassChangeEventHandler();
        public FormMapperSettings(ref Globals globals)
        {
            this.InitializeComponent();
            _globals = globals;
            foreach (string mapperGlobalVariable in _globalVariables)
            {
                if (!CheckedListVariables.Items.Contains(mapperGlobalVariable))
                {
                    int i = CheckedListVariables.Items.Add(mapperGlobalVariable, GetCheckedStateVariable(mapperGlobalVariable));
                }
            }
        }

        public async void Recolor()
        {
            Globals.Presets.Preset menu = _globals.PresetList["ui.menu"];
            Globals.Presets.Preset button = _globals.PresetList["ui.button"];
            Globals.Presets.Preset textbox = _globals.PresetList["ui.textbox"];
            Globals.Presets.Preset window = _globals.PresetList["ui.window"];

            BackColor = window.BgColor;
            ForeColor = window.FgColor;

            _ButtonSetTypeahead.BackColor = button.BgColor;
            _ButtonSetTypeahead.ForeColor = button.FgColor;
            _ButtonSetDragging.BackColor = button.BgColor;
            _ButtonSetDragging.ForeColor = button.FgColor;
            _ButtonSetUserWalk.BackColor = button.BgColor;
            _ButtonSetUserWalk.ForeColor = button.FgColor;
            _ButtonSetClasses.BackColor = button.BgColor;
            _ButtonSetClasses.ForeColor = button.FgColor;
            CheckedListVariables.BackColor = textbox.BgColor;
            CheckedListVariables.ForeColor = textbox.FgColor;
            _TextboxAction.BackColor = textbox.BgColor;
            _TextboxAction.ForeColor = textbox.FgColor;
            _TextboxSuccess.BackColor = textbox.BgColor;
            _TextboxSuccess.ForeColor = textbox.FgColor;
            _TextboxRetry.BackColor = textbox.BgColor;
            _TextboxRetry.ForeColor = textbox.FgColor;
            _TextboxDragging.BackColor = textbox.BgColor;
            _TextboxDragging.ForeColor = textbox.FgColor;
            _TextboxTypeahead.BackColor = textbox.BgColor;
            _TextboxTypeahead.ForeColor = textbox.FgColor;
            _TextboxClass.BackColor = textbox.BgColor;
            _TextboxClass.ForeColor =  textbox.FgColor;

            (Genie4Lib.MyResources.FormResource("DialogDragTarget") as DialogDragTarget)?.Recolor(window, textbox, button);
            (Genie4Lib.MyResources.FormResource("DialogSetTypeahead") as DialogSetTypeahead)?.Recolor(window, textbox, button);
            (Genie4Lib.MyResources.FormResource("DialogUserWalk") as DialogUserWalk)?.Recolor(window, textbox, button);
            (Genie4Lib.MyResources.FormResource("DialogSetClasses") as DialogSetClasses)?.Recolor(window, textbox, button);
        }

        public async void VariableChanged(string variable)
        {
            if (variable == "automapper.typeahead") _TextboxTypeahead.Text = GetVariableValue(variable);
            else if (variable == "drag.target") _TextboxDragging.Text = GetVariableValue(variable);
            else if (variable == "automapper.userwalkaction") _TextboxAction.Text = GetVariableValue(variable);
            else if (variable == "automapper.userwalksuccess") _TextboxSuccess.Text = GetVariableValue(variable);
            else if (variable == "automapper.userwalkretry") _TextboxRetry.Text = GetVariableValue(variable);
            else if (CheckedListVariables.Items.Contains(variable))
            {
                CheckedListVariables.ItemCheck -= CheckedListVariables_ItemCheck;
                CheckedListVariables.SetItemChecked(CheckedListVariables.Items.IndexOf(variable), GetCheckedStateVariable(variable));
                CheckedListVariables.ItemCheck += CheckedListVariables_ItemCheck;
            }
        }

        private string GetVariableValue(string variable)
        {
            Globals.Variables.Variable tvar = _globals.VariableList.get_GetVariable(variable);
            return tvar != null ? tvar.sValue : "";
        }
        private async void FormMapperSettings_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Recolor();
                RefreshVariables();

            }
        }

        private void RefreshVariables()
        {

            for (int i = 0; i < CheckedListVariables.Items.Count; i++)
            {
                Globals.Variables.Variable retrievedVariable = _globals.VariableList.get_GetVariable(CheckedListVariables.Items[i].ToString());
                CheckedListVariables.SetItemChecked(i, retrievedVariable != null && retrievedVariable.sValue == "1");
            }
            _TextboxTypeahead.Text = GetVariableValue("automapper.typeahead");
            _TextboxDragging.Text = GetVariableValue("drag.target");
            _TextboxAction.Text = GetVariableValue("automapper.userwalkaction");
            _TextboxSuccess.Text = GetVariableValue("automapper.userwalksuccess");
            _TextboxRetry.Text = GetVariableValue("automapper.userwalkretry");
            _TextboxClass.Text = GetVariableValue("automapper.class");
            return;
        }

        private void FormMapperSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }


        private void UpdateVariable(string key, string value, bool temp)
        {
            Globals.Variables.VariableType type = temp ? Globals.Variables.VariableType.Temporary : Globals.Variables.VariableType.SaveToFile;
            Globals.Variables.Variable var = new Globals.Variables.Variable(key, value, type);
            _globals.VariableList.set_GetVariable(var.sKey, var);
            EventVariableChanged?.Invoke("$" + var.sKey);
        }

        private void UpdateVariable(Globals.Variables.Variable var)
        {
            _globals.VariableList.set_GetVariable(var.sKey, var);
            EventVariableChanged?.Invoke("$" + var.sKey);
        }

        private bool GetCheckedStateVariable(string variableName)
        {
            return GetVariableValue(variableName) == "1";
        }

        private bool GetCheckedStateClass(string className)
        {
            return _globals.ClassList.GetValue(className);
        }

        private void CheckedListVariables_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int i = (sender as CheckedListBox).SelectedIndex;
            if (i >= 0)
            {
                string key = CheckedListVariables.Items[i].ToString();
                bool isChecked = e.NewValue == CheckState.Checked;
                if (key == "drag")
                {
                    if (isChecked && !SetDragTarget())
                    {
                        e.NewValue = CheckState.Unchecked;
                        isChecked = false;
                        MessageBox.Show("You must select a target to drag.");
                    }

                    if (!isChecked && _globals.VariableList.ContainsKey("drag.target"))
                    {
                        _globals.VariableList.Remove("drag.target");
                    }
                }
                if (key == "automapper.userwalk"
                    && (string.IsNullOrWhiteSpace(GetVariableValue("automapper.userwalkaction"))
                    || string.IsNullOrWhiteSpace(GetVariableValue("automapper.userwalksuccess"))))
                {

                }
                Globals.Variables.Variable var = new Globals.Variables.Variable(key, isChecked ? "1" : "0", Globals.Variables.VariableType.Temporary);
                UpdateVariable(var);
            }
        }

        private void ButtonSetTypeahead_Click(object sender, EventArgs e)
        {
            DialogSetTypeahead dTypeahead = Genie4Lib.MyResources.FormResource("DialogSetTypeahead", true) as DialogSetTypeahead;
            dTypeahead.TargetText = GetVariableValue("automapper.typeahead");
            if (dTypeahead.ShowDialog(Parent) == DialogResult.OK)
            {
                if (int.TryParse(dTypeahead.TargetText.Trim(), out int typeahead)) _TextboxTypeahead.Text = dTypeahead.TargetText.Trim();
                else _TextboxTypeahead.Text = GetVariableValue("automapper.typeahead");

                UpdateVariable("automapper.typeahead", _TextboxTypeahead.Text, false);
            }
        }

        /// <summary>
        /// Opens the DialogUserWalk form.
        /// </summary>
        /// <returns>A bool indicating whether the configuration is valid. A valid configuration has both an Action and Success.</returns>
        private bool ConfigureUserWalk()
        {
            DialogUserWalk dWalk = Genie4Lib.MyResources.FormResource("DialogUserWalk", true) as DialogUserWalk;
            if (dWalk.ShowDialog(Parent) == DialogResult.OK)
            {
                UpdateVariable("automapper.userwalkaction", dWalk.Action, false);
                UpdateVariable("automapper.userwalksuccess", dWalk.Success, false);
                UpdateVariable("automapper.userwalkretry", dWalk.Retry, false);
            }
            return (!string.IsNullOrWhiteSpace(GetVariableValue("automapper.userwalkaction")) && !string.IsNullOrWhiteSpace(GetVariableValue("automapper.userwalksuccess")));
        }
        private bool SetDragTarget()
        {
            DialogDragTarget dTarget = Genie4Lib.MyResources.FormResource("DialogDragTarget", true) as DialogDragTarget;
            dTarget.TargetText = GetVariableValue("drag.target");
            if (dTarget.ShowDialog(Parent) == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(dTarget.TargetText.Trim()))
                {
                    if (_globals.VariableList.ContainsKey("drag.target")) _globals.VariableList.Remove("drag.target");
                }
                else
                {
                    UpdateVariable("drag.target", dTarget.TargetText.Trim(), true);
                }
            }
            else if (string.IsNullOrWhiteSpace(GetVariableValue("drag.target")))
            {
                if (_globals.VariableList.ContainsKey("drag.target")) _globals.VariableList.Remove("drag.target");
            }

            return _globals.VariableList.ContainsKey("drag.target");
        }

        private void _ButtonSetUserWalk_Click(object sender, EventArgs e)
        {
            ConfigureUserWalk();
        }

        private void _ButtonSetDragging_Click(object sender, EventArgs e)
        {
            if (GetVariableValue("drag") != "1")
            {
                if (MessageBox.Show("Dragging is currently disabled. Enable?", "Enable Dragging?", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    UpdateVariable("drag", "1", true);
                }
            }
            if (CheckedListVariables.CheckedItems.Contains("drag"))
            {
                if (!SetDragTarget())
                {
                    MessageBox.Show("You must select a target to drag.");
                    UpdateVariable("drag", "0", true);
                    if (_globals.VariableList.ContainsKey("drag.target"))
                    {
                        _globals.VariableList.Remove("drag.target");
                    }
                    _TextboxDragging.Text = GetVariableValue("drag.target");
                }
            }
        }

        private void ButtonSetClasses_Click(object sender, EventArgs e)
        {
            DialogSetClasses dSet = Genie4Lib.MyResources.FormResource("DialogSetClasses", true) as DialogSetClasses;
            dSet.ClassText = GetVariableValue("automapper.class");
            if (dSet.ShowDialog(Parent) == DialogResult.OK)
            {
                UpdateVariable("automapper.class", dSet.ClassText.Trim(), false);
            }
        }
    }
}
