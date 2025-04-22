using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string[] stringNumbers = { "10", "20", "abc", "30", "40", "def", "50", "60", "70", "80" };

        try
        {
            // 1. Преобразование строк в числа с обработкой исключений
            var numbers = stringNumbers
                .Select(s =>
                {
                    try
                    {
                        return int.Parse(s);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Ошибка преобразования: '{s}' не является числом");
                        return (int?)null;
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine($"Ошибка: число '{s}' слишком большое");
                        return (int?)null;
                    }
                })
                .Where(n => n.HasValue)
                .Select(n => n.Value)
                .ToArray();

            Console.WriteLine("\nУспешно преобразованные числа:");
            foreach (var num in numbers)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine();

            // 2. Подсчет суммы элементов, делящихся на x (последняя цифра индекса)
            int sum = numbers
                .AsParallel() 
                .Select((num, index) =>
                {
                    int x = index % 10;
                    if (x == 0) x = 10;
                    return new { Number = num, Index = index, X = x };
                })
                .Where(item => item.Number % item.X == 0)
                .Sum(item => item.Number);

            Console.WriteLine($"\nСумма элементов, делящихся на последнюю цифру их индекса: {sum}");
        }
        catch (AggregateException ae)
        {
            foreach (var e in ae.InnerExceptions)
            {
                Console.WriteLine($"PLINQ ошибка: {e.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }
    }
}