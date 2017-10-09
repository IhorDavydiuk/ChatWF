using ChatWF.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWF
{
    delegate void MessageEventHandler(object sender, MessageRequest args);
    delegate void SignUpResponseEventHandler(object sender, SignInResponse args);
    delegate void UsersOnlineRequestEventHundler(object sender, UsersOnlineRequest args);
    delegate void RegistrationResponseEventHundler(object sender, RegistrationResponse args);
    class Client : ClientBase, IClient
    {
        public event MessageEventHandler MessageToClient;
        public event SignUpResponseEventHandler MessageOnSignUpResponse;
        public event UsersOnlineRequestEventHundler UsersOnlineRequestEvent;
        public event RegistrationResponseEventHundler RegistrationResponse;
        private TcpClient client;
        private string password;
        public Client(string ipaddress, int portClient, string name, string password) : base(ipaddress, portClient, name)
        {
            this.password = password;
            client = new TcpClient();
        }
        public void Connect()
        {
            client.Connect(ipendpoint);
            InitializationClient(client);
        }
        public void Registration(string login, string password, string name, string surname, string dob, string department)
        {
            stream.Write(new RegistrationRequest() { Login = login, Password = password, UserName = name, Surname = surname, DOB = dob, Department = department });
        }
        public void InitializationClient(TcpClient client)
        {
            NetworkStream networkStream = client.GetStream();
            stream = createStream(networkStream);
            Thread threadReader = new Thread(() => ReadingMessages(stream));
            threadReader.Start();
        }
        public void ReadingMessages(ProtokolStream protocolStream)
        {
            while (true)
            {
                RequestInfo info = protocolStream.Read();
                switch (info.Request)
                {
                    case ProtokolRequest.MessageRequest:
                        MessageToClient?.Invoke(this, (MessageRequest)info.Data);
                        break;
                    case ProtokolRequest.SignInResponse:
                        MessageOnSignUpResponse?.Invoke(this, ((SignInResponse)info.Data));
                        break;
                    case ProtokolRequest.UsersOnlineRequest:
                        UsersOnlineRequestEvent?.Invoke(this, ((UsersOnlineRequest)info.Data));
                        break;
                    case ProtokolRequest.RegistrationResponse:
                        RegistrationResponse?.Invoke(this, ((RegistrationResponse)info.Data));
                        break;
                }
            }
        }
        public void SignInUsers()
        {
            SignIn(userName, password);
        }
        private void SignIn(string login, string parol)
        {
            stream.Write(new SignInRequest(){Login = login,Password = parol});
        }
        public void PrivateSendMessage(string message, string userRecipient)
        {
            stream.Write(new PrivateMessageRequest(userName, message, userRecipient));
        }
        public void SendMessageToAllClients(string message)
        {
            stream.Write(new MessageRequest(userName, message));
        }
    }
}
