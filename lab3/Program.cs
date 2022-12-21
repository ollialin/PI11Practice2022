internal class Program
{
    private static void Main(string[] args)
    {
        const int LOC_RED = 0;
        const int LOC_GREEN = 1;
        const int LOC_BLUE = 2;

        bool redSolved = false;
        bool greenSolved = false;
        bool blueSolved = false;

        void RedTask()
        {
            if (redSolved)
            {
                Print("Здесь больше нечего делать");
                Print();
                Print("Пожалуйста, не подходите к кнопкам");
                Console.ReadKey();
            }
            else
            {
                var redPuzzle = new int[] { 0, 1, 0, 1 };
                Print("На стене вы замечаете 4 кнопки. Вы нажимаете на одну из них...");
                Print();
                while (true)
                {
                    redSolved = (redPuzzle[0] + redPuzzle[1] + redPuzzle[2] + redPuzzle[3]) == 0;
                    if (redSolved)
                        break;

                    Print($"{redPuzzle[0]} {redPuzzle[1]} {redPuzzle[2]} {redPuzzle[3]}");
                    Print();
                    switch (GetChoice(4))
                    {
                        case 1:
                            redPuzzle[0] = 1 - redPuzzle[0];
                            redPuzzle[1] = 1 - redPuzzle[1];
                            redPuzzle[2] = 1 - redPuzzle[2];
                            break;
                        case 2:
                            redPuzzle[0] = 1 - redPuzzle[0];
                            redPuzzle[1] = 1 - redPuzzle[1];
                            break;
                        case 3:
                            redPuzzle[1] = 1 - redPuzzle[1];
                            redPuzzle[2] = 1 - redPuzzle[2];
                            break;
                        case 4:
                            redPuzzle[0] = 1 - redPuzzle[0];
                            redPuzzle[3] = 1 - redPuzzle[3];
                            break;
                    }
                }
                Print("Вы справились с красным заданием!");

                Console.ReadKey(true);
            }
        }
        // 42
        void GreenTask()
        {
            if (greenSolved)
            {
                Print(
                    "Если вы потерялись и не помните, где вы уже были, вы нажмите '6' для завершения игры"
                );
                Print(timeout: 250);
            }
            else
            {
                Print("Lorem ipsum etc.");
                Print();
                Console.ReadKey(true);
                Print("...");
                Print(timeout: 250);
                Print("Мы приносим свои извинения, мы вынуждены заменить следующее испытание.");
                Print(timeout: 250);
                Print("Ваше новое испытание: MCMLXXXIV");
                Print(timeout: 250);
                while (true)
                {
                    greenSolved = Console.ReadLine() == "1984";
                    if (greenSolved)
                        break;
                }
                Print("Вы справились с зелёной задачей");
                Console.ReadKey(true);
            }
        }
        // 1984
        void BlueTask()
        {
            if (blueSolved)
            {
                Print("Вы здесь уже были. И даже что-то сделали");
                Print();
            }
            else
            {
                Print(
                    "Все умеют читать и писать, и вы не исключление. Напишите 'I can write'. Это совсем не сложно."
                );
                Print();
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
                Print();
                if (str == "I can write")
                {
                    blueSolved = true;
                    Print("Было просто, не так ли?");
                }
                else
                {
                    Print("Похоже, что вы исключение из правил...");
                }
                Console.ReadKey(true);
            }
        }
        // C nly hctep
        Story story = new StoryBuilder()
            .SetupStory(
                "Приветствуем Вас, <Имя Субъекта>!\nВам предстоит пройти ряд испытаний.\nВсю необходимую информацию Вы получите на месте.\nВсе следующие сообщения будут выведены в ускоренном режиме.",
                "<Имя Субъекта>, Вы прекрасно показали себя! Мы благодарны за Ваш вклад в развитие мировой науки!",
                LOC_RED
            )
            .AddLocation(LOC_RED, "Сейчас вы в красной комнате. Вы решаете...")
            .AddOption(LOC_RED, LOC_GREEN, "Пойти в зелёную комнату")
            .AddOption(LOC_RED, LOC_BLUE, "Пойти в синюю комнату")
            .AddOption(LOC_RED, "Поискать подсказки", RedTask)
            .AddLocation(LOC_GREEN, "Вы находитесь в зелёной комнате. Вы решаете...")
            .AddOption(LOC_GREEN, LOC_RED, "Пойти в красную комнату")
            .AddOption(LOC_GREEN, LOC_BLUE, "Пойти в синюю комнату")
            .AddOption(LOC_GREEN, "Поискать подсказки", GreenTask)
            .AddLocation(LOC_BLUE, "Сейчас вы в синей комнате. Вы решаете...")
            .AddOption(LOC_BLUE, LOC_RED, "Пойти в красную комнату")
            .AddOption(LOC_BLUE, LOC_GREEN, "Пойти в зелёную комнату")
            .AddOption(LOC_BLUE, "Поискать подсказки", BlueTask)
            .Build();

        Console.Clear();
        Print(story.Intro, 60);
        Console.ReadKey();
        while (true)
        {
            if (redSolved && greenSolved && blueSolved)
            {
                break;
            }
            var loc = story.Locations.First(item => item.Id == story.CurrentLocationId);
            Console.Clear();
            Print(loc.Description);
            Print(timeout: 500);
            for (int i = 0; i < loc.Options.Count; i++)
            {
                Print($"{i + 1}. {loc.Options[i].Title}");
                Print(timeout: 250);
            }
            Print("Ваш выбор: ");
            var n = GetChoice(loc.Options.Count) - 1;
            loc.Options[n].Work();
        }
        Print();
        Console.ReadKey(true);
        Print(story.Finale, 60);
        Console.ReadKey(true);
    }

    static void Print(string line = "\n", int timeout = 20)
    {
        foreach (var ch in line)
        {
            Console.Write(ch);
            System.Threading.Thread.Sleep(timeout);
        }
    }

    static int GetChoice(int max)
    {
        var num = -1;
        var success = int.TryParse(Console.ReadLine(), out num) && (num <= max) && (num >= 1);
        if (!success)
        {
            Print("Такого варианта здесь нет");
            Print();
            num = GetChoice(max);
        }
        return num;
    }
}
