using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatWF
{
    partial class SignInClient : Form
    {
        public string UserName
        {
            get { return textBoxName.Text; }
        }
        public string Password
        {
            get { return textBoxPassword.Text; }
        }
        public SignInClient()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
