using System.Reflection.Metadata.Ecma335;

namespace Employee_Portal.Core
{
    public class FileService : IFileService
    {
        private IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            var contentPath = this._environment.ContentRootPath;
            var path=Path.Combine(contentPath,"Uploads");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //Check the allowed Extensions
            var ext = Path.GetExtension(imageFile.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if(!allowedExtensions.Contains(ext))
            {
                string msg=string.Format("Only {0} extensions are allowed ",string.Join(",", allowedExtensions));
                return new Tuple<int, string>(0, msg);
            }
            string uniqueString=Guid.NewGuid().ToString();
            //We are trying to create unique file name here
            var newFileName = uniqueString + ext;
            var fileWithPath=Path.Combine(path,newFileName);
            var stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile.CopyTo(stream);
            stream.Close();
            return new Tuple<int, string>(1, newFileName);
        }
        public bool DeleteImage(string imageFileName)
        {
            var wwwPath = this._environment.WebRootPath;
            var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
