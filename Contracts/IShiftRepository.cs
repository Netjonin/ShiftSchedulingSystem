using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;
public interface IShiftRepository
{
    Task<PagedList<Shift>> GetShiftsAsync(Guid workerId, ShiftParameters parameters, bool trackChanges);
    Task<Shift> GetShiftAsync(Guid workerId, Guid id, bool trackChanges);
    void CreateShiftForWorker(Guid workerId, Shift shift);
    void DeleteShift(Shift shift);
    Task<IEnumerable<Shift>> GetShiftsForValidation(Guid workerId, bool trackChanges);
}