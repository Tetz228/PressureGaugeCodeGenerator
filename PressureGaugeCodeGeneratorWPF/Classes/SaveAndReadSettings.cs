namespace PressureGaugeCodeGenerator.Classes
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Windows;

    internal static class SaveAndReadSettings
    {
        public static Dictionary<string, string> ReadSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                    MessageBox.Show("Ошибка чтения настроек. Настройки не найдены!", "Настройки не найдены", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    foreach (var key in appSettings.AllKeys)
                        settings.Add(key, appSettings[key]);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка в конфигурации", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return settings;
        }

        #region Сохранение настроек
        /// <summary>Сохранение настроек</summary>
        /// <param name="dictionarySettings">Словарь с настройками</param>
        public static void SaveSettings(Dictionary<string, string> dictionarySettings)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                foreach (var item in dictionarySettings)
                    settings[item.Key].Value = item.Value;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении настроек", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Сохранение размеров форматов
        /// <summary></summary>
        /// <param name="indexFormat">Индекс формата</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="widthPngBmp">Ширина BMP</param>
        /// <param name="heightPngBmp">Высота BMP</param>
        public static void SaveSizesFormats(int indexFormat, string width, string height, string widthPngBmp, string heightPngBmp)
        {
            var settings = ReadSettings();

            switch (indexFormat)
            {
                case (int)GetData.Formats.Bmp:
                    settings["BmpWidth"] = width;
                    settings["BmpHeight"] = height;
                    break;
                case (int)GetData.Formats.Png:
                    settings["PngWidth"] = width;
                    settings["PngHeight"] = height;
                    break;

                case (int)GetData.Formats.Jpeg:
                    settings["JpegWidth"] = width;
                    settings["JpegHeight"] = height;
                    break;
                case (int)GetData.Formats.PngBmp:
                    settings["PngBmpWidthPng"] = width;
                    settings["PngBmpHeightPng"] = height;
                    settings["PngBmpWidthBmp"] = widthPngBmp;
                    settings["PngBmpHeightBmp"] = heightPngBmp;
                    break;
            }
            SaveSettings(settings);
        }
        #endregion
    }
}
