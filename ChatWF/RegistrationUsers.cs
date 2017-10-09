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
    public partial class RegistrationUsers : Form
    {
        public string UserLogin
        {
            get { return textBoxLogin.Text; }
        }
        public string Password
        {
            get { return textBoxPassword.Text; }
        }
        public string UserName
        {
            get { return textBoxName.Text; }
        }
        public string Surname
        {
            get { return textBoxSurname.Text; }
        }
        public string DOB
        {
            get { return textBoxDOB.Text; }
        }
        public string Department
        {
            get { return textBoxDepartment.Text; }
        }
        public RegistrationUsers()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
