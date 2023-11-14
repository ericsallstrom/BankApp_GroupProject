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
            Username = VerifyNewUsername(username);
            Password = VerifyNewPassword(password);
        }

        private string VerifyNewUsername(string username)
        {
            byte minLength = 4;
            byte maxLength = 25;

            while (!_verified)
            {
                if (username == null || username.Length < minLength || username.Length > maxLength || CheckValidChar(username))
                {
                    Console.Write("Fel input! Ditt användarnamn kan endast vara mellan 4-24" +
                                "\ntecken långt och får ej innehålla några specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
                else
                {
                    _verified = true;
                }
            }

            return username;
        }

        private string VerifyNewPassword(string password)
        {
            byte minLength = 6;
            byte maxLength = 30;

            while (!_verified)
            {
                if (password == null || password.Length < minLength || password.Length > maxLength || !CheckValidChar(password))
                {
                    Console.Write("Fel input! Ditt lösenord kan endast vara mellan 6-30 " +
                                "\ntecken långt och måste innehålla minst ett specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
                else
                {
                    _verified = true;
                }
            }

            return password;
        }

        private static bool CheckValidChar(string username)
        {
            char[] invalidChars = { ' ', '@', '$', '/', '\\', '#', '¤', '"', '!', '?', '%', '.', ',',
                                    '\'', '"', '(', ')', '[', ']', '{', '}', '=', '-', '+', '*',
                                    '_', ';', ':', '£', '€', '¨', '^', '~', '`', '<', '<', '|', };

            foreach (var c in username)
            {
                if (invalidChars.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
