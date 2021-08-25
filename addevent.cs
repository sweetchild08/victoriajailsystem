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
    public partial class addevent : Form
    {
        private int id;
        public addevent(int id = 0)
        {
            InitializeComponent();
            this.id = id;
            if (id == 0)
            {
                label1.Text = "Add Event";
                xuiButton1.Text = "Add";
                dateTimePicker1.Value = frmcalendar.selected;
            }
            else
            {
                label1.Text = "Edit Event";
                xuiButton1.Text = "Update";
                try
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "select * from events where id=@id";
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    helper.db.dr = helper.db.comm.ExecuteReader(CommandBehavior.SingleRow);
                    if (helper.db.dr.Read())
                    {
                        dateTimePicker1.Value = DateTime.Parse(helper.db.dr["date"].ToString());
                        textBox1.Text = helper.db.dr["title"].ToString();
                        comboBox1.Text = helper.db.dr["frequency"].ToString();
                        textBox2.Text = helper.db.dr["notes"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "insert into events (date,title,frequency,notes) values (@date,@title,@frequency,@notes)";
                    helper.db.comm.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                    helper.db.comm.Parameters.AddWithValue("@title", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@frequency", comboBox1.SelectedItem.ToString());
                    helper.db.comm.Parameters.AddWithValue("@notes", textBox2.Text);

                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Event Added");
                        this.Close();
                    }
                }
                else
                {

                    helper.db.openconn();
                    helper.db.comm.CommandText = "update events set date=@date,title=@title,frequency=@frequency,notes=@notes where id=@id";
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    helper.db.comm.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                    helper.db.comm.Parameters.AddWithValue("@title", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@frequency", comboBox1.SelectedItem.ToString());
                    helper.db.comm.Parameters.AddWithValue("@notes", textBox2.Text);

                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Event Updated");
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
                if (dateTimePicker1.Value < DateTime.Now)
                {
                    MessageBox.Show("Date should be on future");
                    return;
                }
                if (id == 0)
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "insert into events (date,title,frequency,notes) values (@date,@title,@frequency,@notes)";
                    helper.db.comm.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                    helper.db.comm.Parameters.AddWithValue("@title", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@frequency", comboBox1.SelectedItem.ToString());
                    helper.db.comm.Parameters.AddWithValue("@notes", textBox2.Text);

                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Event Added");
                        this.Close();
                    }
                }
                else
                {

                    helper.db.openconn();
                    helper.db.comm.CommandText = "update events set date=@date,title=@title,frequency=@frequency,notes=@notes where id=@id";
                    helper.db.comm.Parameters.AddWithValue("@id", this.id);
                    helper.db.comm.Parameters.AddWithValue("@date", dateTimePicker1.Value);
                    helper.db.comm.Parameters.AddWithValue("@title", textBox1.Text);
                    helper.db.comm.Parameters.AddWithValue("@frequency", comboBox1.SelectedItem.ToString());
                    helper.db.comm.Parameters.AddWithValue("@notes", textBox2.Text);

                    if (helper.db.comm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Event Updated");
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
