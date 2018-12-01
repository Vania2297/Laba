using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageProcessing
{
    public class StampService
    {
        private static readonly Regex R = new Regex(":");

        public void AddStamp(string folder)
        {
            var filePaths = Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly);

            foreach (var filePath in filePaths)
            {
                var date = GetDateTakenFromImage(filePath) ?? new FileInfo(filePath).CreationTime;
                var dateStr = date.ToShortDateString();

                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                var fs = new FileStream(filePath, FileMode.Open);
                if (!fs.CanWrite)
                {
                    Console.WriteLine($"Sorry, file {filePath} cannot be changed");
                }

                var image = Image.FromStream(fs);
                fs.Close();

                var b = new Bitmap(image);
                var graphics = Graphics.FromImage(b);
                var size = graphics.MeasureString(dateStr, new Font("Tahoma", 60));
                var rectf = new RectangleF(image.Height, 0, size.Width, size.Height);
                graphics.DrawString(dateStr, new Font("Tahoma", 40), Brushes.Black, rectf, format);

                b.Save(filePath);

                image.Dispose();
                b.Dispose();
            }
        }

        public static DateTime? GetDateTakenFromImage(string path)
        {
            using (var myImage = Image.FromFile(path))
            {
                int dateTakenValue = 0x9003; //36867; 

                if (!myImage.PropertyIdList.Contains(dateTakenValue))
                {
                    return null;
                }

                var propItem = myImage.GetPropertyItem(dateTakenValue);
                var dateTaken = R.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                if (DateTime.TryParse(dateTaken, out var date))
                {
                    return date;
                }

                return null;
            }
        }
    }
}
