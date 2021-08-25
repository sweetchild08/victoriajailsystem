using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;


namespace eserve2
{
    public partial class frmlostnfound : Form
    {
        public static readonly HttpClient client = new HttpClient();
        public frmlostnfound()
        {
            InitializeComponent();
            getContents();
        }
        int[] ids = new int[1];
        async void getContents()
        {
            string filter = "";
            if (comboBox3.SelectedIndex > 0)
            {
                filter += "&type="+comboBox3.Text;
            }
            if (comboBox1.SelectedIndex > 0)
            {
               filter += "&status=" + comboBox1.Text;
            }
            var res = await client.GetStringAsync(Properties.Settings.Default.website+"/api.php?lostnfound" + filter) ;

            lost lost= JsonConvert.DeserializeObject<lost>(res);
            listView1.Items.Clear();
            int ctr = 0;
            foreach (Lostnfound l in lost.data )
            {
                Array.Resize(ref ids, ctr + 1);
                ids[ctr] = l.id;
                string[] str = { l.name,l.type,l.contact,l.subject,l.location, l.status, l.notes,l.email };
                listView1.Items.Add(new ListViewItem(str));
                ctr++;
            }
        }
        async void action (string type = "approved" )
        {

            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("select on the list first");
                return;
            }
            string status = listView1.SelectedItems[0].SubItems[5].Text;
            //if (status!="pending")
            //{
            //    MessageBox.Show("selected item was already "+status);
            //    return;
            //}
            var values = new Dictionary<string, string>
            {
                { "lostnfound", ids[listView1.SelectedIndices[0]].ToString() },
                { "value", type}
            };

            //form "postable object" if that makes any sense
            var content = new FormUrlEncodedContent(values);
            var res = await client.PostAsync(Properties.Settings.Default.website+"/api.php", content);
            var responseString = await res.Content.ReadAsStringAsync();
            response reponse = JsonConvert.DeserializeObject<response>(responseString);
            MessageBox.Show(reponse.msg);
            getContents();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void frmlostnfound_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            action();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            getContents();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getContents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            action("rejected");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            getContents();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("select from records first");
            }
            int id = ids[listView1.SelectedIndices[0]];
            getPic(id);
        }
        void getPic(int id)
        {
            viewimage vi = new viewimage(id);
            vi.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Select from the list first");
                return;
            }
            string email = listView1.SelectedItems[0].SubItems[7].Text;
            //MessageBox.Show(email);
            if (email == "")
            {
                MessageBox.Show("No email Available");
                return;
            }
            string message = "";
            string type= listView1.SelectedItems[0].SubItems[1].Text;
            string status = listView1.SelectedItems[0].SubItems[5].Text;
            if (type=="lost" && status =="approved")
                message = "Hello<br><br>Regretfully, the item you mentioned in the web page as being lost has not been turned in our department.<br><br>The information you have given will be kept in our file. If the item should be turned in the future, we will immediately inform you accordingly.<br><br><br>Thank you.";
            if (type == "lost" && status == "returned")
                message = "Hello<br><br>We just want to inform you that the item you mentioned in the web page as being lost has been returned in our department.<br><br>Please present valid id to the in-charge representative to claim your item.<br><br><br>Thank you";
            if (type == "found" && status == "claimed")
                message = "Hello<br><br>This is to inform you that the item that you have returned in our department has been claimed by the owner.<br><br>Thank you!";
            if (message == "")
            {
                MessageBox.Show("No email message Available for this type and status");
                return;
            }
            helper.Email(email, "Lost and Found", message);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            action("returned");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            action("claimed");
        }
    }

}
