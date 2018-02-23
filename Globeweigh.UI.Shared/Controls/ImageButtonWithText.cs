﻿using System.Windows;
using System.Windows.Controls;

namespace Globeweigh.UI.Shared.Controls
{
    public class ImageButtonWithText : Button
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(string), typeof(ImageButtonWithText));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty SecondaryContentProperty = DependencyProperty.Register("SecondaryContent", typeof(string), typeof(ImageButtonWithText));

        public string SecondaryContent
        {
            get { return (string)GetValue(SecondaryContentProperty); }
            set { SetValue(SecondaryContentProperty, value); }
        }
    }
}
