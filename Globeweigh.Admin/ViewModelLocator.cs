﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Globeweigh.UI.Shared.Helpers;
using Globeweigh.UI.Shared.Services;
using Microsoft.Practices.ServiceLocation;

namespace Globeweigh.Admin
{
    public class ViewModelLocator
    {
        /// <summary>
        /// This class contains static references to all the view models in the
        /// application and provides an entry point for the bindings.
        /// </summary>
        private static bool _isInitialized;

        public ViewModelLocator()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            if (UtilitiesShared.InDesignMode) return;

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<EntitiesViewModel>();
            SimpleIoc.Default.Register<PrintExportOptionsViewModel>();
            SimpleIoc.Default.Register<DownloadNewUpdateViewModel>();
            SimpleIoc.Default.Register<ProductListViewModel>();
            SimpleIoc.Default.Register<AddEditProductViewModel>();
            SimpleIoc.Default.Register<OperatorListViewModel>();
            SimpleIoc.Default.Register<AddEditOperatorViewModel>();
            SimpleIoc.Default.Register<ScaleListViewModel>();
            SimpleIoc.Default.Register<AddEditScaleViewModel>();
            SimpleIoc.Default.Register<BatchListViewModel>();
            SimpleIoc.Default.Register<PortionControlMasterViewModel>();
            SimpleIoc.Default.Register<OperatorPerformanceViewModel>();
        }

        public MainWindowViewModel MainWindow => SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        public EntitiesViewModel EntitiesView => SimpleIoc.Default.GetInstance<EntitiesViewModel>();
        public ProductListViewModel ProductListView => SimpleIoc.Default.GetInstance<ProductListViewModel>();
        public OperatorListViewModel OperatorListView => SimpleIoc.Default.GetInstance<OperatorListViewModel>();
        public ScaleListViewModel ScaleListView => SimpleIoc.Default.GetInstance<ScaleListViewModel>();
        public BatchListViewModel BatchListView => SimpleIoc.Default.GetInstance<BatchListViewModel>();
        public OperatorPerformanceViewModel OperatorPerformanceView => SimpleIoc.Default.GetInstance<OperatorPerformanceViewModel>();
        public PortionControlMasterViewModel PortionControlMasterView => SimpleIoc.Default.GetInstance<PortionControlMasterViewModel>();
    }
}