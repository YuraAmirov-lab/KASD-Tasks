using System;
using System.IO;
using System.Collections.Generic;

public class Request : IComparable<Request>
{
    public int Id { get; set; }
    public int Priority { get; set; }
    public int StepAdded { get; set; }

    public Request(int id, int priority, int stepAdded)
    {
        Id = id;
        Priority = priority;
        StepAdded = stepAdded;
    }

    public int CompareTo(Request other)
    {
        return other.Priority.CompareTo(this.Priority);
    }
}

public class MyPriorityQueue<T>
{
    private List<T> heap;
    private IComparer<T> comparer;

    public MyPriorityQueue()
    {
        heap = new List<T>();
        comparer = Comparer<T>.Default;
    }

    public int Count => heap.Count;

    public void Add(T item)
    {
        heap.Add(item);
        int i = heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (comparer.Compare(heap[parent], heap[i]) <= 0)
                break;

            Swap(parent, i);
            i = parent;
        }
    }

    public T Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Queue is empty");
        return heap[0];
    }

    public T Poll()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        T result = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        if (heap.Count > 0)
            Heapify(0);

        return result;
    }

    public bool IsEmpty()
    {
        return heap.Count == 0;
    }

    private void Heapify(int i)
    {
        int left = 2 * i + 1;
        int right = 2 * i + 2;
        int smallest = i;

        if (left < heap.Count && comparer.Compare(heap[left], heap[smallest]) < 0)
            smallest = left;

        if (right < heap.Count && comparer.Compare(heap[right], heap[smallest]) < 0)
            smallest = right;

        if (smallest != i)
        {
            Swap(i, smallest);
            Heapify(smallest);
        }
    }

    private void Swap(int i, int j)
    {
        T temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите количество шагов N: ");
        int N = int.Parse(Console.ReadLine());

        MyPriorityQueue<Request> queue = new MyPriorityQueue<Request>();
        Random random = new Random();
        int requestId = 1;
        int maxWaitTime = 0;
        Request maxWaitRequest = null;

        using (StreamWriter logFile = new StreamWriter("log.txt"))
        {
            for (int step = 1; step <= N; step++)
            {
                Console.WriteLine($"\nШаг {step}:");

                int requestsCount = random.Next(1, 11);
                Console.WriteLine($"Добавлено заявок: {requestsCount}");

                for (int i = 0; i < requestsCount; i++)
                {
                    int priority = random.Next(1, 6);
                    Request request = new Request(requestId, priority, step);
                    queue.Add(request);
                    logFile.WriteLine($"ADD {requestId} {priority} {step}");
                    Console.WriteLine($"  Добавлена заявка {requestId} с приоритетом {priority}");
                    requestId++;
                }

                if (!queue.IsEmpty())
                {
                    Request removed = queue.Poll();
                    int waitTime = step - removed.StepAdded;
                    logFile.WriteLine($"REMOVE {removed.Id} {removed.Priority} {step}");
                    Console.WriteLine($"  Удалена заявка {removed.Id} с приоритетом {removed.Priority} (ожидала {waitTime} шагов)");

                    if (waitTime > maxWaitTime)
                    {
                        maxWaitTime = waitTime;
                        maxWaitRequest = removed;
                    }
                }
                else
                {
                    Console.WriteLine("  Очередь пуста - нечего удалять");
                }
            }
            Console.WriteLine("\nФаза очистки очереди");
            int currentStep = N + 1;
            while (!queue.IsEmpty())
            {
                Request removed = queue.Poll();
                int waitTime = currentStep - removed.StepAdded;
                logFile.WriteLine($"REMOVE {removed.Id} {removed.Priority} {currentStep}");
                Console.WriteLine($"Шаг {currentStep}: Удалена заявка {removed.Id} с приоритетом {removed.Priority} (ожидала {waitTime} шагов)");

                if (waitTime > maxWaitTime)
                {
                    maxWaitTime = waitTime;
                    maxWaitRequest = removed;
                }
                currentStep++;
            }
        }

        if (maxWaitRequest != null)
        {
            Console.WriteLine($"Максимальное время ожидания: {maxWaitTime} шагов");
            Console.WriteLine($"Информация о заявке с максимальным временем ожидания:");
            Console.WriteLine($"  ID заявки: {maxWaitRequest.Id}");
            Console.WriteLine($"  Приоритет: {maxWaitRequest.Priority}");
            Console.WriteLine($"  Шаг добавления: {maxWaitRequest.StepAdded}");
            Console.WriteLine($"  Шаг удаления: {maxWaitRequest.StepAdded + maxWaitTime}");
        }
        else
        {
            Console.WriteLine("Не было обработано ни одной заявки.");
        }

        Console.WriteLine("\nЛог операций сохранен в файл log.txt");
        Console.ReadLine();
    }
}