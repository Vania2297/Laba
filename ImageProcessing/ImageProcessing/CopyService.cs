using System.IO;

namespace ImageProcessing
{
    public class CopyService
    {
        public void Copy(string folderFrom, string folderTo)
        {
            if (!Directory.Exists(folderFrom))
            {
                throw new DirectoryNotFoundException();
            }

            if (!Directory.Exists(folderTo))
            {
                Directory.CreateDirectory(folderTo);
            }

            var filePaths = Directory.GetFiles(folderFrom, "*.jpg", SearchOption.TopDirectoryOnly);

            foreach (var path in filePaths)
            {
                var fileName = Path.GetFileName(path);
                var sendFileName = Path.Combine(folderTo, fileName);

                File.Copy(path, sendFileName, true);
            }
        }
    }
}
