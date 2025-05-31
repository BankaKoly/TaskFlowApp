using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TaskFlowApp
{
    public class XmlManager
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TaskFlowApp",
            "tasks.xml"
        );

        static XmlManager()
        {
            // Створюємо папку якщо не існує
            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void SaveTasks(List<Task> tasks)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
                using (FileStream stream = new FileStream(FilePath, FileMode.Create))
                {
                    serializer.Serialize(stream, tasks);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка збереження: {ex.Message}");
            }
        }

        public static List<Task> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<Task>();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
                using (FileStream stream = new FileStream(FilePath, FileMode.Open))
                {
                    var tasks = (List<Task>)serializer.Deserialize(stream);
                    return tasks ?? new List<Task>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка завантаження: {ex.Message}");
            }
        }
    }
}