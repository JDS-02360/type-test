using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

/*

TODO:

    Quit or replay functionality: DONE
        When finished have text say "Do it again? yes/no"
        If yes, restart application with new sentence
        If no, close application
        If something else, say so, close application

    Get random sentence input from data.txt file: DONE
        Split on new line
        Choose random index of array
        Enter as input to TypeCheck()

    Replace sentence while typing:
        Have text in background
        While typing replace with typed letters, and either green or red colors

*/

internal class Program
{
    readonly static string path = GetPath();

    private static void TypeCheck(string sentence)
    {
        Console.ForegroundColor = ConsoleColor.White;

        string[] split = sentence.Split(' ');

        Console.WriteLine("Repeat this sentence:");
        Console.WriteLine(sentence);

        Stopwatch stopwatch = new();

        if (Console.In is not null)
        {
            stopwatch.Start();
        }

        string[]? typed = Console.ReadLine().Split(' ');

        stopwatch.Stop();

        TimeSpan timeSpan = stopwatch.Elapsed;

        if (typed is not null)
        {
            int i = 0;
            int correct = 0;

            foreach (string s in split)
            {
                try
                {
                    if (typed[i] == s)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        if (i == split.Length)
                        {
                            Console.Write(typed[i]);
                        }
                        else
                        {
                            Console.Write($"{typed[i]} ");
                        }

                        correct++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        if (i == split.Length)
                        {
                            Console.Write(typed[i]);
                        }
                        else
                        {
                            Console.Write($"{typed[i]} ");
                        }
                    }

                    i++;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("\n\nAn exception has been found: the length of the variables, sentence and typed, may not have been the same length.");

                    throw;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;

            double wpm = typed.Length / timeSpan.TotalMinutes;

            Console.WriteLine('\n');
            Console.WriteLine($"You got {correct} out of {split.Length} words correct.");
            Console.WriteLine($"Your WPM is {wpm}.");

            Replay();
        }
    }

    public static string RandomString(string path)
    {
        try
        {
            using StreamReader streamReader = new(path);

            List<string> lines = [];

            string? line;

            while ((line = streamReader.ReadLine()) is not null)
            {
                lines.Add(line);
            }

            streamReader.Close();

            if (lines.Count > 0)
            {
                Random random = new();

                int i = random.Next(lines.Count);

                return lines[i];
            }

            return "The quick brown fox jumped over the lazy dogs.";
        }
        catch (Exception)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n\nAn exception has been found:");

            throw;
        }
    }

    public static void Replay()
    {
        Replay:

        Console.WriteLine("\nDo it again?");

        string? replay = Console.ReadLine();

        if (replay == "yes")
        {
            TypeCheck(RandomString(path));
        }
        else if (replay == "no")
        {
            Console.WriteLine("Press any key to quit application.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("That was not one of the options.");

            goto Replay;
        }
    }

    public static string GetPath()
    {
        Console.WriteLine("Enter the path to the data.txt file.");

        string? path = Console.ReadLine();

        if (path is not null)
        {
            return path;
        }
        else
        {
            // Return default path when running with dotnet run
            return "./data.txt";
        }
    }

    public static void Main(string[] args)
    {
        Console.Clear();
        
        TypeCheck(RandomString(path));
    }
}
