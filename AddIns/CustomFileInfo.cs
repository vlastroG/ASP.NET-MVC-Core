namespace AddIns
{
    public static class CustomFileInfo
    {
        /// <summary>
        /// Читает файл асинхронно построчно и возвращает перечисление строк этого файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Пуречисление всех строк файла.</returns>
        private static async Task<IEnumerable<string>> ReadCharacters(string @path)
        {
            List<string> allLines = new List<string>();

            using (StreamReader reader = new StreamReader(@path))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    allLines.Add(line);
                }
            }
            return allLines as IEnumerable<string>;
        }


        /// <summary>
        /// Проверяет, является ли строка путем к файлу.
        /// </summary>
        /// <param name="path">Строка.</param>
        /// <returns>Является ли строка путем к файлу: true/false.</returns>
        private static bool ValidateFilePath(string? @path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }
            FileInfo fileInfo = new FileInfo(@path);
            if (!fileInfo.Exists)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет поданное перечисление строк на наличие корректных путей к файлам и возвращает их.
        /// </summary>
        /// <param name="stringPathsArray">Перечисление строк - путей к файлам.</param>
        /// <returns>Перечисление строковых путей к файлам.</returns>
        private static IEnumerable<string> ValidateFilePaths(IEnumerable<string> stringPathsArray)
        {
            List<string> filePaths = new();

            foreach (var path in stringPathsArray)
            {
                if (ValidateFilePath(path))
                {
                    filePaths.Add(path);
                }
            }
            return filePaths;
        }

        /// <summary>
        /// Принимает массив строк, являющихся путями к файлам, и считывает все строки из них.
        /// Полученные строки записывает в новый файл.
        /// </summary>
        /// <param name="PathArray">Массив строк - путей к файлам.</param>
        /// <returns>Перечисление всех строк всех файлов.</returns>
        public async static Task<IEnumerable<string>> GetFilesLinesAsync(params string[] pathsArray)
        {
            string[] filePaths = ValidateFilePaths(pathsArray).ToArray();
            List<string> allLinesOfAllFiles = new();

            foreach (string path in filePaths)
            {
                List<string>? allLinesOfFile = new();
                allLinesOfFile = await ReadCharacters(path) as List<string>;

#pragma warning disable CS8604 // Possible null reference argument.
                allLinesOfAllFiles.AddRange(allLinesOfFile);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            return allLinesOfAllFiles;
        }
    }
}
