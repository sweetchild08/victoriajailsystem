using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace eserve2
{
    public partial class frmmain : Form
    {
        public string insession="admin";
        public string name = "Administrator";

        Button[] buttons = new Button[1];
        Form[] forms = { new frmdashboard(),new frmjail(), new frmclearance(), new frmcalendar(),new frmusers(),new frmlostnfound(),new frnmblotter() };

        void accesscontroll()
        {
            string ses = insession.ToLower();
            if (ses == "admin")
            {
                button5.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button6.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
            }
            else if (ses == "desk officer")
            {
                button5.Visible = false;
                button1.Visible = false;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
            }
            else if (ses == "jailer")
            {
                button5.Visible = true;
                button1.Visible = true;
                button2.Visible = false;
                button3.Visible = true;
                button4.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
            }
            else if (ses == "investigator")
            {
                button5.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = true;
                button4.Visible = false;
                button6.Visible = true;
                button7.Visible = true;
            }
        }
        public frmmain()
        {
            InitializeComponent();
            int ctr = 0;
            foreach(Control button in panel3.Controls)
            {
                if(button is Button)
                {
                    Array.Resize(ref buttons, ctr + 1);
                    buttons[ctr] = button as Button;
                    ctr++;
                }
            }
        }
        void resetbuttons()
        {
            foreach(Button b in buttons) 
            {
                b.BackColor= Color.FromArgb(21, 101, 192);
            }
        }


        void changeform(Form f, Button b)
        {
            b.BackColor = Color.FromArgb(13, 71, 161);
            //pictureBox1.Image = imageList1.Images[b.ImageIndex];
            //label1.Text = b.Text;
            resetbuttons();
            b.BackColor = Color.FromArgb(16, 39, 112);
            bool isactive = false;
            foreach (Form d in this.MdiChildren)
            {
                if (d.Name == f.Name)
                {
                    isactive = true;
                    d.Show();
                }
                else
                    d.Hide();
            }
            if (!isactive)
            {
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.Show();
            }
        }

        private void frmmain_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            changeform(forms[1], sender as Button);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeform(forms[2], sender as Button);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changeform(forms[3], sender as Button);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changeform(forms[4], sender as Button);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            changeform(forms[0], sender as Button);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void frmmain_Activated(object sender, EventArgs e)
        {
            label2.Text = name;
            accesscontroll();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            changeform(forms[5], sender as Button);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            changeform(forms[6], sender as Button);
        }
    }
}
