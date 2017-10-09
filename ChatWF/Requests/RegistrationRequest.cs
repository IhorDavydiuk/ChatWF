using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWF.Requests
{
    class RegistrationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string DOB { get; set; }
        public string Department { get; set; }
        public RegistrationRequest(string login, string password, string name,string surname,string dob,string department)
        {
            Login = login;
            Password = password;
            UserName = name;
            Surname = surname;
            this.DOB = dob;
            Department = department;
        }
        public RegistrationRequest()
        {
        }
    }
}
