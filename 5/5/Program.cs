using System;
using System.Collections.Generic;

public class MaxHeap
{
    private List<int> items;

    public MaxHeap()
    {
        items = new List<int>();
    }

    public MaxHeap(int[] numbers)
    {
        items = new List<int>(numbers);
        BuildHeap();
    }

    public int Max()
    {
        if (items.Count == 0)
        {
            throw new Exception("Куча пуста");
        }
        return items[0];
    }

    public int ExtractMax()
    {
        if (items.Count == 0)
        {
            throw new Exception("Куча пуста");
        }

        int max = items[0];

        items[0] = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);

        if (items.Count > 0)
        {
            Down(0);
        }

        return max;
    }

    public void Add(int value)
    {
        items.Add(value);
        Up(items.Count - 1);
    }

    public void ChangeValue(int index, int newValue)
    {
        if (index < 0 || index >= items.Count)
        {
            throw new Exception("Неверный индекс");
        }

        int oldValue = items[index];
        items[index] = newValue;

        if (newValue > oldValue)
        {
            Up(index);
        }
        else 
        {
            Down(index);
        }
    }

    public void Merge(MaxHeap otherHeap)
    {
        for (int i = 0; i < otherHeap.GetCount(); i++)
        {
            Add(otherHeap.GetItem(i));
        }
    }

    public int GetCount()
    {
        return items.Count;
    }

    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    private int GetItem(int index)
    {
        return items[index];
    }

    private void Up(int index)
    {
        int currentIndex = index;

        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;

            if (items[currentIndex] > items[parentIndex])
            {
                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
            }
            else
            {
                break;
            }
        }
    }

    private void Down(int index)
    {
        int currentIndex = index;

        while (true)
        {
            int leftChildIndex = 2 * currentIndex + 1;
            int rightChildIndex = 2 * currentIndex + 2;
            int largestIndex = currentIndex;

            if (leftChildIndex < items.Count &&
                items[leftChildIndex] > items[largestIndex])
            {
                largestIndex = leftChildIndex;
            }

            if (rightChildIndex < items.Count &&
                items[rightChildIndex] > items[largestIndex])
            {
                largestIndex = rightChildIndex;
            }

            if (largestIndex == currentIndex)
            {
                break;
            }

            Swap(currentIndex, largestIndex);
            currentIndex = largestIndex;
        }
    }

    private void BuildHeap()
    {
        for (int i = items.Count / 2 - 1; i >= 0; i--)
        {
            Down(i);
        }
    }

    private void Swap(int index1, int index2)
    {
        int temp = items[index1];
        items[index1] = items[index2];
        items[index2] = temp;
    }

    public void Print()
    {
        Console.WriteLine("Куча: " + string.Join(" ", items));
    }
}

class Program
{
    static void Main()
    {

        int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };
        MaxHeap heap = new MaxHeap(numbers);
        heap.Print();
        Console.WriteLine("Максимальный элемент: " + heap.Max());

        Console.WriteLine("\nИзвлекли максимум: " + heap.ExtractMax());
        heap.Print();
        Console.WriteLine("Новый максимум: " + heap.Max());
        heap.Add(10);
        Console.WriteLine("\nДобавили 10:");
        heap.Print();
        Console.WriteLine("Максимум теперь: " + heap.Max());
        heap.Add(7);
        Console.WriteLine("Добавили 7:");
        heap.Print();
        Console.WriteLine("\nИзменили элемент с индексом 2 на 12:");
        heap.ChangeValue(2, 12);
        heap.Print();
        Console.WriteLine("Максимум: " + heap.Max());
        MaxHeap heap1 = new MaxHeap(new int[] { 10, 5, 3 });
        MaxHeap heap2 = new MaxHeap(new int[] { 8, 2, 7 });

        Console.WriteLine("Куча 1:");
        heap1.Print();
        Console.WriteLine("Куча 2:");
        heap2.Print();
        heap1.Merge(heap2);
        Console.WriteLine("После слияния:");
        heap1.Print();
        Console.WriteLine("\nИзвлекаем все элементы по порядку:");
        while (!heap1.IsEmpty())
        {
            Console.Write(heap1.ExtractMax() + " ");
        }
    }
}