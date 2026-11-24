namespace Qistflow_Semester_Project.Services;

public sealed class ProductImageStore : IProductImageStore
{
    private const long MaxBytes = 3 * 1024 * 1024;
    private readonly IWebHostEnvironment _environment;

    public ProductImageStore(IWebHostEnvironment environment) => _environment = environment;

    public async Task<string?> SaveAsync(
        IFormFile? photo, CancellationToken cancellationToken = default)
    {
        if (photo is null || photo.Length == 0) return null;
        if (photo.Length > MaxBytes)
            throw new ArgumentException("Image must be 3 MB or smaller.");

        string ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
        if (ext is not (".jpg" or ".jpeg" or ".png"))
            throw new ArgumentException("Only JPG and PNG images are allowed.");

        await using var source = photo.OpenReadStream();
        byte[] signature = new byte[8];
        int read = await source.ReadAsync(signature, cancellationToken);
        bool isJpeg = read >= 3 && signature[0] == 0xff &&
                      signature[1] == 0xd8 && signature[2] == 0xff;
        bool isPng = read >= 8 &&
                     signature.AsSpan().SequenceEqual(new byte[]
                     { 137, 80, 78, 71, 13, 10, 26, 10 });
        if ((ext == ".png" && !isPng) ||
            ((ext == ".jpg" || ext == ".jpeg") && !isJpeg))
            throw new ArgumentException("File contents do not match a valid JPG/PNG image.");

        source.Position = 0;
        string webRoot = _environment.WebRootPath ??
            Path.Combine(_environment.ContentRootPath, "wwwroot");
        string folder = Path.Combine(webRoot, "uploads", "products");
        Directory.CreateDirectory(folder);

        string safeExtension = isPng ? ".png" : ".jpg";
        string generatedName = $"{Guid.NewGuid():N}{safeExtension}";
        string filePath = Path.Combine(folder, generatedName);

        await using (var destination = new FileStream(filePath, FileMode.CreateNew))
        {
            await source.CopyToAsync(destination, cancellationToken);
        }
        return "/uploads/products/" + generatedName;
    }

    // Only deletes a server-generated product upload. Never accepts arbitrary paths.
    public Task DeleteIfUploadedAsync(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) ||
            !path.StartsWith("/uploads/products/", StringComparison.Ordinal))
            return Task.CompletedTask;

        string fileName = path["/uploads/products/".Length..];
        if (fileName != Path.GetFileName(fileName))
            return Task.CompletedTask;

        string webRoot = _environment.WebRootPath ??
            Path.Combine(_environment.ContentRootPath, "wwwroot");
        string filePath = Path.Combine(webRoot, "uploads", "products", fileName);
        if (File.Exists(filePath)) File.Delete(filePath);
        return Task.CompletedTask;
    }
}
