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
    public partial class viewimage : Form
    {
        public static readonly HttpClient client = new HttpClient();
        public viewimage( int id)
        {
            InitializeComponent();
            pictureBox1.LoadAsync(Properties.Settings.Default.website+"/getimage.php?id="+id);
        }
        async void loadimage()
        {
            var res = await client.GetStringAsync(Properties.Settings.Default.website + "/api.php?viewaffidavit");

            img lost = JsonConvert.DeserializeObject<img>(res);
            
        }
        private void Viewimage_Load(object sender, EventArgs e)
        {

        }
    }
    class img
    {
        public Image im { get; set; }
    }
}
