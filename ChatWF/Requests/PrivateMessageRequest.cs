using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF.Requests
{
    class PrivateMessageRequest
    {
        public string User { get; set; }
        public string Text { get; set; }
        public string UserRecipient { get; set; }
        public PrivateMessageRequest(string user, string message, string userRecipient)
        {
            UserRecipient = userRecipient;
            User = user;
            Text = message;
        }
        public PrivateMessageRequest()
        {
        }
    }
}
