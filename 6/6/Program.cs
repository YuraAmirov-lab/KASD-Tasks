using System;
using System.Collections.Generic;

public class MyPriorityQueue<T>
{
    private T[] queue;
    private int size;
    private IComparer<T> comparator;

    public MyPriorityQueue()
    {
        queue = new T[11];
        comparator = Comparer<T>.Default;
    }

    public MyPriorityQueue(T[] a)
    {
        if (a == null)
        {
            throw new ArgumentNullException(nameof(a));
        }
        queue = new T[a.Length];
        Array.Copy(a, queue, a.Length);
        size = a.Length;
        comparator = Comparer<T>.Default;
        Heapify();
    }

    public MyPriorityQueue(int initialCapacity)
    {
        if (initialCapacity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCapacity));
        }
        queue = new T[initialCapacity];
        comparator = Comparer<T>.Default;
    }

    public MyPriorityQueue(int initialCapacity, IComparer<T> comparer)
    {
        if (initialCapacity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCapacity));
        }
        queue = new T[initialCapacity];
        if (comparer == null)
        {
            this.comparator = Comparer<T>.Default;
        }
        else
        {
            this.comparator = comparer;
        }
    }

    public MyPriorityQueue(MyPriorityQueue<T> c)
    {
        if (c == null)
        {
            throw new ArgumentNullException(nameof(c));
        }
        queue = new T[c.queue.Length];
        Array.Copy(c.queue, queue, c.size);
        size = c.size;
        comparator = c.comparator;
    }

    public void Add(T e)
    {
        if (e == null)
        {
            throw new ArgumentNullException(nameof(e));
        }
        EnsureCapacity(size + 1);
        queue[size] = e;
        SiftUp(size);
        size++;
    }

    public void AddAll(T[] a)
    {
        if (a == null)
        {
            throw new ArgumentNullException(nameof(a));
        }
        EnsureCapacity(size + a.Length);
        foreach (var item in a)
        {
            if (item == null)
            {
                throw new ArgumentException("Array contains null");
            }
            queue[size] = item;
            SiftUp(size);
            size++;
        }
    }

    public void Clear()
    {
        Array.Clear(queue, 0, size);
        size = 0;
    }

    public bool Contains(object o)
    {
        if (o == null)
        {
            throw new ArgumentNullException(nameof(o));
        }
        for (int i = 0; i < size; i++)
        {
            if (EqualityComparer<T>.Default.Equals((T)o, queue[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null)
        {
            throw new ArgumentNullException(nameof(a));
        }
        foreach (var item in a)
        {
            if (!Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsEmpty()
    {
        if (size == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Remove(object o)
    {
        if (o == null)
        {
            throw new ArgumentNullException(nameof(o));
        }
        for (int i = 0; i < size; i++)
        {
            if (EqualityComparer<T>.Default.Equals((T)o, queue[i]))
            {
                RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void RemoveAll(T[] a)
    {
        if (a == null)
        {
            throw new ArgumentNullException(nameof(a));
        }
        foreach (var item in a)
        {
            while (Remove(item)) { }
        }
    }

    public void RetainAll(T[] a)
    {
        if (a == null)
        {
            throw new ArgumentNullException(nameof(a));
        }
        var temp = new List<T>();
        for (int i = 0; i < size; i++)
        {
            bool found = false;
            foreach (var element in a)
            {
                if (EqualityComparer<T>.Default.Equals(element, queue[i]))
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                temp.Add(queue[i]);
            }
        }
        Clear();
        foreach (var item in temp)
        {
            Add(item);
        }
    }

    public int Size()
    {
        return size;
    }

    public T[] ToArray()
    {
        var result = new T[size];
        Array.Copy(queue, result, size);
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null)
        {
            return ToArray();
        }
        if (a.Length < size)
        {
            return ToArray();
        }
        Array.Copy(queue, a, size);
        if (a.Length > size)
        {
            a[size] = default(T);
        }
        return a;
    }

    public T Element()
    {
        if (size == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        return queue[0];
    }

    public bool Offer(T obj)
    {
        try
        {
            Add(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public T Peek()
    {
        if (size == 0)
        {
            return default(T);
        }
        else
        {
            return queue[0];
        }
    }

    public T Poll()
    {
        if (size == 0)
        {
            return default(T);
        }
        T result = queue[0];
        RemoveAt(0);
        return result;
    }

    private void Heapify()
    {
        for (int i = (size - 1) / 2; i >= 0; i--)
        {
            SiftDown(i);
        }
    }

    private void SiftUp(int idx)
    {
        T x = queue[idx];
        while (idx > 0)
        {
            int parent = (idx - 1) / 2;
            if (comparator.Compare(x, queue[parent]) >= 0)
            {
                break;
            }
            queue[idx] = queue[parent];
            idx = parent;
        }
        queue[idx] = x;
    }

    private void SiftDown(int idx)
    {
        T x = queue[idx];
        while (idx * 2 + 1 < size)
        {
            int child = idx * 2 + 1;
            if (child + 1 < size && comparator.Compare(queue[child], queue[child + 1]) > 0)
            {
                child++;
            }
            if (comparator.Compare(x, queue[child]) <= 0)
            {
                break;
            }
            queue[idx] = queue[child];
            idx = child;
        }
        queue[idx] = x;
    }

    private void RemoveAt(int idx)
    {
        size--;
        if (idx == size)
        {
            queue[idx] = default(T);
            return;
        }
        queue[idx] = queue[size];
        queue[size] = default(T);
        SiftDown(idx);
        if (idx < size && comparator.Compare(queue[idx], queue[size]) == 0)
        {
            SiftUp(idx);
        }
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity <= queue.Length)
        {
            return;
        }

        int newCapacity;
        if (queue.Length < 64)
        {
            newCapacity = queue.Length * 2;
        }
        else
        {
            newCapacity = queue.Length + queue.Length / 2;
        }

        Array.Resize(ref queue, newCapacity);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var queue = new MyPriorityQueue<int>();

        queue.Add(5);
        queue.Add(2);
        queue.Add(8);
        queue.Offer(1);

        Console.WriteLine("Размер: " + queue.Size());
        Console.WriteLine("Первый элемент: " + queue.Peek());
        Console.WriteLine("Элемент (с исключением): " + queue.Element());

        Console.WriteLine("Содержит 2: " + queue.Contains(2));
        Console.WriteLine("Содержит все [1,2]: " + queue.ContainsAll(new[] { 1, 2 }));

        var array = queue.ToArray();
        Console.Write("Массив: ");
        for (int i = 0; i < array.Length; i++)
        {
            if (i > 0) Console.Write(", ");
            Console.Write(array[i]);
        }
        Console.WriteLine();

        queue.Remove(2);
        Console.WriteLine("После удаления 2, размер: " + queue.Size());

        Console.WriteLine("Извлечение элементов:");
        while (!queue.IsEmpty())
        {
            Console.WriteLine(queue.Poll());
        }
    }
}