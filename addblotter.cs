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
    public partial class addblotter : Form
    {
        int bid;
        public addblotter(int bid = 0)
        {
            InitializeComponent();
            this.bid = bid;
            if (bid != 0)
            {
                xuiButton3.ButtonText = "Update";
                helper.db.openconn();
                helper.db.comm.CommandText = "select * from blotter where id=@id";
                helper.db.comm.Parameters.AddWithValue("@id", bid);
                helper.db.dr = helper.db.comm.ExecuteReader();
                if (helper.db.dr.Read())
                {
                    //comboBox2.Text = ;Array.IndexOf()
                    comboBox1.Text = helper.db.dr["suspect"].ToString();
                    textBox2.Text = helper.db.dr["accusation"].ToString();
                    dateTimePicker1.Value = DateTime.Parse(helper.db.dr["datetime"].ToString());
                    textBox3.Text = helper.db.dr["place"].ToString();
                    textBox4.Text = helper.db.dr["responding_officer"].ToString();
                    textBox5.Text = helper.db.dr["notes"].ToString();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        int[] ids = new int[1];
        void loaddata()
        {
            int i = 0;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            helper.db.openconn();
            helper.db.comm.CommandText = "select * from residents";
            helper.db.dr = helper.db.comm.ExecuteReader();
            while (helper.db.dr.Read())
            {
                Array.Resize(ref ids, i + 1);
                ids[i] = int.Parse(helper.db.dr["id"].ToString());
                comboBox1.Items.Add(helper.db.dr["name"]);
                comboBox2.Items.Add(helper.db.dr["name"]);
                i++;
            }
        }

        private void xuiButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addblotter_Activated(object sender, EventArgs e)
        {
            loaddata();
        }

        private void xuiButton2_Click(object sender, EventArgs e)
        {
            addresident ad = new addresident();
            ad.ShowDialog();
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            addresident ad = new addresident();
            ad.ShowDialog();
        }

        private void xuiButton3_Click(object sender, EventArgs e)
        {
            helper.db.openconn();
            if (bid == 0)
                helper.db.comm.CommandText = "insert into blotter (complainant,suspect,accusation,datetime,place,responding_officer,notes) values (@complainant,@suspect,@accusation,@datetime,@place,@responding_officer,@notes)";
            else
            {
                helper.db.comm.CommandText = "update blotter set complainant=@complainant,suspect=@suspect,accusation=@aaccusation,datetime=@datetime,place=@place,responding_officer=@responding_officer,notes=@notes where id=@id";
                helper.db.comm.Parameters.AddWithValue("@id", bid);
            }
            helper.db.comm.Parameters.AddWithValue("@complainant", ids[comboBox2.SelectedIndex]);
            helper.db.comm.Parameters.AddWithValue("@suspect", ids[comboBox1.SelectedIndex]);
            helper.db.comm.Parameters.AddWithValue("@accusation", textBox2.Text);
            helper.db.comm.Parameters.AddWithValue("@datetime", dateTimePicker1.Value);
            helper.db.comm.Parameters.AddWithValue("@place", textBox3.Text);
            helper.db.comm.Parameters.AddWithValue("@responding_officer", textBox4.Text);
            helper.db.comm.Parameters.AddWithValue("@notes", textBox5.Text);
            if (helper.db.comm.ExecuteNonQuery() > 0)
            {
                if (bid == 0)
                    MessageBox.Show("Blottered Successfully");
                else
                    MessageBox.Show("Blotter Updated Successfully");
                this.Close();
            }
        }
    }
}
