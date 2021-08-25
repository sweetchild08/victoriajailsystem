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
    public partial class frnmblotter : Form
    {
        public frnmblotter()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addblotter ad = new addblotter();
            ad.ShowDialog();
        }
        int[] ids = new int[1];
        int[] blotterids = new int[1];
        void loadcombo()
        {
            comboBox2.Items.Clear();
            helper.db.openconn();
            helper.db.comm.CommandText = "select * from residents";
            helper.db.dr = helper.db.comm.ExecuteReader();
            int i = 0;
            while (helper.db.dr.Read())
            {
                Array.Resize(ref ids, i + 1);
                ids[i] = int.Parse(helper.db.dr["id"].ToString());
                comboBox2.Items.Add(helper.db.dr["name"].ToString());
                i++;
            }
        }
        void loaddata(int id=0)
        {
            listView1.Items.Clear();
            helper.db.openconn();
            string where = "";
            if (id != 0)
            {
                where += " where sus.id=@id or com.id=@id";
                helper.db.comm.Parameters.AddWithValue("@id",id);
            }
            helper.db.comm.CommandText = "select blotter.*,blotter.id as caseno,sus.name as suspect_name,sus.address as suspect_address,com.name as complainant_name,com.address as complainant_address,sus.birthdate as suspect_bdate from blotter inner join residents as sus on blotter.suspect=sus.id inner join residents as com on blotter.complainant=com.id"+ where ;
            helper.db.dr = helper.db.comm.ExecuteReader();
            int i = 0;
            while(helper.db.dr.Read())
            {
                Array.Resize(ref blotterids, i + 1);
                blotterids[i] = int.Parse(helper.db.dr["caseno"].ToString());
                string[] a = {
                    helper.db.dr["caseno"].ToString(),
                    helper.db.dr["accusation"].ToString(),
                    helper.db.dr["suspect_name"].ToString(),
                    helper.db.dr["suspect_address"].ToString(),
                    helper.calcAge(DateTime.Parse(helper.db.dr["suspect_bdate"].ToString())).ToString(),
                    helper.db.dr["datetime"].ToString(),
                    helper.db.dr["place"].ToString(),
                    helper.db.dr["responding_officer"].ToString(),
                    helper.db.dr["complainant_name"].ToString(),
                    helper.db.dr["complainant_address"].ToString(),
                    helper.db.dr["status"].ToString(),
                };
                listView1.Items.Add(new ListViewItem(a));
                i++;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frnmblotter_Activated(object sender, EventArgs e)
        {
            loaddata();
            loadcombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loaddata();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loaddata(ids[comboBox2.SelectedIndex]);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select record First");
                return;
            }
            int caseid = blotterids[listView1.SelectedIndices[0]];
            DialogResult d = MessageBox.Show("Confirmation", "Mark record as settled?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (d == DialogResult.Yes)
            {
                helper.db.openconn();
                helper.db.comm.CommandText = "update blotter set status='settled' where id=@id";
                helper.db.comm.Parameters.AddWithValue("@id", caseid);
                if (helper.db.comm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("marked as settled");
                    loaddata();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select record First");
                return;
            }
            int caseid = blotterids[listView1.SelectedIndices[0]];
            addblotter ad = new addblotter(caseid);
            ad.ShowDialog();
            loaddata();
        }
    }
}
