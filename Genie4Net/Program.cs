using GenieClient;

namespace Genie4Net
{
    internal static class Program
    {
        public static FormMain FormMain { get; internal set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            FormMain = Genie4Lib.MyResources.FormResource("FormMain", true) as FormMain;    
            if (FormMain == null)
            {
                throw new InvalidOperationException("Failed to load FormMain.");
            }
            Application.Run(FormMain);
        }
    }
}