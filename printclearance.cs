using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eserve2
{
    public partial class printclearance : Form
    {
        public printclearance(long id)
        {
            InitializeComponent();

            clearance clearance = new clearance();
            clearance.SetParameterValue("id", id);
            crystalReportViewer1.ReportSource = clearance;
            crystalReportViewer1.ReuseParameterValuesOnRefresh = true;
            crystalReportViewer1.RefreshReport();
        }
    }
}
