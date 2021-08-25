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
    public partial class release : Form
    {
        Clearance clr;
        public release(Clearance clr)
        {
            this.clr = clr;
            InitializeComponent();
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in filterInfoCollection)
                mediaDeviceSelector.Items.Add(Device.Name);
            mediaDeviceSelector.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice();
            curr = pictureBox2;
            button1.Click += new EventHandler(chooseframe);
            textBox1.Text = Properties.Settings.Default.clearance_verify_name;
            textBox2.Text = Properties.Settings.Default.clearance_verify_designation;
            textBox4.Text = Properties.Settings.Default.clearance_approve_name;
            textBox3.Text = Properties.Settings.Default.clearance_approve_designation;

            TextBox[] tbs = { textBox1, textBox4 };
            helper.setAlpha(tbs);
        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        PictureBox curr = new PictureBox();

        void chooseframe(object sender, EventArgs e)
        {
            if (videoCaptureDevice.IsRunning == true)
            {
                closecapture();
            }
            else
            {
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[mediaDeviceSelector.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
                button1.Text = "Capture";
                videoCaptureDevice.Start();
            }

        }
        void closecapture()
        {
            if (videoCaptureDevice.IsRunning == true)
            {
                videoCaptureDevice.Stop();
            }
            if (pictureBox2.Image != null)
            {
                button1.Text = "Capture Again";
            }
            else
            {
                button1.Text = "Capture";
            }
        }
        //c# webcam capture picture
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            curr.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void release_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void mediaDeviceSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[mediaDeviceSelector.SelectedIndex].MonikerString);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.clearance_verify_name = textBox1.Text;
            Properties.Settings.Default.clearance_verify_designation = textBox2.Text;
            Properties.Settings.Default.clearance_approve_name = textBox4.Text;
            Properties.Settings.Default.clearance_approve_designation = textBox3.Text;
            Properties.Settings.Default.Save();



            helper.db.openconn();
            helper.db.comm.CommandText = "insert into clearance (full_name,nationality,present_address,alias,date_of_birth,place_of_birth,religion,occupation,civil_status,gender,height,weight,findings,purpose,picture,verify_name,verify_designation,approve_name,approve_designation)" +
                " values(@full_name,@nationality,@present_address,@alias,@date_of_birth,@place_of_birth,@religion,@occupation,@civil_status,@gender,@height,@weight,@findings,@purpose,@picture,@vn,@vd,@an,@ad)";
            helper.db.comm.Parameters.AddWithValue("@full_name", clr.getfullname());
            helper.db.comm.Parameters.AddWithValue("@nationality", clr.citizenship);
            string rochelle = clr.getfulladdress();
            helper.db.comm.Parameters.AddWithValue("@present_address", clr.getfulladdress());
            helper.db.comm.Parameters.AddWithValue("@alias", clr.alias);
            helper.db.comm.Parameters.AddWithValue("@date_of_birth", clr.date_of_birth);
            helper.db.comm.Parameters.AddWithValue("@place_of_birth", clr.place_of_birth);
            helper.db.comm.Parameters.AddWithValue("@religion", clr.religion);
            helper.db.comm.Parameters.AddWithValue("@occupation", clr.occupation);
            helper.db.comm.Parameters.AddWithValue("@civil_status", clr.civil_status);
            helper.db.comm.Parameters.AddWithValue("@gender", clr.sex);
            helper.db.comm.Parameters.AddWithValue("@height", clr.height);
            helper.db.comm.Parameters.AddWithValue("@weight", clr.weight);
            helper.db.comm.Parameters.AddWithValue("@findings", clr.findings);
            helper.db.comm.Parameters.AddWithValue("@purpose", clr.purpose);
            helper.db.comm.Parameters.AddWithValue("@picture", helper.ImageToByteArray(pictureBox2.Image));
            helper.db.comm.Parameters.AddWithValue("@vn", textBox1.Text);
            helper.db.comm.Parameters.AddWithValue("@vd", textBox2.Text);
            helper.db.comm.Parameters.AddWithValue("@an", textBox4.Text);
            helper.db.comm.Parameters.AddWithValue("@ad", textBox3.Text);
            if (helper.db.comm.ExecuteNonQuery() > 0)
            {
                printclearance pc = new printclearance(helper.db.comm.LastInsertedId);
                pc.ShowDialog();
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
