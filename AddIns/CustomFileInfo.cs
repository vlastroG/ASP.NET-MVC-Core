using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddIns
{
    public static class CustomFileInfo
    {
        /// <summary>
        /// Проверяет, является ли строка путем к файлу.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool ValidateFilePath(string? path)
        {
            if (path is null)
            {
                return false;
            }
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Принимает массив строк, являющихся путями к файлам, и считывает все строки из них.
        /// Полученные строки записывает в новый файл.
        /// </summary>
        /// <param name="PathArray"></param>
        /// <returns></returns>
        public async static Task<string> GetFilesLinesAsync(string[] PathArray)
        {

        }
    }
}
