using Blog.Api.Dtos;

namespace Blog.Api.Services;

public interface IPostService
{
    Task<IReadOnlyCollection<PostSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PostDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PostDetailDto> CreateAsync(PostCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, PostUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
