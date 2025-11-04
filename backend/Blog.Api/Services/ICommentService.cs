using Blog.Api.Dtos;

namespace Blog.Api.Services;

public interface ICommentService
{
    Task<IReadOnlyCollection<CommentDto>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);
    Task<CommentDto?> GetByIdAsync(int postId, int commentId, CancellationToken cancellationToken = default);
    Task<CommentDto?> CreateAsync(int postId, CommentCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int postId, int commentId, CancellationToken cancellationToken = default);
}
