using System;
using System.Collections.Generic;
using Globeweigh.Model;
using Globeweigh.Model.Custom;
using Globeweigh.UI.Shared.Services;
using System.Linq;

namespace Globeweigh.Reports
{
    public partial class rptBatchReport2 : DevExpress.XtraReports.UI.XtraReport
    {
        public rptBatchReport2(int batchId, vwBatchView batchView, IBatchRepository batchRepo, IPortionRepository portionRepo)
        {
            InitializeComponent();
            Load(batchId, batchView, batchRepo, portionRepo);
        }

        private void Load(int batchId, vwBatchView batchView, IBatchRepository batchRepo, IPortionRepository portionRepo)
        {
            var operatorBatchList = batchRepo.GetOperatorBatchSummary(batchId);
            var portionList = portionRepo.GetPortions(batchId);


            objectDataSource1.DataSource = operatorBatchList;
            lblBatchId.Text = batchView.id.ToString();
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
                lblAvgGiveaway.Text = averageGiveaway.ToString("N0") + "g" + " (" + percentageGiveaway.ToString("N1") + "%)";
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