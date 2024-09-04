using System.Text.Json;

namespace WorkerService.Test.Util
{
    internal static class FileParse
    {
        public static T ReturnContentsOfFile<T>(string path)
        {
            using FileStream fileStreamReader = File.OpenRead(path);
            return JsonSerializer.Deserialize<T>(fileStreamReader) ?? throw new Exception("Json return null.");
        }

        public static async Task<T> ReturnContentsOfFileAsync<T>(string path)
        {
            using StreamReader streamReader = new StreamReader(path);
            string content = await streamReader.ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(content) ?? throw new Exception("Json return null.");
        }
    }
}
