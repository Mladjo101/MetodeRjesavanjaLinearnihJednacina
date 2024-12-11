using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadaca2
{
    public class Menu
    {
        static void Main()
        {
            //Zadaci rađeni su grupa A
            //Zadaća se sastoji od 4 file-a: main gdje su meniji te file za svaku od traženih metoda
            //Prvo se bira metoda koju želimo koristiti
            //Zatim se bira način unosa tj. da li će korisnik ručno unijeti koeficijente,
            //da li će odabrati preset tj. sisteme sa predavanja (zadatak 4)
            //ili treća opcija to jeste rješavanje problema zadanom u 5. zadatku

            //while petlja koja služi kao početni meni
            //kroz ovu petlju se vrši odabir metode kojom korisnik želi rješavati sisteme
            while (true)
            {
                Console.WriteLine("Odaberite metodu za rješavanje nelinearnih jednačina:");
                Console.WriteLine("1 - Gaussova metoda eliminacije");
                Console.WriteLine("2 - LU dekompozicija");
                Console.WriteLine("3 - Jacobijeva metoda relaksacije");
                Console.WriteLine("0 - Povratak");
                Console.Write("Vaš izbor: ");

                string izbor = Console.ReadLine();

                switch (izbor)
                {
                    case "1":
                        GaussovaMetodaEliminacije.GaussElim();
                        break;
                    case "2":
                        LUDekompozicija.LUD();
                        break;
                    case "3":
                        JacobiRelaksacija.JacobiRelaks();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Pogrešan unos. Pokušajte ponovo.");
                        break;
                }

                Console.WriteLine("\nPritisnite Enter za povratak na meni metoda...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
 }


