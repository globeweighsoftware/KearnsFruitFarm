using System;
using System.IO;
using Globeweigh.Model;

namespace Globeweigh.UI.Shared.Helpers
{
    public class AppShortcut
    {
        public static bool AutoStart(bool enable)
        {
            try
            {

                if (!ClickOnceHelper.IsApplicationNetworkDeployed)
                    return false;

                String startupShortcut = ClickOnceHelper.GetStartupShortcut(ClickOnceHelper.AssemblyProductName,
                                                                            ClickOnceHelper.ShortcutType.Application);

                // Always remove the startup shortcut if it exists.  
                // This will handling disabling the run at startup functionality 
                // or ensure the most recent shortcut is copied into the Startup folder if we're enabling.
                if (File.Exists(startupShortcut))
                {
                    File.Delete(startupShortcut);
                }

                if (!enable)
                {
                    return false;
                }

                String programShortcut = ClickOnceHelper.GetProgramShortcut(ClickOnceHelper.AssemblyCompanyName,
                                                                            ClickOnceHelper.GetApplicationExecutable());

                if (File.Exists(programShortcut))
                {
                    // Enable run at startup by copying the progam shortcut into the startup folder.
                    File.Copy(programShortcut, startupShortcut);
                    GlobalVariables.ApplicationClickOnceProgramShortcut = programShortcut;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "ClickOnce AutoStart");
                return false;
            }

        }
    }
}
