using System;
using System.Collections.Generic;
using System.IO;

namespace YKS_Homework
{
    internal class Program
    {
        const string candidatesFile = "./candidates.txt";
        const string departmentsFile = "./departments.txt";
        static char[] key = { 'A', 'B', 'D', 'C', 'C', 'C', 'A', 'D', 'B', 'C', 'D', 'B', 'A', 'C', 'B', 'A', 'C', 'D', 'C', 'D', 'A', 'D', 'B', 'C', 'D' };

        static List<List<string>> candidateDataList = new List<List<string>>();
        static List<List<string>> departmentDataList = new List<List<string>>();

        static List<List<string>> ReverseCandidates(List<List<string>> candidateDataList)
        {
            List<List<string>> list = new List<List<string>>();

            for (int i = candidateDataList.Count - 1; i >= 0; i--)
                list.Add(candidateDataList[i]);

            return list;
        }


        static List<List<string>> Longc(List<List<string>> candidateDataList)
        {
            int n = candidateDataList.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (Convert.ToInt32(candidateDataList[j][candidateDataList[j].Count - 1]) > Convert.ToInt32(candidateDataList[j + 1][candidateDataList[j + 1].Count - 1]))
                    {
                        List<string> tempVar = candidateDataList[j];

                        candidateDataList[j] = candidateDataList[j + 1];
                        candidateDataList[j + 1] = tempVar;
                    }
                }
            }
            return candidateDataList;
        }

        // This function will load all data in a file and return them in a list by spliting strings in line using "," as delimiter.
        static List<List<string>> ParseFile(string filePath)
        {

            List<List<string>> lineDataList = new List<List<string>>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // Check if it is empty line. If yes, continue.
                if (line == "") continue;

                // Split the strings in line using "," as delimiter.
                string[] values = line.Split(',');
                lineDataList.Add(new List<string>(values));
            }

            return lineDataList;
        }

        // This function will return the correct answer count of candidate.
        static int CheckCandidateCorrectAnswers(List<string> candidateData)
        {
            // First answer starts at index 6.
            int answerSheetIndex = 6;

            // Variable to hold correct answer count.
            int counter = 0;
            for (int i = answerSheetIndex; i < candidateData.Count; i++)
            {
                int relativeIndex = i - answerSheetIndex;
                // Check if index is exceeding "24" as we have maximum of 25 questions
                // which means max index is "24".
                if (relativeIndex > 24) break;

                // Variable to hold current answer.
                string answer = candidateData[i];
                string correctAnswerKey = key[relativeIndex].ToString();

                // If answer is correct, increase correct answer count.
                if (answer == correctAnswerKey)
                    counter++;
            }
            return counter;
        }

        // This function will return the wrong answer count of candidate.
        static int CheckCandidateWrongAnswers(List<string> candidateData)
        {
            // First answer starts at index 6.
            int answerSheetIndex = 6;

            // Variable to hold wrong answer count.
            int counter = 0;

            // Loop over the candidate's data to get each answer.
            for (int i = answerSheetIndex; i < candidateData.Count; i++)
            {
                int relativeIndex = i - answerSheetIndex;

                // Check if index is exceeding "24" as we have maximum of 25 questions
                // which means max index is "24".
                if (relativeIndex > 24) break;

                // Variable to hold current answer.
                string answer = candidateData[i];

                // Variable to hold the current question's correct answer.
                string correctAnswerKey = key[relativeIndex].ToString();

                // If answer is wrong, increase wrong answer count.
                if (answer != correctAnswerKey  && answer != " ")
                    counter++;
            }
            return counter;
        }

        // This function returns the candidate's grade based on correct and wrong answers.
        static int GetCandidateGrade(int D, int Y)
        {
            // 4 point for each correct answer, 3 points for each wrong answer.Grade=4*D-3*Y
            return D * 4 - Y * 3;
        }

        // This function gets the department's data matching with the given department number.
        static List<string> GetDepartmentDataByNo(string departmentNo)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < departmentDataList.Count; i++)
            {
                List<string> departmentData = departmentDataList[i];
                if (departmentData[0] == departmentNo)
                    return departmentData;
            }
            return result;
        }

        // This function gets the index of department's data matching with the given department number.
        static int GetDepartmentIndexByNo(string departmentNo)
        {
            for (int i = 0; i < departmentDataList.Count; i++)
            {
                List<string> departmentData = departmentDataList[i];

                // Check if department number is matching with given one.
                if (departmentData[0] == departmentNo)
                    return i;
            }
            return -1;
        }

        // This function returns the candidate's department choices for assignment.
        static List<string> GetCandidateDepartmentChoices(List<string> candidateData)
        {
            List<string> departments = new List<string>();

            // Check first choice and add it into departments list.
            if (candidateData[3] != " ") departments.Add(candidateData[3]);

            // Check second choice and add it into departments list.
            if (candidateData[4] != " ") departments.Add(candidateData[4]);

            // Check third choice and add it into departments list.
            if (candidateData[5] != " ") departments.Add(candidateData[5]);

            return departments;
        }

        static void Main(string[] args)
        {
            List<List<string>> lineDataList = ParseFile(candidatesFile);

            // Check if candidates are exceeding allowed maximum limit.
            if (lineDataList.Count > 40)
            {
                Console.WriteLine("Candidate numbers are exceeding the allowed maximum limit of 40!");
                Console.ReadKey();
            }

            int i = 0;
            foreach (List<string> lineData in lineDataList)
            {
                candidateDataList.Add(lineData);
                i++;
            }
            List<List<string>> lineDataList2 = ParseFile(departmentsFile);

            // Check if departments are exceeding allowed maximum limit.
            if (lineDataList2.Count > 10)
            {
                Console.WriteLine("Candidate numbers are exceeding the allowed maximum limit of 10!");
                Console.ReadKey();
            }

            i = 0;
            foreach (List<string> lineData in lineDataList2)
            {
                int departmentQuota = Convert.ToInt32(lineData[lineData.Count - 1]);

                if (departmentQuota > 8)
                {
                    Console.WriteLine("Department in line " + (i + 1).ToString() + " has exceeding the allowed quota of 8!");
                    Console.ReadKey();
                }
                departmentDataList.Add(lineData);

                i++;
            }
            List<List<string>> departmentAssignmentList = new List<List<string>>();

            for (int j = 0; j < departmentDataList.Count; j++)
                departmentAssignmentList.Add(new List<string>());

      
            i = 0;
            for (int j = 0; j < candidateDataList.Count; j++)
            {
                List<string> candidateData = candidateDataList[j];


                bool Error1 = false;
                int candidateGrade = -999;

                int z = 0;
                for (int k = 6; k < candidateData.Count; k++)
                    z++;

                // If "z" != 25, some fields are missing in answers.
                if (z != 25) Error1 = true;

                // If there isn't any errors, calculate his/her grade.
                if (!Error1)
                {
                    candidateGrade = GetCandidateGrade(CheckCandidateCorrectAnswers(candidateData), CheckCandidateWrongAnswers(candidateData));
                }
                candidateData.Add(candidateGrade.ToString());

                i++;
            }
            for (int j = 0; j < candidateDataList.Count; j++)
            {
                List<string> candidateData = candidateDataList[j];

                bool isAssigned = false;
                for (int q = 0; q < departmentAssignmentList.Count; q++)
                {
                    for (int p = 0; p < departmentAssignmentList[q].Count; p++)
                    {
                        if (departmentAssignmentList[q][p] == candidateData[0])
                        {
                            isAssigned = true;
                            break;
                        }
                    }
                    if (isAssigned) break;
                }

                if (isAssigned)
                    continue;

                if (candidateData[candidateData.Count - 1] == "-999") continue;

                // Get the candidate's department choices.
                List<string> departmentNumberList = GetCandidateDepartmentChoices(candidateData);

                // Get the candidate's grade based on answer results.
                int candidateGrade = GetCandidateGrade(CheckCandidateCorrectAnswers(candidateData), CheckCandidateWrongAnswers(candidateData));

                // If there is some choice and candidate has min. required grade.
                if (candidateGrade >= 40  && departmentNumberList.Count > 0)
                {
                    for (int k = 0; k < departmentNumberList.Count; k++)
                    {
                        // Get department number from list.
                        string departmentNumber = departmentNumberList[k];

                        // Get it's index by department no.
                        int departmentIndex = GetDepartmentIndexByNo(departmentNumber);

                        // Get it's data by department no.
                        List<string> departmentData = GetDepartmentDataByNo(departmentNumber);

                        // Get the current assignments in the department.
                        List<string> selectedDepartment = departmentAssignmentList[departmentIndex];

                        bool isAssinged = false;
                        for (int l = 0; l < selectedDepartment.Count; l++)
                        {
                            string assignedCandidateNo = selectedDepartment[l];

                            if (candidateData[0] == assignedCandidateNo)
                            {
                                isAssinged = true;
                                break;
                            }
                        }
                        if (isAssinged) break;

                        for (int l = 0; l < candidateDataList.Count; l++)
                        {

                            List<string> otherCandidateData = candidateDataList[l];

                            if (candidateData[0] == otherCandidateData[0]) continue;

                            isAssigned = false;
                            for (int q = 0; q < departmentAssignmentList.Count; q++)
                            {
                                for (int p = 0; p < departmentAssignmentList[q].Count; p++)
                                {
                                    if (departmentAssignmentList[q][p] == otherCandidateData[0])
                                    {
                                        isAssigned = true;
                                        break;
                                    }
                                }

                                if (isAssigned) break;
                            }

                            if (isAssigned)
                                continue;
                            List<string> otherDepartmentNumberList = GetCandidateDepartmentChoices(otherCandidateData);

                            for (int h = 0; h < otherDepartmentNumberList.Count; h++)
                            {
                                string otherDepartmentNumber = otherDepartmentNumberList[h];

                                // It matches, check their grade and diploma grade.
                                if (otherDepartmentNumber != departmentNumber) continue;

                                // Get current candidate's grade.
                                int currentCandidateGrade = Convert.ToInt32(candidateData[candidateData.Count - 1]);

                                // Get other candidate's grade.
                                int otherCandidateGrade = Convert.ToInt32(otherCandidateData[otherCandidateData.Count - 1]);

                                // Other candidate's grade is lower than current candidate, skip.
                                if (otherCandidateGrade < currentCandidateGrade)
                                    continue;

                                if (otherCandidateGrade > currentCandidateGrade && selectedDepartment.Count < Convert.ToInt32(departmentData[2]))
                                {
                                    selectedDepartment.Add(otherCandidateData[0]);
                                }
                                // They have same grade, check their diploma grade.
                                else if (otherCandidateGrade == currentCandidateGrade)
                                {
                                    // Other candidate has higher diploma grade, assign it to department if
                                    // department has quota.
                                    if (Convert.ToInt32(otherCandidateData[2]) > Convert.ToInt32(candidateData[2])
                                    && selectedDepartment.Count < Convert.ToInt32(departmentData[2]))
                                    {
                                        selectedDepartment.Add(otherCandidateData[0]);
                                    }
                                }
                            }
                        }
                        if (departmentData.Count > 0 && selectedDepartment.Count < Convert.ToInt32(departmentData[2]))
                        {
                            selectedDepartment.Add(candidateData[0]);
                            break;
                        }
                    }
                }
            }
            candidateDataList = ReverseCandidates(Longc(candidateDataList));

            // Display the grade results of candidates on the console.
            string formatStr = "{0:000}\t{1,-20} {2}";
            Console.WriteLine("Grade results of all candidates:");
            Console.WriteLine(string.Format(formatStr, "Number", "Name", "Grade"));

            i = 0;
            for (int j = 0; j < candidateDataList.Count; j++)
            {
                List<string> candidateData = candidateDataList[j];
                Console.WriteLine(string.Format(formatStr, candidateData[0], candidateData[1], candidateData[candidateData.Count - 1] != "-999" ? candidateData[candidateData.Count - 1] : "ERROR"));
                i++;
            }
            Console.WriteLine();

            // Display the assignment results of departments on the console.
            formatStr = "{0,-3}\t{1,-30} {2}";
            Console.WriteLine("Department assignments results:");
            Console.WriteLine(string.Format(formatStr, "No", "Department", "Students"));

            for (int j = 0; j < departmentDataList.Count; j++)
            {
                List<string> departmentData = departmentDataList[j];
                int departmentIndex = GetDepartmentIndexByNo(departmentData[0]);

                List<string> departmentAssignment = departmentAssignmentList[departmentIndex];
                Console.WriteLine(formatStr, departmentData[0], departmentData[1], departmentAssignment.Count > 0 ? string.Join(" ", departmentAssignment) : "-");
            }
            Console.ReadKey();
        }
    }
}