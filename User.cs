using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public abstract class User
    {
        public string Username { get; private set; }
        private string Password { get; set; }

        private bool _verified;

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void VerifyNewUsername(string username)
        {
            byte minLength = 4;
            byte maxLength = 24;

            while (!_verified)
            {
                if (username == null || username.Length < minLength ||
                    username.Length > maxLength || CheckValidChar(username))
                {
                    Console.Write("Ogiltigt användarnamn. Ditt användarnamn kan endast vara mellan" +
                                "\n4-24 tecken långt och får ej innehålla några specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Username = username;
                    _verified = true;
                }
            }
        }

        public void ChangePassword()
        {
            Console.Write("Skriv in ditt gamla lösenord: ");
            string oldPassword = Console.ReadLine();

            if (oldPassword == Password)
            {
                Console.Write("Skriv in ditt nya lösenord: ");
                string newPassword = Console.ReadLine();

                VerifyNewPassword(newPassword);                
            }
            else
            {
                Console.WriteLine("Fel lösenord! Var god försök igen.");
            }
        }

        public void VerifyNewPassword(string password)
        {
            byte minLength = 6;
            byte maxLength = 30;

            while (!_verified)
            {
                if (password != null && password.Length >= minLength &&
                    password.Length <= maxLength && password.Any(c => char.IsAsciiLetterUpper(c))
                    && password.Any(c => char.IsAsciiDigit(c)) && CheckValidChar(password))
                {
                    Password = password;
                    _verified = true;
                }
                else
                {
                    Console.Write("Ogiltigt lösenord. Ditt lösenord måste vara mellan 6-30 tecken långt" +
                                "\noch innehålla minst en stor bokstav, en siffra och ett specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        private static bool CheckValidChar(string input)
        {
            char[] symbolArray = { ' ', '@', '$', '/', '\\', '#', '¤', '"', '!', '?', '%', '.', ',',
                                    '\'', '"', '(', ')', '[', ']', '{', '}', '=', '-', '+', '*',
                                    '_', ';', ':', '£', '€', '¨', '^', '~', '`', '<', '>', '|', '&' };

            foreach (var c in input)
            {
                if (symbolArray.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
