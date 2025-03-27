using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALgo_lab_son_hal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double x, y;
            string oprt;

            while (true)
            {
                Console.WriteLine("Please enter your x value between 2 and 50");
                x = Convert.ToDouble(Console.ReadLine());

                if (2 <= x && x <= 50)
                {
                    break;
                }
                Console.WriteLine("You have inputted incorrect value.Please try again.");
            }

            while (true)
            {
                Console.WriteLine("Please enter your y value between 25 and 30");
                y = Convert.ToDouble(Console.ReadLine());

                if (25 <= y && y <= 30)
                {
                    break;
                }
                Console.WriteLine("You have inputted incorrect value.Please try again.");
            }

            while (true)
            {
                Console.WriteLine("Please choose your operator * or +");
                oprt = Console.ReadLine();
                if (oprt == "*")
                {
                    break;
                }
                else if (oprt == "+")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please choose the correct operation.Try again.");
                }
            }
            double fact = 1;
            double result = 0;
            double multipication = 0;
            double over = 1;

            if (oprt == "*")//Using if-else structure, I performed the operation according to the given operator.
            {

                for (int i = 0; i <= 24; i++)
                {
                    for (double fct = y - i; fct > 0; fct--)//According to y, the factorial was taken.
                    {
                        fact *= fct;
                    }

                    for (double b = 3 * i + 2; b <= 3 * i + 2; b++)
                    {
                        if ((b * x) * (b + 3) * x < fact)//I realized that X is small.
                        {
                            for (double j = 2 * i + 1; j <= 4 * i + 3; j = j + 2)// I calculated by looping the exponents and numbers in divisor.
                            {
                                for (double us = 1; us <= i + 1; us++)
                                {
                                    over = over * j;
                                }
                                multipication = over + multipication;
                                over = 1;
                            }
                            if (i == 0 || i % 2 == 1)//I looked at the case of + in the first term and odd-numbered terms.
                            {
                                result = result + (((b * x) * (b + 3) * x) / multipication);
                            }
                            else// I looked at the case of - in the even-numbered terms.
                            {
                                result = result - (((b * x) * (b + 3) * x) / multipication);
                            }
                        }
                        else
                        {
                            for (double j = 2 * i + 1; j <= 4 * i + 3; j = j + 2)
                            {
                                for (int us = 1; us <= i + 1; us++)
                                {
                                    over = over * j;//We don't use the Math.Pow functions.I had to put it in the loop.
                                }
                                multipication = over + multipication;
                                over = 1;
                            }
                            if (i == 0 || i % 2 == 1)
                            {
                                result = result + (fact / multipication);
                            }
                            else
                            {
                                result = result - (fact / multipication);
                            }
                        }
                    }
                    multipication = 0;
                }
            }
            else
            {
                for (int i = 0; i <= 24; i++)
                {
                    for (double f = y - i; f > 0; f--)
                    {
                        fact *= f;
                    }
                    for (double b = 3 * i + 2; b <= 3 * i + 2; b++)
                    {
                        if ((b * x) + (b + 3) * x < fact)
                        {
                            for (double j = 2 * i + 1; j <= 4 * i + 3; j = j + 2)
                            {
                                for (double us = 1; us <= i + 1; us++)
                                {
                                    over = over * j;
                                }
                                multipication = over + multipication;
                                over = 1;
                            }
                            if (i == 0 || i % 2 == 1)
                            {
                                result = result + ((b * x) + (b + 3) * x) / multipication;
                            }
                            else
                            {
                                result = result - ((b * x) + (b + 3) * x) / multipication;
                            }
                        }
                        else
                        {
                            for (double j = 2 * i + 1; j <= 4 * i + 3; j = j + 2)
                            {
                                for (int us = 1; us <= i + 1; us++)
                                {
                                    over = over * j;
                                }
                                multipication = over + multipication;
                                over = 1;
                            }
                            if (i == 0 || i % 2 == 1)
                            {
                                result = result + (fact / multipication);
                            }
                            else
                            {
                                result = result - (fact / multipication);
                            }
                        }
                    }
                    multipication = 0;
                }
            }
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
