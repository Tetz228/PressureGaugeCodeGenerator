namespace PressureGaugeCodeGenerator.Data
{
    using System.IO;

    public static class Data
    {
        public const int DIGITS = 9;
        public const string NAME_FOLDER_QR = "QR-codes\\";
        public static string PatchBaseNumbers = $"{Directory.GetCurrentDirectory()}\\BaseNumbers";
        public const int SCAN_EVENT_СALL_СODE = 1001;
        public const string EVENTS = "<inArgs>" +
                                             "<cmdArgs>" +
                                             "<arg-int>1</arg-int>" +
                                             "<arg-int>1</arg-int>" +
                                             "</cmdArgs>" +
                                             "</inArgs>";
        public const int SCAN_ACTION_CODE = 6000;
        public const string BEEP_THREE_TIMES = "<inArgs>" +
                                             "<scannerID>1</scannerID>" +
                                             "<cmdArgs>" +
                                             "<arg-int>3</arg-int>" +
                                             "</cmdArgs>" +
                                             "</inArgs>";
        public const string BEEP_ONE_TIMES = "<inArgs>" +
                                           "<scannerID>1</scannerID>" +
                                           "<cmdArgs>" +
                                           "<arg-int>0</arg-int>" +
                                           "</cmdArgs>" +
                                           "</inArgs>";
        public const string LED_GREEN = "<inArgs>" +
                                       "<scannerID>1</scannerID>" +
                                       "<cmdArgs>" +
                                       "<arg-int>43</arg-int>" +
                                       "</cmdArgs>" +
                                       "</inArgs>";
        public const string LED_RED = "<inArgs>" +
                                     "<scannerID>1</scannerID>" +
                                     "<cmdArgs>" +
                                     "<arg-int>47</arg-int>" +
                                     "</cmdArgs>" +
                                     "</inArgs>";
    }
}
