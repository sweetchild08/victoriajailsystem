using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace eserve2
{
    public partial class jailreport : Form
    {
        int cid;
        public jailreport(int cid=1)
        {
            this.cid = cid;
            InitializeComponent();
            jail jail = new jail();
            jail.SetParameterValue("cid", cid);
            crystalReportViewer1.ReportSource = jail;
            crystalReportViewer1.ReuseParameterValuesOnRefresh = true;
            crystalReportViewer1.RefreshReport();

        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {

        }
    }
}
