using System;
using System.Collections.Generic;
using System.Text;

namespace PAW.Tasks;

public class Sync
{
    public static async Task Execute(int secs, string identifier)
    {
        await Task.Run(() =>
        {
            for (int i = 0; i < secs; i++)
            {
                Console.WriteLine($"Second {i + 1}");
            }
        });
    }

    public static async Task ExecuteEval(int secs, string identifier)
    {
        await Task.Run(() =>
        {
            for (int i = 0; i < secs; i++)
            {
                if (i % 1000 == 0)
                    Console.WriteLine($"{identifier} - Second {i + 1}");
            }
        });
    }

    public static async Task<IEnumerable<string>> ExecuteItems(int secs, string identifier)
    {
        var items = new List<string>();
        await Task.Run(() =>
        {
            for (int i = 0; i < secs; i++)
            {
                items.Add($"{identifier} - Second {i + 1}");
            }
        });
        return items;
    }
}
