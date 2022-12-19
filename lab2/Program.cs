using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        bool redSolved = false; // 1
        bool greenSolved = false; // 2
        bool blueSolved = false; // 3
        int location = 0;
        while (true)
        {
            bool canEscape = redSolved && greenSolved && blueSolved;

            if (canEscape)
            {
                var outstr =
                    "<Имя Субъекта>, Вы прекрасно показали себя! Мы благодарны за ваш вклад в развитие мировой науки!\n";
                foreach (var ch in outstr)
                {
                    Console.Write(ch);
                    System.Threading.Thread.Sleep(75);
                }
                Console.WriteLine("Спасибо!");
                break;
            }
            // RED ROOM
            if (location == 0)
            {
                Console.Clear();
                Console.WriteLine("Сейчас вы в красной комнате. Вы решаете...");
                Console.WriteLine("1. Пойти в зелёную комнату");
                Console.WriteLine("2. Пойти в синюю комнату");
                Console.WriteLine("3. Поискать подсказки");
                Console.WriteLine("4. Сдаться");
                int choice = GetChoice(4);

                if (choice == 1)
                {
                    location = 1; // GREEN
                }
                else if (choice == 2)
                {
                    location = 2; // BLUE
                }
                else if (choice == 3)
                {
                    // puzzle
                    if (redSolved)
                    {
                        Console.WriteLine("Здесь больше нечего делать!");
                    }
                    else
                    {
                        var puzzle = new int[] { 0, 1, 0, 1 };
                        Console.WriteLine("Удачи!");
                        Console.WriteLine(
                            "На стене вы замечаете 4 кнопки. Вы решаете нажать на одну из них."
                        );
                        while (true)
                        {
                            Console.WriteLine($"{puzzle[0]} {puzzle[1]} {puzzle[2]} {puzzle[3]}");

                            var pressed = GetChoice(4);
                            if (pressed == 1)
                            {
                                puzzle[0] = 1 - puzzle[0];
                                puzzle[1] = 1 - puzzle[1];
                                puzzle[2] = 1 - puzzle[2];
                            }
                            else if (pressed == 2)
                            {
                                puzzle[0] = 1 - puzzle[0];
                                puzzle[1] = 1 - puzzle[1];
                            }
                            else if (pressed == 3)
                            {
                                puzzle[1] = 1 - puzzle[1];
                                puzzle[2] = 1 - puzzle[2];
                            }
                            else if (pressed == 4)
                            {
                                puzzle[0] = 1 - puzzle[0];
                                puzzle[3] = 1 - puzzle[3];
                            }
                            redSolved = ((puzzle[0] + puzzle[1] + puzzle[2] + puzzle[3]) == 0);

                            if (redSolved)
                                break;
                        }
                        Console.WriteLine($"{puzzle[0]} {puzzle[1]} {puzzle[2]} {puzzle[3]}");
                        Console.WriteLine("Вы справились с зелёной задачей!");
                        Console.ReadKey(true);
                    }
                }
                else if (choice == 4)
                {
                    Console.WriteLine("Наверно, вы пытались...");
                    break; // quit
                }
            }
            // WHITE ROOM
            else if (location == 1)
            {
                Console.Clear();
                Console.WriteLine(
                    "Сейчас вы в зелёной комнате. Вы видите книжный шкаф. Вы решаете..."
                );
                Console.WriteLine("1. Пойти в красную комнату");
                Console.WriteLine("2. Пойти в синюю комнату");
                Console.WriteLine("3. Поискать подсказки");
                Console.WriteLine("4. Сдаться");
                int choice = GetChoice(4);

                if (choice == 1)
                {
                    location = 0; // RED
                }
                else if (choice == 2)
                {
                    location = 2; // BLUE
                }
                else if (choice == 3)
                {
                    if (greenSolved)
                    {
                        Console.WriteLine(
                            "Если вы потерялись и не помните, где вы уже были, вы нажмите '6' для завершения игры"
                        );
                    }
                    else
                    {
                        Console.WriteLine("Вы видите книжный шкаф");
                        Console.WriteLine("Lorem ipsum etc.");
                        Console.ReadKey(true);
                        Console.WriteLine("MCMLXXXIV");
                        while (true)
                        {
                            greenSolved = Console.ReadLine() == "1984";
                            if (greenSolved)
                                break;
                        }
                        Console.WriteLine("Вы справились с белой задачей");
                        Console.ReadKey(true);
                    }
                }
                else if (choice == 4)
                {
                    Console.WriteLine("Наверно, вы пытались...");
                    break; // quit
                }
            }
            else if (location == 2)
            {
                Console.Clear();
                Console.WriteLine("Сейчас вы в синей комнате. Вы решаете...");
                Console.WriteLine("1. Пойти в красную комнату");
                Console.WriteLine("2. Пойти в зелёную комнату");
                Console.WriteLine("3. Поискать подсказки");
                Console.WriteLine("4. Сдаться");
                int choice = GetChoice(4);

                if (choice == 1)
                {
                    location = 0; // RED
                }
                else if (choice == 2)
                {
                    location = 1; // GREEN
                }
                else if (choice == 3)
                {
                    // puzzle
                    if (blueSolved)
                    {
                        Console.WriteLine("Вы здесь уже были. И даже что-то сделали");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Все умеют читать и писать, и вы не исключление. Напишите 'I can write'. Это совсем не сложно."
                        );
                        var str = "";
                        while (true)
                        {
                            var key = Console.ReadKey(true);
                            var code = (int)key.KeyChar;

                            if (key.Key == ConsoleKey.Backspace && str.Length > 0)
                            {
                                str = str.Substring(0, str.Length - 1);
                                Console.Write("\b \b");
                            }
                            else if (code >= 48 && code <= 57)
                            {
                                var ch = (char)((code + 8) % 10 + 48);
                                str += ch;
                                Console.Write(ch);
                            }
                            else if (code >= 65 && code <= 90)
                            {
                                var ch = (char)((code + 19) % 26 + 65);
                                str += ch;
                                Console.Write(ch);
                            }
                            else if (code >= 97 && code <= 122)
                            {
                                var ch = (char)((code + 22) % 26 + 97);
                                str += ch;
                                Console.Write(ch);
                            }
                            else
                            {
                                Console.Write(key.KeyChar);
                                str += key.KeyChar;
                            }
                            if (key.Key == ConsoleKey.Enter)
                                break;
                        }

                        str = str.Replace("\r", "");

                        if (str == "I can write")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Было просто, не так ли?");
                            blueSolved = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Похоже, что вы исключение из правил...");
                        }
                        Console.ReadKey(true);
                    }
                }
                else if (choice == 4)
                {
                    Console.WriteLine("Наверно, вы пытались...");
                    break; // quit
                }
            }
        }
        Console.ReadKey(true);
    }

    static int GetChoice(int max)
    {
        var num = -1;
        var success = int.TryParse(Console.ReadLine(), out num) && (num <= max) && (num >= 1);
        if (!success)
        {
            Console.WriteLine("Так нельзя!");
            num = GetChoice(max);
        }
        return num;
    }
}
