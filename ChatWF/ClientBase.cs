using ChatWF.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF
{
    class ClientBase
    {
        protected IPEndPoint ipendpoint;
        protected string userName;
        protected ProtokolStream stream;
        protected string strconnect;
        protected int port;
        public ClientBase(string ipaddress, int port, string userName)
        {
            this.userName = userName;
            ipendpoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
        }
        protected ProtokolStream createStream(Stream stream)
        {
            var protocolStream = new ProtokolStream(stream);
            protocolStream.RegisterRequest<MessageRequest>(ProtokolRequest.MessageRequest);
            protocolStream.RegisterRequest<SignInRequest>(ProtokolRequest.SignInRequest);
            protocolStream.RegisterRequest<SignInResponse>(ProtokolRequest.SignInResponse);
            protocolStream.RegisterRequest<PrivateMessageRequest>(ProtokolRequest.PrivateMessageRequest);
            protocolStream.RegisterRequest<RegistrationRequest>(ProtokolRequest.RegistrationRequest);
            protocolStream.RegisterRequest<UsersOnlineRequest>(ProtokolRequest.UsersOnlineRequest);
            protocolStream.RegisterRequest<RegistrationResponse>(ProtokolRequest.RegistrationResponse);
            return protocolStream;
        }
    }
}
