﻿namespace Employee_Portal.Core
{
    public interface IFileService
    {
        public Tuple<int,string>  SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);


    }
}
