int[,] board = new int[3,3];
bool isPlayer1 = true;

bool runGame = true;

Console.WriteLine("Player 1 name (Default: Player 1):");
string? player1Name = Console.ReadLine();
Console.WriteLine("Player 2 name (Default: Player 2):");
string? player2Name = Console.ReadLine();

if (string.IsNullOrEmpty(player1Name))
    player1Name = "Player 1";
if (string.IsNullOrEmpty(player2Name))
    player2Name = "Player 2";

RunGame();

void RunGame()
{
    while (runGame)
    {

        for (int i = 0; i < 3; i++)
        {
            char columnSign = (char)(i + 65);
            Console.Write($" {columnSign} ");
        }

        Console.WriteLine();

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                char character = GetCharacter(board[x, y]);
                Console.Write($"[{character}]");
            }

            Console.WriteLine($" {y + 1}");
        }

        Console.WriteLine($"{(isPlayer1 ? player1Name : player2Name)}'s turn");

        (int, int) coordinates;
// Input loop
        while (true)
        {
            string input = Console.ReadLine();

            bool validInput = TryGetSelectedSpace(input, out coordinates);

            if (validInput && IsEmptySpace(coordinates))
                break;

            Console.WriteLine("Select valid coordinates.");
        }

        MarkSpace(coordinates);

        bool isWin = CheckVictory();
        bool isTie = CheckStaleMate();

        if (isWin)
        {
            Console.WriteLine($"{(isPlayer1 ? player1Name : player2Name)} wins!");
        }

        else if (isTie)
        {
            Console.WriteLine("It's a tie! No one wins. :'(");
        }

        isPlayer1 = !isPlayer1;

        if (!isWin && !isTie)
            continue;

        Console.WriteLine("Play again? (y/n)");

        while (true)
        {
            string answer = Console.ReadLine();

            if (answer == "y")
            {
                Reset();
                
                Console.WriteLine($"{(isPlayer1 ? player1Name : player2Name)}, choose starting player: ");
                Console.WriteLine($"[1] {player1Name}");
                Console.WriteLine($"[2] {player2Name}");

                while (true)
                {
                    bool isParseable = int.TryParse(Console.ReadLine(), out int input);
                    if (isParseable && input is 1 or 2)
                    {
                        isPlayer1 = input == 1;
                        break;
                    }
                    Console.WriteLine("Incorrect Input");
                }
                
                break;
            }
            else if (answer == "n")
            {
                runGame = false;
                break;
            }
        }
    }
}


char GetCharacter(int value) => value switch
{
  0 => ' ',
  1 => 'X',
  2 => 'O',
  _ => '-'
};

bool TryGetSelectedSpace(string input, out (int, int) selectedSpace)
{
    input = input.Trim().ToUpper();
    selectedSpace = (-1, -1);
    
    char[] allowedNums = ['1', '2', '3'];
    char[] allowedChars = ['A', 'B', 'C'];
    if (input.Length != 2)
        return false;
    
    // Switch places of the coordinates if number is first
    char initialCharacter = input[0];
    if (allowedNums.Contains(initialCharacter))
    {
        input = input.Remove(0, 1);
        input += initialCharacter;
    }

    if (!allowedChars.Contains(input[0]) &&
        !allowedNums.Contains(input[0]))
        return false;

    int column = input[0] - 65; // Convert ABC to int
    int row = input[1] - 49; // Convert 123 to int

    selectedSpace = (column, row);
    return true;
}

bool IsEmptySpace((int, int) coordinates)
{
    return board[coordinates.Item1, coordinates.Item2] == 0;
}

void MarkSpace((int, int) coordinates)
{
    board[coordinates.Item1, coordinates.Item2] = (isPlayer1 ? 1 : 2);
}

bool CheckVictory()
{
    int playerMark = isPlayer1 ? 1 : 2;

    for (int y = 0; y < 3; y++)
    {
        bool isWin = true;
        for (int x = 0; x < 3; x++)
        {
            isWin = board[x, y] == playerMark;
            if (!isWin)
                break;
        }

        if (isWin)
            return true;
    }

    for (int x = 0; x < 3; x++)
    {
        bool isWin = true;
        for (int y = 0; y < 3; y++)
        {
            isWin = board[x, y] == playerMark;
            if (!isWin)
                break;
        }

        if (isWin)
            return true;
    }

    if ((board[0, 0] == playerMark && board[1, 1] == playerMark && board[2, 2] == playerMark) ||
        board[2, 0] == playerMark && board[1, 1] == playerMark && board[0, 2] == playerMark)
        return true;

    return false;
}

bool CheckStaleMate()
{
    for (int y = 0; y < 3; y++)
    {
        for (int x = 0; x < 3; x++)
        {
            if (board[x, y] == 0)
                return false;
        }
    }

    return true;
}

void Reset()
{
    board = new int[3, 3];
}