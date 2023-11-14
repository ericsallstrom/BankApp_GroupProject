using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        private bool _verified;

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void ValidateUsername(string userInput)
        {
            string username = userInput.Trim();

            while (!_verified)
            {
                if (!string.IsNullOrWhiteSpace(username) || CheckValidChar(username))
                {
                    Username = username;
                    Console.WriteLine("Good");
                    _verified = true;
                }
                else
                {
                    Console.WriteLine("Fel input! Var god ange ett korrekt användarnamn.");
                }
            }
        }

        private static bool CheckValidChar(string username)
        {
            char[] invalidChars = { ' ', '@', '$', '/', '\\', '#', '¤', '"', '!', '?' };

            foreach (var c in username)
            {
                if (!invalidChars.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
