using System.Globalization;

class Calculator
{
    //data
    string _screen;
    string _memory;
    string _op;
    CalcState _state;

    public string Screen
    {
        get { return _screen; }
    }

    //methods
    public Calculator()
    {
        _screen = "0";
        _memory = "";
        _op = "";
        _state = CalcState.Input1;
    }

    public void PressKey(string key)
    {
        switch (_state)
        {
            case CalcState.Input1:
                _state = ProcessInput1(key);
                break;
            case CalcState.Operation:
                _state = ProcessOperation(key);
                break;
            case CalcState.Input2:
                _state = ProcessInput2(key);
                break;
            case CalcState.Answer:
                _state = ProcessAnswer(key);
                break;
            case CalcState.Error:
                _state = ProcessError(key);
                break;
        }
    }

    CalcState ProcessInput1(string key)
    {
        switch (GetKind(key))
        {
            case KeyKind.Digit:
                _screen = AddDigit(_screen, key);
                return CalcState.Input1;
            case KeyKind.Dot:
                _screen = AddDot(_screen);
                return CalcState.Input1;
            case KeyKind.ChangeSign:
                _screen = ChangeSign(_screen);
                return CalcState.Input1;
            case KeyKind.Operation:
                _memory = _screen;
                _op = key;
                return CalcState.Operation;
            case KeyKind.Equal:
                return CalcState.Input1;
            case KeyKind.Clear:
                Clear();
                return CalcState.Input1;
            default:
                _screen = "Error";
                return CalcState.Error;
        }
    } //done

    CalcState ProcessOperation(string key)
    {
        switch (GetKind(key))
        {
            case KeyKind.Digit:
                _screen = key;
                return CalcState.Input2; //3
            case KeyKind.Dot:
                _screen = "0.";
                return CalcState.Input2; //3
            case KeyKind.ChangeSign:
                return CalcState.Operation; //2
            case KeyKind.Operation:
                _op = key;
                return CalcState.Operation; //2
            case KeyKind.Equal:
                _screen = Calc(_memory, _screen, _op);
                return CalcState.Answer; //4
            case KeyKind.Clear:
                Clear();
                return CalcState.Input1; //1
            default:
                _screen = "Error";
                return CalcState.Error; //5
        }
    } //done

    CalcState ProcessInput2(string key)
    {
        switch (GetKind(key))
        {
            case KeyKind.Digit:
                _screen = AddDigit(_screen, key);
                return CalcState.Input2; //3
            case KeyKind.Dot:
                _screen = AddDot(_screen);
                return CalcState.Input2; //3
            case KeyKind.ChangeSign:
                _screen = ChangeSign(_screen);
                return CalcState.Input2; //3
            case KeyKind.Operation:
                _screen = Calc(_memory, _screen, _op);
                _op = key;
                return CalcState.Operation; //2
            case KeyKind.Equal:
                var t = Calc(_memory, _screen, _op);
                if (t == "NaN")
                {
                    _screen = "Error";
                    return CalcState.Error; //5*
                }
                _screen = t;
                return CalcState.Answer; //4
            case KeyKind.Clear:
                Clear();
                return CalcState.Input1; //1
            default:
                _screen = "Error";
                return CalcState.Error; //5
        }
    } //done

    CalcState ProcessAnswer(string key)
    {
        switch (GetKind(key))
        {
            case KeyKind.Digit:
                _screen = key;
                return CalcState.Input1; //1
            case KeyKind.Dot:
                _screen = "0.";
                return CalcState.Input1; //1
            case KeyKind.ChangeSign:
                _screen = ChangeSign(_screen);
                return CalcState.Answer; //4
            case KeyKind.Operation:
                _memory = _screen;
                _op = key;
                return CalcState.Operation; //2
            case KeyKind.Equal:
                var t = Calc(_memory, _screen, _op);
                if (t == "NaN")
                {
                    _screen = "Error";
                    return CalcState.Error; //5*
                }
                _screen = t;
                return CalcState.Answer; //4
            case KeyKind.Clear:
                Clear();
                return CalcState.Input1; //1
            default:
                _screen = "Error";
                return CalcState.Error; //5
        }
    }

    CalcState ProcessError(string key)
    {
        switch (GetKind(key))
        {
            case KeyKind.Digit:
            case KeyKind.Dot:
            case KeyKind.ChangeSign:
            case KeyKind.Operation:
            case KeyKind.Equal:
                return CalcState.Error;
            case KeyKind.Clear:
                Clear();
                return CalcState.Input1;
            default:
                _screen = "Error";
                return CalcState.Error;
        }
    }

    string AddDigit(string num, string digit)
    {
        if (num == "0")
            return digit;
        return num + digit;
    }

    string AddDot(string num)
    {
        if (num.Contains("."))
            return num;
        else
            return num + ".";
    }

    string ChangeSign(string num)
    {
        if (num == "0")
            return "0";

        if (num.StartsWith("-"))
            return num.Substring(1);
        else
            return "-" + num;
    }

    void Clear()
    {
        _screen = "0";
        _memory = "";
        _op = "";
    }

    string Calc(string x, string y, string op)
    {
        double arg1 = double.Parse(x, CultureInfo.InvariantCulture);
        double arg2 = double.Parse(y, CultureInfo.InvariantCulture);
        double res = 0;

        switch (op)
        {
            case "+":
                res = arg1 + arg2;
                break;
            case "-":
                res = arg1 - arg2;
                break;
            case "*":
                res = arg1 * arg2;
                break;
            case "/":
                res = (arg2 != 0) ? arg1 / arg2 : double.NaN;
                break;
        }
        Console.WriteLine(res);
        return res.ToString(CultureInfo.InvariantCulture);
    }

    KeyKind GetKind(string key)
    {
        switch (key)
        {
            case "0":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                return KeyKind.Digit;
            case ".":
                return KeyKind.Dot;
            case "-/+": //неудобная комбинация, плохо сочетается с посимвольным вводом
            case "_":
                return KeyKind.ChangeSign;
            case "+":
            case "-":
            case "*":
            case "/":
                return KeyKind.Operation;
            case "=":
                return KeyKind.Equal;
            case "c":
            case "C":
                return KeyKind.Clear;
            default:
                return KeyKind.Undefined;
        }
    }
}

enum CalcState
{
    Input1,
    Operation,
    Input2,
    Answer,
    Error
}

enum KeyKind
{
    Digit,
    Dot,
    ChangeSign,
    Operation,
    Equal,
    Clear,
    Undefined
}
