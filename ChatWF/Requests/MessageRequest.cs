using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF.Requests
{
   class MessageRequest
    {
        public string User { get; set; }
        public string Text { get; set; }
        public MessageRequest(string user, string message)
        {
            User = user;
            Text = message;
        }
        public MessageRequest()
        {
        }
    }
}
