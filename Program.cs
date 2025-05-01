using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a string to encrypt:"); // AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz
        string input = Console.ReadLine();

        Console.WriteLine("Enter a key:");
        string key = Console.ReadLine();

        // Генерация схемы обмена битами на основе ключа
        List<Tuple<int, int>> swapScheme = GenerateSwapScheme(key);

        byte[] encrypted = Encrypt(input, swapScheme);
        Console.WriteLine($"Encrypted string: {BitConverter.ToString(encrypted)}");

        string decrypted = Decrypt(encrypted, swapScheme);
        Console.WriteLine($"Decrypted string: {decrypted}");

        List<Tuple<int, int>> scheme1 = GenerateSwapScheme(key);
        List<Tuple<int, int>> scheme2 = GenerateSwapScheme(key);

        // scheme1 и scheme2 будут полностью идентичны!
        Console.WriteLine(scheme1.SequenceEqual(scheme2)); // Вывод: True
    }

    static List<Tuple<int, int>> GenerateSwapScheme(string key)
    {
        var swapScheme = new List<Tuple<int, int>>();
        Random random = new Random(key.GetHashCode());

        // Генерация случайной схемы обмена битами
        for (int i = 0; i < 7; i++) // Меняем местами 4 пары бит
        {
            int index1 = random.Next(0, 8);
            int index2 = random.Next(0, 8);
            if (index1 != index2)
            {
                swapScheme.Add(Tuple.Create(index1, index2));
            }
        }

        return swapScheme;
    }

    static byte[] Encrypt(string input, List<Tuple<int, int>> swapScheme)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = SwapBits(bytes[i], swapScheme);
        }

        return bytes;
    }

    static string Decrypt(byte[] input, List<Tuple<int, int>> swapScheme)
    {
        swapScheme.Reverse();

        for (int i = 0; i < input.Length; i++)
        {
            input[i] = SwapBits(input[i], swapScheme);
        }

        return Encoding.UTF8.GetString(input);
    }

    static byte SwapBits(byte b, List<Tuple<int, int>> swapScheme)
    {
        // Получаем двоичное представление байта
        char[] bits = Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray();

        // Меняем местами биты по схеме
        foreach (var swap in swapScheme)
        {
            int index1 = swap.Item1;
            int index2 = swap.Item2;

            // Меняем местами биты
            char temp = bits[index1];
            bits[index1] = bits[index2];
            bits[index2] = temp;
        }

        // Преобразуем обратно в байт
        return Convert.ToByte(new string(bits), 2);
    }

    static byte[] EncryptAndPack(string input, string key)
    {
        // Генерация схемы
        List<Tuple<int, int>> swapScheme = GenerateSwapScheme(key);

        // Сериализация схемы в бинарный формат (например, битовая упаковка)
        byte[] schemeBytes = SerializeToBitmask(swapScheme);

        // Шифрование данных
        byte[] encryptedData = Encrypt(input, swapScheme);

        // Объединение: длина схемы (ushort) + схема + зашифрованные данные
        byte[] lengthBytes = BitConverter.GetBytes((ushort)schemeBytes.Length);
        byte[] result = new byte[lengthBytes.Length + schemeBytes.Length + encryptedData.Length];
        
        Buffer.BlockCopy(lengthBytes, 0, result, 0, lengthBytes.Length);
        Buffer.BlockCopy(schemeBytes, 0, result, lengthBytes.Length, schemeBytes.Length);
        Buffer.BlockCopy(encryptedData, 0, result, lengthBytes.Length + schemeBytes.Length, encryptedData.Length);

        return result;
    }

    static string DecryptUnpack(byte[] packedData, string key)
    {
        // Извлечение длины схемы (первые 2 байта)
        ushort schemeLength = BitConverter.ToUInt16(packedData, 0);

        // Извлечение байтов схемы
        byte[] schemeBytes = new byte[schemeLength];
        Buffer.BlockCopy(packedData, 2, schemeBytes, 0, schemeLength);

        // Десериализация схемы
        List<Tuple<int, int>> swapScheme = DeserializeFromBitmask(schemeBytes);

        // Извлечение зашифрованных данных
        byte[] encryptedData = new byte[packedData.Length - 2 - schemeLength];
        Buffer.BlockCopy(packedData, 2 + schemeLength, encryptedData, 0, encryptedData.Length);

        // Дешифровка (с обратным порядком обменов)
        swapScheme.Reverse();
        return Decrypt(encryptedData, swapScheme);
    }

    static byte[] SerializeToBitmask(List<Tuple<int, int>> swapScheme)
    {
        byte[] bytes = new byte[swapScheme.Count];
        for (int i = 0; i < swapScheme.Count; i++)
        {
            int index1 = swapScheme[i].Item1;
            int index2 = swapScheme[i].Item2;
            bytes[i] = (byte)((index1 << 3) | index2); // 3 бита на индекс
        }
        return bytes;
    }

    static List<Tuple<int, int>> DeserializeFromBitmask(byte[] data)
    {
        var scheme = new List<Tuple<int, int>>();
        foreach (byte b in data)
        {
            int index1 = (b >> 3) & 0x07;
            int index2 = b & 0x07;
            scheme.Add(Tuple.Create(index1, index2));
        }
        return scheme;
    }
}
