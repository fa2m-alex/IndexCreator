using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ir
{
    class Program
    {
        const string FILES_PATH = @"C:\Users\Alex\Desktop\Books";
        const string SAVE_DESTINATION = @"C:\Users\Alex\Desktop\dictionary666.txt";
        const string SAVE_DESTINATION2 = @"C:\Users\Alex\Desktop\index666.txt";

        static void Main(string[] args)
        {
            Dictionary d = new Dictionary(FILES_PATH);
            d.SaveIndex(SAVE_DESTINATION2);
            d.SaveDictionary(SAVE_DESTINATION);
        }
    }
}
