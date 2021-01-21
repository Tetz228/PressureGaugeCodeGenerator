

namespace PressureGaugeCodeGenerator.Windows
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Image = System.Windows.Controls.Image;
    using Point = System.Windows.Point;

    /// <summary>
    /// Логика взаимодействия для ImageOverlayWindow.xaml
    /// </summary>
    public partial class ImageOverlayWindow : Window
    {
        public double H { get; set; }
        public double W { get; set; }
        public int c = 0;

        public ImageOverlayWindow()
        {
            InitializeComponent();
            ImageSource image = new BitmapImage(new Uri(@"C:\Users\tetz2\Desktop\0513-0511145.png"));
            IMAGE.Source = image;
            ImageSource imageQR = new BitmapImage(new Uri(
                @"C:\Users\tetz2\source\repos\PressureGaugeCodeGenerator\PressureGaugeCodeGeneratorWPF\bin\Debug\QR-codes\213000001.png"));
            IMAGEQR.Source = imageQR;
            H = IMAGE.Height;
            W = IMAGE.Width;
            
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (e.Delta < 0)
            //{
            //    IMAGE.Height -= 150;
            //    IMAGE.Width -= 150;
            //}
            //else
            //{
            //    IMAGE.Height += 150;
            //    IMAGE.Width += 150;
            //}
        }

        /// <summary>
        /// Наложение 2х изображений друг на друга.
        /// </summary>
        /// <param name="x">1е изображение.</param>
        /// <param name="y">2е изображение.</param>
        /// <param name="percent">Коэффициент прозрачности (от 0 до 1).</param>
        /// <returns></returns>
        Bitmap AlphaBlending(System.Drawing.Image x, System.Drawing.Image y, float percent, Point tePoint)
        {
            if (percent < 0f || percent > 1f)
                throw new ArgumentOutOfRangeException();

            if (x == null || y == null)
                throw new NullReferenceException();

            int x1 = (int)tePoint.X;
            int y1 = (int)tePoint.Y;

            Bitmap bmp = new Bitmap(
                Math.Max(x.Width, y.Width),
                Math.Max(x.Height, y.Height)
            );

            var cm = new ColorMatrix(
                new float[][]
                {
                    new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                    new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
                    new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
                    new float[] { 0.0f, 0.0f, 0.0f, percent, 0.0f },
                    new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
                }
            );

            // Отвечает за расположение изображения
            using (var imgAttr = new ImageAttributes())
            {
                imgAttr.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                using (var g = Graphics.FromImage(bmp))
                {

                    g.DrawImage(x, 0, 0, x.Width, x.Height);
                    //Координаты для расположение QR-кода в 1 строке
                    //Координаты для расположение QR-кода на 1-ой табличке - x:22, y:300
                    //Координаты для расположение QR-кода на 2-ой табличке - x:1100, y:300
                    //Координаты для расположение QR-кода на 3-ой табличке - x:2180, y:288
                    g.DrawImage(
                        y,
                        new System.Drawing.Rectangle(22, 1100, y.Width, y.Height),
                        0, 0, y.Width, y.Height,
                        GraphicsUnit.Point,
                        imgAttr
                    );

                }
            }


            return bmp;
        }



        private void IMAGE_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var windowPosition = Mouse.GetPosition(((Image)sender));
            
            var screenPosition = this.PointToScreen(windowPosition);

            using (var img0 = System.Drawing.Image.FromFile(@"C:\Users\tetz2\Desktop\0513-0511145.png"))
            using (var img1 = System.Drawing.Image.FromFile(
                @"C:\Users\tetz2\source\repos\PressureGaugeCodeGenerator\PressureGaugeCodeGeneratorWPF\bin\Debug\QR-codes\213000001.png")
            ) 
                
            using (var bmp = AlphaBlending(img0, img1, 100 / 100F, windowPosition))
            {
                using (var sfd = new SaveFileDialog())
                {
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        bmp.Save(sfd.FileName);
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IMAGE.Height += 150;
            IMAGE.Width += 150;
            c++;
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            if (H < IMAGE.Height && W < IMAGE.Width)
            {
                IMAGE.Height -= 150;
                IMAGE.Width -= 150;
                c--;
            }
        }
    }
}