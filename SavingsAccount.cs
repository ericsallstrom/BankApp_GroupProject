namespace BankApp_GroupProject
{
    public class SavingsAccount : Account
    {
        public double Interest { get; set; }
        public int InterestTime { get; set; }
        public string Months { get; set; }

        public void InterestChoice()        
        {
            do
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
                            Months = "3 månander";
                            break;
                        case 2:
                            Interest = 0.05; //5% interest
                            InterestTime = 12; //12 months time period
                            Months = "1 år";
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("ogiltigt val");                        
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("ogiltig inmatning. Försök igen");
                }
            } while (Interest == 0);

            Console.WriteLine($"Du har valt att skapa ett sparkonto med {Interest * 100}% ränta");
        }
        public override decimal MakeDeposit()
        {
            IsSavingsAccount = true; // Clearifing that its a savings account
            return base.MakeDeposit(); // Calling the base method
        }
        //metod för att räkna ut räntan
        public void CalcInterest()
        {
            double selectedInterestRate = Interest;
            decimal interestAmount = Deposit * (decimal)selectedInterestRate * InterestTime / 12;
            Console.WriteLine($"Med en räntesats på {Interest * 100}%, skulle du få " +
                $"{interestAmount} i värdeökning på {Months}");
            Console.ReadKey();
        }
    }
}