namespace kanaTrainer;

public static class DictionaryExtensions
{
    public static int GetRandomIndex(this Dictionary<string, string> dictionary) => new Random().Next(0, dictionary.Count);
}