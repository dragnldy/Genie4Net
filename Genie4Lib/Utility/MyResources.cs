using System.Reflection;
using System.Resources;

namespace Genie4Lib
{
    public static class MyResources
    {
        public static ResourceManager rm = new ResourceManager("Genie4Lib.Properties.Resources", Assembly.GetExecutingAssembly());

        public static string GetApplicationName()
        {
            return Application.ProductName;
        }
        public static string GetApplicationVersion()
        {
            Version assemblyVersion = Assembly.GetExecutingAssembly()?.GetName()?.Version;
            return assemblyVersion?.ToString() ?? string.Empty;
        }
        public static string StringResource(string resourceName)
        {
            return rm.GetObject(resourceName)?.ToString() ?? string.Empty;
        }
        public static Image ImageResource(string resourceName)
        {
            //find the resource "Genie4Lib.Properties.Resources.resources" among the resources "GenieClient.ComponentBars.resources", "GenieClient.ComponentIconBar.resources", "Genie4Lib.Forms.Components.ComponentIcons.resources", "GenieClient.ComponentPluginItem.resources", "GenieClient.ComponentRichTextBox.resources", "GenieClient.ComponentRoundtime.resources", "GenieClient.UCAliases.resources", "GenieClient.UCClasses.resources", "GenieClient.UCHighlightStrings.resources", "GenieClient.UCIgnore.resources", ... embedded in the assembly "Genie4Lib", nor among the resources in any satellite assemblies for the specified culture.Perhaps the resources were embedded with an incorrect name.'
            
            object resource = rm.GetObject(resourceName);
            if ( resource is not null)
            {
                return (Image)resource;
            }
            return null;
        }
        public static object Resources(string resourceName)
        {
            return rm.GetObject(resourceName);
        }
        public static Form FormResource(string resourceName, bool createifnotopen = false,bool show = false)
        {
            // To find a form by its name (assuming you set the Name property in the designer)
            Form specificForm = Application.OpenForms[resourceName];
            if (specificForm == null && createifnotopen)
            {
                // If the form is not open, create a new instance
                specificForm = (Form)Activator.CreateInstance(Type.GetType("GenieClient." + resourceName));
            }

            if (specificForm != null && !specificForm.IsHandleCreated)
            {
                // If the form is already open but not created, ensure it is shown
                if (show) specificForm.Show();
            }
            return specificForm;
        }
        //public static Form FormResource(string formType, bool createifnotopen = false)
        //{
        //    // Or, iterate and cast
        //    foreach (Form form1 in Application.OpenForms)
        //    {
        //        if (form1.GetType() == Type.GetType(formType)) // Check if the form is of the desired type
        //        {
        //            return form1;
        //        }
        //    }
        //    return null;
        //}
        public static Form CreateForm(string formType)
        {
            // Create a new instance of the form if it is not already open
            Form specificForm = Application.OpenForms[formType];
            if (specificForm == null)
            {
                specificForm = (Form)Activator.CreateInstance(Type.GetType(formType));
                specificForm.Show();
            }
            return specificForm;
        }
    }
}
