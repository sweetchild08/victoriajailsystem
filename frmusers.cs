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
    public partial class frmusers : Form
    {
        public frmusers()
        {
            InitializeComponent();
            loadtable();
        }

        void loadtable()
        {
            try
            {
                listView1.Items.Clear();
                helper.db.openconn();
                helper.db.comm.CommandText = "Select id,username,name from user where usertype='user'";
                helper.db.dr = helper.db.comm.ExecuteReader();
                string[] str = new string[3];
                while (helper.db.dr.Read())
                {
                    str[0] = helper.db.dr["id"].ToString();
                    str[1] = helper.db.dr["username"].ToString();
                    str[2] = helper.db.dr["name"].ToString();
                    ListViewItem item = new ListViewItem(str);

                    listView1.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addacc add = new addacc();
            add.ShowDialog();
            loadtable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select from table first.");
                return;
            }
            else
            {
                addacc edit = new addacc(false, int.Parse(listView1.SelectedItems[0].Text));
                edit.ShowDialog();
                loadtable();
            }
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
                DialogResult res = MessageBox.Show("Are you sure to delete this user", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res==DialogResult.Yes)
                {
                    try
                    {
                        helper.db.openconn();
                        helper.db.comm.CommandText = "delete from user where id=@id";
                        helper.db.comm.Parameters.AddWithValue("@id", int.Parse(listView1.SelectedItems[0].Text));
                        if (helper.db.comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Deleted Successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                loadtable();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            loadtable();
        }
    }
}
