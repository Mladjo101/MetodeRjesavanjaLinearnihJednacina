using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadaca2
{
    public class JacobiRelaksacija
    {
        public static void JacobiRelaks()
        {
            //biramo način unosa (ručno, preset ili 5. zadatak)
            Console.WriteLine("Jacobieva metoda relaksacije:");
            Console.WriteLine("Odaberite opciju:");
            Console.WriteLine("1 - Unos vrijednosti ručno");
            Console.WriteLine("2 - Koristi unaprijed definirane matrice");
            Console.WriteLine("3 - 5. Zadatak - Rješavanje sistema");
            Console.Write("Vaš izbor: ");

            string izbor = Console.ReadLine();

            //deklarišemo matricu za koeficijente jednačina te niz za nehomogene članove
            double[,] matricaSistema;
            double[] nehomogeniClanovi;
            int n = 0;
            //switch case od kojeg zavisi način unosa
            switch (izbor)
            {
                case "1":
                    // ručni unos gdje korisnik unosi broj jednačina,
                    // vrijednosti za matricu sistema potom vrijednosti nehomogenih članova
                    Console.Write("Unesite broj jednačina: ");
                    n = int.Parse(Console.ReadLine());

                    matricaSistema = new double[n, n];
                    nehomogeniClanovi = new double[n];

                    Console.WriteLine("Unesite koeficijente matrice sistema:");
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            Console.Write($"A[{i + 1},{j + 1}] = ");
                            matricaSistema[i, j] = double.Parse(Console.ReadLine());
                        }
                    }

                    Console.WriteLine("Unesite vektor kolone nehomogenih članova:");
                    for (int i = 0; i < n; i++)
                    {
                        Console.Write($"B[{i + 1}] = ");
                        nehomogeniClanovi[i] = double.Parse(Console.ReadLine());
                    }
                    break;

                case "2":
                    //ovjde korisnik bira par već zadanih matrica koje smo prelazili na predavanjima
                    //u biti ovo služi kao rješenje 4. zadatka
                    Console.WriteLine("Koristite preset sistem:");
                    Console.WriteLine("Opcija - 1\n\t80x[1] - 20x[2] - 20x[3] = 20\n\t-20x[1] + 40x[2] - 20x[3] = 20\n\t-20x[1] - 20x[2] + 130x[3] = 20\n" +
                                        "Opcija - 2\n\t4x[1] − x[2] + x[4] = 100\n\t−x[1] + 4x[2] − x[3] + x[5] = 100\n\t−x[2] + 4x[3] − x[4] = 100\n\tx[1] − x[3] + 4x[4] − x[5] = 100\n\tx[2] − x[4] + 4x[5] = 100\n");
                    Console.Write("Vaš izbor: ");
                    string presetIzbor = Console.ReadLine();
                    //switch case za odabir matrice
                    switch (presetIzbor)
                    {
                        case "1":
                            matricaSistema = new double[,]
                            {
                                { 80, -20, -20 },
                                { -20, 40, -20 },
                                { -20, -20, 130 }
                            };
                            nehomogeniClanovi = new double[] { 20, 20, 20 };
                            n = 3;
                            break;
                        case "2":
                            matricaSistema = new double[,]
                            {
                                { 4, -1, 0, 1, 0 },
                                { -1, 4, -1, 0, 1 },
                                { 0, -1 ,4, -1, 0 },
                                { 1, 0, -1, 4, -1 },
                                { 0, 1, 0, -1, 4 }
                            };
                            nehomogeniClanovi = new double[] { 100, 100, 100, 100, 100 };
                            n = 5;
                            break;
                        default:
                            Console.WriteLine("Pogrešan unos. Povratak na glavni meni.");
                            return;
                    }
                    break;
                //ova opcija predstavlja odabir za rješavanje 5. zadatka
                case "3":
                    matricaSistema = new double[,]
                            {
                                { 30, -6, -12 ,0},
                                { -6, 10, -4, 0 },
                                { -12, -4, 22, -6 },
                                { 0, 0, -6, 18 }
                            };
                    nehomogeniClanovi = new double[] { 12, 8, 12, 24 };
                    n = 4;
                    break;

                default:
                    Console.WriteLine("Pogrešan unos. Povratak na glavni meni.");
                    return;
            }

            //deklaracja trenutnih aproksimacija rješenja, prethodnih aproksimacija rješenja,
            //te podešavanje trenutnih aproskimacija koriteći prosječne vrijednosti  nehomogenih članova
            double[] trenutneAprox = new double[n];
            double[] prethodne = new double[n];
            double avg = nehomogeniClanovi.Average();
            for(int i = 0; i< n; i++)
            {
                trenutneAprox[i] = avg;
            }
            //unos koeficijenta relaksaciije
            Console.Write("Unesite koeficijent relaksacije: ");
            double faktorRelax = double.Parse(Console.ReadLine());
            //unos preciznosti
            Console.Write("Unesite preciznost (Oblik -> 1e-6): ");
            double preciznost = double.Parse(Console.ReadLine());
            //podešen maksimalan broj iteracija kkao je zadano u zadatku
            const int brojIteracija = 500;

            //Provjera dijagonalne dominacije (uslov konvergencije)
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        sum += Math.Abs(matricaSistema[i, j]);
                    }
                }
                if (Math.Abs(matricaSistema[i, i]) <= sum)
                {
                    Console.WriteLine("Matrica nije dijagonalno dominantna. Metoda možda neće konvergirati.");
                    continue;
                }
            }
            //kopiranje trenutnih aproksimacija u prethodne
            Array.Copy(trenutneAprox, prethodne, n);
            //petlja kojoj prolazimo kroz iteracije
            Console.WriteLine("Iteracija\tVrijednosti\tMaksimalna greška");
            for (int iteracija = 1; iteracija <= brojIteracija; iteracija++)
            {
                Array.Copy(trenutneAprox, prethodne, n);
                //deklaracija maksimalne greške tražene zadatkom
                double maksimalnaGreska = 0;
                for (int i = 0; i < n; i++)
                {
                    //računanje sume elemenata isključujići trenutni element
                    double suma = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            suma += matricaSistema[i, j] * trenutneAprox[j];
                        }
                    }

                    //ažuriranje vrijednosti koristeći faktor relaksacije
                    //pokazalo se da se najbolji faktori nalaze između 1 i 2
                    trenutneAprox[i] = (1 - faktorRelax) * prethodne[i] + (faktorRelax * (nehomogeniClanovi[i] - suma) / matricaSistema[i, i]);

                    //računanje maksimalne greške
                    maksimalnaGreska = Math.Max(maksimalnaGreska, Math.Abs(trenutneAprox[i] - prethodne[i]));
                }



                //ispis trenutne iteracije, rezultat i greške
                Console.Write($"{iteracija}\t");
                foreach (var vrijednost in trenutneAprox)
                {
                    Console.Write($"{vrijednost:F6}\t");
                }
                Console.WriteLine($"{maksimalnaGreska:F6}");

                //provjera preciznosti
                if (maksimalnaGreska < preciznost)
                {
                    Console.WriteLine("Kriterij tačnosti je zadovoljen.");
                    break;
                }

                //provjera iteracija
                if (iteracija == brojIteracija)
                {
                    Console.WriteLine("Dosegnut je maksimalan broj iteracija bez zadovoljenja tačnosti.");
                    break;
                }
            }
            //ispis rješenja
            Console.WriteLine("Rješenje:");
            for (int i = 0; i < trenutneAprox.Length; i++)
            {
                Console.WriteLine($"X[{i + 1}] = {trenutneAprox[i]}");
            }
        }
    }
}
