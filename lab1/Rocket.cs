using System.Globalization;

class Rocket
{
    const double MOON_GRAVITY = 1.62;
    const double ENGINE_ACCELERATION = 4;
    const double MAX_SAFE_SPEED = 44.26;

    ///////////////////////
    #region input values
    int engineMode;
    double actionTime;
    #endregion
    ///////////////////////
    #region variables
    double acceleration;
    double speed;
    double height;
    double fuel;
    #endregion
    //////////////////////
    #region land check variables
    double impactTime;
    bool isSuccessfull;
    bool hasLanded;
    double possibleLandTime;
    double previousHeight;

    #endregion
    public Rocket()
    {
        var rnd = new Random();

        hasLanded = false;
        speed = 0;
        acceleration = -MOON_GRAVITY;
        height =
            // rnd.NextDouble() * (25000 - 8000) +
            8000;
        impactTime = Math.Sqrt(2 * height / MOON_GRAVITY);
        // it = (v - sqrt(v*v + 2 * mg * ih)) / -mg => v = 0, it = -sqrt( 2 * mg * ih) / -mg => it = sqrt(2 * ih / mg)
        fuel = .75 * impactTime;
    }

    void Input()
    {
        while (true)
        {
            TimeInput();
            ModeInput();
            var isValid = !((engineMode < 1) && (Math.Round(actionTime - impactTime, 2) > 0));
            if (isValid)
                break;
        }
    }

    void TimeInput()
    {
        var at = double.NaN;
        while (true)
        {
            if (!double.IsNaN(at))
                break;
            Console.Write("Время действия: ");
            var s = Console.ReadLine() + "";
            Console.WriteLine();
            var p = double.TryParse(
                s.Replace(",", "."),
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out at
            );
            if (!p || (at <= 0))
            {
                at = double.NaN;
                Console.WriteLine("Неудачный ввод");
            }
        }
        actionTime = Math.Round(at, 2);
    }

    void ModeInput()
    {
        var em = 0;
        var hasConfirmed = false;
        while (true)
        {
            if (hasConfirmed)
                break;
            Console.WriteLine(
                "Введите 'toggle' или '/', чтобы переключить режим двигателя, 'continue' или '>' - подтвердить"
            );
            Console.Write("Режим двигателя: ");
            var s = (Console.ReadLine() + "").ToLower();
            switch (s)
            {
                case "переключить":
                case "/":
                    em = 1 - em;
                    Console.WriteLine($"Текущий режим: {((em < 1) ? "off" : "on")}");
                    break;
                case "continue":
                case ">":
                    hasConfirmed = true;
                    break;
                default:
                    Console.WriteLine("Неудачный ввод");
                    break;
            }
        }
        engineMode = em;
    }

    double ComputeImpactTime()
    {
        return (speed - Math.Sqrt(speed * speed + 2 * MOON_GRAVITY * height)) / -MOON_GRAVITY;
    }

    double ComputeEngineImpactTime()
    {
        return (
            (speed - Math.Sqrt(speed * speed - 2 * (ENGINE_ACCELERATION - MOON_GRAVITY) * previousHeight))
            / (ENGINE_ACCELERATION - MOON_GRAVITY)
        );
    }

    void ComputeAFHValues() //меняет значения ускорения, топлива и высоты (и сохраняет предыдущую высоту, т.к. вычисление идет без проверки)
    {
        acceleration = -MOON_GRAVITY + ENGINE_ACCELERATION * engineMode;
        previousHeight = height;
        height = height - speed * actionTime + acceleration * actionTime * actionTime / 2;
        fuel = fuel - actionTime * engineMode;
    }

    double GetComputedSpeed()
    {
        var time = ComputeEngineImpactTime();

        if (actionTime > time && (engineMode == 1))
            return speed - acceleration * time;

        return speed - acceleration * actionTime;
    }

    void Print()
    {
        Console.Clear();
        Console.WriteLine($"Высота: {height:F2} м");
        Console.WriteLine($"Скорость {Math.Abs(speed):F2} м/с {((speed >= 0) ? "вниз" : "вверх")}");
        Console.WriteLine($"Время до удара/посадки: {impactTime:F2} с");
        Console.WriteLine($"Топливо: {Math.Abs(fuel):F2} с");
    }

    void CheckIfLanded()
    {
        possibleLandTime = ComputeEngineImpactTime();
        bool c1 = ((Math.Abs(actionTime - impactTime) < 0.01) && (engineMode == 0));
        // условие посадки при аккуратном вводе (игрок вводит значение не больше вычисленного)
        bool c2 = (actionTime > possibleLandTime) && (engineMode == 1);
        // выполняется если игрок включает двигатель на время большее, чем максимально возможное до удара
        hasLanded = c1 || c2;
    }

    void CheckIfSafe()
    {
        isSuccessfull = (speed <= MAX_SAFE_SPEED) ? true : false;
    }

    void Land()
    {
        if (isSuccessfull)
            Console.WriteLine($"Успешная посадка! Скорость {Math.Abs(speed):F2} м/с");
        else
            Console.WriteLine($"Экипажу не удалось пережить столкновение со скоростью {Math.Abs(speed):F2} м/с");
    }

    public void Play()
    {
        while (true)
        {
            Print();
            Input();

            ComputeAFHValues();

            speed = GetComputedSpeed();

            CheckIfLanded();
            CheckIfSafe();

            if (hasLanded)
            {
                Land();
                break;
            }
            
            impactTime = ComputeImpactTime();
        }
    }
}
