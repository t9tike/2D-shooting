using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ControlzEx.Standard;


namespace Perkelesimu
{
    
    class Records
    {
        public int record = formPelialue.score;
        string path = "C:\\Users\\keijo\\Desktop\\Windows ohjelmointi\\Harjoittelua\\Perkelesimu\\Perkelesimu\\Resources\\Save.txt";
        public static int ennatys;
        
        
        public void saveRecord(int pRecord)
        {
            try
            {
                File.AppendAllText("C:\\Users\\keijo\\Desktop\\Windows ohjelmointi\\Harjoittelua\\Perkelesimu\\Perkelesimu\\Resources\\Save.txt",
                pRecord + Environment.NewLine);  // append on hyvä koska ei ylikirjoita vanhoja
            }
            catch (Exception)
            {
                Console.WriteLine("Kaatu koko paska");
                
            }
        }

        public void readRecord()
        {
            try
            {
                ennatys = File.ReadAllLines(path).Select(int.Parse).Max();
            }
            catch (Exception)
            {
                Console.WriteLine("Kaatu koko paska");
            }
            //var lines = File.ReadAllLines(path);      // luetaan kaikki rivit tiedostosta
            //foreach (var line in lines)               // käydään läpi kaikki rivit
            //{
            //    Console.WriteLine("\t" + line);       // tulostetaan kaikki rivit
            //}

            Console.WriteLine(ennatys);
        }











    }
}
