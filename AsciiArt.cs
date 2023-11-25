using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    // The bank-logo used throughout the program.
    public class AsciiArt
    {
        string header = @"
     \|\||
    -' ||||/
   /7   |||||/
  /    |||||||/
  \-' |||||||||
   -|||||||||||
     ||||||    
      |||||\  

 LOGIC LIONS BANK" + "\n";

        // Method that returns the logo as a string.
        public string Header()
        {
            return header;
        }
    }
}
