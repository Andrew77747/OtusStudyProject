namespace InteractiveMenu;

public class ToDoConfigurator
{
    public static (int tasksCount, int taskLength) GetSettings()
    {
        Console.WriteLine("Введите максимальное количество задач (от 1 до 100):");
        var tasksCount = ReadInt(1, 100);

        Console.WriteLine("Введите максимальную длину задачи (от 1 до 100):");
        var taskLength = ReadInt(1, 100);

        return (tasksCount, taskLength);
    }

    private static int ReadInt(int min, int max)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out var num) && num >= min && num <= max)
                return num;

            Console.WriteLine($"Ошибка! Введите число от {min} до {max}:");
        }
    }
}