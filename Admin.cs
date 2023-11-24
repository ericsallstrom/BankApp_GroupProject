namespace BankApp_GroupProject
{
    // The User-class is inherited by the Admin-class.
    public class Admin : User
    {
        // Constructor for the Admin-class. When the
        // the admin-object is instantiated, it 
        // automatically sets the IsAdmin-bool to true.
        public Admin(string username, string password)
            : base(username, password)
        {
            IsAdmin = true;
        }
    }
}