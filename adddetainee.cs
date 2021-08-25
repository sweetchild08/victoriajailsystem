using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace eserve2
{
    public partial class adddetainee : Form
    {
        int currenttab = 0;
        bool[] tabstatus = new bool[4];
        int cid;
        int did;

        public adddetainee(int cid =0)
        {
            this.cid = cid;
            InitializeComponent();
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in filterInfoCollection)
                mediaDeviceSelector.Items.Add(Device.Name);
            mediaDeviceSelector.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice();

            //
            frontview.Click += new EventHandler(chooseframe);
            leftview.Click += new EventHandler(chooseframe);
            rightview.Click += new EventHandler(chooseframe);
            wholebody.Click += new EventHandler(chooseframe);
            if(cid==0)
            {
                this.Text = "Add Detainee";
            }
            else
            {
                this.Text = "Edit Detainee";
                loaddata(cid);
            }
        }
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        PictureBox curr = new PictureBox();
        void loaddata(int id)
        {
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT *,dpi.id as did FROM detainee_personal_information as dpi " +
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
                textBox1.Text= helper.db.dr["first_name"].ToString();
                textBox2.Text = helper.db.dr["middle_name"].ToString();
                textBox4.Text = helper.db.dr["last_name"].ToString();
                textBox3.Text = helper.db.dr["qualifier"].ToString();
                textBox6.Text = helper.db.dr["alias"].ToString();
                comboBox1.Text = helper.db.dr["gender"].ToString();
                dateTimePicker1.Value = DateTime.Parse(helper.db.dr["date_of_birth"].ToString()); 
                textBox5.Text = helper.db.dr["place_of_birth"].ToString();
                numericUpDown1.Value = int.Parse(helper.calcAge(dateTimePicker1.Value).ToString());
                textBox9.Text = helper.db.dr["ethnic_group"].ToString();
                comboBox2.Text = helper.db.dr["civil_status"].ToString();
                textBox20.Text = helper.db.dr["hair_color"].ToString();
                textBox19.Text = helper.db.dr["eye_color"].ToString();
                textBox18.Text = helper.db.dr["height"].ToString();
                textBox17.Text = helper.db.dr["weight"].ToString();
                textBox16.Text = helper.db.dr["build"].ToString();
                textBox15.Text = helper.db.dr["complexion"].ToString();
                textBox14.Text = helper.db.dr["citizenship"].ToString();
                textBox13.Text = helper.db.dr["identifying_marks"].ToString();
                textBox8.Text = helper.db.dr["marks_location"].ToString();
                textBox7.Text = helper.db.dr["language"].ToString();
                textBox22.Text = helper.db.dr["name_of_spouse"].ToString();
                textBox23.Text = helper.db.dr["last_known_address"].ToString();

                
                did = int.Parse(helper.db.dr["did"].ToString());
                textBox36.Text = helper.db.dr["case_status"].ToString();
                textBox35.Text = helper.db.dr["IS/CC_no"].ToString();
                textBox28.Text = helper.db.dr["prosecutor/judge"].ToString();
                comboBox3.Text = helper.db.dr["suspect_status"].ToString();
                dateTimePicker3.Value = DateTime.Parse(helper.db.dr["date_detained"].ToString());
                textBox25.Text = helper.db.dr["modus_operanda"].ToString();
                textBox37.Text = helper.db.dr["name_of_investigator"].ToString();

                textBox34.Text = helper.db.dr["arrest_classification"].ToString();
                dateTimePicker2.Value = DateTime.Parse(helper.db.dr["date_arrested"].ToString());
                textBox32.Text = helper.db.dr["time_arrested"].ToString();
                textBox31.Text = helper.db.dr["place_of_arrest"].ToString();
                textBox30.Text = helper.db.dr["arresting_unit"].ToString();
                textBox29.Text = helper.db.dr["group_affiliation"].ToString();
                textBox24.Text = helper.db.dr["synopsis"].ToString();


                wholebody.Image = helper.bytearrayToImage((byte[])helper.db.dr["whole_body"]);
                leftview.Image = helper.bytearrayToImage((byte[])helper.db.dr["left_view"]);
                rightview.Image = helper.bytearrayToImage((byte[])helper.db.dr["right_view"]);
                frontview.Image = helper.bytearrayToImage((byte[])helper.db.dr["front_view"]);
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }
        //c# webcam capture picture
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            curr.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            closecapture();
        }

        void chooseframe(object sender, EventArgs e)
        {
            closecapture();
            curr = sender as PictureBox;
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[mediaDeviceSelector.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
            videoCaptureDevice.Start();

        }
        void closecapture()
        {
            if (videoCaptureDevice.IsRunning == true)
                videoCaptureDevice.Stop();
        }
        bool checkInputsRequired(string[] inputs)
        {
            foreach(string input in inputs)
            {
                if (string.IsNullOrEmpty(input)) return false;
            }
            return true;
        }
        bool checkInputsRequired(Control.ControlCollection inputs)
        {
            foreach (Control input in inputs)
            {
                if (string.IsNullOrEmpty(input.Text)) return false;
            }
            return true;
        }
        bool checkInputsRequired(Image[] inputs)
        {
            foreach (Image input in inputs)
            {
                if (input==null) return false;
            }
            return true;
        }
        bool validate()
        {
            switch(currenttab)
            {   
                case 0:
                    Image[] images={ frontview.Image, leftview.Image,rightview.Image,wholebody.Image };
                   // return true;
                    return checkInputsRequired(images);
                case 1:
                    return checkInputsRequired(panel7.Controls)&& checkInputsRequired(panel9.Controls);
                case 2:
                    return checkInputsRequired(panel11.Controls) && !string.IsNullOrEmpty( textBox24.Text);
                case 3:
                    return checkInputsRequired(panel13.Controls) ;
                default:
                    return true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = sender as TabControl;
            xuiButton1.Enabled = tab.SelectedIndex != 0;
            //buttonnext.Enabled = tab.SelectedIndex != tab.TabCount-1;
            if(tab.SelectedIndex == tab.TabCount - 1)
                xuiButton2.Text = "Finish";
            else
                xuiButton2.Text = "Next";
        }
        bool insertArrest(long case_id)
        {

            helper.db.openconn();
            if (cid == 0)
            {
                helper.db.comm.CommandText = "insert into arrest " +
                    "(case_id, arrest_classification, date_arrested, time_arrested, place_of_arrest, arresting_unit, group_affiliation, synopsis)" +
                    " values (@cid, @ac, @da, @ta, @poa, @au, @ga, @syn)";
                helper.db.comm.Parameters.AddWithValue("@cid", case_id);
            }
            else
            {
                helper.db.comm.CommandText = "update arrest set " +
                    " arrest_classification=@ac, date_arrested=@da, time_arrested=@ta, place_of_arrest=@poa, arresting_unit=@au, group_affiliation=@ga, synopsis=@syn" +
                    " where case_id=@cid";
                helper.db.comm.Parameters.AddWithValue("@cid", cid);
            }
            helper.db.comm.Parameters.AddWithValue("@ac", textBox34.Text);
            helper.db.comm.Parameters.AddWithValue("@da", dateTimePicker2.Value);
            helper.db.comm.Parameters.AddWithValue("@ta", textBox32.Text);
            helper.db.comm.Parameters.AddWithValue("@poa", textBox31.Text);
            helper.db.comm.Parameters.AddWithValue("@au", textBox30.Text);
            helper.db.comm.Parameters.AddWithValue("@ga", textBox29.Text);
            helper.db.comm.Parameters.AddWithValue("@syn", textBox24.Text);
            return helper.db.comm.ExecuteNonQuery() > 0;
        }
        bool insertMugshot(long case_id)
        {
            helper.db.openconn();
            if (cid == 0)
            {
                helper.db.comm.CommandText = "insert into mugshots (case_id,front_view,right_view,left_view,whole_body) values (@case_id,@fv,@rv,@lv,@wb)";
                helper.db.comm.Parameters.AddWithValue("@case_id", case_id);
            }
            else
            {
                helper.db.comm.CommandText = "update mugshots set front_view=@fv,right_view=@rv,left_view=@lv,whole_body=@wb where case_id=@cid";
                helper.db.comm.Parameters.AddWithValue("@cid", cid);
            }
            helper.db.comm.Parameters.Add("@fv", MySql.Data.MySqlClient.MySqlDbType.Blob);
            helper.db.comm.Parameters["@fv"].Value = helper.ImageToByteArray(frontview.Image);
            helper.db.comm.Parameters.Add("@rv", MySql.Data.MySqlClient.MySqlDbType.Blob);
            helper.db.comm.Parameters["@rv"].Value = helper.ImageToByteArray(rightview.Image);
            helper.db.comm.Parameters.Add("@lv", MySql.Data.MySqlClient.MySqlDbType.Blob);
            helper.db.comm.Parameters["@lv"].Value = helper.ImageToByteArray(leftview.Image);
            helper.db.comm.Parameters.Add("@wb", MySql.Data.MySqlClient.MySqlDbType.Blob);
            helper.db.comm.Parameters["@wb"].Value = helper.ImageToByteArray(wholebody.Image);
            return helper.db.comm.ExecuteNonQuery() > 0;
        }
        long insertCase(long det_id)
        {

            helper.db.openconn();
            if (cid == 0)
            {
                helper.db.comm.CommandText = "insert into `case` " +
                    "(detainee, `IS/CC_no`, `prosecutor/judge`, case_status, suspect_status, date_detained, modus_operanda, name_of_investigator)" +
                    " values (@det, @is, @pj, @cs, @ss, @dd, @mp, @noi)";
            }
            else
            {
                helper.db.comm.CommandText = "update `case` set " +
                      "detainee=@det, `IS/CC_no`=@is, `prosecutor/judge`=@pj, case_status=@cs, suspect_status=@ss, date_detained=@dd, modus_operanda=@mp, name_of_investigator=@noi" +
                      " where id=@cid";
                helper.db.comm.Parameters.AddWithValue("@cid", cid);
            }
            helper.db.comm.Parameters.AddWithValue("@det", det_id);
            helper.db.comm.Parameters.AddWithValue("@is", textBox36.Text);
            helper.db.comm.Parameters.AddWithValue("@pj", textBox35.Text);
            helper.db.comm.Parameters.AddWithValue("@cs", textBox28.Text);
            helper.db.comm.Parameters.AddWithValue("@ss", comboBox3.Text);
            helper.db.comm.Parameters.AddWithValue("@dd", dateTimePicker3.Value);
            helper.db.comm.Parameters.AddWithValue("@mp", textBox25.Text);
            helper.db.comm.Parameters.AddWithValue("@noi", textBox37.Text);
            if(helper.db.comm.ExecuteNonQuery() > 0)
            {
                return helper.db.comm.LastInsertedId;
            }
            return 0;
        }

        private void buttonnext_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                if(currenttab < tabControl1.TabCount)
                    currenttab += 1;
                tabControl1.SelectedIndex = currenttab;
                if (currenttab == tabControl1.TabCount)
                {
                    long det_id=0;
                    if (radioButton1.Checked||!panel16.Visible)
                    {
                        helper.db.openconn();

                        if (cid == 0)
                        {

                            helper.db.comm.CommandText = "insert into detainee_personal_information (" +
                                                   "first_name, middle_name, last_name, qualifier, alias, gender, date_of_birth," +
                                                   " place_of_birth, ethnic_group, civil_status, height, weight, build, complexion, " +
                                                   "citizenship, hair_color, eye_color, identifying_marks, marks_location, name_of_spouse, language, last_known_address)" +
                                                   " values (@fn,@mn,@ln,@qua,@al,@gen,@dob,@pob,@eth,@cs,@hei,@wei,@bui,@comp,@cit,@hc,@ec,@im,@ml,@ns,@lang,@lka)";
                        }
                        else
                        {
                            helper.db.comm.CommandText = "update detainee_personal_information set " +
                                                   "first_name=@fn, middle_name=@mn, last_name=@ln, qualifier=@qua, alias=@al, gender=@gen, date_of_birth=@dob," +
                                                   " place_of_birth=@pob, ethnic_group=@eth, civil_status=@cs, height=@hei, weight=@wei, build=@bui, complexion=@comp, " +
                                                   " citizenship=@cit, hair_color=@hc, eye_color=@ec, identifying_marks=@im, marks_location=@ml, name_of_spouse=@ns, language=@lang, last_known_address=@lka" +
                                                   " where id = @did";
                            helper.db.comm.Parameters.AddWithValue("@did", did);
                        }
                        helper.db.comm.Parameters.AddWithValue("@fn", textBox1.Text);
                        helper.db.comm.Parameters.AddWithValue("@mn", textBox2.Text);
                        helper.db.comm.Parameters.AddWithValue("@ln", textBox4.Text);
                        helper.db.comm.Parameters.AddWithValue("@qua", textBox3.Text);
                        helper.db.comm.Parameters.AddWithValue("@al", textBox6.Text);
                        helper.db.comm.Parameters.AddWithValue("@gen", comboBox1.Text);
                        helper.db.comm.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                        helper.db.comm.Parameters.AddWithValue("@pob", textBox5.Text);
                        helper.db.comm.Parameters.AddWithValue("@eth", textBox9.Text);
                        helper.db.comm.Parameters.AddWithValue("@cs", comboBox2.Text);
                        helper.db.comm.Parameters.AddWithValue("@hei", textBox18.Text);
                        helper.db.comm.Parameters.AddWithValue("@wei", textBox17.Text);
                        helper.db.comm.Parameters.AddWithValue("@bui", textBox16.Text);
                        helper.db.comm.Parameters.AddWithValue("@comp", textBox15.Text);
                        helper.db.comm.Parameters.AddWithValue("@cit", textBox14.Text);
                        helper.db.comm.Parameters.AddWithValue("@hc", textBox20.Text);
                        helper.db.comm.Parameters.AddWithValue("@ec", textBox19.Text);
                        helper.db.comm.Parameters.AddWithValue("@im", textBox13.Text);
                        helper.db.comm.Parameters.AddWithValue("@ml", textBox8.Text);
                        helper.db.comm.Parameters.AddWithValue("@ns", textBox22.Text);
                        helper.db.comm.Parameters.AddWithValue("@lang", textBox7.Text);
                        helper.db.comm.Parameters.AddWithValue("@lka", textBox23.Text);
                        if (helper.db.comm.ExecuteNonQuery() > 0)
                        {
                            if (cid == 0)
                                det_id = helper.db.comm.LastInsertedId;
                            else
                                det_id = did;

                        }

                    }
                    else
                    {
                        det_id = ids[comboBox4.SelectedIndex];
                    }
                    long case_id = insertCase(det_id);
                    if (insertArrest(case_id) && insertMugshot(case_id))
                    {
                        if (cid == 0)
                            MessageBox.Show("Detainee added");
                        else
                            MessageBox.Show("Detainee updated");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Complete Fields First");
            }
        }

        private void buttonback_Click(object sender, EventArgs e)
        {
            currenttab -= 1;
            tabControl1.SelectedIndex = currenttab;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            (sender as TabControl).SelectedIndex = currenttab;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime bdate = (sender as DateTimePicker).Value;
            if (bdate > DateTime.Today)
            {
                MessageBox.Show("Invalid Birthdate");
                dateTimePicker1.Value = DateTime.Now.AddYears(-1);
            }
            numericUpDown1.Value = helper.calcAge((sender as DateTimePicker).Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel15.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel15.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mediaDeviceSelector.Items.Clear();
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in filterInfoCollection)
                mediaDeviceSelector.Items.Add(Device.Name);
        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        void addtype()
        {
            if(radioButton1.Checked)
            {
                label43.Visible = false;
                comboBox4.Visible = false;
                panel6.Enabled = true;
                panel7.Enabled = true;
                panel8.Enabled = true;
                panel9.Enabled = true;
                cleartb();
            }
            else
            {
                label43.Visible = true;
                comboBox4.Visible = true;
                panel6.Enabled = false;
                panel7.Enabled = false;
                panel8.Enabled = false;
                panel9.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            addtype();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            addtype();
        }

        int[] ids = new int[1];
        void loadSelector()
        {
            helper.db.openconn();
            helper.db.comm.CommandText = "Select * from detainee_personal_information as dpi inner join `case` on dpi.id=`case`.detainee  where `case`.suspect_status<>'Detained'";
            helper.db.dr = helper.db.comm.ExecuteReader();
            comboBox4.Items.Clear();
            int counter = 1;
            while (helper.db.dr.Read())
            {
                Array.Resize(ref ids, counter);
                ids[counter - 1] = int.Parse(helper.db.dr["id"].ToString());
                string name = helper.db.dr["first_name"].ToString() + " " + helper.db.dr["middle_name"].ToString() + " " + helper.db.dr["last_name"].ToString();
                comboBox4.Items.Add(name);
                counter++;
            }
            //comboBox2.SelectedIndex = 0;
        }
        private void adddetainee_Load(object sender, EventArgs e)
        {
            if (cid == 0)
            {
                radioButton2.Checked = true;
                loadSelector();
                panel16.Visible = true ;
            }
            else
            {
                panel16.Visible = false;
            }
            TextBox[] tbs = { textBox1, textBox2, textBox4 };
            helper.setAlpha(tbs);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectdetainee(ids[comboBox4.SelectedIndex]);
        }
        void selectdetainee(int id)
        {
            helper.db.openconn();
            helper.db.comm.CommandText = "Select * from detainee_personal_information where id=@id";
            helper.db.comm.Parameters.AddWithValue("@id", id);
            helper.db.dr = helper.db.comm.ExecuteReader();
            if (helper.db.dr.Read())
            {
                textBox1.Text = helper.db.dr["first_name"].ToString();
                textBox2.Text = helper.db.dr["middle_name"].ToString();
                textBox4.Text = helper.db.dr["last_name"].ToString();
                textBox3.Text = helper.db.dr["qualifier"].ToString();
                textBox6.Text = helper.db.dr["alias"].ToString();
                comboBox1.Text = helper.db.dr["gender"].ToString();
                dateTimePicker1.Value = DateTime.Parse(helper.db.dr["date_of_birth"].ToString());
                textBox5.Text = helper.db.dr["place_of_birth"].ToString();
                numericUpDown1.Value = int.Parse(helper.calcAge(dateTimePicker1.Value).ToString());
                textBox9.Text = helper.db.dr["ethnic_group"].ToString();
                comboBox2.Text = helper.db.dr["civil_status"].ToString();
                textBox20.Text = helper.db.dr["hair_color"].ToString();
                textBox19.Text = helper.db.dr["eye_color"].ToString();
                textBox18.Text = helper.db.dr["height"].ToString();
                textBox17.Text = helper.db.dr["weight"].ToString();
                textBox16.Text = helper.db.dr["build"].ToString();
                textBox15.Text = helper.db.dr["complexion"].ToString();
                textBox14.Text = helper.db.dr["citizenship"].ToString();
                textBox13.Text = helper.db.dr["identifying_marks"].ToString();
                textBox8.Text = helper.db.dr["marks_location"].ToString();
                textBox7.Text = helper.db.dr["language"].ToString();
                textBox22.Text = helper.db.dr["name_of_spouse"].ToString();
                textBox23.Text = helper.db.dr["last_known_address"].ToString();

            }
        }
        void cleartb()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox6.Clear();
            comboBox1.SelectedIndex=-1;
            dateTimePicker1.Value = DateTime.Now.AddYears(-18);
            textBox5.Clear();
            textBox9.Clear();
            comboBox2.SelectedIndex = -1;
            textBox20.Clear();
            textBox19.Clear();
            textBox18.Clear();
            textBox17.Clear();
            textBox16.Clear();
            textBox15.Clear();
            textBox14.Clear();
            textBox13.Clear();
            textBox8.Clear();
            textBox7.Clear();
            textBox22.Clear();
            textBox23.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            currenttab -= 1;
            tabControl1.SelectedIndex = currenttab;
        }

        private void xuiButton2_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                if (currenttab < tabControl1.TabCount)
                    currenttab += 1;
                tabControl1.SelectedIndex = currenttab;
                if (currenttab == tabControl1.TabCount)
                {
                    long det_id = 0;
                    if (radioButton1.Checked || !panel16.Visible)
                    {
                        helper.db.openconn();

                        if (cid == 0)
                        {

                            helper.db.comm.CommandText = "insert into detainee_personal_information (" +
                                                   "first_name, middle_name, last_name, qualifier, alias, gender, date_of_birth," +
                                                   " place_of_birth, ethnic_group, civil_status, height, weight, build, complexion, " +
                                                   "citizenship, hair_color, eye_color, identifying_marks, marks_location, name_of_spouse, language, last_known_address)" +
                                                   " values (@fn,@mn,@ln,@qua,@al,@gen,@dob,@pob,@eth,@cs,@hei,@wei,@bui,@comp,@cit,@hc,@ec,@im,@ml,@ns,@lang,@lka)";
                        }
                        else
                        {
                            helper.db.comm.CommandText = "update detainee_personal_information set " +
                                                   "first_name=@fn, middle_name=@mn, last_name=@ln, qualifier=@qua, alias=@al, gender=@gen, date_of_birth=@dob," +
                                                   " place_of_birth=@pob, ethnic_group=@eth, civil_status=@cs, height=@hei, weight=@wei, build=@bui, complexion=@comp, " +
                                                   " citizenship=@cit, hair_color=@hc, eye_color=@ec, identifying_marks=@im, marks_location=@ml, name_of_spouse=@ns, language=@lang, last_known_address=@lka" +
                                                   " where id = @did";
                            helper.db.comm.Parameters.AddWithValue("@did", did);
                        }
                        helper.db.comm.Parameters.AddWithValue("@fn", textBox1.Text);
                        helper.db.comm.Parameters.AddWithValue("@mn", textBox2.Text);
                        helper.db.comm.Parameters.AddWithValue("@ln", textBox4.Text);
                        helper.db.comm.Parameters.AddWithValue("@qua", textBox3.Text);
                        helper.db.comm.Parameters.AddWithValue("@al", textBox6.Text);
                        helper.db.comm.Parameters.AddWithValue("@gen", comboBox1.Text);
                        helper.db.comm.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                        helper.db.comm.Parameters.AddWithValue("@pob", textBox5.Text);
                        helper.db.comm.Parameters.AddWithValue("@eth", textBox9.Text);
                        helper.db.comm.Parameters.AddWithValue("@cs", comboBox2.Text);
                        helper.db.comm.Parameters.AddWithValue("@hei", textBox18.Text);
                        helper.db.comm.Parameters.AddWithValue("@wei", textBox17.Text);
                        helper.db.comm.Parameters.AddWithValue("@bui", textBox16.Text);
                        helper.db.comm.Parameters.AddWithValue("@comp", textBox15.Text);
                        helper.db.comm.Parameters.AddWithValue("@cit", textBox14.Text);
                        helper.db.comm.Parameters.AddWithValue("@hc", textBox20.Text);
                        helper.db.comm.Parameters.AddWithValue("@ec", textBox19.Text);
                        helper.db.comm.Parameters.AddWithValue("@im", textBox13.Text);
                        helper.db.comm.Parameters.AddWithValue("@ml", textBox8.Text);
                        helper.db.comm.Parameters.AddWithValue("@ns", textBox22.Text);
                        helper.db.comm.Parameters.AddWithValue("@lang", textBox7.Text);
                        helper.db.comm.Parameters.AddWithValue("@lka", textBox23.Text);
                        if (helper.db.comm.ExecuteNonQuery() > 0)
                        {
                            if (cid == 0)
                                det_id = helper.db.comm.LastInsertedId;
                            else
                                det_id = did;

                        }

                    }
                    else
                    {
                        det_id = ids[comboBox4.SelectedIndex];
                    }
                    long case_id = insertCase(det_id);
                    if (insertArrest(case_id) && insertMugshot(case_id))
                    {
                        if (cid == 0)
                            MessageBox.Show("Detainee added");
                        else
                            MessageBox.Show("Detainee updated");
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Complete Fields First");
            }
        }

        private void xuiButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
