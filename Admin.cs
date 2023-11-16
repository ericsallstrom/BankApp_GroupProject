namespace BankApp_GroupProject
{
    public class Admin : User
    {
        public Admin(string username, string password)
            : base(username, password)
        {
           

        }
        
        //anropar metoden från "user" klassen för att skapa ny användare namn samt lösenord
        public void VerifyNewUsername()
        {
            Console.WriteLine("Skriv ny användarnamn" + 
                "Den kan endast vara mellan 4-24 tecken långt och får ej innehålla några specialtecken");
        }

        public void VerifyNewPassword()
        {
            Console.WriteLine("Skriv nytt lösenord" +
                "tänk på det måste vara mellan 6-30 tecken långt" +
                "Lösenordet ska innehålla minst en stor bokstav, en siffra och ett specialtecken");
        }

        public void DeleteUser()
        {
            Console.WriteLine("Användaren är borttagen");
        }

        public void PrintUsers()
        {
            //Finns i LoginManager klassen, Vet ej om vi behöver den här
        }




    }
}