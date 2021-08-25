using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace eserve2
{
    public partial class frmdashboard : Form
    {

        Series series1 = new Series();
        public frmdashboard()
        {
            InitializeComponent();
            series1.ChartArea = "ChartArea1";
            series1.ChartType = SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
        }

        void loadvals()
        {
            chart2.Series["gender"].Points.Clear();
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT suspect_status as status,COUNT(*) as total FROM `case` GROUP BY suspect_status";
            helper.db.dr = helper.db.comm.ExecuteReader();
            while (helper.db.dr.Read())
            {
                switch (helper.db.dr["status"].ToString())
                {
                    case "Detained":
                        tdetainees.Text = helper.db.dr["total"].ToString();
                        break;
                    case "Transfered":
                        ttransfered.Text = helper.db.dr["total"].ToString();
                        break;
                    case "Released":
                        treleased.Text = helper.db.dr["total"].ToString();
                        break;
                }
            }

            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT gender,COUNT(*) as total FROM `detainee_personal_information` GROUP BY gender order by gender desc";
            helper.db.dr = helper.db.comm.ExecuteReader();
            while (helper.db.dr.Read())
            {
                string label = helper.db.dr["gender"].ToString();
                int val = int.Parse(helper.db.dr["total"].ToString());
                switch (label)
                {
                    case "Male":
                        chart2.Series["gender"].Points.AddXY(label, val);
                        chart2.Series["gender"].Points[0].CustomProperties = "LabelsHorizontalLineSize=2, PieLabelStyle=Outside";
                        break;
                    case "Female":
                        chart2.Series["gender"].Points.AddXY(label, val);
                        chart2.Series["gender"].Points[1].Color = Color.OrangeRed;
                        chart2.Series["gender"].Points[1].CustomProperties = "LabelsHorizontalLineSize=2, PieLabelStyle=Outside";
                        break;
                }
            }

            chart1.Series["Age"].Points.Clear();
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT timestampdiff(YEAR,date_of_birth,now()) as age,COUNT(*) as total FROM `detainee_personal_information` GROUP BY timestampdiff(YEAR,date_of_birth,now())";
            helper.db.dr = helper.db.comm.ExecuteReader();
            int[] ages = { 0, 30, 45, 60 };
            int[] cnt = new int[ages.Length];
            while (helper.db.dr.Read())
            {
                for (int i = 0; i < ages.Length; i++) {
                    int age = int.Parse(helper.db.dr["age"].ToString());
                    int total = int.Parse(helper.db.dr["total"].ToString());
                    if (i == ages.Length - 1)
                    {
                        cnt[i + 1] += total;
                        break;
                    }
                    else if (age >= ages[i] && age <= ages[i + 1])
                    {
                        cnt[i] += total;
                        break;
                    }
                }
            }
            for (int i = 0; i < ages.Length; i++)
            {
                string label = "";
                if (i == ages.Length - 1)
                {
                    label = "above " + ages[i].ToString();
                }
                else
                {
                    label = (ages[i] + 1).ToString() + "-" + ages[i + 1].ToString();
                }
                chart1.Series["Age"].Points.AddXY(label, cnt[i]);
            }
            DateTime now = DateTime.Now;
            label2.Text = "Incoming Events\r\nfor this month \r\n(" + now.ToString("MMMM") + ")";
            listBox1.Items.Clear();
            helper.db.openconn();
            helper.db.comm.CommandText = "select day(date) as day,title from events where (month(date)=@month and year(date)=@year) or (month(date)=@month and frequency='yearly')";
            helper.db.comm.Parameters.AddWithValue("@month", now.Month);
            helper.db.comm.Parameters.AddWithValue("@year", now.Year);
            helper.db.dr = helper.db.comm.ExecuteReader();
            while (helper.db.dr.Read())
            {
                listBox1.Items.Add(helper.db.dr["day"].ToString() + " - " + helper.db.dr["title"].ToString());
            }

        }

        private void frmdashboard_Enter(object sender, EventArgs e)
        {
            loadvals();
        }
    }
}
