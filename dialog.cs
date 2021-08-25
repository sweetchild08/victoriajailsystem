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
    public partial class dialog : Form
    {
        public dialog()
        {
            InitializeComponent();
            txtusername.Text = Properties.Settings.Default.DBhost;
            txtpassword.Text = Properties.Settings.Default.DBusername;
            textBox1.Text = Properties.Settings.Default.DBpassword;
            textBox2.Text = Properties.Settings.Default.DBname;
            textBox3.Text = Properties.Settings.Default.website;
        }

        private void phusername_Click(object sender, EventArgs e)
        {
            helper.focus(txtusername, sender as Label);
        }

        private void phpassword_Click(object sender, EventArgs e)
        {
            helper.focus(txtpassword, sender as Label);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            helper.focus(textBox1, sender as Label);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            helper.focus(textBox2, sender as Label);
        }

        private void txtusername_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, phusername);
        }

        private void txtpassword_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, phpassword);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, label2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, label3);
        }

        private void txtusername_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, phusername);
        }

        private void txtpassword_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, phpassword);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, label2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, label3);
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DBhost = txtusername.Text;
            Properties.Settings.Default.DBusername = txtpassword.Text;
            Properties.Settings.Default.DBpassword = textBox1.Text;
            Properties.Settings.Default.DBname = textBox2.Text;
            Properties.Settings.Default.website = textBox3.Text;
            helper.db = new db();
            if (!helper.db.testCon())
                MessageBox.Show("Cant connect to database, Please Reconfigure");
            else
            {
                MessageBox.Show("Connection ok");
                Properties.Settings.Default.Save();
                this.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            helper.focus(textBox3, sender as Label);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            helper.focus(sender as TextBox, label4);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            helper.leave(sender as TextBox, label4);
        }
    }
}
