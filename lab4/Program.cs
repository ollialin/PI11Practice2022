using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();
        Console.CursorVisible = false;

        var maze = new Maze(ink: ConsoleColor.Gray, paper: ConsoleColor.DarkGray);

        while (true)
        {
            maze.ClearLine(2);
            maze.CheckSanity();
            if (!maze.isPlaying)
                break;

            maze.Print(3, 2, $"Sanity: {maze.sanity}");
            maze.Print(3, 3);

            var ki = Console.ReadKey(true);
            switch (ki.Key)
            {
                case ConsoleKey.LeftArrow:
                    maze.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    maze.Move(1, 0);
                    break;
                case ConsoleKey.UpArrow:
                    maze.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    maze.Move(0, 1);
                    break;
                case ConsoleKey.Escape:
                    return;
            }

            maze.ClearLine(17);
            maze.Print(3, 17, maze.message, ink: ConsoleColor.DarkGreen);
            // Debug.WriteLine(maze.message);
        }

        if (maze.isSane)
        {
            maze.Print(3, 17, "You've made it! Congratulations!", ink: ConsoleColor.Yellow);
        }
        else
        {
            maze.ClearLine(2);
            maze.Print(3, 17, "You've failed to escape", ink: ConsoleColor.Red);
        }
        Console.ReadKey();
    }
}
