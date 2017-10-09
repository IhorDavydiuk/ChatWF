using ChatWF.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF
{
    class RequestInfo
    {
        public ProtokolRequest Request { get; }
        public object Data { get; }
        public RequestInfo(ProtokolRequest request, object data)
        {
            Request = request;
            Data = data;
        }
    }
}
