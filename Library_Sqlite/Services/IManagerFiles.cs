namespace Library.Services
{
    public interface IManagerFiles
    {
        Task<string> EditFile(byte[] content, string extension, string folder, string route,
            string contentType);
        Task DeleteFile(string routa, string folder);
        Task<string> SaveFile(byte[] content, string extension, string folder, string contentType);
    }
}
