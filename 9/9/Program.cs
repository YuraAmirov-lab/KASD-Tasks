using System;
using System.IO;
using System.Text;

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
            throw new ArgumentOutOfRangeException(nameof(capacity));

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
        var array = ToArray();
        return $"[{string.Join(", ", array)}] (Size: {size}, Capacity: {Capacity})";
    }
}

class Program
{
    static void Main()
    {
        try
        {
            string[] lines = File.ReadAllLines("input.txt");
            MyArrayList<string> tags = new MyArrayList<string>();

            foreach (string line in lines)
            {
                ExtractTagsFromString(line, tags);
            }

            Console.WriteLine("Все найденные теги:");
            for (int i = 0; i < tags.Size; i++)
            {
                Console.WriteLine(tags.Get(i));
            }

            RemoveDuplicateTags(tags);

            Console.WriteLine("\nУникальные теги после удаления дубликатов:");
            for (int i = 0; i < tags.Size; i++)
            {
                Console.WriteLine(tags.Get(i));
            }

            Console.WriteLine($"\nВсего уникальных тегов: {tags.Size}");

            SaveTagsToFile("output.txt", tags);
            Console.WriteLine("Результат сохранен в output.txt");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл input.txt не найден!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void ExtractTagsFromString(string str, MyArrayList<string> tags)
    {
        int i = 0;
        while (i < str.Length)
        {
            if (str[i] == '<')
            {
                int start = i;
                i++;

                if (i < str.Length && str[i] == '/')
                {
                    i++;
                }

                if (i < str.Length && char.IsLetter(str[i]))
                {
                    while (i < str.Length && (char.IsLetterOrDigit(str[i])))
                    {
                        i++;
                    }

                    if (i < str.Length && str[i] == '>')
                    {
                        string tag = str.Substring(start, i - start + 1);
                        if (IsValidTag(tag))
                        {
                            tags.Add(tag);
                        }
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
            else
            {
                i++;
            }
        }
    }

    static bool IsValidTag(string tag)
    {
        if (tag.Length < 3) return false;
        if (tag[0] != '<' || tag[tag.Length - 1] != '>') return false;

        int startIndex = 1;
        if (tag[1] == '/')
        {
            startIndex = 2;
            if (tag.Length < 4) return false;
        }

        if (!char.IsLetter(tag[startIndex])) return false;

        for (int i = startIndex + 1; i < tag.Length - 1; i++)
        {
            if (!char.IsLetterOrDigit(tag[i]))
            {
                return false;
            }
        }

        return true;
    }

    static void RemoveDuplicateTags(MyArrayList<string> tags)
    {
        MyArrayList<string> uniqueTags = new MyArrayList<string>();

        for (int i = 0; i < tags.Size; i++)
        {
            string currentTag = tags.Get(i);
            bool isDuplicate = false;

            for (int j = 0; j < uniqueTags.Size; j++)
            {
                if (AreTagsEqual(currentTag, uniqueTags.Get(j)))
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                uniqueTags.Add(currentTag);
            }
        }

        tags.Clear();
        for (int i = 0; i < uniqueTags.Size; i++)
        {
            tags.Add(uniqueTags.Get(i));
        }
    }

    static bool AreTagsEqual(string tag1, string tag2)
    {
        string normalizedTag1 = NormalizeTag(tag1);
        string normalizedTag2 = NormalizeTag(tag2);

        return normalizedTag1 == normalizedTag2;
    }

    static string NormalizeTag(string tag)
    {
        string result = tag.Substring(1, tag.Length - 2);

        if (result.Length > 0 && result[0] == '/')
        {
            result = result.Substring(1);
        }

        return result.ToLower();
    }

    static void SaveTagsToFile(string filename, MyArrayList<string> tags)
    {
        using (StreamWriter writer = new StreamWriter(filename, false, Encoding.UTF8))
        {
            writer.WriteLine($"Всего тегов: {tags.Size}");
            writer.WriteLine("=====================");

            for (int i = 0; i < tags.Size; i++)
            {
                writer.WriteLine(tags.Get(i));
            }
        }
    }
}