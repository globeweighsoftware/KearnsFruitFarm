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
    public class BatchDetailsViewModel : BindableBase, IViewModel
    {
        #region private fields

        private readonly IOperatorRepository _operatorRepo = SimpleIoc.Default.GetInstance<IOperatorRepository>();
        private readonly IDialogService _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        private readonly IPortionRepository _portionRepo = SimpleIoc.Default.GetInstance<IPortionRepository>();
        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private readonly IScaleRepository _scaleRepo = SimpleIoc.Default.GetInstance<IScaleRepository>();

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

        private string _GiveawayDisplay;

        public string GiveawayDisplay
        {
            get { return _GiveawayDisplay; }
            set { Set(ref _GiveawayDisplay, value); }
        }

        private Decimal _TotalWeight;

        public Decimal TotalWeight
        {
            get { return _TotalWeight; }
            set { Set(ref _TotalWeight, value); }
        }

        private int _AverageWeight;

        public int AverageWeight
        {
            get { return _AverageWeight; }
            set { Set(ref _AverageWeight, value); }
        }

        private bool _BatchInProgress;

        public bool BatchInProgress
        {
            get { return _BatchInProgress; }
            set { Set(ref _BatchInProgress, value); }
        }

        private DateTime _CurrentDate;

        public DateTime CurrentDate
        {
            get { return _CurrentDate; }
            set { Set(ref _CurrentDate, value); }
        }

        private List<BatchLoginOperator> _OperatorList;
        public List<BatchLoginOperator> OperatorList
        {
            get { return _OperatorList; }
            set { Set(ref _OperatorList, value); }
        }

        private List<Scale> _ScaleList;

        public List<Scale> ScaleList
        {
            get { return _ScaleList; }
            set { Set(ref _ScaleList, value); }
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



        public int BatchUpperLimit
        {
            get
            {
                if (SelectedBatchView.Override) return SelectedBatchView.BatchUpperLimit;
                return (int)(SelectedBatchView.BatchUpperLimit);

            }
        }

        public int BatchLowerLimit
        {
            get
            {
                if (SelectedBatchView.Override) return SelectedBatchView.BatchLowerLimit;
                return (int)SelectedBatchView.NominalWeight;

            }
        }

        #endregion

        #region Commands

        public RelayCommand<KeyEventArgs> KeyPreviewDownCommand => new RelayCommand<KeyEventArgs>(OnKeyPreviewDown);
        public RelayCommand NavigateBackCommand => new RelayCommand(OnExit, CanExit);
        public RelayCommand<Scale> OperatorLogOffCommand => new RelayCommand<Scale>(OnOperatorLogOff);
        public RelayCommand<Scale> SelectUserCommand => new RelayCommand<Scale>(OnSelectUser);
        public RelayCommand<Scale> RetryConnectionCommand => new RelayCommand<Scale>(OnRetryConnectionCommand);
        public RelayCommand OverrideCommand => new RelayCommand(OnOverrideCommand);
        public RelayCommand BreakCommand => new RelayCommand(OnBreakCommand, CanOnBreak);



        #endregion

        private bool CanExit()
        {
            return !BatchInProgress;
        }

        private bool CanOnBreak()
        {
            return BatchInProgress;
        }

        private void OnBreakCommand()
        {
            try
            {
                var dialogVm1 = SimpleIoc.Default.GetInstanceWithoutCaching<OkCancelViewModel>();
                dialogVm1.HeaderText = "Break Mode";
                dialogVm1.MessageText = "Break mode will pause all operators and scales. Continue?";
                dialogVm1.OkText = "Ok";
                bool? success = _dialogService.ShowDialog<OkCancelView>(this, dialogVm1);
                if (success == true)
                {
                    _timer.Stop();
                    var dialogVm2 = SimpleIoc.Default.GetInstanceWithoutCaching<OkCancelViewModel>();
                    dialogVm2.HeaderText = null;
                    dialogVm2.MessageText = "On Break";
                    dialogVm2.HideCancel = true;
                    dialogVm2.OkText = "Resume";
                    dialogVm2.DialogResult = null;
                    bool? success1 = _dialogService.ShowDialog<OkCancelView>(this, dialogVm2);
                    if (success1 == true)
                    {
                        _timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "OnBreakCommand");
            }
        }

        private void OnOverrideCommand()
        {
            try
            {
                //                var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<OverrideBatchLimitsViewModel>();
                //                dialogViewModel.CurrentBatchView = SelectedBatchView;
                //                bool? success = _dialogService.ShowDialog<OverrideBatchLimitsView>(this, dialogViewModel);
                //                if (success == true)
                //                {
                //                    SelectedBatchView = dialogViewModel.CurrentBatchView;
                //                    SelectedBatchView.RaisePropertyChanged("Override");
                //                    SelectedBatchView.RaisePropertyChanged("BatchTare");
                //                    SelectedBatchView.RaisePropertyChanged("BatchNominal");
                //                    SendLimitCommands();
                //
                //                }
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "OnOverrideCommand");
            }
        }

        public async void OnExit()
        {
            if (ScaleList.Any(a => a.OperatorId != null && a.Active)) return;
            await _batchRepo.UpdateBatchLiveFlagAsync(SelectedBatchView.id, false);
            SimpleIoc.Default.GetInstance<MainWindowViewModel>().CurrentViewModel = SimpleIoc.Default.GetInstance<BatchListViewModel>();
        }

        private async void OnRetryConnectionCommand(Scale scale)
        {
            await Task.Run(() => OpenAndSetTcpConnection(scale, true));
        }

        private async void OnSelectUser(Scale scale)
        {
            try
            {
                if (scale == null) return;
                var selectedScale = scale;
                if (OperatorList == null) return;
                var dialogViewModel = SimpleIoc.Default.GetInstanceWithoutCaching<SelectOperatorViewModel>();
                var availableOperators = OperatorList.Where(oper => !ScaleList.Any(a => a.OperatorId == oper.id)).ToList();
                dialogViewModel.AllOperatorList = availableOperators;
                bool? success = _dialogService.ShowDialog<SelectOperatorView>(this, dialogViewModel);
                if (success == true)
                {
                    await AddOperatorToScale(selectedScale, dialogViewModel.SelectedOperator);
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "OnSelectUser");
            }
        }

        private async Task AddOperatorToScale(Scale selectedScale, BatchLoginOperator selectedOperator)
        {
            selectedScale.OperatorId = selectedOperator.id;
            selectedScale.OperatorName = selectedOperator.FullName;
            if (selectedOperator.TimeElapsedTicks == null)
            {
                await _batchRepo.AddBatchOperatorTimeAsync(SelectedBatchView.id, selectedOperator.id);
            }
            else
            {
                if (selectedOperator.TimeElapsedTicks == null)
                    selectedScale.TimeElapsedTicks = 0;
                else
                    selectedScale.TimeElapsedTicks = (long)selectedOperator.TimeElapsedTicks;
            }
            selectedScale.UserPackCount = PortionList.Count(a => a.OperatorId == selectedScale.OperatorId);
            await _scaleRepo.AddOperatorIdToScale(selectedOperator.id, selectedScale.id);
        }

        private async void OnOperatorLogOff(Scale scale)
        {
            try
            {
                await _batchRepo.UpdateTimeElapsedAsync(SelectedBatchView.id, (int)scale.OperatorId, scale.TimeElapsedTicks);
                await _scaleRepo.AddOperatorIdToScale(null, scale.id);

                scale.UserPackCount = 0;
                scale.OperatorId = null;
                scale.TimeElapsedTicks = 0;
                OperatorList = new List<BatchLoginOperator>(await _operatorRepo.GetOperatorsForBatch(SelectedBatchView.id));

                if (ScaleList.All(a => a.OperatorId == null))
                {
                    await _batchRepo.UpdateBatchTimeElapsedAsync(SelectedBatchView.id, SelectedBatchView.TimeElapsedTicks, 0);
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

                var changes = await _scaleRepo.RefreshOperatorsForDisplayDevice(ScaleList);
                if (!changes) return;

                foreach (var scale in ScaleList.Where(a => a.OperatorChanged))
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
                ErrorLogging.LogError(ex, "slaveScaleTimer_tick");
            }

        }

        private async void timer_tick(object sender, EventArgs e)
        {
            try
            {
                bool batchTimeupDated = false;
                foreach (var scale in ScaleList.Where(scale => scale.Active))
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
                    PacksPerMin = ((Decimal)(TotalPacksCount /
                                              (TimeSpan.FromTicks(SelectedBatchView.TimeElapsedTicks).TotalSeconds) *
                                              60));

                BatchInProgress = batchTimeupDated;
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "timer_tick");
            }
            finally
            {
                NavigateBackCommand.RaiseCanExecuteChanged();
            }

        }

        private async Task RefreshPortionList()
        {
            PortionList = new List<vwPortionView>(await _portionRepo.GetPortionsAsync(SelectedBatchView.id));
            TotalPacksCount = PortionList.Count;
            if (TotalPacksCount == 0) return;
            foreach (var scale in ScaleList)
            {
                if (!scale.Active) continue;
                if (scale.OperatorId == null) continue;
                scale.UserPackCount = PortionList.Count(a => a.OperatorId == scale.OperatorId);
            }

            TotalWeight = decimal.Round((decimal)PortionList.Sum(a => a.Weight) / 1000, 2);
            AverageWeight = Convert.ToInt32(PortionList.Average(a => a.Weight));
            decimal averageGiveaway = Convert.ToDecimal(PortionList.Average(a => a.Giveaway));
            if (SelectedBatchView.NominalWeight != null)
            {
                decimal percentageGiveaway = (averageGiveaway / (int)SelectedBatchView.NominalWeight) * 100;
                GiveawayDisplay = averageGiveaway.ToString("N0") + "g" + " (" + percentageGiveaway.ToString("N1") +
                                  "%)";
            }
        }

        private async void OnKeyPreviewDown(KeyEventArgs e)
        {
            if (!UtilitiesShared.IsMyMachine) return;
            int scaleNo = 0;
            if (e.Key == Key.NumPad1 || e.Key == Key.D1) scaleNo = 1;
            if (e.Key == Key.NumPad2 || e.Key == Key.D2) scaleNo = 2;
            if (e.Key == Key.NumPad3 || e.Key == Key.D3) scaleNo = 3;
            if (e.Key == Key.NumPad4 || e.Key == Key.D4) scaleNo = 4;
            if (e.Key == Key.NumPad5 || e.Key == Key.D5) scaleNo = 5;
            if (e.Key == Key.NumPad6 || e.Key == Key.D6) scaleNo = 6;
            if (e.Key == Key.NumPad7 || e.Key == Key.D6) scaleNo = 7;
            if (e.Key == Key.NumPad8 || e.Key == Key.D6) scaleNo = 8;
            if (e.Key == Key.NumPad9 || e.Key == Key.D6) scaleNo = 9;
            if (scaleNo == 0) return;

            var firstOrDefault = ScaleList.FirstOrDefault(a => a.ScaleNumber == scaleNo);
            if (firstOrDefault == null) return;
            var operatorId = firstOrDefault.OperatorId;
            if (operatorId == null) return;
            await _portionRepo.AddDummyPortionAsync(scaleNo, (int)operatorId, SelectedBatchView.id, BatchLowerLimit,
                BatchUpperLimit);
            await RefreshPortionList();
        }

        private async Task OpenAndSetTcpConnections()
        {
            //MY MACHINE DEMO MODE
            if (UtilitiesShared.IsMyMachine)
            {
                foreach (var scale in ScaleList)
                {
                    scale.IsConnected = true;
                }
                return;
            }

            var bag = new ConcurrentBag<object>();
            await ScaleList.ParallelForEachAsync(async item =>
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
                    byte[] lowerLimitBytesToSend = ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetLowerLimitCommand(BatchLowerLimit));
                    byte[] nominalBytesToSend = ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetNominalCommand(BatchLowerLimit));
                    byte[] upperLimitBytesToSend = ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetUpperLimitCommand(BatchUpperLimit));
                    byte[] tarebytesToSend = ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetTareCommand(SelectedBatchView.Tare));

                    var success = scale.TcpConnection.Connect(IPAddress.Parse(scale.ScaleIpAddress), 23);
                    if (!success)
                    {
                        scale.IsConnected = false;
                        return null;
                    }
                    scale.IsConnected = true;

                    if (fromBatchStart)
                    {
                        scale.TcpConnection.Send(ASCIIEncoding.ASCII.GetBytes("#7 \n"));
                        Thread.Sleep(1000);
                        scale.TcpConnection.Send(ASCIIEncoding.ASCII.GetBytes("q#\n"));
                        Thread.Sleep(1000);
                        scale.TcpConnection.Send(ASCIIEncoding.ASCII.GetBytes("q!\n"));
                        Thread.Sleep(1000);
                    }


                    scale.TcpConnection.Send(lowerLimitBytesToSend);
                    scale.TcpConnection.Send(nominalBytesToSend);
                    scale.TcpConnection.Send(upperLimitBytesToSend);
                    scale.TcpConnection.Send(tarebytesToSend);
                    scale.TcpConnection.OnDataReceived += NetConnectionOnOnDataReceived;

                    //                    scale.IsBusy = false;
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

            //            _connectionStatusTimer.Start();

        }

        private void SendLimitCommands()
        {
            OnPropertyChanged("BatchLowerLimit");
            OnPropertyChanged("BatchUpperLimit");
            foreach (var scale in ScaleList)
            {
                try
                {
                    if (scale.Active)
                    {
                        if (!scale.IsConnected) continue;
                        byte[] lowerLimitBytesToSend =ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetLowerLimitCommand(BatchLowerLimit));
                        byte[] nominalBytesToSend =ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetNominalCommand(BatchLowerLimit));
                        byte[] upperLimitBytesToSend =ASCIIEncoding.ASCII.GetBytes(ScaleCommandBuilder.GetUpperLimitCommand(BatchUpperLimit));

                        scale.TcpConnection.Send(lowerLimitBytesToSend);
                        scale.TcpConnection.Send(nominalBytesToSend);
                        scale.TcpConnection.Send(upperLimitBytesToSend);
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogging.LogError(ex, "SendOver100Commands - Scale:" + scale.ScaleNumber);
                }

            }
        }

        private void NetConnectionOnOnDataReceived(object sender, NetConnection connection, byte[] bytes)
        {
            try
            {
                var message = ASCIIEncoding.ASCII.GetString(bytes, 0, bytes.Length);
                //                Console.WriteLine(message);
                if (message.Length < 29) return;

                IPEndPoint remoteIpEndPoint = connection.RemoteEndPoint as IPEndPoint;
                var scale = ScaleList.FirstOrDefault(a => a.ScaleIpAddress == remoteIpEndPoint.Address.ToString());
                if (scale != null)
                {
                    if (scale.OperatorId == null) return;
                    var stringWeight = message.Substring(50, 5);
                    int weight = int.Parse(stringWeight, NumberStyles.AllowThousands);
                    AddPortion(scale, weight);
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "NetConnectionOnOnDataReceived");
            }
        }

        private async void AddPortion(Scale scale, int weight)
        {
            try
            {
                await _portionRepo.AddPortionAsync(scale.ScaleNumber, (int)scale.OperatorId, SelectedBatchView.id,
                    weight);
                await RefreshPortionList();
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "AddPortion");
            }

        }

        public async void Load(FrameworkElement element)
        {
            if (UtilitiesShared.InDesignMode) return;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _timer.Tick += timer_tick;
            _timer.Start();
            //            _connectionStatusTimer = new DispatcherTimer();
            //            _connectionStatusTimer.Interval = new TimeSpan(0, 0, 0, 10);
            //            _connectionStatusTimer.Tick += conectionStatusTimer_tick;

            CurrentDate = DateTime.Now;
            OperatorList = new List<BatchLoginOperator>(await _operatorRepo.GetOperatorsForBatch(SelectedBatchView.id));
            ScaleList = new List<Scale>(await _scaleRepo.GetScales());
            SelectedBatchView.TimeElapsedDisplay = TimeSpan.FromTicks(SelectedBatchView.TimeElapsedTicks).ToString();
            await _batchRepo.UpdateBatchLiveFlagAsync(SelectedBatchView.id, true);
            //_50Countdown = SelectedBatchView.FiftyCount;
            await RefreshPortionList();
            OpenAndSetTcpConnections();
        }

        private async void conectionStatusTimer_tick(object sender, EventArgs e)
        {
            foreach (var scale in ScaleList)
            {
                if (!scale.Active) continue;
                await CheckConnectionStatus(scale);
            }
        }

        private async Task CheckConnectionStatus(Scale scale)
        {
            try
            {
                var isPingable = await scale.TcpConnection.IsPingable(scale.ScaleIpAddress);
                if (scale.IsConnected == isPingable) return;
                if (!isPingable)
                {
                    scale.IsConnected = false;
                    if (scale.OperatorId != null)
                        OnOperatorLogOff(scale);
                }
                else
                {
                    if (!scale.IsConnected)
                        await OpenAndSetTcpConnection(scale, false);
                }
            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "ConnectionStatusTimer");
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
                foreach (var scale in ScaleList)
                {
                    if (!scale.Active) continue;
                    if (scale.TcpConnection == null) continue;
                    scale.TcpConnection.OnDataReceived -= NetConnectionOnOnDataReceived;
                    scale.TcpConnection.Disconnect();
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.LogError(ex, "BatchDetailsViewModel_Unload");
            }
            finally
            {
                ScaleList = null;
                PacksPerMin = 0;
                TotalPacksCount = 0;
                TotalWeight = 0;
                GiveawayDisplay = "";
                AverageWeight = 0;
                PortionList = null;
                OperatorList = null;
            }

        }
    }
}
