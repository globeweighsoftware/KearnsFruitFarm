using System.Windows;
using System.Windows.Interop;
using Globeweigh.Admin.Helpers;
using Globeweigh.UI.Shared.Helpers;

namespace Globeweigh.Admin
{
    public class WindowBase : Window
    {
        public static readonly DependencyProperty NoOpacityChangeProperty = DependencyProperty.Register("NoOpacityChange", typeof(bool), typeof(WindowBase), new UIPropertyMetadata(false));
        public bool NoOpacityChange
        {
            get { return (bool)GetValue(NoOpacityChangeProperty); }
            set { SetValue(NoOpacityChangeProperty, value); }
        }

        public static readonly DependencyProperty IsMainWindowSizeProperty = DependencyProperty.Register("IsMainWindowSize", typeof(bool), typeof(WindowBase), new UIPropertyMetadata(false));
        public bool IsMainWindowSize
        {
            get { return (bool)GetValue(IsMainWindowSizeProperty); }
            set { SetValue(IsMainWindowSizeProperty, value); }
        }

        public WindowBase()
        {
            Style style = this.FindResource("WindowBaseStyle") as Style;
            this.Style = style;


            SourceInitialized += (s, e) =>
            {
                if (this.Owner == null) return;
                if (!NoOpacityChange) this.Owner.Opacity = 0.4;
                if (IsMainWindowSize)
                {
                    if (UtilitiesShared.IsMyMachine)
                    {
                        var mainWindowHandle = Utilities.GetMainWindow();
                        this.Width = mainWindowHandle.Width;
                        this.Height = mainWindowHandle.Height;
                        this.Top = mainWindowHandle.Top;
                        this.Left = mainWindowHandle.Left;
                        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    }
                    else
                    {
                        WindowState = WindowState.Maximized;
                    }
                }
                else
                {
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }

            };
            Closed += (s, e) =>
            {
                if (this.Owner == null) return;
                this.Owner.Opacity = 1;
            };
        }
    }
}
