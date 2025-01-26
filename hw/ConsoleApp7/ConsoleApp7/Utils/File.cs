namespace ConsoleApp7.Utils;
using System.Text.Json;
using System.IO;
using System;

public class FileHelper
{
    public static void SerializeToFile<T>(string filePath, T data)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to {filePath}: {ex.Message}");
        }
    }

    public static T DeserializeFromFile<T>(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from {filePath}: {ex.Message}");
        }
        return default;
    }
}