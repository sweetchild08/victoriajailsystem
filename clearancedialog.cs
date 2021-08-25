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
    public partial class clearancedialog : Form
    {
        Clearance clr;
        public clearancedialog(Clearance clr)
        {
            this.clr = clr;
            InitializeComponent();
            loaddata();
        }


        void loaddata()
        {
            listView1.Items.Clear();
            helper.db.openconn();
            helper.db.comm.CommandText = "SELECT *,dpi.id as detaineeid,timestampdiff(YEAR,date_of_birth,now()) as age FROM detainee_personal_information as dpi " +
                " INNER JOIN `case` ON dpi.id =`case`.`detainee`" +
                " where dpi.first_name like @fname or dpi.middle_name like @mname or dpi.last_name like @mname";
            helper.db.comm.Parameters.AddWithValue("@fname", "%"+clr.first_name+"%");
            helper.db.comm.Parameters.AddWithValue("@mname", "%" + clr.middle_name + "%");
            helper.db.comm.Parameters.AddWithValue("@lname", "%" + clr.last_name + "%");
            helper.db.dr = helper.db.comm.ExecuteReader();
            while (helper.db.dr.Read())
            {
                string[] str = {
                     helper.db.dr["first_name"]+" "+helper.db.dr["middle_name"]+" "+helper.db.dr["last_name"],
                     helper.db.dr["gender"].ToString(),
                     helper.db.dr["age"].ToString(),
                     helper.db.dr["citizenship"].ToString(),
                     helper.db.dr["date_of_birth"].ToString(),
                     helper.db.dr["identifying_marks"].ToString(),
                     helper.db.dr["marks_location"].ToString(),
                     helper.db.dr["suspect_status"].ToString(),
                };
                listView1.Items.Add(new ListViewItem(str));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clr.findings = textBox1.Text;
            release r = new release(clr);
            r.ShowDialog();
            this.Close();
        }
    }
}
