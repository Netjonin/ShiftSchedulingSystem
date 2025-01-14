using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository;

public class ShiftRepository : RepositoryBase<Shift>, IShiftRepository
{
    public ShiftRepository(ApplicationContext ctx) : base(ctx)
    {
    }

    public async Task<PagedList<Shift>> GetShiftsAsync(Guid workerId, ShiftParameters parameters, bool trackChanges)
    {
        var shifts = await FindByCondition(e => e.WorkerId.Equals(workerId), trackChanges)
            .Search(parameters.SearchTerm!)
            .OrderBy(e => e.Date)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var count = await FindByCondition(e => e.WorkerId.Equals(workerId), trackChanges).CountAsync();
        return new PagedList<Shift>(shifts, count, parameters.PageNumber, parameters.PageSize);
    }


    public async Task<Shift> GetShiftAsync(Guid workerId, Guid id, bool trackChanges) =>
        await FindByCondition(e => e.WorkerId.Equals(workerId) && e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

    public void CreateShiftForWorker(Guid workerId, Shift shift)
    {
        shift.WorkerId = workerId;
        Create(shift);
    }
    public void DeleteShift(Shift shift) => Delete(shift);

    public async Task<IEnumerable<Shift>> GetShiftsForValidation(Guid workerId, bool trackChanges)
    {
        var shifts = await FindByCondition(e => e.WorkerId.Equals(workerId), trackChanges).ToListAsync();
        return shifts;
    }
}