
using Entities.Models;

namespace Repository.Extensions;

public static class ShiftExtensions
{
    public static IQueryable<Shift> Search(this IQueryable<Shift> shifts, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return shifts;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return shifts.Where(e => e.ShiftType.ToLower().Contains(lowerCaseTerm));
    }
}