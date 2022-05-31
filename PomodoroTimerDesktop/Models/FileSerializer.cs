using Newtonsoft.Json;
using PomodoroTimerDesktop.Abstractions;
using System;
using System.IO;

namespace PomodoroTimerDesktop.Models
{
    public class FileSerializer : IFileSerializer
    {
        private JsonSerializerSettings _settings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        public T Deserialize<T>(string fileName) where T : new()
        {
            T deserializedObject;

            try
            {
                using var streamReader = new StreamReader($"{fileName}.json");
                string obj = streamReader.ReadToEnd();

                deserializedObject = JsonConvert.DeserializeObject<T>(obj);
            }
            catch
            {
                return new T();
            }

            return deserializedObject;
        }

        public bool Serialize(object obj, string fileName)
        {
            try
            {
                string serializedObj = JsonConvert.SerializeObject(obj, _settings);
                string[] lines = serializedObj.Split(Environment.NewLine);

                using var streamWriter = new StreamWriter($"{fileName}.json");

                foreach (var line in lines)
                {
                    streamWriter.WriteLine(line);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
