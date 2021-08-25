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
    public partial class addacc : Form
    {
        private bool isadd;
        private int id;
        public addacc(bool isadd = true, int id = 0)
        {
            this.isadd = isadd;
            this.id = id;
            InitializeComponent();
            if (this.isadd)
            {
                xuiButton1.Text = "Add";
                label1.Text = "Add User";
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
            }
            else
            {
                xuiButton1.Text = "Update";
                label1.Text = "Update User";
                try
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "select * from user where id=@id";
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    helper.db.dr = helper.db.comm.ExecuteReader(CommandBehavior.SingleRow);
                    if (helper.db.dr.Read())
                    {
                        textBox1.Text = helper.db.dr["username"].ToString();
                        textBox3.Text = helper.db.dr["name"].ToString();
                        comboBox1.Text = helper.db.dr["usertype"].ToString();
                        comboBox2.Text = helper.db.dr["designation"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            TextBox[] tbs = { textBox3 };
            helper.setAlpha(tbs);
        }

        private void addacc_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isadd)
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "insert into user values (0,@username,MD5(@password),@name,@acctype,@designation,now());";
                    helper.db.comm.Parameters.AddWithValue("@username", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@password", textBox2.Text);
                    helper.db.comm.Parameters.AddWithValue("@name", textBox3.Text);
                    helper.db.comm.Parameters.AddWithValue("@acctype", comboBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@designation", comboBox2.Text);
                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("User Addded");
                        this.Close();
                    }
                }
                else
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "update  user set username=@username,password=MD5(@password),name=@name,usertype=@acctype,designation=@designation where id=@id;";
                    helper.db.comm.Parameters.AddWithValue("@username", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@password", textBox2.Text);
                    helper.db.comm.Parameters.AddWithValue("@name", textBox3.Text);
                    helper.db.comm.Parameters.AddWithValue("@acctype", comboBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@designation", comboBox2.Text);
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("User Updated");
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isadd)
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "insert into user values (0,@username,MD5(@password),@name,@acctype,@designation,now());";
                    helper.db.comm.Parameters.AddWithValue("@username", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@password", textBox2.Text);
                    helper.db.comm.Parameters.AddWithValue("@name", textBox3.Text);
                    helper.db.comm.Parameters.AddWithValue("@acctype", comboBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@designation", comboBox2.Text);
                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("User Addded");
                        this.Close();
                    }
                }
                else
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "update  user set username=@username,password=MD5(@password),name=@name,usertype=@acctype,designation=@designation where id=@id;";
                    helper.db.comm.Parameters.AddWithValue("@username", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@password", textBox2.Text);
                    helper.db.comm.Parameters.AddWithValue("@name", textBox3.Text);
                    helper.db.comm.Parameters.AddWithValue("@acctype", comboBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@designation", comboBox2.Text);
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("User Updated");
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
    }

        private void xuiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
