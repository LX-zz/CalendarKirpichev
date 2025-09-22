using System;
using System.Collections.Generic;
using System.Linq;

class SimpleCalendar
{
    private static DateTime currentDate = DateTime.Today;
    private static Dictionary<DateTime, string> notes = new Dictionary<DateTime, string>();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            DisplayMonth();
            DisplayMenu();

            var key = Console.ReadKey().Key;
            HandleInput(key);
        }
    }

    static void DisplayMonth()
    {
        Console.WriteLine($"\n {currentDate:MMMM yyyy}".ToUpper());
        Console.WriteLine(" Пн Вт Ср Чт Пт Сб Вс");

        var firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
        var startDay = firstDay.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)firstDay.DayOfWeek - 1;

        for (int i = 0; i < startDay; i++)
        {
            Console.Write("   ");
        }

        for (int day = 1; day <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month); day++)
        {
            var date = new DateTime(currentDate.Year, currentDate.Month, day);
            var hasNote = notes.ContainsKey(date.Date);
            var isToday = date == DateTime.Today;

            Console.Write($"{(isToday ? "[" : " ")}{day,2}{(hasNote ? "*" : " ")}{(isToday ? "]" : " ")}");

            if ((startDay + day) % 7 == 0)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine("\n");
    }

    static void DisplayMenu()
    {
        Console.WriteLine("←→: Месяцы | ↑↓: Годы | Enter: Заметка | T: Сегодня | S: Поиск | Esc: Выход");
    }

    static void HandleInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                currentDate = currentDate.AddMonths(-1);
                break;
            case ConsoleKey.RightArrow:
                currentDate = currentDate.AddMonths(1);
                break;
            case ConsoleKey.UpArrow:
                currentDate = currentDate.AddYears(1);
                break;
            case ConsoleKey.DownArrow:
                currentDate = currentDate.AddYears(-1);
                break;
            case ConsoleKey.Enter:
                AddNote();
                break;
            case ConsoleKey.T:
                currentDate = DateTime.Today;
                break;
            case ConsoleKey.S:
                SearchNotes();
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
        }
    }

    static void AddNote()
    {
        Console.Write("\nВведите день: ");
        if (int.TryParse(Console.ReadLine(), out int day))
        {
            try
            {
                var date = new DateTime(currentDate.Year, currentDate.Month, day);
                Console.Write("Заметка: ");
                var note = Console.ReadLine();

                if (string.IsNullOrEmpty(note))
                {
                    if (notes.ContainsKey(date))
                        notes.Remove(date);
                }
                else
                {
                    notes[date] = note;
                }
            }
            catch
            {
                Console.WriteLine("Неверный день!");
                Console.ReadKey();
            }
        }
    }

    static void SearchNotes()
    {
        Console.Write("\nПоиск заметок: ");
        var search = Console.ReadLine().ToLower();

        var results = notes.Where(n => n.Value.ToLower().Contains(search))
                          .OrderBy(n => n.Key);

        if (results.Any())
        {
            Console.WriteLine("\nНайдено:");
            foreach (var note in results)
            {
                Console.WriteLine($" {note.Key:dd.MM.yyyy}: {note.Value}");
            }
        }
        else
        {
            Console.WriteLine("Заметки не найдены");
        }

        Console.ReadKey();
    }
}