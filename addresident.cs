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
    public partial class addresident : Form
    {
        public addresident()
        {
            InitializeComponent();
            TextBox[] tbs = { textBox2 };
            helper.setAlpha(tbs);
        }

        private void xuiButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xuiButton3_Click(object sender, EventArgs e)
        {
            helper.db.openconn();
            helper.db.comm.CommandText = "INSERT into residents (name,address,birthdate) values (@name,@add,@bdate)";
            helper.db.comm.Parameters.AddWithValue("@name", textBox2.Text);
            helper.db.comm.Parameters.AddWithValue("@add", textBox1.Text);
            helper.db.comm.Parameters.AddWithValue("@bdate", dateTimePicker1.Value);
            if(helper.db.comm.ExecuteNonQuery()>0)
            {
                MessageBox.Show("Resident added");
                this.Close();
            }
        }
    }
}
