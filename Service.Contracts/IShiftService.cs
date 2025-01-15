using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;
public interface IShiftService
{
    Task<(IEnumerable<ShiftDto> shifts, MetaData metaData)> GetShiftsAsync(Guid workerId, ShiftParameters parameters, bool trackChanges);
    Task<ShiftDto> GetShiftAsync(Guid shiftId, Guid id, bool trackChanges);
    Task<ShiftDto> CreateShiftAsync(Guid shiftId, ShiftForCreationDto shift, bool trackChanges);
    Task DeleteShiftAsync(Guid workerId, Guid id, bool trackChanges);
    Task UpdateShiftAsync(Guid shiftid, Guid id, ShiftForUpdateDto shiftForUpdate, bool workerTrackChanges, bool shiftTrackChanges);

}
