using System;
using System.Text;

class Program
{

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Random rnd = new Random();

        //Create cards
        char[][] p1 = CreateCardForPlayer();
        char[][] p2 = CreateCardForPlayer();

        int p1_score = 0;
        int p2_score = 0;

        //Finding min and max ascii values
        char first_row_min_for_p1 = FindMinASCIICharacterForGame(p1, 0);
        char first_row_min_for_p2 = FindMinASCIICharacterForGame(p2, 0);
        char first_row_max_for_p2 = FindMaxASCIICharacterForGame(p2, 0);
        char first_row_max_for_p1 = FindMaxASCIICharacterForGame(p1, 0);
        char second_row_min_for_p1 = FindMinASCIICharacterForGame(p1, 1);
        char second_row_min_for_p2 = FindMinASCIICharacterForGame(p2, 1);
        char second_row_max_for_p2 = FindMaxASCIICharacterForGame(p2, 1);
        char second_row_max_for_p1 = FindMaxASCIICharacterForGame(p1, 1);

        //Create the bag  '\u263A' = Joker
        char[] bag = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '\u263A' };

        int tour = 1;
        bool first_Cinco = false;

        bool first_row_min_for_p1_boolean = false;
        bool first_row_min_for_p2_boolean = false;
        bool second_row_min_for_p2_boolean = false;
        bool second_row_min_for_p1_boolean = false;

        bool first_row_max_for_p1_boolean = false;
        bool first_row_max_for_p2_boolean = false;
        bool second_row_max_for_p2_boolean = false;
        bool second_row_max_for_p1_boolean = false;

        while (bag.Length > 0)
        {
            PrintPlayerCardsAndScores(p1, p2, p1_score, p2_score);

            // Select a random letter from the bag and remove it
            char selected = bag[rnd.Next(bag.Length)];
            bag = TakeOutLetterReplaceSpaceFromBag(bag, selected);
            Console.WriteLine(tour + ". selected letter: " + selected);

            // If the selected letter is not a Joker
            if (selected != '\u263A'){
                int p1_Points = TakeOutLetterFromCard(p1, selected, tour);
                int p2_Points = TakeOutLetterFromCard(p2, selected, tour);

                if (p1_Points > 0){
                    p1_score += p1_Points;
                    Console.WriteLine("Player 1 gained " + p1_Points + " points.");
                }

                if (p2_Points > 0){
                    p2_score += p2_Points;
                    Console.WriteLine("Player 2 gained " + p2_Points + " points.");
                }

                if(p1_Points == 0){
                    if(p2_Points == 0)
                        Console.WriteLine("Neither player scored a point.");
                } 
            }
            // If the selected letter is a Joker
            else{
                char temporary1 = TakeOutHighestASCIILetterFromCard(p1, tour);
                char temporary2 = TakeOutHighestASCIILetterFromCard(p2, tour);
                int temp_point1 = 0;
                int temp_point2 = 0;

                // Check if the removed letter matches specific conditions for players
                if (temporary1 == first_row_min_for_p1)
                    first_row_min_for_p1_boolean = true;
                else if (temporary1 == first_row_max_for_p1)
                    first_row_max_for_p1_boolean = true;
                else if (temporary1 == second_row_max_for_p1)
                    second_row_max_for_p1_boolean = true;
                else if (temporary1 == second_row_min_for_p1)
                    second_row_min_for_p1_boolean = true;

                if (temporary2 == first_row_min_for_p2)
                    first_row_min_for_p2_boolean = true;
                else if (temporary2 == first_row_max_for_p2)
                    first_row_max_for_p2_boolean = true;
                else if (temporary2 == second_row_max_for_p2)
                    second_row_max_for_p2_boolean = true;
                else if (temporary2 == second_row_min_for_p2)
                    second_row_min_for_p2_boolean = true;

                // Calculate points based on whether the letter is a vowel or not
                if (temporary1 == 'A' || temporary1 == 'E' || temporary1 == 'I' || temporary1 == 'O' || temporary1 == 'U')
                    temp_point1 = (30 - tour + 1) * 3;
                else
                    temp_point1 = (30 - tour + 1) * 3;

                if (temporary2 == 'A' || temporary2 == 'E' || temporary2 == 'I' || temporary2 == 'O' || temporary2 == 'U')
                    temp_point2= (30 - tour + 1) * 3;
                else
                    temp_point2= (30 - tour + 1) * 2;

                Console.WriteLine("Player 1 gained " + temp_point1 + " points.");
                Console.WriteLine("Player 2 gained " + temp_point2 + " points.");

                p1_score += temp_point1;
                p2_score += temp_point2;

                // Check if both max ASCII characters are removed for players and award bonus points
                if (first_row_min_for_p1_boolean && first_row_max_for_p1_boolean){
                    p1_score += 100;
                    Console.WriteLine("Player 1 gained 100 points.");
                    first_row_max_for_p1_boolean = false;
                    first_row_min_for_p1_boolean = false;
                }

                if (first_row_min_for_p2_boolean && first_row_max_for_p2_boolean){
                    p2_score += 100;
                    Console.WriteLine("Player 2 gained 100 points.");
                    first_row_max_for_p2_boolean = false;
                    first_row_min_for_p2_boolean = false;
                }

                if (second_row_min_for_p2_boolean && second_row_max_for_p2_boolean){
                    p2_score += 100;
                    Console.WriteLine("Player 2 gained 100 points.");
                    second_row_min_for_p2_boolean = false;
                    second_row_min_for_p2_boolean = false;
                }

                if (second_row_min_for_p1_boolean && second_row_max_for_p1_boolean){
                    p1_score += 100;
                    Console.WriteLine("Player 1 gained 100 points.");
                    second_row_min_for_p1_boolean = false;
                    second_row_max_for_p1_boolean = false;
                }

                // Check if both min ASCII characters are removed for players and award bonus points
                if (first_row_max_for_p1_boolean && first_row_min_for_p1_boolean){
                    p1_score += 100;
                    Console.WriteLine("Player 1 gained 100 points.");
                    first_row_max_for_p1_boolean = false;
                    first_row_min_for_p1_boolean = false;
                }

                if (first_row_max_for_p2_boolean && first_row_min_for_p2_boolean){
                    p2_score += 100;
                    Console.WriteLine("Player 2 gained 100 points.");
                    first_row_max_for_p2_boolean = false;
                    first_row_min_for_p2_boolean = false;
                }

                if (second_row_max_for_p2_boolean && second_row_min_for_p2_boolean){
                    p2_score += 100;
                    Console.WriteLine("Player 2 gained 100 points.");
                    second_row_max_for_p2_boolean = false;
                    second_row_min_for_p2_boolean = false;
                }

                if (second_row_max_for_p1_boolean && second_row_min_for_p1_boolean){
                    p1_score += 100;
                    Console.WriteLine("Player 1 gained 100 points.");
                    second_row_max_for_p1_boolean = false;
                    second_row_min_for_p1_boolean = false;
                }
            }


            // Check if both max ASCII characters are removed for players and award bonus points
            if (first_row_min_for_p1_boolean && first_row_max_for_p1 == selected){
                p1_score += 100;
                Console.WriteLine("Player 1 gained 100 points.");
                first_row_max_for_p1_boolean = false;
                first_row_min_for_p1_boolean = false;
            }
            else if (first_row_max_for_p1 == selected)
               first_row_max_for_p1_boolean = true;


            if (first_row_min_for_p2_boolean && first_row_max_for_p2 == selected){
                p2_score += 100;
                Console.WriteLine("Player 2 gained 100 points.");
                first_row_max_for_p2_boolean = false;
                first_row_min_for_p2_boolean = false;
            }
            else if (first_row_max_for_p2 == selected)
                first_row_max_for_p2_boolean = true;


            if (second_row_min_for_p2_boolean && second_row_max_for_p2 == selected){
                p2_score += 100;
                Console.WriteLine("Player 2 gained 100 points.");
                second_row_min_for_p2_boolean = false;
                second_row_min_for_p2_boolean = false;
            }
            else if (second_row_max_for_p2 == selected)
               second_row_max_for_p2_boolean = true;

            if (second_row_min_for_p1_boolean && second_row_max_for_p1 == selected){
                p1_score += 100;
                Console.WriteLine("Player 1 gained 100 points.");
                second_row_min_for_p1_boolean = false;
                second_row_max_for_p1_boolean = false;
            }
            else if (second_row_max_for_p1 == selected)
               second_row_max_for_p1_boolean = true;

            // Check if both min ASCII characters are removed for players and award bonus points
            if (first_row_max_for_p1_boolean && first_row_min_for_p1 == selected){
                p1_score += 100;
                Console.WriteLine("Player 1 gained 100 points.");
                first_row_max_for_p1_boolean = false;
                first_row_min_for_p1_boolean = false;
            }
            else if (first_row_min_for_p1 == selected)
               first_row_min_for_p1_boolean = true;

            if (first_row_max_for_p2_boolean && first_row_min_for_p2 == selected){
                p2_score += 100;
                Console.WriteLine("Player 2 gained 100 points.");
                first_row_max_for_p2_boolean = false;
                first_row_min_for_p2_boolean = false;
            }
            else if (first_row_min_for_p2 == selected)
               first_row_min_for_p2_boolean = true;


            if (second_row_max_for_p2_boolean && second_row_min_for_p2 == selected){
                p2_score += 100;
                Console.WriteLine("Player 2 gained 100 points.");
                second_row_max_for_p2_boolean = false;
                second_row_min_for_p2_boolean = false;
            }
            else if (second_row_min_for_p2 == selected)
               second_row_min_for_p2_boolean = true;


            if (second_row_max_for_p1_boolean && second_row_min_for_p1 == selected){
                p1_score += 100;
                Console.WriteLine("Player 1 gained 100 points.");
                second_row_max_for_p1_boolean = false;
                second_row_min_for_p1_boolean = false;
            }
            else if (second_row_min_for_p1 == selected)
               second_row_min_for_p1_boolean = true;

            // Cinko control: Checking if the rows for both players are cleared
            bool First_row_cleared_for_p2 = true;
            bool Second_row_cleared_for_p2 = true;
            bool First_row_cleared_for_p1 = true;
            bool second_row_cleared_for_p1 = true;

            for (int i = 0; i < 4; i++)
            {
                // Check if there are any characters other than a space in players's rows
                if (p2[0][i] != ' ') First_row_cleared_for_p2 = false; 
                if (p2[1][i] != ' ') Second_row_cleared_for_p2 = false; 
                if (p1[0][i] != ' ') First_row_cleared_for_p1 = false; 
                if (p1[1][i] != ' ') second_row_cleared_for_p1 = false; 
            }

            if (!first_Cinco){
                bool First_Cinco_for_p1 = !(Second_row_cleared_for_p2 || First_row_cleared_for_p2)  && (second_row_cleared_for_p1 || First_row_cleared_for_p1);
                bool First_Cinco_for_p2 = !(second_row_cleared_for_p1 || First_row_cleared_for_p1) && (Second_row_cleared_for_p2 || First_row_cleared_for_p2) ;

                if (First_Cinco_for_p2){
                    p2_score += 150;  
                    Console.WriteLine("First Çinko- Player2 wins the prize and gains 150 points.");
                    first_Cinco = true; 
                }
                if (First_Cinco_for_p1){
                    p1_score += 150;  
                    Console.WriteLine("First Çinko- Player1 wins the prize and gains 150 points.");
                    first_Cinco = true; 
                }
            }

            // Tombala control: Checking for winning conditions and game state
            if (First_row_cleared_for_p1 && second_row_cleared_for_p1){
                if (First_row_cleared_for_p2 && Second_row_cleared_for_p2){
                    PrintPlayerCardsAndScores(p1, p2, p1_score, p2_score);
                    Console.WriteLine("Both players cleared their cards simultaneously! It's a draw!");
                    break;
                }
            }

            if (First_row_cleared_for_p2){
                if (Second_row_cleared_for_p2){
                    PrintPlayerCardsAndScores(p1, p2, p1_score, p2_score);
                    Console.WriteLine("Tombala-Player2 wins the grand prize.");
                    break;
                }
            }

            if (second_row_cleared_for_p1){
                if (First_row_cleared_for_p1){
                    PrintPlayerCardsAndScores(p1, p2, p1_score, p2_score);
                    Console.WriteLine("Tombala-Player1 wins the grand prize.");
                    break;
                }
            }
            tour++;
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine("The game is over after " + tour + " steps.");
        Console.WriteLine();
        Console.WriteLine("Good Bye!");
    }

    // Generates a card with two rows of four characters for a player.
    static char[][] CreateCardForPlayer()
    {
        Random rnd = new Random();
        char[][] card_for_player = new char[2][];
        card_for_player[0] = new char[4]; 
        card_for_player[1] = new char[4]; 

        
        char[] first_semi = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' }; 
        char[] second_semi = { 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        // Shuffle the first array (first_semi)
        int n = first_semi.Length;
        do
        {
            int k = rnd.Next(n--);
            char temp = first_semi[n];
            first_semi[n] = first_semi[k];
            first_semi[k] = temp;
        } while (n > 1);

        // Shuffle the second array (second_semi)
        int l = second_semi.Length;
        do
        {
            int p = rnd.Next(l--);
            char temp = second_semi[n];
            second_semi[n] = second_semi[p];
            second_semi[p] = temp;
        } while (l > 1);

        for (int i = 0; i < 4; i++)
        {
            card_for_player[0][i] = first_semi[i];
            card_for_player[1][i] = second_semi[i];
        }

        return card_for_player;
    }

    //Removes all occurrences of a specified character from the 'bag' array and returns a new array with the character removed.
    static char[] TakeOutLetterReplaceSpaceFromBag(char[] bag, char characters)
    {
        int cnt = 0;
        for (int i = 0; i < bag.Length; i++)
        {
            if (bag[i] == characters)
                cnt++;
        }

        int newSizeforbag = bag.Length - cnt;
        char[] updatebag = new char[newSizeforbag];
        int index = 0;

        for (int i = 0; i < bag.Length; i++)
        {
            // If the character is not the one we're removing, add it to the new 'updatebag' array
            if (bag[i] != characters)
                updatebag[index++] = bag[i];
            else
                bag[i] = ' ';// Replace the character with a space
        }
        return updatebag;
    }

    //Removes a specified letter from a player's tombala card and calculates the score based on the letter and the current tour.
    static int TakeOutLetterFromCard(char[][] player_card_for_tombala, char characters, int tour)
    {
        int marks = 0;
        
        for (int i = 0; i < 2; i++)
        {
            int currentIndex = 0;
            for (int j = 0; j < 4; j++)
            {
                // If the current cell contains the character to be removed
                if (player_card_for_tombala[i][j] == characters)
                {
                    switch (characters)
                    {
                        case 'A':
                            marks = (30 - tour + 1) * 3;
                            break;
                        case 'E':
                            marks = (30 - tour + 1) * 3;
                            break;
                        case 'I':
                            marks = (30 - tour + 1) * 3;
                            break;
                        case 'O':
                            marks = (30 - tour + 1) * 3;
                            break;
                        case 'U':
                            marks = (30 - tour + 1) * 3;
                            break;
                        default:
                            marks = (30 - tour + 1) * 2;
                            break;
                    }
                    player_card_for_tombala[i][j] = ' ';
                }
                else if (player_card_for_tombala[i][j] != ' ')
                    // Move the character to the next available space in the row
                    player_card_for_tombala[i][currentIndex++] = player_card_for_tombala[i][j];
            }
            for (int j = currentIndex; j < 4; j++)
                player_card_for_tombala[i][j] = ' ';
        }
        return marks;
    }

    //Displays the players' tombala cards and their current scores.
    static void PrintPlayerCardsAndScores(char[][] player1Card, char[][] player2Card, int player1Score, int player2Score)
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == 0) {
                Console.Write("Player1: ");
                for (int j = 0; j < 4; j++)
                    Console.Write(player1Card[i][j] + " ");
            }
            if (i == 1){
                Console.Write("         ");
                for (int j = 0; j < 4; j++)
                   Console.Write(player1Card[i][j] + " ");
            }

            Console.Write("    ");

            if (i == 0){
                Console.Write("Player2: ");
                for (int j = 0; j < 4; j++)
                   Console.Write(player2Card[i][j] + " ");
            }
            if (i == 1){
                Console.Write("         ");
                for (int j = 0; j < 4; j++)
                   Console.Write(player2Card[i][j] + " ");
            }

            if (i == 0){
                Console.Write("    ");
                Console.Write("Player 1 score: " + player1Score);
            }
            else if (i == 1){
                Console.Write("    ");
                Console.Write("Player 2 score: " + player2Score);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    //removes the highest ASCII value letter from the player's card and returns the character that was removed.
    static char TakeOutHighestASCIILetterFromCard(char[][] player_card_for_tombala, int tour)
    {
        char ch1 = ' ';
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (player_card_for_tombala[i][j] > ch1){
                    if (player_card_for_tombala[i][j] != ' ')
                        ch1 = player_card_for_tombala[i][j];// Store the highest ASCII character found
                }
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int currentIndex = 0;
            for (int j = 0; j < 4; j++)
            {
                if (player_card_for_tombala[i][j] == ch1)
                    player_card_for_tombala[i][j] = ' ';
                else if (player_card_for_tombala[i][j] != ' ')
                    player_card_for_tombala[i][currentIndex++] = player_card_for_tombala[i][j];
            }
            for (int j = currentIndex; j < 4; j++)
                player_card_for_tombala[i][j] = ' ';
        }
        return ch1;
    }

    //Finds and returns the character with the lowest ASCII value in the specified row of the card.
    static char FindMinASCIICharacterForGame(char[][] characters, int row)
    {
        char min = characters[row][0];
        for (int i = 1; i < 4; i++)
        {
            if (characters[row][i] < min)
            {
                min = characters[row][i];
            }
        }
        return min;
    }
    //Finds and returns the character with the highest ASCII value in the specified row of the card.
    static char FindMaxASCIICharacterForGame(char[][] characters, int row)
    {
       
        char max = characters[row][0];
        for (int i = 1; i < 4; i++)
        {
            if (characters[row][i] > max){
                max = characters[row][i];
            }
        }
        return max;
    }
}
