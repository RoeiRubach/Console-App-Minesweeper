﻿using System;
using System.Threading;

namespace ConsoleAppMinesweeper
{
    public struct Cell
    {
        public bool isHidden;
        public bool isMarked;
        public char cellValue;
        public char markValue;
        public int minesAround;
    }

    class Program
    {
        public const char frameValue = '^';
        public const char emptyCellValue = '~';
        public const char mineValue = '*';
        public const char markValue = '!';
        public static int iLeft;
        public static int iHidden;

        static void Main(string[] args)
        {
            string userInput;
            int rows, columns, mines;
            bool isGameOver = false;

            while (!isGameOver)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                DifficultyLevel(out userInput);
                if (userInput == "Expert")
                {
                    Console.Clear();
                    Console.SetCursorPosition(24, 3);
                    Console.WriteLine(userInput + "? Wow you're brave!");
                    Console.SetCursorPosition(29, 5);
                    Console.WriteLine("Anyway - Good luck!");
                    Console.SetCursorPosition(38, 6);
                    Console.Write("---------");
                }
                else
                {
                    Console.Clear();
                    Console.SetCursorPosition(31, 3);
                    Console.WriteLine(userInput + " it is!");
                    Console.SetCursorPosition(31, 4);
                    Console.Write("-------------");
                }
                GetMeasures(out rows, out columns, out mines, userInput);
                Cell[,] game2DArray = new Cell[rows, columns];

                InitializeMineSweeper(game2DArray, mines);
                LoadingScreen();
                Console.ForegroundColor = ConsoleColor.White;
                CordsForILeft(game2DArray, userInput);
                Console.Clear();

                Print(game2DArray);
                MovingAlongTheArray(game2DArray, mines);
                isGameOver = isAnotherGame();
            }
        }

        /// <summary>
        /// Prints the game instructions.
        /// </summary>
        static void GameInstructors()
        {
            bool loop = false;
            while (!loop)
            {
                Console.SetCursorPosition(25, 1);
                Console.Write("Welcome to my Minesweeper!");
                Console.SetCursorPosition(25, 2);
                Console.Write("-------------------------");
                Console.SetCursorPosition(29, 4);
                Console.Write("Game Instructions:");
                Console.SetCursorPosition(29, 5);
                Console.Write("-----------------");
                Console.SetCursorPosition(14, 7);
                Console.Write("Use the -  Enter button  - to choose a cell/location.");
                Console.SetCursorPosition(14, 8);
                Console.Write("Use the - Insert button  - to place a flag.");
                Console.SetCursorPosition(14, 9);
                Console.Write("Use the - Delete button  - to remove the flag.");
                Console.SetCursorPosition(14, 10);
                Console.Write("Use the - arrows buttons - to move around the field.");
                Console.SetCursorPosition(9, 11);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Above the cursor's position is where will you be clicking on.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(27, 12);
                Console.Write("Write OK to continue - ");
                string sInput = Console.ReadLine();
                switch (sInput)
                {
                    case "OK":
                    case "ok":
                    case "oK":
                    case "Ok":
                    case "K":
                    case "k":
                        loop = true;
                        break;

                    default:
                        Console.SetCursorPosition(49, 12);
                        Console.WriteLine("\t\t\t\t\t\t");
                        break;
                }
            }

        }

        /// <summary>
        /// Prints all the different levels and asks the user to write the difficulty that he wants to play.
        /// </summary>
        /// <param name="userInput">The variable that gets the user's input.</param>
        static void DifficultyLevel(out string userInput)
        {
            int check = 0;
            GameInstructors();
            do
            {
                Console.Clear();
                Console.SetCursorPosition(15, 5);
                Console.Write("Choose difficulty by writing it and press - Enter.\n               Beginner, Amateur, Expert or Customized: ");
                userInput = Console.ReadLine();

                //The variable get set by the user's input.
                switch (userInput)
                {
                    case "Beginner":
                    case "beginner":
                    case "Begin":
                    case "begin":
                    case "B":
                    case "b":
                        check = 1;
                        userInput = "Beginner";
                        break;
                    case "Amateur":
                    case "amateur":
                    case "Amatu":
                    case "amatu":
                    case "A":
                    case "a":
                        check = 1;
                        userInput = "Amateur";
                        break;
                    case "Expert":
                    case "expert":
                    case "Exp":
                    case "exp":
                    case "E":
                    case "e":
                        check = 1;
                        userInput = "Expert";
                        break;
                    case "Customized":
                    case "customized":
                    case "Custom":
                    case "custom":
                    case "C":
                    case "c":
                        check = 1;
                        userInput = "Customized";
                        break;
                    default:
                        Console.SetCursorPosition(21, 3);
                        Console.WriteLine("Invalid content, please try again.");
                        Thread.Sleep(2000);
                        Console.SetCursorPosition(21, 3);
                        Console.WriteLine("\t\t\t\t\t");
                        Console.Clear();
                        break;
                }
            } while (check != 1);
        }

        /// <summary>
        /// Gets the user's difficulty input and converting it to the game's measures - borders and the number of mines.
        /// </summary>
        /// <param name="rowNum">Keeps the row number that was set.</param>
        /// <param name="colNum">Keeps the column number that was set.</param>
        /// <param name="mineNum">Keeps the mines number that was set.</param>
        /// <param name="userInput">Uses the user's input.</param>
        static void GetMeasures(out int rowNum, out int colNum, out int mineNum, string userInput)
        {
            if (userInput == "Beginner")
            {
                rowNum = 11;
                colNum = 11;
                mineNum = 10;
            }
            else if (userInput == "Amateur")
            {
                rowNum = 18;
                colNum = 18;
                mineNum = 40;
            }
            else if (userInput == "Expert")
            {
                rowNum = 18;
                colNum = 32;
                mineNum = 99;
            }

            else
            {
                do
                {
                    Console.Clear();
                    Console.SetCursorPosition(31, 3);
                    Console.Write(userInput + " it is!");
                    Console.SetCursorPosition(31, 4);
                    Console.Write("----------------");
                    Console.SetCursorPosition(29, 6);
                    Console.Write("Enter the game width: ");
                } while (!int.TryParse(Console.ReadLine(), out rowNum));
                while (rowNum >= 41 || rowNum <= 5)
                {
                    if (rowNum >= 41)
                    {
                        do
                        {
                            Console.Clear();
                            Console.SetCursorPosition(10, 4);
                            Console.Write("The width number you've entered is greater than the field.");
                            Console.SetCursorPosition(12, 6);
                            Console.Write("Please enter a new width number, which is 40 or less: ");
                        } while (!int.TryParse(Console.ReadLine(), out rowNum));
                    }
                    else if (rowNum <= 5)
                    {
                        do
                        {
                            Console.Clear();
                            Console.SetCursorPosition(12, 4);
                            Console.Write("The width number you've entered can't create a field.");
                            Console.SetCursorPosition(10, 6);
                            Console.Write("Please enter a bigger width number, which is 6 or higher: ");
                        } while (!int.TryParse(Console.ReadLine(), out rowNum));
                    }
                }
                do
                {
                    Console.Clear();
                    Console.SetCursorPosition(31, 3);
                    Console.Write(userInput + " it is!");
                    Console.SetCursorPosition(31, 4);
                    Console.Write("----------------");
                    Console.SetCursorPosition(29, 6);
                    Console.Write("Enter the game height: ");
                } while (!int.TryParse(Console.ReadLine(), out colNum));
                while (colNum >= 41 || colNum <= 5)
                {
                    if (colNum >= 41)
                    {
                        do
                        {
                            Console.Clear();
                            Console.SetCursorPosition(10, 4);
                            Console.Write("The height number you've entered is greater than the field.");
                            Console.SetCursorPosition(12, 6);
                            Console.Write("Please enter a new height number, which is 40 or less: ");
                        } while (!int.TryParse(Console.ReadLine(), out colNum));
                    }
                    else if (colNum <= 5)
                    {
                        do
                        {
                            Console.Clear();
                            Console.SetCursorPosition(12, 4);
                            Console.Write("The height number you've entered can't create a field.");
                            Console.SetCursorPosition(10, 6);
                            Console.Write("Please enter a bigger height number, which is 6 or higher: ");
                        } while (!int.TryParse(Console.ReadLine(), out colNum));
                    }
                }
                do
                {
                    Console.Clear();
                    Console.SetCursorPosition(11, 5);
                    Console.Write("Enter the number of mines you want in your minesweeper: ");
                } while (!int.TryParse(Console.ReadLine(), out mineNum));
                while (mineNum >= (rowNum * colNum) / 5)
                {
                    do
                    {
                        Console.Clear();
                        Console.SetCursorPosition(9, 4);
                        Console.Write("The number of mines you've entered is greater than the field.");
                        Console.SetCursorPosition(20, 6);
                        Console.Write("Please enter a smaller mine number: ");
                    } while (!int.TryParse(Console.ReadLine(), out mineNum));
                }
            }
        }

        /// <summary>
        /// Initializes the values of the cells, first sets random mines then sets numbers around the mines
        /// and then sets default values to all empty cells.
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        /// <param name="mineCount">The number of mines to create.</param>
        static void InitializeMineSweeper(Cell[,] game2DArray, int mineCount)
        {
            //Initializes 2D array border as frames.
            for (int topAndBottomRow = 0; topAndBottomRow < game2DArray.GetLength(1); topAndBottomRow++)
            {
                game2DArray[0, topAndBottomRow].cellValue = frameValue;
                game2DArray[0, topAndBottomRow].isHidden = false;
                game2DArray[game2DArray.GetLength(0) - 1, topAndBottomRow].cellValue = frameValue;
                game2DArray[game2DArray.GetLength(0) - 1, topAndBottomRow].isHidden = false;
            }
            for (int leftAndRightColumn = 0; leftAndRightColumn < game2DArray.GetLength(0); leftAndRightColumn++)
            {
                game2DArray[leftAndRightColumn, 0].cellValue = frameValue;
                game2DArray[leftAndRightColumn, 0].isHidden = false;
                game2DArray[leftAndRightColumn, game2DArray.GetLength(1) - 1].cellValue = frameValue;
                game2DArray[leftAndRightColumn, game2DArray.GetLength(1) - 1].isHidden = false;
            }

            int mineIndex = 0;
            Random rnd = new Random();


            while (mineIndex < mineCount)
            {
                //Generates new coordinates for the mines on the field.
                int firstRndNum = rnd.Next(1, game2DArray.GetLength(0) - 1);
                int secondRndNum = rnd.Next(1, game2DArray.GetLength(1) - 1);
                if (game2DArray[firstRndNum, secondRndNum].cellValue != mineValue)
                {
                    game2DArray[firstRndNum, secondRndNum].cellValue = mineValue;

                    //Increases the cells value around the current mine.
                    for (int aroundMineI = firstRndNum - 1; aroundMineI <= firstRndNum + 1; aroundMineI++)
                    {
                        for (int aroundMineJ = secondRndNum - 1; aroundMineJ <= secondRndNum + 1; aroundMineJ++)
                        {
                            game2DArray[aroundMineI, aroundMineJ].minesAround++;
                        }
                    }
                    mineIndex++;
                }
            }

            //Gives a value for each cell on the 2D array - excluding the mines!
            //Also set all cell's isHidden as true.
            for (int i = 1; i < game2DArray.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < game2DArray.GetLength(1) - 1; j++)
                {
                    //Checks if the current cell isn't a mine.
                    if (game2DArray[i, j].cellValue != mineValue)
                    {
                        //?: Operator - condition ? first_expression : second_expression;
                        game2DArray[i, j].cellValue = game2DArray[i, j].minesAround == 0 ? emptyCellValue : (char)game2DArray[i, j].minesAround;
                    }
                    game2DArray[i, j].isHidden = true;
                    game2DArray[i, j].isMarked = false;
                }
            }
        }

        /// <summary>
        /// Prints dots with a delay of 0.130 seconds.
        /// </summary>
        static void LoadingScreen()
        {
            int tempToMoveDot = 30;

            for (int i = 0; i <= 12; i++)
            {
                Console.SetCursorPosition(tempToMoveDot, 7);
                Console.Write("  .");
                Thread.Sleep(130);
                tempToMoveDot++;
            }
        }

        /// <summary>
        /// Sets the iLeft according the user's difficulty. For later used by the SetCursorPosition(Left).
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        /// <param name="copyOfInput">The copy of the user's difficulty input.</param>
        static void CordsForILeft(Cell[,] game2DArray, string copyOfInput)
        {
            switch (copyOfInput)
            {
                case "Beginner":
                    iLeft = 29;
                    break;

                case "Amateur":
                    iLeft = 22;
                    break;

                case "Expert":
                    iLeft = 8;
                    break;

                default:
                    if (game2DArray.GetLength(0) >= 34 || game2DArray.GetLength(1) >= 34)
                    {
                        iLeft = 0;
                    }
                    else if (game2DArray.GetLength(0) >= 27 || game2DArray.GetLength(1) >= 27)
                    {
                        iLeft = 7;
                    }
                    else if (game2DArray.GetLength(0) >= 20 || game2DArray.GetLength(1) >= 20)
                    {
                        iLeft = 15;
                    }
                    else if (game2DArray.GetLength(0) >= 13 || game2DArray.GetLength(1) >= 13)
                    {
                        iLeft = 21;
                    }
                    else if (game2DArray.GetLength(0) >= 6 || game2DArray.GetLength(1) >= 6)
                    {
                        iLeft = 31;
                    }
                    break;
            }
        }

        /// <summary>
        /// Uses a nested loop to prints all the unhidden values (The frame).
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        static void Print(Cell[,] game2DArray)
        {
            int iTop = 5, temp;
            for (int i = 0; i < game2DArray.GetLength(0); i++)
            {
                temp = iLeft;
                for (int j = 0; j < game2DArray.GetLength(1); j++)
                {
                    if (!game2DArray[i, j].isHidden)
                    {
                        Console.SetCursorPosition(temp, iTop);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write(game2DArray[i, j].cellValue + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    temp += 2;
                }
                iTop++;
                Console.WriteLine();
            }
        }

        /// <summary>
        /// The logic of the movement around the field and pressed playable buttons.
        /// </summary>
        /// <param name="playableArray">The 2D Array the user is playing on.</param>
        /// <localVariable name="upAndDown">The number for the SetCourserPosition. - Top </localVariable>
        /// <localVariable name="sidesCount">Tracking on which cell the user is currently on. - Rows</localVariable>
        /// <localVariable name="upAndDownCount">Tracking on which cell the user is currently on. - Columns</localVariable>
        /// <localVariable name="tempSides">Saved the last number 'sides' used for the SetCourserPosition. - Left</localVariable>
        /// <localVariable name="tempUpAndDown">Saved the last number 'upAndDown' used for the SetCourserPosition. - Top</localVariable>
        /// <localVariable name="tempForSides">Saved the first number 'sides' used for the SetCourserPosition. - Left</localVariable>
        /// <localVariable name="tempForUpAndDown">Saved the first number 'upAndDown' used for the SetCourserPosition. - Top</localVariable>
        /// <localVariable name="redo">True / false if the player - won / lost - the game.</localVariable>
        static void MovingAlongTheArray(Cell[,] playableArray, int minesCounter)
        {
            int upAndDown = 5, sidesCount = 0, upAndDownCount = 0, tempSides = iLeft + 2, tempUpAndDown = upAndDown + 1,
                tempForSides = iLeft + 2, tempForUpAndDown = upAndDown + 1;
            bool redo = false;

            ConsoleKeyInfo keyInfo;
            do
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t");
                Console.SetCursorPosition(iLeft, upAndDown);
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    //Moving the cursor 1 time to the left.
                    case ConsoleKey.LeftArrow:
                        iLeft -= 2;
                        sidesCount--;
                        //Checks if the user is staying on the field.
                        if (iLeft < 0)
                        {
                            iLeft += 2;
                            sidesCount++;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(iLeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(iLeft, upAndDown);
                        break;

                    //Moving the cursor 1 time to the right.
                    case ConsoleKey.RightArrow:
                        iLeft += 2;
                        sidesCount++;
                        //Checks if the user is staying on the field.
                        if (iLeft > 79)
                        {
                            iLeft -= 2;
                            sidesCount--;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(iLeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(iLeft, upAndDown);
                        break;

                    //Moving the cursor 1 time up.
                    case ConsoleKey.UpArrow:
                        upAndDown--;
                        upAndDownCount--;
                        //Checks if the user is staying on the field.
                        if (upAndDown < 0)
                        {
                            upAndDown++;
                            upAndDownCount++;
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to run away huh?\nYou'll never get out from here.\nWell.. Unless you'll finish my Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(iLeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(iLeft, upAndDown);
                        break;

                    //Moving the cursor 1 time down.
                    case ConsoleKey.DownArrow:
                        upAndDown++;
                        upAndDownCount++;
                        //Checks if the user is staying on the field.
                        if (upAndDown > 50)
                        {
                            upAndDown--;
                            upAndDownCount--;
                            Console.SetCursorPosition(0, 47);
                            Console.Write("There's sharks out there.. Trust me, I'm doing you a favor.\nNow go up there and finish my Minesweeper!");
                            Console.SetCursorPosition(iLeft, upAndDown);
                            break;
                        }
                        Console.SetCursorPosition(iLeft, upAndDown);
                        break;

                    case ConsoleKey.Enter:
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("\t\t\t\t\t\t\t\n\t\t\t\t\t\t\t");
                        Console.SetCursorPosition(iLeft, upAndDown);

                        //Checks if the Enter was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < playableArray.GetLength(0) && sidesCount < playableArray.GetLength(1))
                        {

                            //Checks if the current location is hidden.
                            if (playableArray[upAndDownCount, sidesCount].isHidden)
                            {

                                //Checks if the current location contains exclamation mark.
                                if (playableArray[upAndDownCount, sidesCount].isMarked)
                                {
                                    Console.SetCursorPosition(0, 0);
                                    Console.Write("To clear this exclamation mark press - Delete");
                                    Thread.Sleep(2500);
                                    Console.SetCursorPosition(iLeft, upAndDown);
                                }

                                //Checks if the current location contains a mine.
                                else if (playableArray[upAndDownCount, sidesCount].cellValue == mineValue)
                                {
                                    Console.BackgroundColor = ConsoleColor.Red;
                                    playableArray[upAndDownCount, sidesCount].isHidden = false;
                                    Console.Write(playableArray[upAndDownCount, sidesCount].cellValue);
                                    playableArray[upAndDownCount, sidesCount].cellValue = '0';
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    upAndDown = tempForUpAndDown;

                                    //Prints all the mines on the field. Then waits 2.5 seconds, clears everything and print "Game over".
                                    for (int i = 1; i < playableArray.GetLength(0) - 1; i++)
                                    {
                                        iLeft = tempForSides;
                                        for (int j = 1; j < playableArray.GetLength(1) - 1; j++)
                                        {
                                            if (playableArray[i, j].cellValue == mineValue)
                                            {
                                                Console.SetCursorPosition(iLeft, upAndDown);
                                                (playableArray[i, j].isHidden) = false;
                                                Console.Write(playableArray[i, j].cellValue);
                                            }
                                            iLeft += 2;
                                        }
                                        upAndDown++;
                                    }
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Thread.Sleep(2500);
                                    Console.Clear();
                                    Console.SetCursorPosition(31, 1);
                                    Console.WriteLine("Game over!");
                                    Console.SetCursorPosition(30, 2);
                                    Console.WriteLine("Tough luck..");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    redo = true;
                                }
                                else
                                {
                                    UnlockSlotsIfEmpty(playableArray, upAndDown, tempSides = iLeft, tempUpAndDown = upAndDown, sidesCount, upAndDownCount);
                                    iHidden = 0;

                                    //Counts the number of hidden values.
                                    for (int i = 1; i < playableArray.GetLength(0) - 1; i++)
                                    {
                                        for (int j = 1; j < playableArray.GetLength(1) - 1; j++)
                                        {
                                            if (playableArray[i, j].isHidden)
                                            {
                                                iHidden++;
                                            }
                                        }
                                    }
                                    //Checks if the amount of hidden values is equal to the number of mines.
                                    //If it does, uses a nested loop to print all the mines locations as exclamation marks.
                                    if (iHidden == minesCounter)
                                    {
                                        upAndDown = tempForUpAndDown;
                                        for (int i = 1; i < playableArray.GetLength(0) - 1; i++)
                                        {
                                            iLeft = tempForSides;
                                            for (int j = 1; j < playableArray.GetLength(1) - 1; j++)
                                            {
                                                if (playableArray[i, j].cellValue == mineValue)
                                                {
                                                    Console.SetCursorPosition(iLeft, upAndDown);
                                                    (playableArray[i, j].isMarked) = true;
                                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                                    Console.Write(playableArray[i, j].cellValue = markValue);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                                iLeft += 2;
                                            }
                                            upAndDown++;
                                        }
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Thread.Sleep(2500);
                                        Console.Clear();
                                        Console.SetCursorPosition(28, 1);
                                        Console.WriteLine("Congratulations!");
                                        Console.SetCursorPosition(31, 2);
                                        Console.WriteLine("You've won!");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        redo = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("What are you trying to click on?\nYou totally missed the board..");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(iLeft, upAndDown);
                        }
                        break;

                    case ConsoleKey.Insert:

                        //Checks if the Insert was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < playableArray.GetLength(0) && sidesCount < playableArray.GetLength(1))
                        {
                            //Checks if the current location isn't hidden or marked by exclamation mark.
                            if (!playableArray[upAndDownCount, sidesCount].isHidden || playableArray[upAndDownCount, sidesCount].isMarked)
                            {
                                break;
                            }
                            playableArray[upAndDownCount, sidesCount].isMarked = true;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(playableArray[upAndDownCount, sidesCount].markValue = markValue);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("Trying to redecorate my Minesweeper I see..\nMaybe it's better to focus on finish it.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(iLeft, upAndDown);
                        }
                        break;

                    case ConsoleKey.Delete:

                        //Checks if the Delete was inside the field.
                        if (sidesCount >= 0 && upAndDownCount >= 0 && upAndDownCount < playableArray.GetLength(0) && sidesCount < playableArray.GetLength(1))
                        {
                            //Checks if the current location is marked by exclamation mark.
                            if (playableArray[upAndDownCount, sidesCount].isMarked)
                            {
                                playableArray[upAndDownCount, sidesCount].isMarked = false;
                                Console.Write(playableArray[upAndDownCount, sidesCount].markValue = '\0');
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.Write("There is nothing to delete out there..\nMaybe it's better to focus on the Minesweeper.");
                            Thread.Sleep(2500);
                            Console.SetCursorPosition(iLeft, upAndDown);
                        }
                        break;
                }
            } while (!redo);
        }

        /// <summary>
        /// Reveals the cell/s if the specific location is in the field and it isn't a frame, a mine, hidden or exclamation mark.
        /// First, checks if current location is empty then passes around it to seek for another empty cell.
        /// If found - Starts all over again. If not - Prints the number.
        /// </summary>
        /// <param name="playableArray">The 2D Array the user is playing on.</param>
        /// <param name="upAndDown">The number for the SetCourserPosition. - Top</param>
        /// <param name="tempSides">Saved the last number 'sides' used for the SetCourserPosition. - Left</param>
        /// <param name="tempUpAndDown">Saved the last number 'upAndDown' used for the SetCourserPosition. - Top</param>
        /// <param name="sidesCount">Tracking on which cell the user is currently on. - Rows</param>
        /// <param name="upAndDownCount">Tracking on which cell the user is currently on. - Columns</param>
        static void UnlockSlotsIfEmpty(Cell[,] playableArray, int upAndDown, int tempSides, int tempUpAndDown, int sidesCount, int upAndDownCount)
        {
            int tempForLeft = iLeft;
            Console.SetCursorPosition(iLeft, upAndDown);

            //Checks if the current location is equal to 'empty'.
            if (playableArray[upAndDownCount, sidesCount].minesAround == 0)
            {
                playableArray[upAndDownCount, sidesCount].isHidden = false;
                Console.Write(playableArray[upAndDownCount, sidesCount].cellValue);
                upAndDown = tempUpAndDown - 1;

                //Passes around the current location.
                for (int aroundCharI = upAndDownCount - 1; aroundCharI <= upAndDownCount + 1; aroundCharI++)
                {
                    iLeft = tempSides - 2;

                    for (int aroundCharJ = sidesCount - 1; aroundCharJ <= sidesCount + 1; aroundCharJ++)
                    {
                        //Checks if the current location isn't a frame, a mine and marked by exclamation mark.
                        if (playableArray[aroundCharI, aroundCharJ].cellValue != frameValue && playableArray[aroundCharI, aroundCharJ].cellValue != mineValue && !playableArray[aroundCharI, aroundCharJ].isMarked)
                        {
                            //Checks if the current location is hidden and is it an empty cell.
                            //If it does, starts all over again from the current location.
                            if (playableArray[aroundCharI, aroundCharJ].isHidden && playableArray[aroundCharI, aroundCharJ].cellValue == emptyCellValue)
                            {
                                UnlockSlotsIfEmpty(playableArray, upAndDown, tempSides = iLeft, tempUpAndDown = upAndDown, sidesCount = aroundCharJ, upAndDownCount = aroundCharI);
                                break;
                            }

                            Console.SetCursorPosition(iLeft, upAndDown);
                            //Checks if the current location isn't equal to 'empty' and it's hidden.
                            if (playableArray[aroundCharI, aroundCharJ].minesAround != 0 && playableArray[aroundCharI, aroundCharJ].isHidden)
                            {
                                DyeNumbers(playableArray, aroundCharI, aroundCharJ);
                                playableArray[aroundCharI, aroundCharJ].isHidden = false;
                                Console.Write(playableArray[aroundCharI, aroundCharJ].minesAround);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            //Checks if the current location isn't hidden.
                            if (playableArray[aroundCharI, aroundCharJ].isHidden != false)
                            {
                                playableArray[aroundCharI, aroundCharJ].isHidden = false;
                                Console.Write(playableArray[aroundCharI, aroundCharJ].cellValue);
                            }
                        }
                        iLeft += 2;
                    }
                    upAndDown++;
                }
            }
            //Prints a number.
            else
            {
                DyeNumbers(playableArray, upAndDownCount, sidesCount);
                playableArray[upAndDownCount, sidesCount].isHidden = false;
                Console.Write(playableArray[upAndDownCount, sidesCount].minesAround);
                Console.ForegroundColor = ConsoleColor.White;
            }
            iLeft = tempForLeft;
        }

        /// <summary>
        /// Sets the current number by its color.
        /// First, gets the current location then changes the color according to its value.
        /// </summary>
        /// <param name="game2DArray">The 2D Array the user is playing on.</param>
        /// <param name="i">Gets the row number.</param>
        /// <param name="j">Gets the column number.</param>
        static void DyeNumbers(Cell[,] game2DArray, int i, int j)
        {
            switch (game2DArray[i, j].minesAround)
            {
                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case 7:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
        }

        /// <summary>
        /// The "restart game" logic. Asks the user if he would like to play again. - Yes or No question.
        /// </summary>
        /// <param name="condition">A returned boolean to set if the user want to restart the game.</param>
        /// <returns></returns>
        static bool isAnotherGame()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            bool loop = false, condition = false;
            Console.SetCursorPosition(20, 3);
            Console.Write("Would you like to restart the game?");
            Console.SetCursorPosition(19, 4);
            Console.Write("Write your answer and press - Enter.");
            Console.SetCursorPosition(29, 7);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("!Yes");
            Console.SetCursorPosition(36, 7);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("/");
            Console.SetCursorPosition(40, 7);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("No!");
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 6; i <= 8; i += 2)
            {
                Console.SetCursorPosition(30, i);
                Console.Write("------------");
            }
            while (!loop)
            {
                Console.SetCursorPosition(35, 9);
                Console.WriteLine("\t");
                Console.SetCursorPosition(35, 9);
                string sInput = Console.ReadLine();
                switch (sInput)
                {
                    case "Yes":
                    case "yes":
                    case "Y":
                    case "y":
                        loop = true;
                        Console.Clear();
                        break;

                    case "No":
                    case "no":
                    case "N":
                    case "n":
                        loop = true;
                        condition = true;
                        Console.SetCursorPosition(23, 10);
                        Console.WriteLine("Thanks for playing. Goodbye.");
                        Thread.Sleep(2000);
                        break;

                    default:
                        Console.SetCursorPosition(35, 9);
                        Console.WriteLine("\t\t\t\t\t\t\t");
                        Console.SetCursorPosition(20, 10);
                        Console.WriteLine("Invalid content, please try again.");
                        Thread.Sleep(2000);
                        Console.SetCursorPosition(20, 10);
                        Console.WriteLine("\t\t\t\t\t");
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            return condition;
        }
    }
}