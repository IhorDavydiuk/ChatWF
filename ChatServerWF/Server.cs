using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerWF
{
    class Server
    {
        class Server : ClientBase, IClient
        {
            private WorkWithDB queryDB;
            private TcpListener server;
            private readonly object syncObject = new object();
            List<ProtokolStream> descriptors = new List<ProtokolStream>();
            private List<string> usersOnline = new List<string>();
            public Server(string strconnect, int port) : base(strconnect, port, "Server")
            {
                server = new TcpListener(ipendpoint);
                queryDB = new WorkWithDB(ConfigurationManager.ConnectionStrings["ConnectionInformationAboutTheUser"].ConnectionString);
            }
            public void Start()
            {
                server.Start();
                Thread threadServer = new Thread(AcceptClient);
                threadServer.Start();
            }
            private void AcceptClient()
            {
                while (true)
                {
                    var client = server.AcceptTcpClient();
                    InitializationClient(client);
                }
            }
            public void InitializationClient(TcpClient client)
            {
                var networkStream = client.GetStream();
                stream = createStream(networkStream);
                lock (syncObject)
                {
                    descriptors.Add(stream);
                }
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
                            onMassageReceived((MessageRequest)info.Data);
                            break;
                        case ProtokolRequest.PrivateMessageRequest:
                            onPrivateMassageReceived((PrivateMessageRequest)info.Data);
                            break;
                        case ProtokolRequest.SignInRequest:

                            onSignInRequest(protocolStream, (SignInRequest)info.Data);
                            break;
                        case ProtokolRequest.RegistrationRequest:
                            onRegistrationRequest((RegistrationRequest)info.Data);
                            break;
                    }
                }
            }
            private void onRegistrationRequest(RegistrationRequest data)
            {
                if (queryDB.LoginValidate(data.Login))
                {
                    stream.Write(new RegistrationResponse() { RespondentIsName = userName, Success = false });
                }
                else
                {
                    queryDB.RegistrationUser(data.Login, data.Password, data.UserName, data.Surname, data.DOB, data.Department);
                    stream.Write(new RegistrationResponse() { RespondentIsName = userName, Success = true });
                }
            }
            private void onMassageReceived(MessageRequest message)
            {
                var mes = message;
                lock (syncObject)
                {
                    foreach (var clientStream in descriptors)
                    {
                        clientStream.Write(message);
                        clientStream.Write(new UsersOnlineRequest() { UsersOnline = usersOnlineCatalog(usersOnline) });
                    }
                }
            }
            private void onSignInRequest(ProtokolStream stream, SignInRequest data)
            {
                if (queryDB.LoginValidate(data.Login))
                {
                    lock (syncObject)
                    {
                        descriptors.Last().userName = data.Login;
                        usersOnline.Add(data.Login);
                    }
                    stream.Write(new SignInResponse() { Name = userName, Success = true });
                    stream.Write(new UsersOnlineRequest() { UsersOnline = usersOnlineCatalog(usersOnline) });
                }
                else
                {
                    descriptors.Remove(descriptors.Last());
                    stream.Write(new SignInResponse() { Name = userName, Success = false });
                }
            }
            private string usersOnlineCatalog(List<string> usersOnline)
            {
                string userOnlineString = "";
                foreach (var user in usersOnline)
                {
                    userOnlineString += user.ToString() + ",";
                }
                return userOnlineString;
            }
            private void onPrivateMassageReceived(PrivateMessageRequest privatMessage)
            {
                lock (syncObject)
                {
                    foreach (var clientStream in descriptors.Where(f => f.userName == privatMessage.UserRecipient || f.userName == privatMessage.User))
                    {
                        clientStream.Write(new MessageRequest(privatMessage.User, privatMessage.Text));
                        clientStream.Write(new UsersOnlineRequest() { UsersOnline = usersOnlineCatalog(usersOnline) });
                    }
                }
            }
        }
    }
}
