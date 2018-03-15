using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows;
using DevExpress.Utils.TouchHelpers;
using DevExpress.Xpf.Core;
using System.Threading;
using Globeweigh.UI.Shared;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.UI.Shared.Services;
using MahApps.Metro.Controls;

namespace Globeweigh.UI.Touch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private int exitCout;
        private static IDeviceRepository _deviceRepo = SimpleIoc.Default.GetInstance<IDeviceRepository>();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        //        private SerialPort _serialCom1;

        public MainWindow()
        {
            InitializeComponent();
            //            TouchKeyboardSupport.EnableTouchKeyboard = true;
            TouchKeyboardSupport.EnableFocusTracking();

            WindowState = UtilitiesShared.IsMyMachine ? WindowState.Normal : WindowState.Maximized;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);

//            DocumentPreviewControl.DocumentSource = new rptBoxLabelDefault(null);
        }

        private async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() => TickMethod());
        }

        private async Task TickMethod()
        {
            bool restart = await UtilitiesShared.PollDevice(_deviceRepo);
            if (restart)
            {
                dispatcherTimer.Stop();

                Dispatcher.Invoke(() =>
                {
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                });


            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
//            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
//            {
//                GlobalVariables.CurrentDevice.IsDisplayDevice = true;
//            }
            dispatcherTimer.Start();
        }
    }
}
