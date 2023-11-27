using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    // Base class for both the Customer- and the Admin-class.
    public class User
    {
        // Username with private set to prevent outsiders to change the value.
        public string Username { get; private set; }
        // Password is private to prevent outsiders to access the value.
        private string Password { get; set; }
        public bool IsAdmin { get; protected set; }
        public bool IsBlocked { get; set; }
        public int LogInAttempts { get; private set; }

        // Constructor for the User-class.
        public User(string username, string password)
        {
            Username = username;
            Password = password;

            // The user only gets three attempts to log in. This property
            // helps keeping track which user that's attempting to log in.            
            LogInAttempts = 3;
        }

        // Method that returns a decremented log in attempt.
        public int DecrementLogInAttempt()
        {
            return LogInAttempts--;
        }

        // Method that restores a users' log in attempts.
        public void RestoreLogInAttempts()
        {
            LogInAttempts = 3;
        }

        // Method that blocks a user after failing to log in.
        public void Block()
        {
            IsBlocked = true;
        }

        // Method that unblocks a customer, if the admin chooses to.
        public void Unblock()
        {
            IsBlocked = false;
        }

        // Method that verifies the username when a new customer is created.
        public static bool VerifyNewUsername(string username)
        {
            byte minLength = 4;
            byte maxLength = 24;

            // THe username cannot be null, cannot consist of fewer OR more than
            // 4-24 letters/numbers or contain any special symbols.
            if (username == "" || username.Length < minLength ||
                username.Length > maxLength || CheckValidChar(username))
            {
                return false;
            }
            return true;
        }

        // Method that allows both admin and customers to change their password.
        public void ChangePassword()
        {
            string newPassword;
            string oldPassword = "";
            string heading = "Ditt nya lösenord måste vara mellan 6-30 tecken långt och " +
                           "\ninnehålla minst en stor bokstav, en siffra och ett specialtecken." +
                           "\nDet nya lösenordet får heller inte vara samma som ditt gamla lösenord." +
                           "\nMata in \"0\" för att avbryta processen.\n";

            while (true)
            {
                Console.Clear();
                Console.WriteLine(heading);

                Console.Write("Skriv in ditt gamla lösenord: ");
                oldPassword = Console.ReadLine();

                // If the user entered a 0, the process of changing password is terminated.
                if (oldPassword == "0")
                {
                    Console.Write("\nDu har valt att avsluta processen!" +
                        "\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    break;
                }
                // Compares the entered password with the user's existing password.
                else if (CheckPassword(oldPassword))
                {
                    break;
                }
                else
                {
                    Console.Write("\nFel lösenord!" +
                                  "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                }
            }

            while (oldPassword != "0")
            {
                Console.Write("Skriv in ditt nya lösenord: ");
                newPassword = Console.ReadLine();

                if (VerifyNewPassword(newPassword) && newPassword != oldPassword)
                {
                    // The new password is assigned to the property that holds the password.
                    Password = newPassword;

                    Console.Write("\nDitt lösenord har nu ändrats!" +
                         "\nTryck \"ENTER\" för att återgå till föregående meny.");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.Write("\nOgiltigt lösenord! Tryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine(heading);
                }
            }
        }

        // Method for verifying a new password.
        public static bool VerifyNewPassword(string password)
        {
            byte minLength = 6;
            byte maxLength = 30;

            // The password must be between 6-30 signs long and contain
            // at least one uppercase letter, a number and a symbol.
            if (password != "" && password.Length >= minLength &&
                password.Length <= maxLength && password.Any(c => char.IsAsciiLetterUpper(c))
                && password.Any(c => char.IsAsciiDigit(c)) && CheckValidChar(password))
            {
                return true;
            }
            return false;
        }

        // Method that checks if entered password matches the password 
        // for a specific user. If its a match, the method returns true.
        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        // Method that checks if a username/password contains one of 
        // the symbols in the symbolArray. Returns true if it does.
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
