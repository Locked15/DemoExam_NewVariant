using System;
using System.IO;

namespace _41_размер.Resources
{
    public static class ResourceManager
    {
        /// <summary>
        /// Полный путь к папке с проектом.
        /// </summary>
        public static String PathToProject { get; private set; }

        /// <summary>
        /// Полный путь к папке с ресурсами.
        /// </summary>
        public static String PathToResources { get; private set; }

        /// <summary>
        /// Статический конструктор класса.
        /// </summary>
        static ResourceManager()
        {
            String path = Environment.CurrentDirectory;
            PathToProject = Directory.GetParent(path).Parent.FullName;

            PathToResources = Path.Combine(PathToProject, "Resources");
        }

        /// <summary>
        /// Проверяет указанный файл на существование.
        /// </summary>
        /// <param name="imageName">Путь к файлу для проверки.</param>
        /// <returns>
        /// Если файл существует — абсолютный путь к файлу;<br/>
        /// В ином случае — путь к файлу-заглушке.
        /// </returns>
        public static String GetSafeImagePath(String imageName)
        {
            String path = Path.Combine(PathToResources, "Images");

            if (!String.IsNullOrEmpty(imageName) && File.Exists(Path.Combine(path, imageName)))
            {
                return Path.Combine(path, imageName);
            }

            else
            {
                return Path.Combine(path, "!Picture.png");
            }
        }

        /// <summary>
        /// Сохраняет указанное изображение в папке ресурсов.
        /// </summary>
        /// <param name="fullPath">Полный путь до изображения, которое нужно сохранить.</param>
        /// <returns>Название сохраненного изображения.</returns>
        public static String SaveImageToResources(String fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Copy(fullPath, Path.Combine(PathToResources, "Images", Path.GetFileName(fullPath)), true);

                return Path.GetFileName(fullPath);
            }

            return String.Empty;
        }
    }
}
