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
    public partial class frmjail : Form
    {
        public frmjail()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        int[] ids = new int[1];
        int cid = 0;
        void loadSelector()
        {
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT dpi.*,ms.case_id FROM detainee_personal_information as dpi " +
                " INNER JOIN `case` ON dpi.id =`case`.`detainee`" +
                " INNER JOIN mugshots as ms ON `case`.`id`= ms.case_id" +
                " INNER JOIN arrest as arr ON `case`.`id`= arr.case_id";
            if (comboBox3.SelectedIndex >0)
            {
                helper.db.comm.CommandText += " where `case`.suspect_status=@ss";
                helper.db.comm.Parameters.AddWithValue("@ss", comboBox3.Text);

            }
            helper.db.dr = helper.db.comm.ExecuteReader();
            listBox1.Items.Clear();
            int counter = 1;
            while (helper.db.dr.Read())
            {
                Array.Resize(ref ids, counter);
                ids[counter - 1] = int.Parse(helper.db.dr["case_id"].ToString());
                string name = helper.db.dr["first_name"].ToString() +" "+ helper.db.dr["middle_name"].ToString() +" "+ helper.db.dr["last_name"].ToString();
                listBox1.Items.Add(name);
                counter++;
            }
            //comboBox2.SelectedIndex = 0;
        }
        void loaddata(int id)
        {
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT *,dpi.id as detaineeid FROM detainee_personal_information as dpi " +
                " INNER JOIN `case` ON dpi.id =`case`.`detainee`" +
                " INNER JOIN mugshots as ms ON `case`.`id`= ms.case_id" +
                " INNER JOIN arrest as arr ON `case`.`id`= arr.case_id" +
                " where ms.case_id=@cid";
            helper.db.comm.Parameters.AddWithValue("@cid", id);
            if (comboBox3.SelectedIndex > 0)
            {
                helper.db.comm.CommandText += " and `case`.suspect_status=@ss";
                helper.db.comm.Parameters.AddWithValue("@ss", comboBox3.Text);
            }
            helper.db.dr = helper.db.comm.ExecuteReader();
            if (helper.db.dr.Read())
            {
                string name = helper.db.dr["first_name"].ToString() + " " + helper.db.dr["middle_name"].ToString() + " " + helper.db.dr["last_name"].ToString();
                lblname.Text = name;
                pictureBox1.Image = helper.bytearrayToImage((byte[])helper.db.dr["front_view"]);
                lblalias.Text = helper.db.dr["alias"].ToString();
                lblheight.Text = helper.db.dr["height"].ToString() + " m";
                lblweight.Text = helper.db.dr["weight"].ToString() +" kg";
                lbldob.Text = helper.db.dr["date_of_birth"].ToString();
                lblgender.Text = helper.db.dr["gender"].ToString();
                DateTime bdate = DateTime.Parse(helper.db.dr["date_of_birth"].ToString());
                lblpob.Text = helper.db.dr["place_of_birth"].ToString();
                lbleth.Text = helper.db.dr["ethnic_group"].ToString();
                lblhc.Text = helper.db.dr["hair_color"].ToString();
                lblec.Text = helper.db.dr["eye_color"].ToString();
                lblbuild.Text = helper.db.dr["build"].ToString();
                lblcomp.Text = helper.db.dr["complexion"].ToString();
                lblciti.Text = helper.db.dr["citizenship"].ToString();
                lblim.Text = helper.db.dr["identifying_marks"].ToString();
                lblml.Text = helper.db.dr["marks_location"].ToString();
                lbllang.Text = helper.db.dr["language"].ToString();
                lblcs.Text = helper.db.dr["civil_status"].ToString();
                lblnos.Text = helper.db.dr["name_of_spouse"].ToString();
                lblage.Text = helper.calcAge(bdate).ToString()+ " years old";
                int detid= int.Parse(helper.db.dr["detaineeid"].ToString());

                db db = new db();
                db.openconn();
                db.comm.CommandText = "select count(*) as total from `case` where detainee=@detid";
                db.comm.Parameters.AddWithValue("@detid", detid);
                db.dr = db.comm.ExecuteReader();
                if (db.dr.Read())
                {
                    timesdetained.Text = db.dr["total"].ToString();
                }

                string status = helper.db.dr["suspect_status"].ToString();
                cid = int.Parse(helper.db.dr["case_id"].ToString());
                lbliscc.Text = helper.db.dr["IS/CC_no"].ToString();
                lblpjudge.Text = helper.db.dr["prosecutor/judge"].ToString();
                lblcstatus.Text = helper.db.dr["case_status"].ToString();
                lblsstatus.Text = helper.db.dr["suspect_status"].ToString();
                lbldatedetained.Text = helper.db.dr["date_detained"].ToString();
                lblmodus.Text = helper.db.dr["modus_operanda"].ToString();
                lblnoi.Text = helper.db.dr["name_of_investigator"].ToString();

                lblaclass.Text = helper.db.dr["arrest_classification"].ToString();
                lbldatearrested.Text = helper.db.dr["date_arrested"].ToString();
                lbltimearrested.Text = helper.db.dr["time_arrested"].ToString();
                lblplaceofarrest.Text = helper.db.dr["place_of_arrest"].ToString();
                lblarrestingunit.Text = helper.db.dr["arresting_unit"].ToString();
                lblgroupaffiliation.Text = helper.db.dr["group_affiliation"].ToString();

                if (status == "Detained")
                {
                    button7.Enabled = true;
                    button8.Enabled = true;
                }
                else
                {
                    button7.Enabled = false;
                    button8.Enabled = false;
                }
            }
        }
        void loadMugshot()
        {
            if(comboBox1.SelectedIndex==-1)
            {

                return;
            }
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT * FROM mugshots where case_id=@cid";
            helper.db.comm.Parameters.AddWithValue("@cid", ids[listBox1.SelectedIndex]);
            helper.db.dr = helper.db.comm.ExecuteReader();
            if (helper.db.dr.Read())
            {
                pictureBox1.Image = helper.bytearrayToImage((byte[])helper.db.dr[db.views[comboBox1.SelectedIndex]]);
            }
        }
        private void frmjail_Load(object sender, EventArgs e)
        {
            loadSelector();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loaddata(ids[listBox1.SelectedIndex]);
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMugshot();
            button2.Enabled = comboBox1.SelectedIndex != 0;
            button3.Enabled = comboBox1.SelectedIndex != comboBox1.Items.Count-1;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex--;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("select Detainee first");
                return;
            }
            jailreport jr = new jailreport(ids[listBox1.SelectedIndex]);
            jr.Show();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("select Detainee first");
                return;
            }
            adddetainee ad = new adddetainee(ids[listBox1.SelectedIndex]);
            ad.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            adddetainee det = new adddetainee();
            det.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            loadSelector();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select detainee first.");
                return;
            }
            else
            {
                transfer transfer = new transfer(ids[listBox1.SelectedIndex]);
                transfer.ShowDialog();
                loadSelector();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Select detainee first.");
                return;
            }
            else
            {
                DialogResult d = MessageBox.Show("Confirm release of detainee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.Yes)
                {
                    helper.db.openconn();
                    helper.db.comm.CommandText = "update `case` set suspect_status=@status where id=@cid";
                    helper.db.comm.Parameters.AddWithValue("@cid", ids[listBox1.SelectedIndex]);
                    helper.db.comm.Parameters.AddWithValue("@status", "Released");
                    if(helper.db.comm.ExecuteNonQuery()>0)
                    {
                        MessageBox.Show("Detainee was released");
                    }
                    loadSelector();
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSelector();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                loaddata(ids[listBox1.SelectedIndex]);
                comboBox1.SelectedIndex = 0;
            }
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }

















}
