namespace Company.Menna.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1. Upload
        // ImageName
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Folder Location
            //string folderPath = "F:\\Company.Menna\\Company.Menna.PL\\wwwroot\\File\\Images\\"+folderName;

          //var folderPath =  Directory.GetCurrentDirectory() + "\\wwwroot\\File\\" + folderName;
          var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\File", folderName);

            // Get File Name And Make ItUnique
            // 2. File Path

            var fileName = $"{Guid.NewGuid()}{file.FileName}" ;

            // File Path
            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath , FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }
        // 2. Delete
        public static void DeleteFile(string fileName , string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\File" ,folderName , fileName);

            if (File.Exists(folderPath))
                File.Delete(folderPath);
            
        }


    }
}
