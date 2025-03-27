
Random random = new Random();
int totalScore = 0;
bool flag = true;

while (flag)
{
    // Generate random numbers and colors
    int number1 = random.Next(1, 7);
    int number2 = random.Next(1, 7);
    int number3 = random.Next(1, 7);

    string color1 = GetRandomColor(random);
    string color2 = GetRandomColor(random);
    string color3 = GetRandomColor(random);

    // Print numbers with their corresponding colors
    PrintColoredNumber(number1, color1);
    PrintColoredNumber(number2, color2);
    PrintColoredNumber(number3, color3);

    Console.WriteLine();

    // Calculate score

    // Check if all three numbers are the same
    bool allSameNumber = (number1 == number2 && number2 == number3);

    // Check if all three colors are the same
    bool allSameColor = (color1 == color2 && color2 == color3);

    // Check if all three colors are different
    bool allDifferentColors = (color1 != color2 && color2 != color3 && color1 != color3);

    // Find the minimum, maximum, and middle values of the three numbers
    int min = Math.Min(number1, Math.Min(number2, number3));
    int max = Math.Max(number1, Math.Max(number2, number3));
    int mid = number1 + number2 + number3 - min - max; // Calculate the middle number by subtracting min and max from the total

    // Check if the numbers are consecutive 
    bool consecutiveNumbers = (mid == min + 1) && (max == mid + 1);

    // Check if all three numbers are different
    bool allDifferentNumbers = (number1 != number2 && number2 != number3 && number1 != number3);

    // Check if all three numbers are either even or odd
    bool allEvenOrOdd = (number1 % 2 == number2 % 2) && (number2 % 2 == number3 % 2);

    // Initialize the score to 0
    int score = 0;

    // Assign scores based on conditions
    if (allSameNumber && allSameColor)
        score = 30; // All numbers and colors are the same
    else if (allSameNumber && allDifferentColors)
        score = 28; // All numbers are the same, but all colors are different
    else if (allSameNumber && !allSameColor && !allDifferentColors)
        score = 26; // All numbers are the same, but colors are mixed
    else if (consecutiveNumbers && allSameColor)
        score = 20; // Numbers are consecutive and all colors are the same
    else if (consecutiveNumbers && allDifferentColors)
        score = 18; // Numbers are consecutive, but all colors are different
    else if (consecutiveNumbers && !allSameColor && !allDifferentColors)
        score = 16; // Numbers are consecutive, but colors are mixed
    else if (allDifferentNumbers && allSameColor)
        score = 10; // All numbers are different and all colors are the same
    else if (allDifferentNumbers && allDifferentColors)
        score = 8; // All numbers and colors are different
    else if (!allSameNumber && allSameColor && !consecutiveNumbers && !allDifferentNumbers)
        score = 6; // Numbers are not the same or consecutive, but all colors are the same


    // Display score
    if (score > 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Congratulations!");
        Console.WriteLine($"You win ${score}.");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("You lost :(");
        Console.ResetColor();
    }

    // Bonus addition
    if (allEvenOrOdd)
    {
        score += 3;
        Console.WriteLine("You win $3 bonus.");
    }

    totalScore += score;

    // Ask to play again
    bool validInput = false;
    do
    {
        Console.Write("Do you want to play again? (y or n): ");
        string userInput = Console.ReadLine();

        if (userInput == "y")//Continue game
        {
            Console.WriteLine("-----------------------------------");
            flag = true;
            validInput = true;
        }
        else if (userInput == "n")//Finish game
        {
            flag = false;
            validInput = true;
            Console.WriteLine("...................................");
            Console.WriteLine("The game is finished!");
            Console.WriteLine($"Total score obtained is ${totalScore}");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Sorry, that’s not valid. Use 'y' to retry or 'n' to stop.");
            Console.WriteLine();
        }
    } while (!validInput); // The loop continues until a valid input is received.

}

    // Generate random color
    static string GetRandomColor(Random random)
    {
        switch (random.Next(3))
        {
            case 0:
                return "Red";
            case 1:
                return "Blue";
            default:
                return "Green";
        }
    }

    // Print number in corresponding color
    static void PrintColoredNumber(int number, string color)
{
    switch (color)
    {
        case "Green":
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            break;
        case "Red":
            Console.ForegroundColor = ConsoleColor.DarkRed;
            break;
        case "Blue":
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            break;
    }

    Console.Write(number + " ");
    Console.ResetColor(); // Reset to default color after printing
}

