class Maze
{
    // 0 - free space
    // 1 - wall
    // 2 - key
    // 3 - door
    // 4 - coin

    //данные
    int playerX = 1;
    int playerY = 1;
    double visibleDistance = 3.5;
    public int sanity = 150;
    int decrement = 3;
    bool hasKey = false;
    public bool isPlaying = true;
    public bool isSane = true;
    Dictionary<string, int[]> specialPoints = new Dictionary<string, int[]>(); //unique cells: exit, portal-1, portal-2
    public string message = "";

    /////////////////////////////////
    int width;
    int height;
    int[,] maze = new int[,]
    {
        //0     2     4     6     8     10    12
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, // 0
        { 1, 0, 1, 6, 0, 0, 0, 1, 0, 0, 0, 8, 1 },
        { 1, 0, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1 }, // 2
        { 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1 },
        { 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1 }, // 4
        { 1, 1, 1, 0, 0, 0, 2, 1, 0, 1, 0, 1, 1 },
        { 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 2, 1 }, // 6
        { 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1 },
        { 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1 }, // 8
        { 1, 0, 10, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
        { 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1 }, // 10
        { 1, 0, 0, 0, 2, 1, 0, 0, 0, 1, 4, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1 } // 12
    };
    ConsoleColor ink;
    ConsoleColor paper;

    public Maze(ConsoleColor ink, ConsoleColor paper, int width = 13, int height = 13)
    {
        this.height = height;
        this.width = width;
        this.ink = ink;
        this.paper = paper;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                switch (maze[i, j])
                {
                    case 3:
                        specialPoints.Add("exit", new int[] { j, i });
                        break;
                    case 8:
                        specialPoints.Add("portalBlue", new int[] { j, i });
                        break;
                    case 10:
                        specialPoints.Add("portalOrange", new int[] { j, i });
                        break;
                }
            }
        }
    }

    //методы
    public void Move(int dx, int dy)
    {
        ClearMessage();

        if (playerX == specialPoints["exit"][0] && playerY == specialPoints["exit"][1])
        {
            isPlaying = false;
        }
        else
        {
            int nx = playerX + dx;
            int ny = playerY + dy;
            if (maze[ny, nx] % 2 == 0)
            {
                DecreaseSanity(decrement);
                playerX = nx;
                playerY = ny;

                switch (maze[ny, nx])
                {
                    case 2:
                        EatApple(ny, nx);
                        break;
                    case 4:
                        PickUpKey(ny, nx);
                        break;
                    case 6:
                        PickUpTorch(ny, nx);
                        break;
                    case 8:
                        //blue
                        Teleport(specialPoints["portalOrange"]);
                        break;
                    case 10:
                        //orange
                        Teleport(specialPoints["portalBlue"]);
                        break;
                }
            }
            else
            {
                switch (maze[ny, nx])
                {
                    case 3:
                        OpenDoor();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void CheckSanity()
    {
        if (sanity <= 0)
        {
            isSane = false;
            isPlaying = false;
        }
    }

    public void Print(int shiftX, int shiftY)
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                double relativeDistance = Math.Sqrt(
                    (playerX - x) * (playerX - x) + (playerY - y) * (playerY - y)
                );
                if (relativeDistance > visibleDistance)
                {
                    Print(shiftX + x, shiftY + y, " ", paper: ConsoleColor.Black);
                }
                else
                {
                    switch (maze[y, x])
                    {
                        case 0: //empty space
                            Print(shiftX + x, shiftY + y, " ");
                            break;
                        case 1: //wall
                            Print(shiftX + x, shiftY + y, "+", ink, paper);
                            break;
                        case 2: //apple
                            Print(shiftX + x, shiftY + y, "o", ink: ConsoleColor.Red);
                            break;
                        case 3: //exit
                            Print(shiftX + x, shiftY + y, "#", ink);
                            break;
                        case 4: //key
                            Print(shiftX + x, shiftY + y, "q", ink: ConsoleColor.Gray);
                            break;
                        case 6: //torch
                            Print(shiftX + x, shiftY + y, "i", ink: ConsoleColor.Yellow);
                            break;
                        case 8: //portal blue
                            Print(shiftX + x, shiftY + y, "@", ink: ConsoleColor.Blue);
                            break;
                        case 10: //portal orange
                            Print(shiftX + x, shiftY + y, "@", ink: ConsoleColor.DarkYellow);
                            break;
                    }
                }
            }

        Print(shiftX + playerX, shiftY + playerY, "&");
    }

    public void Print(
        int x,
        int y,
        string s,
        ConsoleColor ink = ConsoleColor.White,
        ConsoleColor paper = ConsoleColor.Black
    )
    {
        Console.ForegroundColor = ink;
        Console.BackgroundColor = paper;
        Console.CursorLeft = x;
        Console.CursorTop = y;
        Console.Write(s);
    }

    void EatApple(int ny, int nx)
    {
        maze[ny, nx] = 0;
        sanity = Math.Min(150, sanity + 20);
        message = "You ate an apple";
    }

    void PickUpKey(int ny, int nx)
    {
        maze[ny, nx] = 0;
        sanity += 10;
        hasKey = true;
        message = "You've got a key";
    }

    void PickUpTorch(int ny, int nx)
    {
        maze[ny, nx] = 0;
        sanity += 15;
        decrement = 1;
        visibleDistance = 6.5;
        message = "You've got a torch";
    }

    void OpenDoor()
    {
        if (hasKey)
        {
            maze[specialPoints["exit"][1], specialPoints["exit"][0]] = 0;
            message = "Door's open";
        }
        else
        {
            message = "Door's locked";
        }
    }

    void Teleport(int[] coords)
    {
        sanity -= 25;
        playerX = coords[0];
        playerY = coords[1];
        message = "#@1!){@?!]?";
    }

    void ClearMessage()
    {
        message = "";
    }

    public void ClearLine(int index)
    {
        var x = Console.CursorLeft;
        var y = Console.CursorTop;
        Console.SetCursorPosition(0, index);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(x, y);
    }

    void DecreaseSanity(int d)
    {
        sanity = (d > 0) ? sanity - d : sanity;
    }
}
