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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            helper.dialog.ShowDialog();
        }

        private void txtusername_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, phusername);
        }

        private void txtpassword_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, phpassword);
        }

        private void txtusername_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, phusername);
        }

        private void txtpassword_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, phpassword);
        }

        private void phusername_Click(object sender, EventArgs e)
        {
            helper.focus(txtusername, phusername);
        }

        private void phpassword_Click(object sender, EventArgs e)
        {
            helper.focus(txtpassword, phpassword);
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.admin_username == txtusername.Text && Properties.Settings.Default.admin_password == txtpassword.Text)
            {
                helper.frmmain.insession = Properties.Settings.Default.admin_username;
                helper.frmmain.name = Properties.Settings.Default.admin_name;
                helper.frmmain.Show();
                this.Hide();
                return;
            }

            helper.db.openconn();
            helper.db.comm.CommandText = "select * from user where username=@username and password=MD5(@password);";
            helper.db.comm.Parameters.AddWithValue("@username", txtusername.Text);
            helper.db.comm.Parameters.AddWithValue("@password", txtpassword.Text);
            helper.db.dr = helper.db.comm.ExecuteReader();
            if (helper.db.dr.Read())
            {
                MessageBox.Show("login successful");
                helper.frmmain.insession = helper.db.dr["designation"].ToString();
                helper.frmmain.name = txtusername.Text;
                helper.frmmain.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }
    }
}
