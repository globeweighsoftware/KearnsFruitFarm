using System.Collections;
using System.ComponentModel;

namespace Globeweigh.UI.Touch
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(this.Context.Parameters["AssemblyPath"]) + @"\Globeweigh.UI.Touch.exe");
        }
    }
}
