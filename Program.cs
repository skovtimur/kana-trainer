namespace kanaTrainer;

public static class Program
{
    private static readonly Dictionary<string, string> HiraganaDictionary = new()
    {
        { "あ", "a" }, { "い", "i" }, { "う", "u" }, { "え", "e" }, { "お", "o" },
        { "か", "ka" }, { "き", "ki" }, { "く", "ku" }, { "け", "ke" }, { "こ", "ko" },
        { "さ", "sa" }, { "し", "shi" }, { "す", "su" }, { "せ", "se" }, { "そ", "so" },
        { "た", "ta" }, { "ち", "chi" }, { "つ", "tsu" }, { "て", "te" }, { "と", "to" },
        { "な", "na" }, { "に", "ni" }, { "ぬ", "nu" }, { "ね", "ne" }, { "の", "no" },
        { "は", "ha" }, { "ひ", "hi" }, { "ふ", "fu" }, { "へ", "he" }, { "ほ", "ho" },
        { "ま", "ma" }, { "み", "mi" }, { "む", "mu" }, { "め", "me" }, { "も", "mo" },
        { "や", "ya" }, { "ゆ", "yu" }, { "よ", "yo" },
        { "ら", "ra" }, { "り", "ri" }, { "る", "ru" }, { "れ", "re" }, { "ろ", "ro" },
        { "わ", "wa" }, { "を", "wo" }, { "ん", "n" },
    };

    private static readonly Dictionary<string, string> HiraganaDakutenAndHandukenDictionary = new()
    {
        { "が", "ga" }, { "ぎ", "gi" }, { "ぐ", "gu" }, { "げ", "ge" }, { "ご", "go" },
        { "ざ", "za" }, { "じ", "ji" }, { "ず", "zu" }, { "ぜ", "ze" }, { "ぞ", "zo" },
        { "だ", "da" }, { "ぢ", "ji" }, { "づ", "dzu" }, { "で", "de" }, { "ど", "do" },
        { "ば", "ba" }, { "び", "bi" }, { "ぶ", "bu" }, { "べ", "be" }, { "ぼ", "bo" },
        { "ぱ", "pa" }, { "ぴ", "pi" }, { "ぷ", "pu" }, { "ぺ", "pe" }, { "ぽ", "po" }
    };


    private static readonly Dictionary<string, string> KatakanaDictionary = new()
    {
        { "ア", "a" }, { "イ", "i" }, { "ウ", "u" }, { "エ", "e" }, { "オ", "o" },
        { "カ", "ka" }, { "キ", "ki" }, { "ク", "ku" }, { "ケ", "ke" }, { "コ", "ko" },
        { "サ", "sa" }, { "シ", "shi" }, { "ス", "su" }, { "セ", "se" }, { "ソ", "so" },
        { "タ", "ta" }, { "チ", "chi" }, { "ツ", "tsu" }, { "テ", "te" }, { "ト", "to" },
        { "ナ", "na" }, { "ニ", "ni" }, { "ヌ", "nu" }, { "ネ", "ne" }, { "ノ", "no" },
        { "ハ", "ha" }, { "ヒ", "hi" }, { "フ", "fu" }, { "ヘ", "he" }, { "ホ", "ho" },
        { "マ", "ma" }, { "ミ", "mi" }, { "ム", "mu" }, { "メ", "me" }, { "モ", "mo" },
        { "ヤ", "ya" }, { "ユ", "yu" }, { "ヨ", "yo" },
        { "ラ", "ra" }, { "リ", "ri" }, { "ル", "ru" }, { "レ", "re" }, { "ロ", "ro" },
        { "ワ", "wa" }, { "ヲ", "wo" }, { "ン", "n" },
    };

    private static readonly Dictionary<string, string> KatakanaDakutenAndHandukenDictionary = new()
    {
        { "ガ", "ga" }, { "ギ", "gi" }, { "グ", "gu" }, { "ゲ", "ge" }, { "ゴ", "go" },
        { "ザ", "za" }, { "ジ", "ji" }, { "ズ", "zu" }, { "ゼ", "ze" }, { "ゾ", "zo" },
        { "ダ", "da" }, { "ヂ", "ji" }, { "ヅ", "dzu" }, { "デ", "de" }, { "ド", "do" },
        { "バ", "ba" }, { "ビ", "bi" }, { "ブ", "bu" }, { "ベ", "be" }, { "ボ", "bo" },
        { "パ", "pa" }, { "ピ", "pi" }, { "プ", "pu" }, { "ペ", "pe" }, { "ポ", "po" }
    };

    private static readonly Dictionary<string, string> WrongAnswers = new() { };

    private enum Alphabet
    {
        None,
        Hiragana,
        Katakana
    }

    private static Alphabet _currentAlphabet = Alphabet.None;
    private static bool _guessMode;

    public static void Main(string[] args)
    {
        Write("Kana console  trainer");
        Thread.Sleep(1000);
        Console.Clear();

        ChooseAlphabet();
        Console.Clear();

        if (args.Contains("-d") == false || args.Contains("-e"))
        {
            Write("With the Dakuten and handakuten? If so, write 'y'", ConsoleColor.Magenta);
            var outputKey = Console.ReadKey();
            if (outputKey.KeyChar == 'y' || outputKey.KeyChar == 'Y')
            {
                foreach (var x in HiraganaDakutenAndHandukenDictionary)
                    HiraganaDictionary.Add(x.Key, x.Value);
                foreach (var y in KatakanaDakutenAndHandukenDictionary)
                    KatakanaDictionary.Add(y.Key, y.Value);
            }
        }

        Console.Clear();

        _guessMode = args.Contains("-g");
        if (_guessMode == false)
        {
            Write("Enable a guess mode?  If so, write 'y'", ConsoleColor.Gray);
            var guessModeKeyChar = Console.ReadKey().KeyChar;
            _guessMode = guessModeKeyChar == 'y' || guessModeKeyChar == 'Y';
        }

        Console.Clear();

        Console.WriteLine("\n");

        while (true)
        {
            var randomLetter = GetRangomLetter();
            Write($"{randomLetter.Key}", ConsoleColor.Cyan);

            if (_guessMode)
            {
                var maxLetters = 5;
                var randomNumber = new Random().Next(0, maxLetters);
                var variantsString = " ";

                for (int i = 0; i < randomNumber; i++)
                    variantsString += $"[{GetRangomLetter(false, randomLetter.Key).Value}] ";

                variantsString += $"[{randomLetter.Value}] ";

                for (int i = randomNumber; i < maxLetters - 1; i++)
                    variantsString += $"[{GetRangomLetter(false, randomLetter.Key).Value}] ";

                Write($"{variantsString}", ConsoleColor.Yellow);
            }
            var letter = Console.ReadLine();

            if (letter == randomLetter.Value)
            {
                WriteDashesLine(ConsoleColor.Green);

                WrongAnswers.Remove(randomLetter.Key);
            }
            else
                switch (letter)
                {
                    case "change":
                        ChooseAlphabet();
                        Console.Clear();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                    {
                        var wrongColor = ConsoleColor.Red;

                        Write($"{randomLetter.Key}({randomLetter.Value})", wrongColor);
                        WriteDashesLine(wrongColor);

                        if (WrongAnswers.ContainsKey(randomLetter.Key) == false)
                            WrongAnswers.Add(randomLetter.Key, randomLetter.Value);

                        break;
                    }
                }
        }
    }

    private static KeyValuePair<string, string> GetRangomLetter(bool withWrongAnswer = true,
        string forbiddenLetter = "")
    {
        var result = KeyValuePair.Create(forbiddenLetter, string.Empty);

        while (result.Key == forbiddenLetter)
        {
            if (withWrongAnswer && WrongAnswers.Count > 0)
            {
                var returnWrongAnswer = new Random().Next(0, 2);

                if (returnWrongAnswer == 0)
                {
                    result = WrongAnswers.ElementAt(WrongAnswers.GetRandomIndex());
                    continue;
                }
            }

            result = _currentAlphabet == Alphabet.Hiragana
                ? HiraganaDictionary.ElementAt(HiraganaDictionary.GetRandomIndex())
                : KatakanaDictionary.ElementAt(KatakanaDictionary.GetRandomIndex());
        }

        return result;
    }

    private static void ChooseAlphabet()
    {
        _currentAlphabet = Alphabet.None;

        while (_currentAlphabet == Alphabet.None)
        {
            Write("Choose alphabet: ", ConsoleColor.Green);
            Console.WriteLine("Hiragana: 1 or h");
            Console.WriteLine("Katakana: 2 or k");

            var keyChar = Console.ReadKey().KeyChar;

            _currentAlphabet =
                (keyChar == 'h' || keyChar == '1') ? Alphabet.Hiragana :
                (keyChar == 'k' || keyChar == '2') ? Alphabet.Katakana :
                Alphabet.None;

            if (_currentAlphabet is not Alphabet.None)
            {
                WrongAnswers.Clear();
            }
        }
    }

    private static void Write(string text, ConsoleColor color = ConsoleColor.White)
    {
        var consoleWidth = Console.WindowWidth;
        var textStartPosition = (consoleWidth - text.Length) / 2;
        var spaces = textStartPosition > 0 ? new string(' ', textStartPosition) : "";


        var defColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(spaces + text);

        Console.ForegroundColor = defColor;
    }

    private static void WriteDashesLine(ConsoleColor color = ConsoleColor.White)
    {
        var consoleWidth = Console.WindowWidth;
        var dashes = consoleWidth > 2 ? new string('-', consoleWidth - 2) : null;

        if (dashes != null)
            Write(dashes, color);
    }
}