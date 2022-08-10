using MeetingApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingApp.DAL.Entities
{
    public class Person 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime TimeAddedToMeeting { get; set; }
        public Person()
        {
            Id = Guid.NewGuid();
        }

        public Person(string name, string username, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Username: {Username}";
        }
    }
}
