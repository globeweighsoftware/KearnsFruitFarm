using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Services;
using Globeweigh.UI.Touch.Model;
using Globeweigh.UI.Shared;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.UI.Shared.Helpers;
using System;
using System.Windows.Threading;
using Globeweigh.Model;
using MvvmDialogs;

namespace Globeweigh.UI.Touch
{
    public class BatchWaitViewModel : BindableBase, IViewModel
    {
        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private DispatcherTimer _timer;

        public RelayCommand ExitCommand => new RelayCommand(OnExit);


        private void OnExit()
        {
            Application.Current.Shutdown();
        }

        private async void timer_tick(object sender, EventArgs e)
        {
            try
            {
                var batch = await _batchRepo.GetLiveBatchAsync();
                if (batch == null)
                {
                    if (SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel.GetType() == typeof(BatchDetailsViewModel))
                    {
                        var vm = SimpleIoc.Default.GetInstance<BatchDetailsViewModel>();
                        vm.OnExit();
                    }
                    return;
                }
                //            _timer.Stop();
                var batchDetailsViewModel = SimpleIoc.Default.GetInstance<BatchDisplayViewModel>();
                batchDetailsViewModel.SelectedBatchView = batch;
                SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = batchDetailsViewModel;
            }
            catch 
            {
            }

        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _timer.Tick += timer_tick;
            _timer.Start();
        }

        public void Unload(FrameworkElement element)
        {
            _timer?.Stop();
            _timer = null;
        }
    }
}
