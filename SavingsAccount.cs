namespace BankApp_GroupProject
{
    public class SavingsAccount : Account
    {
        public double Interest { get; set; }
        public int InterestTime { get; set; }


        public SavingsAccount(double interest, int interestTime)
        {
            Interest = interest;
            InterestTime = interestTime;
        }

        public void InterestChoice()
        {
            Console.WriteLine("Välj räntan för ditt sparkonto");
            Console.WriteLine("1: 3% ränta bundet på 3 månader");
            Console.WriteLine("2: 5% ränta bundet på 1 år");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Interest = 0.03; //3% interest
                        InterestTime = 3; //3 months time period
                        break;
                    case 2:
                        Interest = 0.05; //5% interest
                        InterestTime = 12; //12 months time period
                        break;

                    default:
                        Console.WriteLine("ogiltigt val");
                        break;
                }
            }
            else
            {
                Console.WriteLine("ogiltig inmatning");
            }
            Console.WriteLine($"Du har valt {Interest * 100}% ränta");
        }
        public override decimal MakeDeposit()
        {
            IsSavingsAccount = true; // Clearifing that its a savings account
            return base.MakeDeposit(); // Calling the base method
        }
        public void CalcInterest(decimal userDeposit)
        {
            double selectedInterestRate = Interest;
            decimal interestAmount = userDeposit * (decimal)selectedInterestRate * InterestTime / 12;
            Console.WriteLine($"Med en räntesats på {Interest * 100}%, skulle du få " +
                $"{interestAmount} i värdeökning");
        }
    }
}