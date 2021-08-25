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
    public partial class viewevent : Form
    {
        public viewevent(int id)
        {
            InitializeComponent();
            helper.db.openconn();
            helper.db.comm.CommandText = "select * from events where id=@id";
            helper.db.comm.Parameters.AddWithValue("@id", id);
            helper.db.dr = helper.db.comm.ExecuteReader(CommandBehavior.SingleRow);
            if (helper.db.dr.Read())
            {
                lbldate.Text = DateTime.Parse(helper.db.dr["date"].ToString()).ToLongDateString(); ;
                lbltitle.Text = helper.db.dr["title"].ToString();
                lblfreq.Text = helper.db.dr["frequency"].ToString();
                lblnote.Text = helper.db.dr["notes"].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
