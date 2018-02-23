using System;
using System.Windows;
using System.Windows.Controls;

namespace Globeweigh.UI.Shared.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(Uri), typeof(ImageButton));

        public Uri ImageSource
        {
            get { return (Uri)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty SecondaryContentProperty = DependencyProperty.Register("SecondaryContent", typeof(string), typeof(ImageButton));

        public string SecondaryContent
        {
            get { return (string)GetValue(SecondaryContentProperty); }
            set { SetValue(SecondaryContentProperty, value); }
        }
    }
}
