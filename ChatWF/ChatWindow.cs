using ChatWF.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatWF
{
    public partial class ChatWindow : Form
    {
        Client client;
        public ChatWindow()
        {
            InitializeComponent();
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server server = new Server("127.0.0.1", 8080);
            server.Start();
            button1.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SignInClient FormEnterClient = new SignInClient();
            FormEnterClient.Show();
            FormEnterClient.FormClosing += onEnterClientFormClosing;
        }
        private void onEnterClientFormClosing(object sender, FormClosingEventArgs e)
        {
            client = new Client(textBox3.Text, 8080, ((SignInClient)sender).UserName, ((SignInClient)sender).Password);
            client.MessageToClient += onMessageToClient;
            client.MessageOnSignUpResponse += onMessageOnSignInResponse;
            client.UsersOnlineRequestEvent += onUsersOnlineRequestEvent;
            client.Connect();
            client.SignInUsers();
        }
        private void onMessageToClient(object sender, MessageRequest args)
        {
            textBox1.AppendText($"{args.User}: {args.Text}\r\n");
        }
        private void onRegistrationResponse(object sender, RegistrationResponse args)
        {
            if (args.Success)
            {
                textBox1.AppendText($"{args.RespondentIsName}: Registration Accepted \r\n");
                button3.Enabled = true;
            }
            else
            {
                textBox1.AppendText($"{args.RespondentIsName}: Registration Not Accepted \r\n");
            }
        }
        private void onUsersOnlineRequestEvent(object sender, UsersOnlineRequest args)
        {
            listBox1.Items.Clear();
            comboBox1.Items.Clear();
            string[] collectionOnlimeUsers = args.UsersOnline.Split(',');
            listBox1.Items.AddRange(collectionOnlimeUsers);
            comboBox1.Items.AddRange(collectionOnlimeUsers);
        }
        private void onMessageOnSignInResponse(object sender, SignInResponse args)
        {
            if (args.Success)
            {
                textBox1.AppendText($"{args.Name}: Enter Accepted\r\n");
                button3.Enabled = true;
            }
            else
            {
                textBox1.AppendText($"{args.Name}: Enter Not Accepted \r\n");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                client.SendMessageToAllClients(textBox2.Text);
                textBox2.Text = "";
            }
            else
            {
                client.PrivateSendMessage(textBox2.Text, comboBox1.Text);
                textBox2.Text = "";
            }
        }
        private void form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
            Environment.Exit(0);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            client = new Client(textBox3.Text, 8080, "RegisterClient", "111");
            client.Connect();
            RegistrationUsers registrationTable = new RegistrationUsers();
            registrationTable.Show();
            registrationTable.FormClosing += sendMessageOnRegistrationRequest;
            client.RegistrationResponse += onRegistrationResponse;
        }
        private void sendMessageOnRegistrationRequest(object sender, FormClosingEventArgs e)
        {
            client.Registration(((RegistrationUsers)sender).UserLogin, ((RegistrationUsers)sender).Password, ((RegistrationUsers)sender).UserName, ((RegistrationUsers)sender).Surname, ((RegistrationUsers)sender).DOB, ((RegistrationUsers)sender).Department);
        }
    }
}
