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
    public partial class checklist : Form
    {
        public DialogResult d { get; set; }
        private CheckBox[] checkboxes = new CheckBox[1];
        public checklist(string[] checklist, string but = "")
        {
            InitializeComponent();
            int w = this.Width;
            for (int i = 0; i < checklist.Length; i++)
            {
                Array.Resize(ref checkboxes, i + 1);
                this.checkboxes[i] = new CheckBox();
                this.checkboxes[i].AutoSize = true;
                this.checkboxes[i].Location = new Point(w / 2 - 90, 20 + (i * 40));
                this.checkboxes[i].Name = "checkBox" + i;
                this.checkboxes[i].Size = new Size(137, 29);
                this.checkboxes[i].TabIndex = 7 + i;
                this.checkboxes[i].Text = checklist[i];
                this.checkboxes[i].UseVisualStyleBackColor = true;
                this.panel3.Controls.Add(this.checkboxes[i]);

                this.checkboxes[i].CheckedChanged += new EventHandler(checkit);
            }

            if (but != "")
            {
                xuiButton2.ButtonText = but;
            }

        }

        void checkit(object sender, EventArgs e)
        {
            bool en = true;
            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (!checkboxes[i].Checked)
                {
                    en = false;
                    break;
                }
            }
            xuiButton2.Enabled = en;
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
            this.d = DialogResult.Cancel;
            this.Close();
        }

        private void xuiButton2_Click(object sender, EventArgs e)
        {
            this.d = DialogResult.OK;
            this.Close();
        }
    }
}
