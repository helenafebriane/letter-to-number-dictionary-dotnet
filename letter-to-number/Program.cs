using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Dictionary<char, int> letToNum = new Dictionary<char, int>()
    {
        {'A',0},
        {'B',1},{'C',1},{'D',1},
        {'E',2},
        {'F',3},{'G',3},{'H',3},
        {'I',4},
        {'J',5},{'K',5},{'L',5},{'M',5},{'N',5},
        {'O',6},
        {'P',7},{'Q',7},{'R',7},{'S',7},{'T',7},
        {'U',8},
        {'V',9},{'W',9},{'X',9},{'Y',9},{'Z',9},
        {'a',9},
        {'b',8},{'c',8},{'d',8},
        {'e',7},
        {'f',6},{'g',6},{'h',6},
        {'i',5},
        {'j',4},{'k',4},{'l',4},{'m',4},{'n',4},
        {'o',3},
        {'p',2},{'q',2},{'r',2},{'s',2},{'t',2},
        {'u',1},
        {'v',0},{'w',0},{'x',0},{'y',0},{'z',0},
        {' ',0}
    };

    static Dictionary<int, char> numToLet = new Dictionary<int, char>()
    {
        {0,'A'},
        {1,'B'},
        {2,'E'},
        {3,'F'},
        {4,'I'},
        {5,'J'},
        {6,'O'},
        {7,'P'},
        {8,'U'},
        {9,'V'}
    };

    static void Main()
    {
        ShowHeader();
        while (true)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim();
            if (choice == "1") RunProcessor();
            else if (choice == "2") ShowDictionary();
            else if (choice == "0") break;
            else Console.WriteLine("Invalid option. Try again.");
        }
    }

    static void ShowHeader()
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("        LETTER TO NUMBER CLI       ");
        Console.WriteLine("===================================\n");
    }

    static void ShowMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Process a sentence");
        Console.WriteLine("2. Show Dictionary");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }

    static void RunProcessor()
    {
        Console.Write("\nEnter a sentence: ");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("No input provided!"); return; }
        Console.WriteLine("\nProcessing...\n");
        ProcessText(input);
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        Console.Clear();
        ShowHeader();
    }

    static void ProcessText(string input)
    {
        // Step 1: text to numbers
        var step1 = input.Select(c => letToNum.ContainsKey(c) ? letToNum[c] : 0).ToList();
        Console.WriteLine("Step 1: " + string.Join(" ", step1));

        // Step 2: alternate sum
        int result = step1[0];
        string expr = step1[0].ToString();
        for (int i = 1; i < step1.Count; i++)
        {
            if (i % 2 == 1)
            {
                expr += " + " + step1[i];
                result += step1[i];
            }
            else
            {
                expr += " - " + step1[i];
                result -= step1[i];
            }
        }
        Console.WriteLine("Step 2: " + expr + " = " + result);

        // Step 3: number to letter sequence
        var step3 = NumToLetSeq(result);
        Console.WriteLine("Step 3: " + string.Join(" ", step3));

        // Step 4: refine sequence
        var step4 = RefineSeq(step3);
        Console.WriteLine("Step 4: " + string.Join(" ", step4));

        // Step 5: final number sequence
        var step5 = step4.Select(ch =>
        {
            int n = letToNum[ch];
            return n % 2 == 0 ? (n + 1) % 10 : n;
        }).ToList();
        Console.WriteLine("Step 5: " + string.Join(" ", step5));
    }

    static List<char> NumToLetSeq(int num)
    {
        num = Math.Abs(num);
        if (num == 0) return new List<char> { 'A' };

        List<int> seq = new List<int>();
        int sum = 0, d = 0;
        while (sum < num)
        {
            if (sum + d <= num)
            {
                seq.Add(d);
                sum += d;
            }
            d = (d + 1) % 10;
        }
        return seq.Select(n => numToLet[n]).ToList();
    }

    static List<char> RefineSeq(List<char> seqLetters)
    {
        var nums = seqLetters.Select(c => letToNum.ContainsKey(c) ? letToNum[c] : 0).ToList();
        int result = nums[0];
        for (int i = 1; i < nums.Count; i++)
        {
            if (i % 2 == 1) result += nums[i];
            else result -= nums[i];
        }

        var replacement = NumToLetSeq(result).Skip(1).Take(2).ToList();
        var refined = seqLetters.Take(seqLetters.Count - 2).Concat(replacement).ToList();
        return refined;
    }

    static void ShowDictionary()
    {
        Console.Clear();
        Console.WriteLine("=========== NUM → LETTER ===========");
        foreach (var kvp in numToLet)
        {
            Console.WriteLine($"{kvp.Key} -> {kvp.Value}");
        }

        Console.WriteLine("\n=========== LETTER → NUM ===========");
        foreach (var kvp in letToNum)
        {
            Console.WriteLine($"{kvp.Key} -> {kvp.Value}");
        }

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
        Console.Clear();
        ShowHeader();
    }
}
