using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Globeweigh.UI.Shared;
using Globeweigh.UI.Shared.Services;
using MvvmDialogs;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.Model;
using Globeweigh.UI.Shared.Helpers;
using System.Threading.Tasks;
using System.Collections.Async;
using DevExpress.Mvvm.POCO;
using Globeweigh.Model.Custom;

namespace Globeweigh.UI.Touch
{
    public class BatchDisplayViewModel : BindableBase, IViewModel
    {
        #region private fields

        private readonly IOperatorRepository _operatorRepo = SimpleIoc.Default.GetInstance<IOperatorRepository>();
        private readonly IPortionRepository _portionRepo = SimpleIoc.Default.GetInstance<IPortionRepository>();
        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private readonly IScaleRepository _scaleRepo = SimpleIoc.Default.GetInstance<IScaleRepository>();

        private DispatcherTimer _displayRefreshTimer;
        private DispatcherTimer _timer;


        #endregion

        #region Properties

        private int _TotalPacksCount;

        public int TotalPacksCount
        {
            get { return _TotalPacksCount; }
            set
            {
                Set(ref _TotalPacksCount, value);
                OnPropertyChanged("IsOver100");
                OnPropertyChanged("BatchLowerLimit");
                OnPropertyChanged("BatchUpperLimit");
            }
        }

        private Decimal _PacksPerMin;

        public Decimal PacksPerMin
        {
            get { return _PacksPerMin; }
            set { Set(ref _PacksPerMin, value); }
        }

//        private string _GiveawayDisplay;
//
//        public string GiveawayDisplay
//        {
//            get { return _GiveawayDisplay; }
//            set { Set(ref _GiveawayDisplay, value); }
//        }
//
//        private Decimal _TotalWeight;
//
//        public Decimal TotalWeight
//        {
//            get { return _TotalWeight; }
//            set { Set(ref _TotalWeight, value); }
//        }
//
//        private int _AverageWeight;
//
//        public int AverageWeight
//        {
//            get { return _AverageWeight; }
//            set { Set(ref _AverageWeight, value); }
//        }

        private bool _BatchInProgress;
        public bool BatchInProgress
        {
            get { return _BatchInProgress; }
            set { Set(ref _BatchInProgress, value); }
        }

        private bool _EmptyOperatorsExist;
        public bool EmptyOperatorsExist
        {
            get { return _EmptyOperatorsExist; }
            set { Set(ref _EmptyOperatorsExist, value); }
        }

        

        private List<BatchLoginOperator> _OperatorList;
        public List<BatchLoginOperator> OperatorList
        {
            get { return _OperatorList; }
            set { Set(ref _OperatorList, value); }
        }

        private List<Scale> _ScaleDisplayList;
        public List<Scale> ScaleDisplayList
        {
            get { return _ScaleDisplayList; }
            set { Set(ref _ScaleDisplayList, value); }
        }

        private List<Scale> _MainScaleList;

        public List<Scale> MainScaleList
        {
            get { return _MainScaleList; }
            set { Set(ref _MainScaleList, value); }
        }

        private vwBatchView _SelectedBatchView;
        public vwBatchView SelectedBatchView
        {
            get { return _SelectedBatchView; }
            set { Set(ref _SelectedBatchView, value); }
        }

        private List<vwPortionView> _PortionList;

        public List<vwPortionView> PortionList
        {
            get { return _PortionList; }
            set { Set(ref _PortionList, value); }
        }

        public bool IsOver100
        {
            get { return TotalPacksCount >= 100; }
        }

        public bool IsDisplayDevice => GlobalVariables.CurrentDevice.IsDisplayDevice;


        public int BatchUpperLimit
        {
            get
            {
                if (SelectedBatchView.Override) return SelectedBatchView.BatchUpperLimit;
                //                if (IsOver100 && _50Countdown == 0)
                //                    return (int)SelectedBatchView.BatchLowerLimit + SelectedBatchView.Band;
                return (int)(SelectedBatchView.BatchUpperLimit);

            }
        }

        public int BatchLowerLimit
        {
            get
            {
                if (SelectedBatchView.Override) return SelectedBatchView.BatchLowerLimit;
                //                if (IsOver100 && _50Countdown == 0)
                //                    return (int)SelectedBatchView.BatchLowerLimit;
                return (int)SelectedBatchView.NominalWeight;

            }
        }

        #endregion

        #region Commands

        #endregion

        private async Task AddOperatorToScale(Scale selectedScale, BatchLoginOperator selectedOperator)
        {
            selectedScale.OperatorId = selectedOperator.id;
            selectedScale.OperatorName = selectedOperator.FullName;
            if (selectedOperator.TimeElapsedTicks == null)
            {
            }
            else
            {
                if (selectedOperator.TimeElapsedTicks == null)
                    selectedScale.TimeElapsedTicks = 0;
                else
                    selectedScale.TimeElapsedTicks = (long)selectedOperator.TimeElapsedTicks;
            }
            selectedScale.UserPackCount = PortionList.Count(a => a.OperatorId == selectedScale.OperatorId);
        }

        private async void OnOperatorLogOff(Scale scale)
        {
            try
            {
                scale.UserPackCount = 0;
                scale.OperatorId = null;
                scale.TimeElapsedTicks = 0;
                OperatorList =new List<BatchLoginOperator>(await _operatorRepo.GetOperatorsForBatch(SelectedBatchView.id));

                if (!IsDisplayDevice)
                {
                    if (MainScaleList.All(a => a.OperatorId == null))
                    {
                        await _batchRepo.UpdateBatchTimeElapsedAsync(SelectedBatchView.id, SelectedBatchView.TimeElapsedTicks, 0);
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "OnOperatorLogOff");
            }

        }

        private async void slaveScaleTimer_tick(object sender, EventArgs e)
        {
            try
            {
                bool batchLive = await _batchRepo.IsBatchLive(SelectedBatchView.id);

                if (!batchLive)
                {
                    SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = SimpleIoc.Default.GetInstance<BatchWaitViewModel>();
                    return;
                }

                var changes = await _scaleRepo.RefreshOperatorsForDisplayDevice(MainScaleList);
                if (!changes) return;

                foreach (var scale in MainScaleList.Where(a => a.OperatorChanged))
                {
                    if (scale.OperatorId == null)
                    {
                        OnOperatorLogOff(scale);
                    }
                    else
                    {
                        var operatorRefData = _OperatorList.FirstOrDefault(a => a.id == scale.OperatorId);
                        if (operatorRefData == null) continue;
                        await AddOperatorToScale(scale, operatorRefData);
                    }
                }
            }
            catch (Exception ex)
            {
//                ErrorLogging.LogError(ex, "slaveScaleTimer_tick");
            }

        }

        private async void timer_tick(object sender, EventArgs e)
        {
            try
            {
                if (IsDisplayDevice)
                {
                    await RefreshPortionList();
//                    var changes = await _scaleRepo.RefreshOperatorsForDisplayDevice(ScaleList);
                }
                bool batchTimeupDated = false;
                foreach (var scale in MainScaleList.Where(scale => scale.Active))
                {
                    if (scale.OperatorId != null)
                    {
                        TimeSpan ts = TimeSpan.FromSeconds(1);
                        scale.TimeElapsedTicks = scale.TimeElapsedTicks + ts.Ticks;
                        if (!batchTimeupDated)
                        {
                            SelectedBatchView.TimeElapsedTicks = SelectedBatchView.TimeElapsedTicks + ts.Ticks;
                            SelectedBatchView.TimeElapsedDisplay =
                                TimeSpan.FromTicks(SelectedBatchView.TimeElapsedTicks).ToString();
                            batchTimeupDated = true;
                        }
                    }
                }
                if (TotalPacksCount == 0 || TimeSpan.FromTicks(SelectedBatchView.TimeElapsedTicks).TotalMinutes == 0)
                    PacksPerMin = 0;
                else
                    PacksPerMin = ((Decimal) (TotalPacksCount /
                                              (TimeSpan.FromTicks(SelectedBatchView.TimeElapsedTicks).TotalSeconds) *
                                              60));

                BatchInProgress = batchTimeupDated;
            }
            catch (Exception ex)
            {
//                ErrorLogging.LogError(ex, "timer_tick");
            }
            finally
            {
            }

        }

        private async Task RefreshPortionList()
        {
            EmptyOperatorsExist = MainScaleList.Any(a => a.OperatorId != null && a.OperatorName == null);
            if(EmptyOperatorsExist) return;


            PortionList = new List<vwPortionView>(await _portionRepo.GetPortionsAsync(SelectedBatchView.id));
            TotalPacksCount = PortionList.Count;
            if (TotalPacksCount == 0) return;
            foreach (var scale in MainScaleList)
            {
                if (!scale.Active) continue;
                if (scale.OperatorId == null) continue;
                scale.UserPackCount = PortionList.Count(a => a.OperatorId == scale.OperatorId);
            }

            ScaleDisplayList = new List<Scale>(MainScaleList.Where(a=>a.OperatorId !=null).OrderByDescending(a=>a.UserPacksPerMin));

        }

        private async Task OpenAndSetTcpConnections()
        {
            //MY MACHINE DEMO MODE
            if (UtilitiesShared.IsMyMachine)
            {
                foreach (var scale in MainScaleList)
                {
                    scale.IsConnected = true;
                }
                return;
            }

            var bag = new ConcurrentBag<object>();
            await MainScaleList.ParallelForEachAsync(async item =>
            {
                // some pre stuff
                var response = await Task.Run(() => OpenAndSetTcpConnection(item, true));
                bag.Add(response);
                // some post stuff
            }, maxDegreeOfParalellism: 10);
            var count = bag.Count;
        }



        private async Task<object> OpenAndSetTcpConnection(Scale scale, bool fromBatchStart)
        {
            try
            {

                scale.IsBusy = true;

                scale.OperatorId = null;
                if (scale.Active)
                {
                    scale.TcpConnection = new NetConnection();
                    var success = scale.TcpConnection.Connect(IPAddress.Parse(scale.ScaleIpAddress), 23);
                    if (!success)
                    {
                        scale.IsConnected = false;
                        return null;
                    }
                    scale.IsConnected = true;
                }
            }

            catch (Exception ex)
            {
                scale.IsConnected = false;
            }
            finally
            {
                scale.IsBusy = false;
            }
            return null;
        }



        public async void Load(FrameworkElement element)
        {
            try
            {
                if (UtilitiesShared.InDesignMode) return;
                _timer = new DispatcherTimer();
                _timer.Interval = new TimeSpan(0, 0, 0, 1);
                _timer.Tick += timer_tick;
                _timer.Start();
                //            _connectionStatusTimer = new DispatcherTimer();
                //            _connectionStatusTimer.Interval = new TimeSpan(0, 0, 0, 10);
                //            _connectionStatusTimer.Tick += conectionStatusTimer_tick;

                if (IsDisplayDevice)
                {
                    _displayRefreshTimer = new DispatcherTimer();
                    _displayRefreshTimer.Interval = new TimeSpan(0, 0, 0, 1);
                    _displayRefreshTimer.Tick += slaveScaleTimer_tick;
                    _displayRefreshTimer.Start();
                }

                //            CurrentDate = DateTime.Now;
                OperatorList = new List<BatchLoginOperator>(await _operatorRepo.GetOperatorsForBatch(SelectedBatchView.id));
                MainScaleList = new List<Scale>(await _scaleRepo.GetScales());
                SelectedBatchView.TimeElapsedDisplay = TimeSpan.FromTicks(SelectedBatchView.TimeElapsedTicks).ToString();
                await RefreshPortionList();
                OpenAndSetTcpConnections();
            }
            catch (Exception e)
            {
            }

        }

        public void Unload(FrameworkElement element)
        {
            try
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Tick -= timer_tick;
                    _timer = null;
                }
                if (_displayRefreshTimer != null)
                {
                    _displayRefreshTimer.Stop();
                    _displayRefreshTimer.Tick -= slaveScaleTimer_tick;
                    _displayRefreshTimer = null;
                }

                //                _connectionStatusTimer.Stop();
                foreach (var scale in MainScaleList)
                {
                    if (!scale.Active) continue;
                    if (scale.TcpConnection == null) continue;
                    scale.TcpConnection.Disconnect();
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "BatchDetailsViewModel_Unload");
            }
            finally
            {
                MainScaleList = null;
                PacksPerMin = 0;
                TotalPacksCount = 0;
//                TotalWeight = 0;
//                GiveawayDisplay = "";
//                AverageWeight = 0;
                PortionList = null;
                OperatorList = null;
            }

        }
    }
}
