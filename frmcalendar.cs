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
    public partial class frmcalendar : Form
    {
        public frmcalendar()
        {
            InitializeComponent();
            rfr();
        }
        DateTime[] dts = new DateTime[10];
        public static DateTime selected = DateTime.Now;

        DateTime prev = new DateTime(DateTime.Now.Year, 1, 1);
        void rfr()
        {
            try
            {
                listView1.Items.Clear();
                monthCalendar1.SelectionStart = prev;
                int year = monthCalendar1.SelectionStart.Date.Year;
                helper.db.openconn();
                helper.db.comm.CommandText = "select * from events where year(date)=@year or frequency='yearly' or frequency='monthly'";
                helper.db.comm.Parameters.AddWithValue("@year", year);
                helper.db.dr = helper.db.comm.ExecuteReader();
                string[] a = new string[4];
                monthCalendar1.RemoveAllBoldedDates();
                monthCalendar1.UpdateBoldedDates();
                int c = 0;
                while (helper.db.dr.Read())
                {
                    DateTime d = DateTime.Parse(helper.db.dr["date"].ToString());
                    string freq = helper.db.dr["frequency"].ToString();
                    a[0] = helper.db.dr["id"].ToString();
                    a[3] = freq;
                    if (freq == "yearly")
                    {
                        DateTime dd = new DateTime(year, d.Month, d.Day);
                        a[1] = dd.ToString("MMM dd, yyyy");
                        a[2] = helper.db.dr["title"].ToString();
                        listView1.Items.Add(new ListViewItem(a));
                        monthCalendar1.AddAnnuallyBoldedDate(dd);
                        Array.Resize(ref dts, c + 1);
                        dts[c] = dd;
                        c++;
                    }
                    else if (freq == "monthly")
                    {
                        DateTime dd = new DateTime();
                        for (int i = 1; i <= 12; i++)
                        {
                            dd = new DateTime(year, i, d.Day);
                            a[1] = dd.ToString("MMM dd, yyyy");
                            a[2] = helper.db.dr["title"].ToString();
                            listView1.Items.Add(new ListViewItem(a));
                            Array.Resize(ref dts, c + 1);
                            dts[c] = dd;
                            c++;
                        }
                        monthCalendar1.AddMonthlyBoldedDate(dd);
                    }
                    else if (freq == "once")
                    {
                        a[1] = d.ToString("MMM dd, yyyy");
                        a[2] = helper.db.dr["title"].ToString();
                        listView1.Items.Add(new ListViewItem(a));
                        monthCalendar1.AddBoldedDate(d);
                        Array.Resize(ref dts, c + 1);
                        dts[c] = d;
                        c++;
                    }
                }
                monthCalendar1.UpdateBoldedDates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            selected = e.Start;
            if (e.Start.Year != prev.Year)
            {
                rfr();
                prev = e.Start;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addevent add = new addevent();
            add.ShowDialog();
            rfr();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select from table first.");
                return;
            }
            else
            {
                addevent edit = new addevent(int.Parse(listView1.SelectedItems[0].Text));
                edit.ShowDialog();
                rfr();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select from table first.");
                return;
            }
            else
            {
                DialogResult d = MessageBox.Show("are you sure to delete this?", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (d == DialogResult.Yes)
                {
                    db db = new db();
                    if (db.delete("events", int.Parse(listView1.SelectedItems[0].Text)) > 0)
                    {
                        MessageBox.Show("Event Deleted Successfully");
                        rfr();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select from table first.");
                return;
            }
            else
            {
                viewevent v = new viewevent(int.Parse(listView1.SelectedItems[0].Text));
                v.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rfr();
        }
    }
}
