using System;

namespace Supportbot.Application.Model
{
    public class Employee : IEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Employee() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Employee(string username, string firstname, string lastname, DateTime birth, string passwordHash)
        {
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
            Birth = birth;
            PasswordHash = passwordHash;
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birth { get; set; }
        public string PasswordHash { get; set; }
    }
}
