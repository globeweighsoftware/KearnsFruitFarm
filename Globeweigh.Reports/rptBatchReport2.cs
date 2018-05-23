using System;
using System.Collections.Generic;
using Globeweigh.Model;
using Globeweigh.Model.Custom;
using Globeweigh.UI.Shared.Services;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;

namespace Globeweigh.Reports
{
    public partial class rptBatchReport2 : DevExpress.XtraReports.UI.XtraReport
    {
        private readonly IBatchRepository _batchRepo = SimpleIoc.Default.GetInstance<IBatchRepository>();
        private readonly IPortionRepository _portionRepo = SimpleIoc.Default.GetInstance<IPortionRepository>();

        public rptBatchReport2(vwBatchView batchView)
        {
            InitializeComponent();
            Load(batchView);
        }

        private void Load (vwBatchView batchView)
        {
            var operatorBatchList = _batchRepo.GetOperatorBatchSummary(batchView.id);
            var portionList = _portionRepo.GetPortions(batchView.id);

            if (operatorBatchList == null)return;
            if(portionList.Count == 0)return;


                objectDataSource1.DataSource = operatorBatchList;
            lblBatchId.Text = batchView.id.ToString();
            lblBatchNo.Text = batchView.BatchNo;
            lblProduct.Text = batchView.ProductName;
            lblT1.Text = batchView.LowerLimit.ToString();
            lblUpper.Text = batchView.UpperLimit.ToString();
            lblNominal.Text = batchView.NominalWeight.ToString();
            lblStartDate.Text = batchView.DateCreated.ToString("dd/MM/yyyy HH:mm");
            if (batchView.DateClosed != null)
                lblEndDate.Text = ((DateTime)batchView.DateClosed).ToString("dd/MM/yyyy HH:mm");
            lblTotalRunningTime.Text =TimeSpan.FromTicks(batchView.TimeElapsedTicks).ToString();
            lblNoOfOperators.Text = operatorBatchList.Count.ToString();
            lblTotalWeight.Text = decimal.Round((decimal)portionList.Sum(a => a.Weight) / 1000, 2) + "Kg";
            lblAverageWeight.Text = Convert.ToInt32(portionList.Average(a => a.Weight)).ToString() + "g";
            lblTotalPacks.Text = batchView.PortionCount.ToString();
            decimal averageGiveaway = Convert.ToDecimal(portionList.Average(a => a.Giveaway));
            if (batchView.NominalWeight != null)
            {
                decimal percentageGiveaway = (averageGiveaway / (int)batchView.NominalWeight) * 100;
                lblAvgGiveaway.Text = averageGiveaway.ToString("N1") + "g" + " (" + percentageGiveaway.ToString("N1") + "%)";
            }

            if (TimeSpan.FromTicks(batchView.TimeElapsedTicks).TotalMinutes == 0 || batchView.PortionCount == 0)
                lblTotalPacksPerMin.Text = "0";
            else
            {
                lblTotalPacksPerMin.Text = ((int)batchView.PortionCount / TimeSpan.FromTicks(batchView.TimeElapsedTicks).TotalMinutes).ToString("N1");
            }
        }
    }
}