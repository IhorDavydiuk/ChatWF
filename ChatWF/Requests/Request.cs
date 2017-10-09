using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF.Requests
{
    enum ProtokolRequest
    {
        MessageRequest,
        SignInRequest,
        SignInResponse,
        PrivateMessageRequest,
        RegistrationRequest,
        UsersOnlineRequest,
        RegistrationResponse
    }
}
