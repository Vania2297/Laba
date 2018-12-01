using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageProcessing
{
    class RenamingService
    {
        private static readonly Regex R = new Regex(":");

        public void Raname(string folder)
        {
            var filePaths = Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly);

            foreach (var path in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                var date = GetDateTakenFromImage(path) ?? new FileInfo(path).CreationTime;
                var name = $"{fileName}-{date.ToShortDateString()}{Path.GetExtension(path)}";

                var newFileName = Path.Combine(folder, name);

                File.Move(path, newFileName);
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
