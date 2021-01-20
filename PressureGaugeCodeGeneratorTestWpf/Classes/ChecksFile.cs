using System.IO;
using System.Linq;

namespace PressureGaugeCodeGeneratorTestWpf.Classes
{
    internal static class ChecksFile
    {
        #region Проверка пути файла
        /// <summary>Проверка пути файла</summary>
        /// <param name="path">Строка пути</param>
        /// <returns>Возвращает true, если путь существует, иначе false</returns>
        public static bool CheckPath(string path) => Directory.Exists(Path.GetDirectoryName(path));
        #endregion

        #region Проверка на то, существует ли файл
        /// <summary>Проверка на то, существует ли файл</summary>
        /// <param name="path">Строка пути до файла</param>
        /// <returns>Возвращает true, если путь до файла существует, иначе false</returns>
        public static bool FileExist(string path) => File.Exists(path);
        #endregion

        #region Проверка файла на пустоту
        /// <summary>Проверка файла на пустоту</summary>
        /// <param name="path">Строка пути до файла</param>
        /// <returns>Возвращает true, если файл в себе ничего не содержит, иначе false</returns>
        public static bool EmptyFile(string path) => File.ReadAllLines(path).Length == 0;
        #endregion

        #region Проверка на номер ли в строке
        /// <summary>Проверка на номер ли в строке</summary>
        /// <param name="number">Строка с данными</param>
        /// <returns>Возвращает true, если в строке номер соответствует int, иначе false</returns>
        public static bool IsNumber(string number) => int.TryParse(number, out _);
        #endregion

        #region Проверка введенного номера в строку и последнего номера в файле
        /// <summary>Проверка введенного номера в строку и последнего номера в файле</summary>
        /// <param name="path">Путь до файла</param>
        /// <param name="number">Номер</param>
        /// <returns>Возвращает true, если введенный номер больше, чем последний номер в файле, иначе false</returns>
        public static bool ValidNumber(string path, string number) => int.Parse(number) > int.Parse(File.ReadLines(path).Last());
        #endregion

        #region Вызов всех проверок файла и пути
        /// <summary>Вызов всех проверок файла и пути</summary>
        /// <param name="path">Путь до файла</param>
        /// <returns>Возвращает true, если все проверки возвращают true, иначе false</returns>
        public static bool CheckFullPathAndFile(string path) => CheckPath(path) && FileExist(path) && !EmptyFile(path) && IsNumber(File.ReadLines(path).Last());
        #endregion
    }
}
