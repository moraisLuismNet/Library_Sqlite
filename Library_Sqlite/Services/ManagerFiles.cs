namespace Library.Services
{
    public class ManagerFiles : IManagerFiles
    {
        private readonly IWebHostEnvironment env; // In order to locate wwwroot
        private readonly IHttpContextAccessor httpContextAccessor;
        // To know the server configuration to build the image url

        public ManagerFiles(IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string route, string folder)
        {
            if (route != null)
            {
                var fileName = Path.GetFileName(route);
                string fileDirectory = Path.Combine(env.WebRootPath, folder, fileName);

                if (File.Exists(fileDirectory))
                {
                    File.Delete(fileDirectory);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string folder, string contentType)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentNullException(nameof(folder), "Folder path cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException(nameof(extension), "File extension cannot be null or empty.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            string folderF = Path.Combine(env.WebRootPath, folder);

            if (!Directory.Exists(folderF))
            {
                Directory.CreateDirectory(folderF);
            }

            string route = Path.Combine(folderF, fileName);
            await File.WriteAllBytesAsync(route, content);

            var currentUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlForBD = Path.Combine(currentUrl, folder, fileName).Replace("\\", "/");
            return urlForBD;
        }
        public async Task<string> EditFile(byte[] content, string extension, string folder, string route, string contentType)
        {
            await DeleteFile(route, folder);
            return await SaveFile(content, extension, folder, contentType);
        }
    }
}

