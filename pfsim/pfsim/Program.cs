using Nu.CommandLine;
using Nu.CommandLine.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim
{
    class Program
    {
        static void Main(string[] args)
        {
            var cp = CommandProcessor.GenerateCommandProcessor(new InteractiveCommandLineCommunicator("pfsim"));
            cp.Start();
        }
    }
}
