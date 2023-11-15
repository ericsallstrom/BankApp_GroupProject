﻿using System;
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

        public bool CheckPassword(string enteredPassword)
        {
            return Password == enteredPassword;             
        }

        public string VerifyNewUsername(string enteredUsername)
        {
            byte minLength = 4;
            byte maxLength = 24;

            while (!_verified)
            {
                if (enteredUsername == null || enteredUsername.Length < minLength || 
                    enteredUsername.Length > maxLength || CheckValidChar(enteredUsername))
                {
                    Console.Write("Ogiltigt användarnamn. Ditt användarnamn kan endast vara mellan" +
                                "\n4-24 tecken långt och får ej innehålla några specialtecken.\n" +
                                "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    _verified = true;
                }
            }
            Username = enteredUsername;
            return Username;
        }

        public string VerifyNewPassword(string enteredPassword)
        {
            byte minLength = 6;
            byte maxLength = 30;

            while (!_verified)
            {
                if (enteredPassword != null && enteredPassword.Length >= minLength &&
                    enteredPassword.Length <= maxLength && enteredPassword.Any(c => char.IsAsciiLetterUpper(c))
                    && enteredPassword.Any(c => char.IsAsciiDigit(c)) && CheckValidChar(enteredPassword))
                {
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
            Password = enteredPassword;
            return Password;
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
