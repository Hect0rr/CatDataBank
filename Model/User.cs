using System;

namespace CatDataBank.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DisableDate { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string ConfirmationToken { get; set; }
        public DateTime TokenValidity { get; set; }
    }
}