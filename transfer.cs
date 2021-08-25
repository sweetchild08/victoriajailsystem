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
    public partial class transfer : Form
    {
        private int id;
        public transfer(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xuiButton2_Click(object sender, EventArgs e)
        {

            string[] list = { "Warrant of Arrest", "Court Origin", "Report", "quarantine", "Medical", "X-Ray", "Mugshot" };
            checklist checklist = new checklist(list, "Transfer");
            checklist.ShowDialog();
            if (checklist.d == DialogResult.OK)
            {
                try
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "update `case` set suspect_status='Transfered',transfer_to=@transferto where id=@id";
                    helper.db.comm.Parameters.AddWithValue("@transferto", comboBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Transfered to " + comboBox1.Text);
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
