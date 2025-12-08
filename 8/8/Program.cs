using System;

public class MyArrayList<T>
{
    private T[] elementData;
    private int size;
    private const int DefaultCapacity = 10;

    public MyArrayList()
    {
        elementData = new T[DefaultCapacity];
        size = 0;
    }

    public MyArrayList(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        size = a.Length;
    }

    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Емкость не может быть отрицательной");

        elementData = new T[capacity];
        size = 0;
    }

    public int Size => size;
    public int Capacity => elementData.Length;
    public bool IsEmpty => size == 0;

    public void Add(T e)
    {
        EnsureCapacity(size + 1);
        elementData[size++] = e;
    }

    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        EnsureCapacity(size + a.Length);
        Array.Copy(a, 0, elementData, size, a.Length);
        size += a.Length;
    }

    public void Clear()
    {
        for (int i = 0; i < size; i++)
        {
            elementData[i] = default(T);
        }
        size = 0;
    }

    public bool Contains(object o)
    {
        return IndexOf(o) >= 0;
    }

    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }

    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    public void RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
        {
            while (Remove(item)) { }
        }
    }

    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        var newArray = new T[size];
        int newSize = 0;

        for (int i = 0; i < size; i++)
        {
            if (Array.IndexOf(a, elementData[i]) >= 0)
            {
                newArray[newSize++] = elementData[i];
            }
        }

        elementData = newArray;
        size = newSize;
    }

    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(elementData, result, size);
        return result;
    }

    public T[] ToArray(T[] a)
    {
        if (a == null)
            return ToArray();

        if (a.Length < size)
        {
            return ToArray();
        }

        Array.Copy(elementData, a, size);
        if (a.Length > size)
        {
            a[size] = default(T);
        }
        return a;
    }

    public void Add(int index, T e)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));

        EnsureCapacity(size + 1);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + 1, size - index);
        }
        elementData[index] = e;
        size++;
    }

    public void AddAll(int index, T[] a)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        EnsureCapacity(size + a.Length);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + a.Length, size - index);
        }
        Array.Copy(a, 0, elementData, index, a.Length);
        size += a.Length;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));

        return elementData[index];
    }

    public int IndexOf(object o)
    {
        for (int i = 0; i < size; i++)
        {
            if (Equals(elementData[i], o))
                return i;
        }
        return -1;
    }

    public int LastIndexOf(object o)
    {
        for (int i = size - 1; i >= 0; i--)
        {
            if (Equals(elementData[i], o))
                return i;
        }
        return -1;
    }

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));

        T removed = elementData[index];
        int numMoved = size - index - 1;
        if (numMoved > 0)
        {
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        }
        elementData[--size] = default(T);
        return removed;
    }

    public T Set(int index, T e)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));

        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }

    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        int newSize = toIndex - fromIndex;
        T[] newArray = new T[newSize];
        Array.Copy(elementData, fromIndex, newArray, 0, newSize);
        return new MyArrayList<T>(newArray);
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > elementData.Length)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;

            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, size);
            elementData = newArray;
        }
    }

    public T this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", ToArray())}] (Size: {size}, Capacity: {Capacity})";
    }
}
class Program
{
    static void Main()
    {
        var list1 = new MyArrayList<int>();
        list1.Add(1);
        list1.Add(2);
        list1.Add(3);
        Console.WriteLine($"list1: {list1}");

        int[] arr = { 4, 5, 6 };
        var list2 = new MyArrayList<int>(arr);
        Console.WriteLine($"list2: {list2}");
        list1.Add(1, 99);
        Console.WriteLine($"list1 после вставки: {list1}");
        list1.Remove(2);
        Console.WriteLine($"list1 после удаления 2: {list1}");
        var subList = list1.SubList(1, 3);
        Console.WriteLine($"subList: {subList}");
        Console.WriteLine($"Содержит 99? {list1.Contains(99)}");
        Console.WriteLine($"Размер: {list1.Size}, Ёмкость: {list1.Capacity}");
        list1.Clear();
        Console.WriteLine($"list1 после очистки: {list1}");
    }
}