using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadaca2
{
    public class GaussovaMetodaEliminacije
    {
        public static void GaussElim()
        {
            //biramo način unosa (ručno, preset ili 5. zadatak)
            Console.WriteLine("Gaussova metoda eliminacije");
            Console.WriteLine("Odaberite opciju:");
            Console.WriteLine("1 - Unos vrijednosti ručno");
            Console.WriteLine("2 - Koristi unaprijed definirane matrice");
            Console.WriteLine("3 - 5. Zadatak - Rješavanje sistema");
            Console.Write("Vaš izbor: ");

            string izbor = Console.ReadLine();

            //deklarišemo matricu za koeficijente jednačina te niz za nehomogene članove
            double[,] matricaSistema;
            double[] nehomogeniClanovi;

            //switch case od kojeg zavisi način unosa
            switch (izbor)
            {
                case "1":
                    // ručni unos gdje korisnik unosi broj jednačina,
                    // vrijednosti za matricu sistema potom vrijednosti nehomogenih članova
                    Console.Write("Unesite broj jednačina: ");
                    int n = int.Parse(Console.ReadLine());

                    matricaSistema = new double[n, n];
                    nehomogeniClanovi = new double[n];
                    //unos matrice
                    Console.WriteLine("Unesite koeficijente matrice sistema:");
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            Console.Write($"A[{i + 1},{j + 1}] = ");
                            matricaSistema[i, j] = double.Parse(Console.ReadLine());
                        }
                    }
                    //unos nehomogenih članova
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
                                        "Opcija - 2\n\tx[1] + x[2] + x[3] = 6\n\t2x[2] + 5x[3] = -4\n\t2x[1] + 5x[2] - x[3] = 27\n");
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
                            break;
                        case "2":
                            matricaSistema = new double[,]
                            {
                                { 1, 1, 1 },
                                { 0, 2, 5 },
                                { 2, 5, -1 }
                            };
                            nehomogeniClanovi = new double[] { 6, -4, 27 };
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
                    break;

                default:
                    Console.WriteLine("Pogrešan unos. Povratak na glavni meni.");
                    return;
            }
            //poziv za rješavanje sistema, proslijeđujemo unijete podatke
            double[] rezultat = SolveGauss(matricaSistema, nehomogeniClanovi);
            //ispis konačnog rješenja
            Console.WriteLine("Rješenje:");
            for (int i = 0; i < rezultat.Length; i++)
            {
                Console.WriteLine($"X[{i + 1}] = {rezultat[i]}");
            }
        }
        //rješavanje se sastoji iz dvije glavne petlje
        //prva petlja predstavlja sam proces eliminacije
        //druga petlja je zamjena unazad gdje rješenje iz zadnje jednačine uvrštavamo u prthodnu i tako do kraja
        private static double[] SolveGauss(double[,] matricaSistema, double[] nehomogeniClanovi)
        {
            int n = nehomogeniClanovi.Length;
            //prolazak kroz kolone matrice
            for (int k = 0; k < n; k++)
            {
                //prolazak kroz redove matrice
                for (int i = k + 1; i < n; i++)
                {
                    //faktor predstavlja količnik između elementa [i,k] i [k,k] te se koristi za eliminaciju
                    //u biti cilj nam je dobiti gornu trougaonu matricu
                    double faktor = matricaSistema[i, k] / matricaSistema[k, k];
                    for (int j = k; j < n; j++)
                    {
                        //eliminacija elementa
                        matricaSistema[i, j] -= faktor * matricaSistema[k, j];
                    }
                    // od datog nehomogenog člana oduzimamo proizvod faktora i prethodnog nehomogenog člana
                    nehomogeniClanovi[i] -= faktor * nehomogeniClanovi[k];
                }
            }
            //zamjena unazad sada prolazi nazad kroz sistem kako bi dobili rješenje
            double[] rezultat = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                rezultat[i] = nehomogeniClanovi[i];
                for (int j = i + 1; j < n; j++)
                {
                    rezultat[i] -= matricaSistema[i, j] * rezultat[j];
                }
                rezultat[i] /= matricaSistema[i, i];
            }

            return rezultat;
        }
    }
}
