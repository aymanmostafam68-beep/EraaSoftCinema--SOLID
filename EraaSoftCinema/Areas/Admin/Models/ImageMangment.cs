namespace EraaSoftCinema.Areas.Admin.Models
{
    public class ImageMangment
    {
        public string filePath { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
        public string fileExtension { get; set; } = string.Empty;
        public string? uploadonefile(string dest, IFormFile file)
        {


            var rootPath = Path.Combine("wwwroot", dest);
            if (file != null && file.Length > 0)
            {
                var filename = Guid.NewGuid().ToString().Substring(startIndex: 0, 7) + Path.GetExtension(file.FileName);

                this.fileName = filename;
                filePath = Path.Combine(rootPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
                return Path.Combine(dest, fileName);
            }
            return null;

        }
        public void Removefile(string filepath)
        {
            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }

        }
    }
}
