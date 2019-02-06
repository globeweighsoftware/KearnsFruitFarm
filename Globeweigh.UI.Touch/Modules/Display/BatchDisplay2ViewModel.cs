using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Globeweigh.UI.Shared;
using Globeweigh.UI.Shared.Services;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Helpers;
using System.Threading.Tasks;
using System.Collections.Async;
using GalaSoft.MvvmLight.CommandWpf;
using Globeweigh.Model.Custom;

namespace Globeweigh.UI.Touch
{
    public class BatchDisplay2ViewModel : BindableBase, IViewModel
    {
        #region private fields

        private readonly IPortionDisplayRepository _portionDisplayRepo = SimpleIoc.Default.GetInstance<IPortionDisplayRepository>();
        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();

        private DispatcherTimer _timer;

        public RelayCommand ExitCommand => new RelayCommand(OnExit);


        #endregion

        #region Properties

        private List<UserDisplay> _OperatorList;
        public List<UserDisplay> OperatorList
        {
            get { return _OperatorList; }
            set { Set(ref _OperatorList, value); }
        }

        private vwBatchView _LiveBatchView;
        public vwBatchView LiveBatchView
        {
            get { return _LiveBatchView; }
            set { Set(ref _LiveBatchView, value); }
        }


        #endregion

        private void OnExit()
        {
            Application.Current.Shutdown();
        }

        private async void timer_tick(object sender, EventArgs e)
        {
            try
            {
                LiveBatchView = await _batchRepo.GetLiveBatchAsync();
                if(LiveBatchView == null)return;
                
                OperatorList = new List<UserDisplay>(await _portionDisplayRepo.GetDisplayUserList());
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }

        }

        public async void Load(FrameworkElement element)
        {
            try
            {
                LiveBatchView = await _batchRepo.GetLiveBatchAsync();


                if (UtilitiesShared.InDesignMode) return;
                _timer = new DispatcherTimer();
                _timer.Interval = new TimeSpan(0, 0, 0, 1);
                _timer.Tick += timer_tick;
                _timer.Start();
            }
            catch (Exception e)
            {
            }

        }


        public void Unload(FrameworkElement element)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= timer_tick;
                _timer = null;
            }
        }
    }
}
