using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;


namespace eserve2
{
    class helper
    {
        public static db db = new db();
        public static Form1 frmlogin = new Form1();
        public static frmmain frmmain = new frmmain();

        public static dialog dialog = new dialog();

        public static void SavePictureToFileSystem(Image picture,string path="")
        {
            string pictureFolderPath = Properties.Settings.Default.file_directory;
            Path.Combine(pictureFolderPath,path);
            if (!Directory.Exists(pictureFolderPath))
            {
                Directory.CreateDirectory(pictureFolderPath);
            }

            picture.Save(Path.Combine(pictureFolderPath, "1.jpg"));
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        public static Image bytearrayToImage(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Image im=new Bitmap(ms);
            return im;

        }
        public static int calcAge(DateTime bdate)
        {
            DateTime bday = bdate;
            DateTime today = DateTime.Today;
            int age = today.Year - bday.Year;
            return bday.Date > today.AddYears(-age) ? age - 1 : age;
        }

        public static void focus(TextBox t, Label placeholder)
        {
            t.Visible = true;
            t.Focus();
            placeholder.Visible = false;
        }

        public static void leave(TextBox t, Label placeholder)
        {
            if (string.IsNullOrEmpty(t.Text))
            {
                t.Visible = false;
                placeholder.Visible = true;
            }
            else
            {
                t.Visible = true;
                placeholder.Visible = false;
            }

        }
        public static void Email(string to,string subject,string htmlString)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("fortestingpurposeonly08@gmail.com");
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body =htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("testing.only221@gmail.com", "testingonly");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                MessageBox.Show("Email sent");
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        public static void setAlpha(TextBox[] tbs)
        {
            foreach(TextBox tb in tbs)
            {
                tb.KeyPress += fornames;
            }
        }
        public void num(KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter);
        }
        public static  void alpha(KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Space);
        }
        public void floatnum(KeyPressEventArgs e, TextBox t)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == '.');
            if (t.Text.Contains('.'))
                if (e.KeyChar == '.')
                    e.Handled = true;
        }
        public void numonly(object sender, KeyPressEventArgs e)
        {
            num(e);
        }
        static void fornames(object sender, KeyPressEventArgs e)
        {
            alpha(e);
        }

        void validnum(object sender, KeyPressEventArgs e)
        {

            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Enter || e.KeyChar == '.');
            if ((sender as TextBox).Text.Contains('.'))
                if (e.KeyChar == '.')
                    e.Handled = true;
        }
        public bool notempty(string[] a)
        {
            if (a.Contains(""))
                return false;
            else
                return true;
        }
    }
    public class Clearances
    {
        public List<Clearance> data { get; set; }
    }
    public class Clearance
    {
        public int id { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string alias { get; set; }
        public string sitio { get; set; }
        public string barangay { get; set; }
        public string date_of_birth { get; set; }
        public string sex { get; set; }
        public string civil_status { get; set; }
        public string place_of_birth { get; set; }
        public string citizenship { get; set; }
        public string religion { get; set; }
        public string occupation { get; set; }
        public string contact_number { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string purpose { get; set; }
        public string findings { get; set; }
        public string email { get; set; }

        public  string getfullname()
        {
            return first_name + " " + middle_name + " " + last_name;
        }
        public string getfulladdress()
        {
            return "sitio "+sitio + ", barangay " + barangay + ", Victoria, Oriental Mindoro";
        }
    }
}
