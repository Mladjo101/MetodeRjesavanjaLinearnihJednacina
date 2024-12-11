using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadaca2
{
    public class LUDekompozicija
    {
        public static void LUD()
        {
            //biramo način unosa (ručno, preset ili 5. zadatak)
            Console.WriteLine("LU dekompozicija (Doolittle metoda)");
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
                            n = 3;
                            break;
                        case "2":
                            matricaSistema = new double[,]
                            {
                                { 1, 1, 1 },
                                { 0, 2, 5 },
                                { 2, 5, -1 }
                            };
                            nehomogeniClanovi = new double[] { 6, -4, 27 };
                            n = 3;
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

            //poziv za dekompoziciju sistema, tj. podjelu na gornju trougaonu matricu i donju trougaonu matricu
            (double[,] L, double[,] U) = DecomposeLU(matricaSistema);
            //prikaz donje trougaone matrice
            Console.WriteLine("Donja trougaona matrica (L):");
            PrintmatricaSistema(L);
            //prikaz gornje trougaone matrice
            Console.WriteLine("Gornja trougaona matrica (U):");
            PrintmatricaSistema(U);
            //poziv za rješavanje sistema, proslijeđujemo matrice i nehomogene članove
            double[] rezultat = SolveLU(L, U, nehomogeniClanovi);
            //ispis konačnog rješenja
            Console.WriteLine("Rješenje:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"X[{i + 1}] = {rezultat[i]}");
            }
        }
        //metoda za dobijanje gornje i donje trougaone matrice
        private static (double[,], double[,]) DecomposeLU(double[,] matricaSistema)
        {
            int n = matricaSistema.GetLength(0);
            double[,] L = new double[n, n];
            double[,] U = new double[n, n];
            //trougaone matrice dobijamo pomoću gaussove metode eliminacije
            for (int i = 0; i < n; i++)
            {
                L[i, i] = 1;
                //petlja za gornju trougaonu matricu
                for (int j = i; j < n; j++)
                {
                    U[i, j] = matricaSistema[i, j];
                    for (int k = 0; k < i; k++)
                    {
                        U[i, j] -= L[i, k] * U[k, j];
                    }
                }
                //petlja za donju trougaonu matricu
                for (int j = i + 1; j < n; j++)
                {
                    L[j, i] = matricaSistema[j, i];
                    for (int k = 0; k < i; k++)
                    {
                        L[j, i] -= L[j, k] * U[k, i];
                    }
                    L[j, i] /= U[i, i];
                }
            }
            // vraćamo obe trougaone matrice
            return (L, U);
        }

        private static double[] SolveLU(double[,] L, double[,] U, double[] nehomogeniClanovi)
        {
            int n = nehomogeniClanovi.Length;

            double[] y = new double[n];
            //zamjena unaprijed, tj. idemo od početka donje trougaone matrice pema kraju
            for (int i = 0; i < n; i++)
            {
                y[i] = nehomogeniClanovi[i];
                for (int j = 0; j < i; j++)
                {
                    y[i] -= L[i, j] * y[j];
                }
            }

            double[] x = new double[n];
            //zamjena unazad, idemo od kraja gornje trougaone matrice unazad
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = y[i];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= U[i, j] * x[j];
                }
                x[i] /= U[i, i];
            }
            //na kraju vraćamo listu rezultata za x

            return x;
        }
        //metoda za ispis gornje i donje trougaone matrice što je tražno zadatkom da bude prikazano
        private static void PrintmatricaSistema(double[,] matricaSistema)
        {
            int redovi = matricaSistema.GetLength(0);
            int kolone = matricaSistema.GetLength(1);

            for (int i = 0; i < redovi; i++)
            {
                for (int j = 0; j < kolone; j++)
                {
                    Console.Write($"{matricaSistema[i, j]:F2}\t");
                }
                Console.WriteLine();
            }
        }
    }
}
