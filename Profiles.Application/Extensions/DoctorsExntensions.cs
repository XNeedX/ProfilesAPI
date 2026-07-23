using System.Linq;
using Profiles.Domain.Models;
using Profiles.Application.DTOs;

namespace Profiles.Application.Extensions;

public static class DoctorQueryExtensions
{
    public static IQueryable<DoctorProfile> ApplyFilters(this IQueryable<DoctorProfile> query, DoctorFilterDto filter)
    {
        if (filter == null)
            return query;

        if (!string.IsNullOrWhiteSpace(filter.SearchName))
        {
            var search = filter.SearchName.ToLower();
            query = query.Where(d =>
                (d.FirstName + " " + d.LastName + " " + d.MiddleName).ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Specialization))
        {
            query = query.Where(d => d.Specialization == filter.Specialization);
        }

        if (!string.IsNullOrWhiteSpace(filter.OfficeAddress))
        {
            query = query.Where(d => d.OfficeAddress == filter.OfficeAddress);
        }

        return query;
    }
}