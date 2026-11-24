namespace Qistflow_Semester_Project.Services;

public interface IProductImageStore
{
    Task<string?> SaveAsync(IFormFile? photo, CancellationToken cancellationToken = default);
    Task DeleteIfUploadedAsync(string? path);
}
