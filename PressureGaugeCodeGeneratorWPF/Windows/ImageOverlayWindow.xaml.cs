

namespace PressureGaugeCodeGenerator.Windows
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using DragEventArgs = System.Windows.DragEventArgs;
    using Point = System.Windows.Point;

    /// <summary>
    /// Логика взаимодействия для ImageOverlayWindow.xaml
    /// </summary>
    public partial class ImageOverlayWindow : Window
    {
        public double H { get; set; }
        public double W { get; set; }
        public int c = 0;
        private Point spotClicked;
        BitmapImage f = new BitmapImage(new Uri(@"C:\Users\tetz2\OneDrive\Рабочий стол\0513-0511145.png"));

        public ImageOverlayWindow()
        {
            
            InitializeComponent();
            ImageSource image = new BitmapImage(new Uri(@"C:\Users\tetz2\OneDrive\Рабочий стол\0513-0511145.png"));
            IMAGE.Source = image;
            ImageSource imageQR = new BitmapImage(new Uri(
                @"C:\Users\tetz2\OneDrive\Рабочий стол\211000001.png"));
            IMAGEQR.Source = imageQR;
            H = IMAGE.Height;
            W = IMAGE.Width;
        }

        private void MouseButtonIsDown(object sender, MouseButtonEventArgs e)
        {

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
        System.Drawing.Bitmap AlphaBlending(System.Drawing.Image x, System.Drawing.Image y, Point tePoint)
        {
            int x1 = (int)tePoint.X;
            int y1 = (int)tePoint.Y;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Convert.ToInt32(x.Width), Convert.ToInt32(x.Height));

            // Отвечает за расположение изображения
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(x, 0, 0, x.Width, x.Height);
                //Координаты для расположение QR-кода в 1 строке
                //Координаты для расположение QR-кода на 1-ой табличке - x:22, y:300
                //Координаты для расположение QR-кода на 2-ой табличке - x:1100, y:300
                //Координаты для расположение QR-кода на 3-ой табличке - x:2180, y:288
                g.DrawImage(y, new System.Drawing.Rectangle(x1, y1, y.Width, y.Height), 0, 0, y.Width, y.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            return bmp;
        }


        private void IMAGE_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var windowPosition = Mouse.GetPosition(IMAGE);

            using (var img0 = System.Drawing.Image.FromFile(@"C:\Users\tetz2\OneDrive\Рабочий стол\0513-0511145.png"))
            using (var img1 = System.Drawing.Image.FromFile(@"C:\Users\tetz2\OneDrive\Рабочий стол\211000001.png"))
            using (var bmp = AlphaBlending(img0, img1, windowPosition))
            {
                using (var sfd = new SaveFileDialog())
                {
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) bmp.Save(sfd.FileName);
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

        private void IMAGE_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBox1.Text = Mouse.GetPosition((IInputElement)sender).ToString();
        }

        private void IMAGE_OnDrop(object sender, DragEventArgs e)
        {
            IMAGE.Source = (ImageSource)e.Data.GetData(typeof(ImageSource));
        }

        private void IMAGEQR_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image image = e.Source as System.Windows.Controls.Image;
            System.Windows.DataObject data = new System.Windows.DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, System.Windows.DragDropEffects.All);
        }
    }
}