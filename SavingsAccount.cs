namespace BankApp_GroupProject
{
    // The Account-class is inherited by the SavingsAccount-class.
    public class SavingsAccount : Account
    {   
        public double Interest { get; set; }
        public int InterestTime { get; set; }
        public string Months { get; set; }

        readonly AsciiArt ascii = new();

        public SavingsAccount(AccountType type, Customer customer) 
            : base(type, customer)
        {
        }

        // Method for allowing the customer to chose between two different interest rates.        
        public void InterestChoice()
        {
            do
            {
                Console.Clear();
                Console.WriteLine(ascii.Header());
                Console.Write("Nedan kan du välja vilken typ av ränta du vill ha för ditt sparkonto:" +
                            "\n[1] 3% ränta bundet på 3 månader" +
                            "\n[2] 5% ränta bundet på 1 år" +
                            "\n---" +
                            "\nDitt val: ");
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        Interest = 0.03; // 3% interest
                        InterestTime = 3; // 3 months time period
                        Months = "3 månader";
                        break;
                    case "2":
                        Interest = 0.05; // 5% interest
                        InterestTime = 12; // 12 months time period
                        Months = "1 år";
                        break;
                    default:
                        Console.Write("\nOgiltig inmatning! Tryck \"ENTER\" och försök igen.");
                        Console.ReadKey();
                        break;
                }
            } while (Interest == 0);

            Console.Write($"\nDu har valt att skapa ett sparkonto med {Interest * 100}% ränta.\n");
        }

        // Method that's calculating the interest based on the choice from the customer. 
        public void CalcInterest()
        {
            Console.Clear();
            Console.WriteLine(ascii.Header());

            double selectedInterestRate = Interest;
            decimal interestAmount = _deposit * (decimal)selectedInterestRate * InterestTime / 12;

            if (interestAmount >= 1)
            {
                Console.WriteLine();
                Console.WriteLine($"Med en räntesats på {Interest * 100}%, skulle du få " +
                                    $"{interestAmount:c} i värdeökning över {Months}.");
            }
            Console.Write("\nTryck \"ENTER\" för att återgå till föregående meny.");
            Console.ReadKey();
        }
    }
}