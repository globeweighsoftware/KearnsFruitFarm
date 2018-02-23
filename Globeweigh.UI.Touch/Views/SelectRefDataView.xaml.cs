﻿using DevExpress.Xpf.Grid;
using Globeweigh.UI.Shared.Helpers;
using System.Windows;

namespace Globeweigh.UI.Touch
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SelectRefDataView
    {
        public SelectRefDataView()
        {
            InitializeComponent();
        }

        private void grdRefData_Unloaded(object sender, RoutedEventArgs e)
        {
            ((GridControl) sender).ColumnsSource = null;
        }
    }
}
