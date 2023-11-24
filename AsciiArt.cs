using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp_GroupProject
{
    public class AsciiArt
    {
        string header = @"
   \|\||
  -' ||||/
 /7   |||||/
/    |||||||/`-.____________
\-' |||||||||               `-._
 -|||||||||||               |` -`.
   ||||||               \   |   `\\
    |||||\  \______...---\_  \    \\
       |  \  \           | \  |    ``-.__--.
       |  |\  \         / / | |       ``---'
     _/  /_/  /      __/ / _| |
    (,__/(,__/      (,__/ (,__/
        LOGICLIONS BANK" + "\n";
		public string Header()
		{
			return header;
		}
    }
}
