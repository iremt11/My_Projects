using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace Homework3
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            string[] corpus = File.ReadAllLines("corpus.txt");//I used it to open the corpus file.
            string[] questions = File.ReadAllLines("questions.txt");//I used it to open the question file.
            char[] operators = { '.', ',', ';', '’', '\"', '?', '/', '*', '-', '+', '=', ' ' };//I made the operators a char array.
            string[] stop_words = { "a", "after", "again", "all", "am", "and", "any", "are", "as", "at", "be", "been", "before", "between", "both", "but", "by", "can", "could", "for",
            "from", "had", "has", "he", "her", "here", "him", "in", "into", "I", "is", "it", "me", "my", "of", "on", "our", "she", "so", "such", "than", "that", "the", "then", "they",
            "this", "to", "until", "we", "was", "were", "with", "you" };//This is how I defined stop words to make a string array and pull it through.
            string question1 = "What are the top10 words in the pattern";//I assigned it to a variable to find the most used word.
            string question2 = "What is the result of expression";//I assigned the question of the results of mathematical operations to a variable.
            for (int i = 0; i < questions.Length; i++)
            {
                if (questions[i].Contains(question1))//I used the other if structure if the variable to which I assigned the most used words is present in the questions txt.
                {
                    bool flag = true;
                    int counter = 1;
                    string[] pat = new string[999];
                    string[] vocable1 = questions[i].Replace("?", "").Split(' ');//the questions are contained in the text "?" I have assigned their marks as spaces.
                    char[] a = new char[vocable1[8].Length];
                    a = vocable1[8].ToLower().ToCharArray();
                    char[] b = new char[vocable1[8].Length];
                    for (int j = 0; j < corpus.Length; j++)
                    {
                        string[] wrd = corpus[j].Replace("?", "").Replace(".", "").Replace(";", "").Replace(",", "").ToLower().Split(' ');//the answers are found in the txts "?", "," , ";" , "." I defined it as a space by changing it.
                        for (int k = 0; k < wrd.Length; k++)
                        {
                            if (vocable1[8].Length == wrd[k].Length)
                            {
                                if (pat.Contains(wrd[k]) != flag)
                                {
                                    if (counter < 11)
                                    {
                                        b = wrd[k].ToCharArray();
                                        for (int z = 0; z < vocable1[8].Length; z++)
                                        {
                                            if (a[z] != '-')
                                            {
                                                if (a[z] != b[z])
                                                    break;
                                            }
                                            if (z == vocable1[8].Length - 1)
                                            {
                                                pat[counter] = wrd[k];
                                                Console.Write(pat[counter] + " ");
                                                counter++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Console.WriteLine();
                }
                else if (questions[i].Contains(question2))//if there are questions that contain mathematical operations in the questions txt, it makes you find them with the if structure.
                {
                    for (int j = 0; j < operators.Length; j++)
                    {
                        if (questions[i].IndexOf(operators[j]) > -1)
                        {
                            string a = questions[i].Substring(questions[i].IndexOf("=") + 1, questions[i].Length - questions[i].IndexOf("=") - 2);
                            int variable = Convert.ToInt32(a);
                            string[] exp = questions[i].Substring(33, questions[i].IndexOf("for") - 34).Split(operators[j]);
                            double sum = 0;
                            double mul = 1;
                            if (j == 6)
                            {
                                for (int k = exp.Length - 1; k >= 0; k--)
                                {
                                    string[] num = exp[k].Split('x');
                                    mul = Convert.ToInt32(num[0]) * Math.Pow(variable, Convert.ToInt32(num[1])) / mul;
                                }
                                Console.WriteLine("The result is " + mul + ".");
                                break;
                            }
                            else if (j == 7)
                            {
                                for (int k = 0; k < exp.Length; k++)
                                {
                                    string[] num = exp[k].Split('x');
                                    mul = mul * Convert.ToInt32(num[0]) * Math.Pow(variable, Convert.ToInt32(num[1]));
                                }
                                Console.WriteLine("The result is " + mul + ".");
                                break;
                            }
                            else if (j == 8)
                            {
                                for (int k = 0; k < exp.Length; k++)
                                {
                                    string[] num = exp[k].Split('x');
                                    sum = sum - Convert.ToInt32(num[0]) * Math.Pow(variable, Convert.ToInt32(num[1]));
                                }
                                Console.WriteLine("The result is " + sum + ".");
                                break;
                            }
                            else if (j == 9)
                            {
                                for (int k = 0; k < exp.Length; k++)
                                {
                                    string[] num = exp[k].Split('x');
                                    sum = sum + Convert.ToInt32(num[0]) * Math.Pow(variable, Convert.ToInt32(num[1]));
                                }
                                Console.WriteLine("The result is " + sum + ".");
                                break;
                            }
                        }
                    }
                }
                else //the questions that are in the txt print the questions if the corpus contains the answers that are in the txt.
                {
                    string[] w1 = questions[i].Replace("?", "").Replace(",", "").Replace(";", "").Replace(".", "").Split(' ');//the answers are found in the txts "?", "," , ";" , "." I defined it as a space by changing it.
                    string corpus1 = "NO ANSWER !!!";
                    int temporary = 1;
                    int corpus_counter = 1;
                    bool flag = false;
                    for (int j = 0; j < corpus.Length; j++)
                    {
                        string[] w2 = corpus[j].Replace("?", "").Replace(".", "").Replace(";", "").Replace(",", "").Split(' ');//the answers are found in the txts "?", "," , ";" , "." I defined it as a space by changing it.
                        for (int k = 0; k < w1.Length; k++)
                        {
                            for (int l = 0; l < w2.Length; l++)
                            {
                                if (flag == true)
                                {
                                    flag = false;
                                    break;
                                }
                                if (w1[k] == w2[l])
                                {
                                    for (int m = 0; m < stop_words.Length; m++)
                                    {
                                        if (m == stop_words.Length - 1)
                                        {
                                            corpus_counter++;
                                            flag = true;
                                        }
                                        else if (w1[k] == stop_words[m])
                                            break;
                                    }
                                }
                            }
                            if (corpus_counter > temporary)
                            {
                                if (corpus_counter == 3 || corpus_counter > 3)
                                {
                                    corpus1 = corpus[j];
                                    temporary = corpus_counter;
                                }
                            }
                        }
                        corpus_counter = 1;
                    }
                    Console.WriteLine(corpus1);
                }
            }
            Console.ReadKey();
        }
    }
}