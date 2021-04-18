using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan
{
    //***********************************************************
    // Title: Hangman
    // Description: An interactive, text-based version of Hangman
    // Author: Olszewski, James
    // Date Created: 4/11/2021
    // Last Modified: 4/18/2021
    //***********************************************************
    enum Letters
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }
        /// <summary>
        /// *****************************************************************
        /// *                       Menu Screen                             *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            (int toLose, bool letterMemory) ruleSet;
            ruleSet.toLose = 5;
            ruleSet.letterMemory = true;

            List<Letters[]> dictionary = new List<Letters[]>()
            {
                new Letters[] {Letters.C, Letters.L, Letters.O, Letters.U, Letters.D },
                new Letters[] {Letters.T, Letters.A, Letters.B, Letters.L, Letters.E },
                new Letters[] {Letters.C, Letters.O, Letters.N, Letters.C, Letters.H },
                new Letters[] {Letters.Z, Letters.E, Letters.B, Letters.R, Letters.A },
                new Letters[] {Letters.B, Letters.E, Letters.L, Letters.T, Letters.S }
            };

            do
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Modify Hangman Dictionary");
                Console.WriteLine("\tb) Difficulty Settings");
                Console.WriteLine("\tc) Play Hangman");
                Console.WriteLine("\td) Instructions & Credits");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice: ");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayModifyDictionary(dictionary);
                        break;

                    case "b":
                        ruleSet = DisplayDifficultySettings();
                        break;

                    case "c":
                        DisplayGame(ruleSet, dictionary);
                        break;

                    case "d":
                        DisplayCredits();
                        break;

                    case "q":

                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region Modify Dictionary
        /// <summary>
        /// *****************************************************************
        /// *                  Modify Dictionary Screen                     *
        /// *****************************************************************
        /// </summary>
        static void DisplayModifyDictionary(List<Letters[]> dictionary)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            DisplayScreenHeader("Modify Dictionary");

            int displayCount = 1;
            string userinput;
            bool valid = false;
            bool exit = false;
            Letters[] newWord = {Letters.A, Letters.A, Letters.A, Letters.A, Letters.A };

            foreach (Letters[] word in dictionary)
            {
                Console.Write(displayCount + ". ");
                foreach(Letters ltr in word)
                {
                    Console.Write(ltr);
                }
                Console.WriteLine();
                displayCount += 1;
            }

            Console.Write("Would you like to add or remove from the dictionary? (type anything else to leave)");
            userinput = Console.ReadLine();

            if (userinput == "add")
            {
                do
                {

                    Console.WriteLine("\tPlease enter one letter at a time");
                    for (int index = 0; index < 5; ++index)
                    {
                        do
                        {
                            Console.Write("\tEnter Letter: ");
                            userinput = Console.ReadLine();
                            if (Enum.TryParse<Letters>(userinput.ToUpper(), out Letters letter))
                            {
                                newWord[index] = letter;
                                valid = true;
                            }
                            else
                            {
                                Console.Write("\tEnter a valid english letter");
                            }
                        } while (!valid);
                        valid = false;
                    }
                    foreach (Letters ltr in newWord)
                    {
                        Console.WriteLine("\t" + ltr);
                    }
                    
                    do
                    {
                        Console.Write("\tAre you sure you want to add this word to the dictionary (y/n)? ");
                        userinput = Console.ReadLine();
                        if (userinput == "y" || userinput == "yes")
                        {
                            dictionary.Add(newWord);
                            exit = true;
                            valid = true;
                        }
                        else if (userinput == "n" || userinput == "no")
                        {
                            valid = true;
                        }
                        else
                        {
                            Console.WriteLine("\tPlease answer with y/yes or n/no");
                        }
                    } while (!valid);

                } while (!exit);
            } 
            else if (userinput == "remove")
            {
                do
                {
                    Console.WriteLine("\tSelect a word to remove (by number): ");
                    userinput = Console.ReadLine();
                    Console.WriteLine(dictionary.Count);
                    if (int.TryParse(userinput, out int number))
                    {
                        if (int.Parse(userinput) > dictionary.Count || int.Parse(userinput) < 1)
                        {
                            Console.WriteLine("\tPlease choose a number from the list above");
                        }
                        else
                        {
                            do
                            {
                                Console.WriteLine("\tAre you sure you want to remove this word (y/n)? (You can always add it back) ");
                                userinput = Console.ReadLine();
                                if (userinput == "y" || userinput == "yes")
                                {
                                    dictionary.RemoveAt(number - 1);
                                    valid = true;
                                    exit = true;
                                }
                                else if (userinput == "n" || userinput == "no")
                                {
                                    valid = true;
                                }
                                else
                                {
                                    Console.WriteLine("\tPlease answer with y/yes or n/no");
                                }
                            } while (!valid);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\tPlease choose a valid number");
                    }
                } while (!exit);
            }
            else

            DisplayContinuePrompt();
        }
        #endregion

        #region Difficulty Settings
        /// <summary>
        /// *****************************************************************
        /// *                 Difficulty Settings Screen                    *
        /// *****************************************************************
        /// </summary>
        static (int toLose, bool letterMemory) DisplayDifficultySettings()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            (int toLose, bool letterMemory) ruleSet;
            ruleSet.toLose = 5;
            ruleSet.letterMemory = true;
            string letterMemory;
            bool valid = false;

            DisplayScreenHeader("Difficulty Settings");
            Console.WriteLine("All values have been reset to default.");
            
            do
            {
                Console.Write("\tHow many incorrect guesses would you like to have before you lose? ");
                if (int.TryParse(Console.ReadLine(), out int toLose))
                {
                    if (toLose < 0)
                    {
                        Console.WriteLine("\tPlease enter an integer greater than 0");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"\tYou have entered: {toLose} as the number of incorrect guesses.");
                        ruleSet.toLose = toLose;
                        valid = true;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter an integer for the number of incorrect guesses.");
                    Program.DisplayContinuePrompt();
                }

            } while (valid == false);

            Console.WriteLine();
            Console.WriteLine("\tWould you like the game to keep track of the letters you've chosen? (Y/N) ");
            Console.Write("\t(WARNING! If this is set to No, The game will not stop you from picking duplicate letters!): ");
            letterMemory = Console.ReadLine();
            if (letterMemory.ToUpper() == "YES" || letterMemory.ToUpper() == "Y")
            {
                Console.WriteLine();
                Console.WriteLine($"\tLetter Memory will occur.");
                ruleSet.letterMemory = true;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"\tLetter Memory will not occur.");
                ruleSet.letterMemory = false;
            }

            DisplayContinuePrompt();

            return ruleSet;
        }
        #endregion

        #region Game
        /// <summary>
        /// *****************************************************************
        /// *                        Game Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayGame((int toLose, bool letterMemory) ruleSet, List<Letters[]> dictionary)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            var random = new Random();
            int toLose = ruleSet.toLose;
            int count = 0;
            string letterChoice = "A";
            bool letterMemory = ruleSet.letterMemory;
            bool clearCondition = false;

            string[] letterFill = { "_", "_", "_", "_", "_" };
            List<Letters> memory = new List<Letters>(Enum.GetValues(typeof(Letters)).Cast<Letters>());
            List<Letters> word = new List<Letters>();

            foreach (Letters wordLetter in dictionary[random.Next(dictionary.Count)])
            {
                word.Add(wordLetter);
            }

            do
            {
                DisplayScreenHeader("Save Mr. Hangmin!");

                DisplayHangman(toLose);
                DisplayLetterMemory(letterMemory, letterChoice, memory);
          
                foreach (string fillLetter in letterFill)
                {
                    Console.Write(" " + fillLetter);
                }
            
                Console.Write(" Please input a new letter: ");
                letterChoice = Console.ReadLine().ToUpper();
                if (Enum.TryParse<Letters>(letterChoice, out Letters letter))
                {
                    if (!memory.Contains(letter) && letterMemory)
                    {
                        Console.WriteLine("You have already chosen that letter, Please choose a new one");
                    }
                    else
                    {
                        if (!word.Contains(letter))
                        {
                            Console.WriteLine("I'm sorry, The secret word does not contain that letter");
                            toLose = toLose - 1;
                        }
                        else
                        {
                            Console.WriteLine("The secret word does contain that letter");
                            foreach (Letters wordLetter in word)
                            {
                                if(wordLetter.ToString() == letterChoice)
                                {
                                    letterFill[count] = letterChoice;
                                }
                                count += 1;
                            }
                        }
                        count = 0;
                        memory.Remove(letter);
                    }
                }
                else
                {
                    Console.WriteLine("Please choose an english letter.");
                }
                if (toLose == 0 || !letterFill.Contains("_"))
                {
                    clearCondition = true;
                }
                DisplayContinuePrompt();
            } while (!clearCondition);

            if (toLose == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                DisplayScreenHeader("Your Ultimate Fate");
                DisplayHangman(toLose);
                Console.WriteLine("You weren't fast enough to save Mr. Hangmin. (LOSE)");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                DisplayScreenHeader("Your Ultimate Fate");
                DisplayHangman(-1);
                Console.WriteLine("You have successfully saved Mr. Hangmin. (WIN)");
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                      Display LetterMemory                     *
        /// *****************************************************************
        /// </summary>
        private static void DisplayLetterMemory(bool letterMemory, string letterChoice, List<Letters> memory)
        {
            if (letterMemory)
            {
                foreach (Letters letter in memory)
                {
                    Console.Write(letter + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// *****************************************************************
        /// *                        Display Hangman                        *
        /// *****************************************************************
        /// </summary>
        static void DisplayHangman(int toLose)
        {
            if (toLose >= 0)
            {
                if (toLose <= 4)
                {
                    Console.WriteLine("  _____");
                }
                else
                {
                    Console.WriteLine();
                }
                if (toLose <= 3)
                {
                    Console.WriteLine("  |  | ");
                }
                else
                {
                    Console.WriteLine();
                }
                if (toLose <= 2)
                {
                    Console.WriteLine("  |  O ");
                }
                else
                {
                    Console.WriteLine();
                }
                if (toLose <= 1)
                {
                    Console.WriteLine("  |  T ");
                }
                else
                {
                    Console.WriteLine();
                }
                if (toLose <= 0)
                {
                    Console.WriteLine("  |__^_");
                    Console.WriteLine("  |   |");
                }
                else
                {
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("       ");
                Console.WriteLine("    O  ");
                Console.WriteLine("    T  ");
                Console.WriteLine("  __^__");
                Console.WriteLine("  |   |");
            }
        }
        #endregion

        #region Credits
        /// <summary>
        /// *****************************************************************
        /// *                        Credits Screen                         *
        /// *****************************************************************
        /// </summary>
        static void DisplayCredits()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DisplayScreenHeader("Credits");

            DisplayHangman(-1);

            Console.WriteLine("\tTo play Hangman, try to guess the 5 letter word to save Mr. Hangmin.");
            Console.WriteLine("\tGuess one letter at a time. If you get too many guesses wrong, you lose.");
            Console.WriteLine("\tYou win if you can guess the word with as few guesses as you can.");
            Console.WriteLine("\t(If you are still unclear, the application will guide you through it.");

            Console.WriteLine("\tUse Modify Hangman Dictionary to see, add, and delete words the game will have you guess.");
            Console.WriteLine("\tUse Difficulty Settings to make the game easier or harder.");
            Console.WriteLine("\t(letter memory means the wordbank-type dealeo you that shows you what letters you can use)");
            Console.WriteLine();

            Console.WriteLine("\tAll code was written by James Olszewski.");
            Console.WriteLine("\tW3schools and John Velis's work were used as references.");

            DisplayContinuePrompt();
        }
        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tHangman O|<");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for playing!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
