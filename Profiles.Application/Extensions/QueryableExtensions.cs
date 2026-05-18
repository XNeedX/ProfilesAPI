using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Models;

namespace Profiles.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedAsync<T>(this IQueryable<T> query, PageParams pageParams)
    {
        var count = await query.CountAsync();

        if (count == 0)
            return new PagedResult<T>([], 0);

        var skip = (pageParams.Page - 1) * pageParams.PageSize;

        var result = await query.Skip(skip)
            .Take(pageParams.PageSize)
            .ToArrayAsync();

        return new PagedResult<T>(result, count);
    }
}