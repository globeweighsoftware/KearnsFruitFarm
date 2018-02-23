using System.Windows;
using Globeweigh.UI.Shared.Helpers;

namespace Globeweigh.UI.Shared
{
    public class WindowBase : Window
    {
        public static readonly DependencyProperty NoOpacityChangeProperty = DependencyProperty.Register("NoOpacityChange", typeof(bool), typeof(WindowBase), new UIPropertyMetadata(false));
        public bool NoOpacityChange
        {
            get { return (bool)GetValue(NoOpacityChangeProperty); }
            set { SetValue(NoOpacityChangeProperty, value); }
        }

        public static readonly DependencyProperty IsOwnerSizeProperty = DependencyProperty.Register("IsOwnerSize", typeof(bool), typeof(WindowBase), new UIPropertyMetadata(false));
        public bool IsOwnerSize
        {
            get { return (bool)GetValue(IsOwnerSizeProperty); }
            set { SetValue(IsOwnerSizeProperty, value); }
        }

        public WindowBase()
        {
            Style style = this.FindResource("WindowBaseStyle") as Style;
            this.Style = style;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Normal;

            SourceInitialized += (s, e) =>
            {
                if (this.Owner == null) return;
                if (!NoOpacityChange) this.Owner.Opacity = 0.4;
                if (IsOwnerSize)
                {
                    if (UtilitiesShared.IsMyMachine)
                    {
                        this.Width = this.Owner.Width;
                        this.Height = this.Owner.Height;
                        this.Left = this.Owner.Left;
                        this.Top = this.Owner.Top;
                    }
                    else
                    {
                        WindowState = WindowState.Maximized;
                    }




                }
                else
                {
                    //                    this.Width -= this.Margin.Left;
                    //                    this.Height -= this.Margin.Top;
                    //                    this.Left += this.Margin.Left/2;
                    //                    this.Top += this.Margin.Top/2;
                    //                    this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

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
