using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace PressureGaugeCodeGeneratorTestWpf.Classes
{
    internal static class SaveAndReadSettings
    {
        #region Чтение настроек
        /// <summary>Чтение настроек</summary>
        /// <returns>Возвращает словарь с настройками</returns>
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
        #endregion

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
                {
                    settings[item.Key].Value = item.Value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении настроек", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
