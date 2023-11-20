using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public class User
    {
        public string Username { get; private set; }
        private string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
        private LogInManager LogInManager { get; set; }        

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public void Block()
        {
            IsBlocked = true;
        }

        public void Unblock()
        {
            IsBlocked = false;
        }

        // Metod för att verifiera en användares användarnamn.
        public static bool VerifyNewUsername(string username)
        {
            byte minLength = 4;
            byte maxLength = 24;

            // Användarnamn får inte vara null, innehålla färre eller mer än  
            // 4-24 tecken eller innehålla ett eller flera specialtecken.
            if (username == "" || username.Length < minLength ||
                username.Length > maxLength || CheckValidChar(username))
            {
                return false;
            }
            return true;
        }

        // Metod som tillåter användaren att byta lösenord.
        public void ChangePassword()
        {
            string newPassword;
            string oldPassword;

            while (true)
            {
                Console.Clear();

                Console.Write("Skriv in ditt gamla lösenord: ");
                oldPassword = Console.ReadLine();

                if (CheckPassword(oldPassword))
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

            while (true)
            {
                Console.Write("Skriv in ditt nya lösenord: ");
                newPassword = Console.ReadLine();

                if (VerifyNewPassword(newPassword) && newPassword != oldPassword)
                {
                    Password = newPassword;
                    break;
                }                
                else
                {
                    Console.Write("\nOgiltigt lösenord. Lösenordet måste vara mellan 6-30 tecken långt" +
                                  "\noch innehålla minst en stor bokstav, en siffra och ett specialtecken." +
                                  "\nDitt nya lösenord får heller inte vara samma som ditt gamla." +
                                  "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.Write("\nDitt lösenord har nu ändrats!" +
                          "\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }

        // Verifierar en användares lösenord.
        public static bool VerifyNewPassword(string password)
        {
            byte minLength = 6;
            byte maxLength = 30;

            // Lösenordet måste ha minst en stor bokstav, en siffra och ett specialtecken
            // och lösenordet kan inte vara null och måste vara mellan 6-30 tecken långt.
            if (password != "" && password.Length >= minLength &&
                password.Length <= maxLength && password.Any(c => char.IsAsciiLetterUpper(c))
                && password.Any(c => char.IsAsciiDigit(c)) && CheckValidChar(password))
            {
                return true;
            }
            return false;
        }

        // Kollar om det inmatade lösenordet matchar lösenordet 
        // för en viss användare. Returnerar true om det matchar.
        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        // Kollar om användarnamn/lösenord innehåller ett visst specialtecken.
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
