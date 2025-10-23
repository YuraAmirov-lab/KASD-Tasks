using System;
class Complex
{
    public double real;
    public double imaginary;
}

class Program
{
    static Complex Number = new Complex();
    static void Main()
    {
        Console.WriteLine("Программа для работы с комплексными числами");
        Console.WriteLine("Начальное число: 0");

        while (true)
        {
            ShowMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    InputNumber();
                    break;
                case "2":
                    Add();
                    break;
                case "3":
                    Subtract();
                    break;
                case "4":
                    Multiply();
                    break;
                case "5":
                    Divide();
                    break;
                case "6":
                    ShowMagnitude();
                    break;
                case "7":
                    ShowArgument();
                    break;
                case "8":
                    ShowReal();
                    break;
                case "9":
                    ShowImaginary();
                    break;
                case "10":
                    Console.WriteLine("Текущее число: " + GetNumberString());
                    break;
                case "q":
                case "Q":
                    Console.WriteLine("Выход из программы...");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        //Console.WriteLine($"Текущее число: {GetNumberString()}");
        Console.WriteLine("Меню:");
        Console.WriteLine("1 - Ввести новое число");
        Console.WriteLine("2 - Сложение");
        Console.WriteLine("3 - Вычитание");
        Console.WriteLine("4 - Умножение");
        Console.WriteLine("5 - Деление");
        Console.WriteLine("6 - Модуль");
        Console.WriteLine("7 - Аргумент");
        Console.WriteLine("8 - Вещественная часть");
        Console.WriteLine("9 - Мнимая часть");
        Console.WriteLine("10 - Вывод");
        Console.WriteLine("q или Q - Выход");
        Console.Write("Выберите операцию: ");
    }

    static string GetNumberString()
    {
        if (Number.imaginary >= 0)
            return $"{Number.real} + {Number.imaginary}i";
        else
            return $"{Number.real} - {-Number.imaginary}i";
    }

    static void InputNumber()
    {
        Console.Write("Введите вещественную часть: ");
        Number.real = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть: ");
        Number.imaginary = double.Parse(Console.ReadLine());
        Console.WriteLine($"Установлено число: {GetNumberString()}");
    }

    static void Add()
    {
        Console.Write("Введите вещественную часть второго числа: ");
        double r2 = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть второго числа: ");
        double i2 = double.Parse(Console.ReadLine());

        Number.real += r2;
        Number.imaginary += i2;
        Console.WriteLine($"Результат: {GetNumberString()}");
    }

    static void Subtract()
    {
        Console.Write("Введите вещественную часть второго числа: ");
        double r2 = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть второго числа: ");
        double i2 = double.Parse(Console.ReadLine());

        Number.real -= r2;
        Number.imaginary -= i2;
        Console.WriteLine($"Результат: {GetNumberString()}");
    }

    static void Multiply()
    {
        Console.Write("Введите вещественную часть второго числа: ");
        double r2 = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть второго числа: ");
        double i2 = double.Parse(Console.ReadLine());

        double newReal = Number.real * r2 - Number.imaginary * i2;
        double newImaginary = Number.real * i2 + Number.imaginary * r2;
        Number.real = newReal;
        Number.imaginary = newImaginary;
        Console.WriteLine($"Результат: {GetNumberString()}");
    }

    static void Divide()
    {
        Console.Write("Введите вещественную часть второго числа: ");
        double r2 = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть второго числа: ");
        double i2 = double.Parse(Console.ReadLine());

        double denominator = r2 * r2 + i2 * i2;
        double newReal = (Number.real * r2 + Number.imaginary * i2) / denominator;
        double newImaginary = (Number.imaginary * r2 - Number.real * i2) / denominator;
        Number.real = newReal;
        Number.imaginary = newImaginary;
        Console.WriteLine($"Результат: {GetNumberString()}");
    }

    static void ShowMagnitude()
    {
        double magnitude = Math.Sqrt(Number.real * Number.real + Number.imaginary * Number.imaginary);
        Console.WriteLine($"Модуль: {magnitude}");
    }

    static void ShowArgument()
    {
        double argument = Math.Atan2(Number.imaginary, Number.real);
        Console.WriteLine($"Аргумент: {argument} радиан");
    }

    static void ShowReal()
    {
        Console.WriteLine($"Вещественная часть: {Number.real}");
    }

    static void ShowImaginary()
    {
        Console.WriteLine($"Мнимая часть: {Number.imaginary}");
    }
}