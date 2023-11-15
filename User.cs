using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public abstract class User
    {
        public string Username { get; }
        private string Password { get; }

        private bool _verified;

        public User(string username, string password)
        {
            Username = username;
            Password = password;            
        }

        public bool CheckPassword(string enteredPassword)
        {
            return Password == enteredPassword;             
        }

        public string VerifyNewUsername(string username)
        {
            byte minLength = 4;
            byte maxLength = 24;

            while (!_verified)
            {
                if (username == null || username.Length < minLength || username.Length > maxLength || CheckValidChar(username))
                {
                    Console.Write("Fel input! Ditt användarnamn kan endast vara mellan 4-24" +
                                "\ntecken långt och får ej innehålla några specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    _verified = true;
                }
            }
            return username;
        }

        public string VerifyNewPassword(string password)
        {
            byte minLength = 6;
            byte maxLength = 30;

            while (!_verified)
            {
                if (password != null && password.Length >= minLength &&
                    password.Length <= maxLength && password.Any(c => char.IsAsciiLetterUpper(c))
                    && password.Any(c => char.IsAsciiDigit(c)) && CheckValidChar(password))
                {
                    _verified = true;
                }
                else
                {
                    Console.Write("Fel input! Ditt lösenord måste vara mellan 6-30 tecken långt och" +
                                "\ninnehålla minst en stor bokstav, en siffra och ett specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            return password;
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
