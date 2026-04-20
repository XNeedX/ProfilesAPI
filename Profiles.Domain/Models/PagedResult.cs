namespace Profiles.Domain.Models;

public class PagedResult<T>
{
    public T[] Data { get; set; }
    public int TotalCount { get; set; } 

    public PagedResult(T[] data, int totalCount)
    {
        Data = data;
        TotalCount = totalCount; 
    }
}