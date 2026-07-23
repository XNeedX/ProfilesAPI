namespace Profiles.Domain.Models;

public record PageParams(
    int Page = 1, 
    int PageSize = 10
);
