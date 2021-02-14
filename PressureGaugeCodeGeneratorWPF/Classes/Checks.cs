namespace PressureGaugeCodeGenerator.Classes
{
    using PressureGaugeCodeGenerator.Data;
    using System.IO;
    using System.Linq;
    using System.Windows;

    internal static class Checks
    {
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
        public static bool CheckFullPathAndFile(string path) => FileExist(path) && !EmptyFile(path) && IsNumber(File.ReadLines(path).Last());
        #endregion

        #region Валидация полей при генерации номеров
        /// <summary>Валидация полей при генерации номеров</summary>
        /// <returns>Возвращает true, если все поля заполнены верно, иначе false</returns>
        public static bool CheckingFieldsGeneratingNumbers(string startNumber, string countNumber, string path, bool? checkStartNumber)
        {
            if (startNumber.Length < Data.DIGITS)
            {
                MessageBox.Show($"В номере должно быть {Data.DIGITS} цифр", "Некорректный начальный номер!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsNumber(countNumber))
            {
                MessageBox.Show("Введите корректное количество номеров", "Некорректное количество номеров!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsNumber(startNumber))
            {
                MessageBox.Show("Введите корректный начальный номер", "Некорректный начальный номер!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (int.Parse(countNumber) < 1 || int.Parse(countNumber) > 1000)
            {
                MessageBox.Show("Количество номеров должно быть в диапазоне от 1 до 1000",
                                "Некорректное количество номеров!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (int.Parse(startNumber) + int.Parse(countNumber) >= 1_000_000_000)
            {
                MessageBox.Show("Последнее генерируемое число должно быть не более 999999999",
                                "Некорректное генерируемое число!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!FileExist(path))
            {
                MessageBox.Show("Введите корректный путь", "Некорректный путь!", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            if (!EmptyFile(path))
            {
                if (!IsNumber(File.ReadLines(path).Last()))
                {
                    MessageBox.Show("В файле последний номер содержит недопустимые знаки",
                                    "Некорректный последний номер в файле!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (checkStartNumber == true)
                    if (!ValidNumber(path, startNumber))
                    {
                        MessageBox.Show("Номер должен быть больше, чем уже сгенерированное число в файле",
                                        "Некорректный начальный номер!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
            }
            return true;
        }
        #endregion

        #region Проверка на существование расшифрованного номера в файле
        /// <summary>Проверка на существование расшифрованного номера в файле</summary>
        /// <param name="decodedNumber">Расшифрованный номер</param>
        /// <returns>Возвращает true, если номер присутствует в файле, иначе false</returns>
        public static bool CheckingExistenceNumber(string decodedNumber)
        {
            using (StreamReader sr = new StreamReader(Data.PatchBaseNumbers))
            {
                string line = sr.ReadLine();

                while (line != null)
                {
                    if (line == decodedNumber)
                        return true;

                    line = sr.ReadLine();
                }
            }

            return false;
        }
        #endregion
    }
}
