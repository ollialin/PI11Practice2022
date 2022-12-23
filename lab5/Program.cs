using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        Calculator c = new Calculator();

        var ch = "";
        while (true)
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine(c.Screen);
            Console.Write("Последняя нажатая кнопка: ");
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(ch);
            Console.ForegroundColor = color;

            ch = Console.ReadKey(true).KeyChar.ToString(CultureInfo.InvariantCulture);
            c.PressKey(ch);
        }
    }
}
