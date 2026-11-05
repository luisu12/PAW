using PAW.Tasks;

Console.WriteLine("Hello, World!");

Sync.Execute(1000, "AAA");

Sync.Execute(1000, "BBB").Wait();

Console.ReadLine();
Console.ReadKey();