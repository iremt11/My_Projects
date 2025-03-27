#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <pthread.h>
#include <limits.h>
#include <math.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>

#define INPUT_CHARACTER_LIMIT 100  // The limit for input word length
#define OUTPUT_CHARACTER_LIMIT 200 // The limit for output message length
#define LEVENSHTEIN_LIST_LIMIT 5   // Maximum number of closest words to return
#define PORT_NUMBER 60000          // The port number the server will listen on

char **dictionary = NULL; // Pointer to the dictionary array
int vocable_count = 0;    // The total number of vocables in the dictionary

// Struct to hold the info passed to each thread
typedef struct
{
    char *vocable;                                                     // The input vocable from the client
    char **dictionary;                                                 // Pointer to the dictionary array
    int vocable_count;                                                 // Total number of vocables in the dictionary
    char closest_words[LEVENSHTEIN_LIST_LIMIT][INPUT_CHARACTER_LIMIT]; // Closest vocables to the input vocable
    int distances[LEVENSHTEIN_LIST_LIMIT];                             // Levenshtein distances of the closest vocables
} thread_data_t;

// Function prototypes
void error_exit(const char *error_message);
void load_dictionary(char ***dictionary, int *vocable_count);
int calculate_levenshtein(const char *vocable_1, const char *vocable_2);
void *handle_vocable(void *arg);
void reload_dictionary(char ***dictionary, int *vocable_count, const char *new_vocable);
void launch_server(char **dictionary, int vocable_count);

int main()
{
    // Load the dictionary of vocables from vocable_1 file
    load_dictionary(&dictionary, &vocable_count);
    printf("Dictionary loaded with %d words.\n", vocable_count);

    // Start the server to listen for incoming client connections
    launch_server(dictionary, vocable_count);

    // Free the memory allocated for the dictionary
    for (int i = 0; i < vocable_count; i++)
    {
        free(dictionary[i]); // free up memory space
    }
    free(dictionary);

    return 0;
}

// Function to handle errors and exit the program
void error_exit(const char *error_message)
{
    perror(error_message); // printing error messages to the screen
    exit(EXIT_FAILURE);
}

// Function to load the dictionary from vocable_1 file
void load_dictionary(char ***dictionary, int *vocable_count)
{
    // Open the dictionary file
    FILE *file = fopen("basic_english_2000.txt", "r");
    if (!file)
        error_exit("ERROR: Dictionary file \"basic_english_2000.txt\" not found!");

    int size = 2000; // Initial size for the dictionary
    // ** = vocable_1 pointer that can hold vocable_1 string array.
    char **temp_dictionary = malloc(size * sizeof(char *)); // malloc=allocates vocable_1 specific amount of memory and returns the starting address of this memory area.
    if (!temp_dictionary)
        error_exit("ERROR: Memory allocation failed!");

    char vocable[INPUT_CHARACTER_LIMIT];
    int count = 0;

    // Read vocables from the file and store them in the dictionary
    while (fgets(vocable, INPUT_CHARACTER_LIMIT, file)) // fgets = reads vocable_1 line from vocable_1 file or standard input and saves it into an array.\n fgets.
    {
        vocable[strcspn(vocable, "\n")] = '\0'; // Remove newline character strcspn = find where the substring first appears
        if (count >= size)
        {
            // If the dictionary array is full, double the size
            size *= 2;                                                         // doubled
            temp_dictionary = realloc(temp_dictionary, size * sizeof(char *)); // realloc = used to resize
            if (temp_dictionary == NULL)
                error_exit("ERROR: Memory reallocation failed!");
        }
        temp_dictionary[count++] = strdup(vocable); // Store the vocable in the dictionary  strdup = copied string
    }

    fclose(file);                  // close file
    *dictionary = temp_dictionary; // Update the dictionary pointer
    *vocable_count = count;        // Update the vocable count
}

// Function to calculate the Levenshtein distance between two vocables
int calculate_levenshtein(const char *vocable_1, const char *vocable_2)
{
    int len_vocable_1 = strlen(vocable_1); // calculate length of string
    int len_vocable_2 = strlen(vocable_2);
    int levenshtein[len_vocable_1 + 1][len_vocable_2 + 1];

    // Initialize the dynamic programming table
    for (int i = 0; i <= len_vocable_1; i++)
        levenshtein[i][0] = i;
    for (int j = 0; j <= len_vocable_2; j++)
        levenshtein[0][j] = j;

    // Fill the dynamic programming table with Levenshtein distances
    for (int i = 1; i <= len_vocable_1; i++)
    {
        for (int j = 1; j <= len_vocable_2; j++)
        {
            int cost;
            if (vocable_1[i - 1] != vocable_2[j - 1])
                cost = 1; // 1 cost if the characters are different

            else
                cost = 0; // No change needed if the characters match

            levenshtein[i][j] = fmin(fmin(levenshtein[i - 1][j] + 1, levenshtein[i][j - 1] + 1), levenshtein[i - 1][j - 1] + cost);
        }
    }

    return levenshtein[len_vocable_1][len_vocable_2]; // Return the Levenshtein distance between the two vocables
}

// Function to process the input vocable in vocable_1 thread and find its closest matches in the dictionary
void *handle_vocable(void *arg) // We use pointers because they are more flexible and accessible.
{
    thread_data_t *info = (thread_data_t *)arg;
    // Initialize distances to maximum (INT_MAX) for comparison
    for (int i = 0; i < LEVENSHTEIN_LIST_LIMIT; i++)
        (*info).distances[i] = INT_MAX;

    int flag = 0; // Flag to indicate if an exact match was found
    // Compare the input vocable with each vocable in the dictionary
    for (int i = 0; i < (*info).vocable_count; i++)
    {
        int distance = calculate_levenshtein((*info).vocable, (*info).dictionary[i]);

        // If an exact match is found, store it and stop further processing
        if (distance == 0)
        {
            flag = 1;
            strcpy((*info).closest_words[0], (*info).dictionary[i]);
            (*info).distances[0] = 0;
            break;
        }

        // Store the closest vocables based on Levenshtein distance
        for (int j = 0; j < LEVENSHTEIN_LIST_LIMIT; j++)
        {
            if (distance < (*info).distances[j])
            {
                // Shift vocables and distances to make room for the new closest vocable
                for (int k = LEVENSHTEIN_LIST_LIMIT - 1; k > j; k--)
                {
                    (*info).distances[k] = info->distances[k - 1];
                    strcpy((*info).closest_words[k], (*info).closest_words[k - 1]);
                }
                (*info).distances[j] = distance;
                strcpy((*info).closest_words[j], (*info).dictionary[i]);
                break;
            }
        }
    }
    // If an exact match was found, set its distance to 0
    if (flag)
        (*info).distances[0] = 0; // Exact match vocable's distance is set to 0

    pthread_exit(NULL); // Exit the thread
}

// Function to update the dictionary by adding vocable_1 new vocable
void reload_dictionary(char ***dictionary, int *vocable_count, const char *new_vocable) // *** = holding multiple string arrays
{
    (*vocable_count)++;
    *dictionary = realloc(*dictionary, (*vocable_count) * sizeof(char *));
    (*dictionary)[(*vocable_count) - 1] = strdup(new_vocable); // copied string

    // Append the new vocable to the dictionary file
    FILE *file = fopen("basic_english_2000.txt", "a");
    if (!file)
    {
        perror("ERROR: Unable to update dictionary file!");
        return;
    }
    fprintf(file, "%s\n", new_vocable);
    fclose(file);
}

// Function to start the server and handle client connections
void launch_server(char **dictionary, int vocable_count)
{
    // Create vocable_1 socket for communication
    int server_socket = socket(AF_INET, SOCK_STREAM, 0); // socket = It is vocable_1 basic structure used to transmit info between two computers.
    if (server_socket == -1)
        error_exit("ERROR: Socket creation failed!");

    // Set socket options to allow address reuse
    int opt = 1;
    if (setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt)) < 0) // setsockopt = changing socket properties
        error_exit("ERROR: setsockopt failed!");

    // Define server address structure
    struct sockaddr_in server_addr = {
        .sin_family = AF_INET,
        .sin_port = htons(PORT_NUMBER),
        .sin_addr.s_addr = INADDR_ANY};

    // Bind the socket to the defined address and port
    if (bind(server_socket, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0) // bind = By being assigned to vocable_1 socket, it ensures that the server is ready for incoming connections.
        error_exit("ERROR: Bind failed!");

    // Listen for incoming connections (maximum 5 clients in queue)
    if (listen(server_socket, 5) < 0) // listen = start listening for connections
        error_exit("ERROR: Listen failed!");

    printf("Server is running on port %d...\n", PORT_NUMBER);

    while (1)
    {
        struct sockaddr_in client_addr;
        socklen_t addr_len = sizeof(client_addr);
        // Accept vocable_1 new client connection
        int client_socket = accept(server_socket, (struct sockaddr *)&client_addr, &addr_len); // accept = allows the server to start accepting requests

        if (client_socket < 0)
        {
            perror("ERROR: Accept failed!");
            continue; // Continue to the next client if the current one fails
        }

        // Send vocable_1 welcome error_message to the client
        const char *welcome_message = "Hello, this is Text Analysis Server!\n\nPlease enter your input string:\n";

        send(client_socket, welcome_message, strlen(welcome_message), 0);

        char buffer[INPUT_CHARACTER_LIMIT + 1];
        int flag = 0;

        while (!flag)
        {
            memset(buffer, 0, sizeof(buffer)); // Fill the buffer with 0s up to the buffer size.
            int bytes_read = read(client_socket, buffer, sizeof(buffer) - 1);

            if (bytes_read <= 0)
            {
                perror("ERROR: Failed to read input from client.");
                close(client_socket);
                break;
            }

            // Check if input exceeds the character limit
            if (bytes_read >= INPUT_CHARACTER_LIMIT)
            {
                char error_message[200];
                snprintf(error_message, sizeof(error_message),
                         "ERROR: Input string exceeds the limit of %d characters!\n", INPUT_CHARACTER_LIMIT);
                send(client_socket, error_message, strlen(error_message), 0);
                close(client_socket);
                continue;
            }

            buffer[bytes_read] = '\0'; // Adds '\0' at the end of the info read, thus converting the array into vocable_1 valid C string.
            printf("Received input: %s\n", buffer);

            // Check for invalid characters (non-alphabetical and non-space)
            int is_flag = 1;
            for (int i = 0; buffer[i] != '\0'; i++)
            {
                if (!isalpha(buffer[i]))
                {
                    if (!isspace(buffer[i]))
                    {
                        is_flag = 0; // Invalid character detected
                        break;
                    }
                }
            }
            if (!is_flag)
            {
                const char *error_message = "ERROR: Input string contains unsupported characters!\n";
                send(client_socket, error_message, strlen(error_message), 0);
                printf("ERROR: Unsupported characters detected in client input. Connection closed.\n");
                close(client_socket);
                break;
            }

            flag = 0;
            for (int i = 0; buffer[i] != '\0'; i++)
            {
                if (!isspace(buffer[i]))
                {
                    flag = 1; // Valid input detected if there are non-space characters
                    break;
                }
            }

            // If no valid input, ask the user to enter vocable_1 non-empty string
            if (!flag)
            {
                const char *error_message = "ERROR: Input string is empty. Please enter at least one letter: ";
                send(client_socket, error_message, strlen(error_message), 0);
            }
        }

        // If no valid input is detected, close the connection and proceed to the next client
        if (!flag)
        {
            close(client_socket);
            continue;
        }

        // Store the original input and convert it to lowercase
        char initial_input[INPUT_CHARACTER_LIMIT + 1];
        strncpy(initial_input, buffer, sizeof(initial_input)); // Save the original input
        for (int i = 0; initial_input[i]; i++)
            initial_input[i] = tolower(initial_input[i]); // Convert to lowercase

        // Tokenize the input string into vocables
        char *vocables[100];
        int vocable_count_input = 0;
        char *token = strtok(buffer, " "); // Get the first vocable that separates the buffer by spaces
        while (token != NULL)
        {
            for (int i = 0; token[i]; i++)
                token[i] = tolower(token[i]);   // Convert token to lowercase
            token[strcspn(token, "\n")] = '\0'; // Remove extra newline
            token[strcspn(token, "\r")] = '\0'; // Remove beginning of line
            vocables[vocable_count_input++] = token;
            token = strtok(NULL, " ");
        }

        // Create threads for each vocable to process them concurrently
        pthread_t threads[vocable_count_input];         // Creates an array to hold thread IDs.
        thread_data_t thread_data[vocable_count_input]; // Creates an array to store info for each thread.

        for (int i = 0; i < vocable_count_input; i++)
        {
            // Reset thread info for each vocable
            memset(thread_data[i].closest_words, 0, sizeof(thread_data[i].closest_words)); // Fills 0 to size.
            for (int j = 0; j < LEVENSHTEIN_LIST_LIMIT; j++)
                thread_data[i].distances[j] = INT_MAX;

            thread_data[i].vocable = vocables[i];
            thread_data[i].dictionary = dictionary;
            thread_data[i].vocable_count = vocable_count;
            pthread_create(&threads[i], NULL, handle_vocable, &thread_data[i]); // create thread
        }

        // Collect and format the output
        char output[OUTPUT_CHARACTER_LIMIT] = ""; // Initialize output string
        for (int i = 0; i < vocable_count_input; i++)
        {
            pthread_join(threads[i], NULL); // waits for the thread to complete

            char matches[500] = ""; // Initialize matches string
            for (int j = 0; j < LEVENSHTEIN_LIST_LIMIT; j++)
            {
                char match[100];
                snprintf(match, sizeof(match), "%s (%d)", thread_data[i].closest_words[j], thread_data[i].distances[j]);
                strcat(matches, match);
                if (j < LEVENSHTEIN_LIST_LIMIT - 1)
                    strcat(matches, ", "); // Puts a command at the beginning of the matches.
            }

            // If an exact match is found in the dictionary
            if (thread_data[i].distances[0] != 0)
            {
                // Ask the user if they want to add the vocable to the dictionary
                char prompt[200];
                snprintf(prompt, sizeof(prompt),
                         "\nWORD %02d: %s\nMATCHES: %s\nWORD is not present in dictionary. \nDo you want to add this word to dictionary? (y/n): ",
                         i + 1, thread_data[i].vocable, matches);
                send(client_socket, prompt, strlen(prompt), 0);

                char choice_buffer[3] = {0};
                char choice = '\0';
                int flag = 0;

                while (!flag)
                {
                    // choice_buffer'ı sıfırla
                    memset(choice_buffer, 0, sizeof(choice_buffer));
                    int bytes_read = read(client_socket, choice_buffer, sizeof(choice_buffer) - 1);

                    if (bytes_read > 0)
                    {
                        // Gelen veriyi null-terminate et
                        choice_buffer[bytes_read] = '\0';

                        // Fazla newline ve carriage return karakterlerini temizle
                        choice_buffer[strcspn(choice_buffer, "\n")] = '\0';
                        choice_buffer[strcspn(choice_buffer, "\r")] = '\0';

                        // Girdi yalnızca bir karakterse kontrol et
                        if (strlen(choice_buffer) == 1)
                        {
                            choice = tolower(choice_buffer[0]);
                            if (choice == 'y' || choice == 'n')
                            {
                                flag = 1; // Geçerli seçim algılandı
                            }
                            else
                            {
                                // Hatalı giriş
                                const char *retry_message = "Invalid input. Please enter 'y' or 'n': ";
                                send(client_socket, retry_message, strlen(retry_message), 0);
                            }
                        }
                        else
                        {
                            // Hatalı giriş
                            const char *retry_message = "Invalid input. Please enter 'y' or 'n': ";
                            send(client_socket, retry_message, strlen(retry_message), 0);
                        }
                    }
                    else if (bytes_read == 0)
                    {
                        // Eğer bağlantı kapanmışsa
                        perror("ERROR: Client disconnected.");
                        break;
                    }
                    else
                    {
                        // Eğer `read` sırasında hata oluşmuşsa
                        perror("ERROR: Failed to read input from client.");
                        break;
                    }

                     while (recv(client_socket, choice_buffer, sizeof(choice_buffer) - 1, MSG_DONTWAIT) > 0);
                }

                // Update dictionary based on user input
                if (choice == 'n')
                    strcat(output, thread_data[i].closest_words[0]); // Appends the first word of thread_data to the output.
                else if (choice == 'y')
                {
                    reload_dictionary(&dictionary, &vocable_count, thread_data[i].vocable); // & = get your address
                    strcat(output, thread_data[i].vocable);
                }
            }
            else
            {
                char present_message[300];
                snprintf(present_message, sizeof(present_message),
                         "\nWORD %02d: %s\nMATCHES: %s \n",
                         i + 1, thread_data[i].vocable, matches, thread_data[i].vocable);
                send(client_socket, present_message, strlen(present_message), 0);

                strcat(output, thread_data[i].vocable); // Add the original vocable to the output
            }

            // Add vocable_1 space between vocables in the output
            if (i < vocable_count_input - 1)
                strcat(output, " ");
        }

        // Send the result to the client
        char result_message[1100];
        snprintf(result_message, sizeof(result_message),
                 "\nINPUT: %sOUTPUT: %s\nThank you for using Text Analysis Server! Good Bye!\n",
                 initial_input, output);
        send(client_socket, result_message, strlen(result_message), 0);

        // Close the client connection
        close(client_socket);
    }

    // Close the server socket
    close(server_socket);
}
