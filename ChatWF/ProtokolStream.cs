using ChatWF.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF
{
    class ProtokolStream
    {
        public string userName { get; set; }
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private Dictionary<ProtokolRequest, Type> requests = new Dictionary<ProtokolRequest, Type>();
        public ProtokolStream(Stream stream)
        {
            this.stream = stream;
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        public void RegisterRequest<T>(ProtokolRequest request)
        {
            requests[request] = typeof(T);
        }
        public RequestInfo Read()
        {
            var r = (ProtokolRequest)reader.ReadByte();
            Type t = requests[r];
            object data = Activator.CreateInstance(t);
            foreach (var prop in t.GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(data, reader.ReadString());
                else if (prop.PropertyType == typeof(int))
                    prop.SetValue(data, reader.ReadInt32());
                else if (prop.PropertyType == typeof(bool))
                    prop.SetValue(data, reader.ReadBoolean());
            }
            return new RequestInfo(r, data);
        }
        public void Write<T>(T data)
        {
            Type objType = typeof(T);
            ProtokolRequest request = requests.First(f => f.Value == typeof(T)).Key;
            writer.Write((byte)request);
            foreach (var prop in objType.GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    writer.Write((string)prop.GetValue(data));
                else if (prop.PropertyType == typeof(int))
                    writer.Write((int)prop.GetValue(data));
                else if (prop.PropertyType == typeof(bool))
                    writer.Write((bool)prop.GetValue(data));
            }
        }
    }
}
