namespace BankApp_GroupProject
{
    public class Customer : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Customer(string username, string password)
            : base(username, password)
        {

        }
    }
}