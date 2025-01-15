using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

public sealed class ShiftService : IShiftService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public ShiftService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ShiftDto> CreateShiftAsync(Guid workerId, ShiftForCreationDto shiftForCreationDto, bool trackChanges)
    {
        await CheckIfWorkerExists(workerId, trackChanges);

        var shiftforVal = await _repository.Shift.GetShiftsForValidation(workerId, trackChanges);
        var isExisting = shiftforVal.Any(s => s.Date.Equals(shiftForCreationDto.Date));
        

        var shift = _mapper.Map<Shift>(shiftForCreationDto);
        _repository.Shift.CreateShiftForWorker(workerId, shift);
        await _repository.SaveAsync();
        var shiftDto = _mapper.Map<ShiftDto>(shift);
        return shiftDto;
    }

    public async Task DeleteShiftAsync(Guid workerId, Guid id, bool trackChanges)
    {
        await CheckIfWorkerExists(workerId, trackChanges);

        var shift = await GetShiftForWorkerAndCheckIfItExists(workerId, id, trackChanges);
        _repository.Shift.DeleteShift(shift);
        await _repository.SaveAsync();
    }

    public async Task<ShiftDto> GetShiftAsync(Guid workerId, Guid id, bool trackChanges)
    {
        await CheckIfWorkerExists(workerId, trackChanges);

        var shift = await GetShiftForWorkerAndCheckIfItExists(workerId, id, trackChanges);

        var shiftDto = _mapper.Map<ShiftDto>(shift);
        return shiftDto;
    }

    public async Task<(IEnumerable<ShiftDto> shifts, MetaData metaData)> GetShiftsAsync(Guid workerId, ShiftParameters parameters, bool trackChanges)
    {
        await CheckIfWorkerExists(workerId, trackChanges);
        var shifts = await _repository.Shift.GetShiftsAsync(workerId, parameters, trackChanges);

        var shiftsDto = _mapper.Map<IEnumerable<ShiftDto>>(shifts);
        return (shifts: shiftsDto, metaData: shifts.MetaData);
    }

    public async Task UpdateShiftAsync(Guid workerId, Guid id, ShiftForUpdateDto shiftForUpdate, bool workerTrackChanges, bool shiftTrackChanges)
    {
        await CheckIfWorkerExists(workerId, workerTrackChanges);

        var shift = await GetShiftForWorkerAndCheckIfItExists(workerId, id, shiftTrackChanges);
        _mapper.Map(shiftForUpdate, shift);
        await _repository.SaveAsync();
    }

    private async Task CheckIfWorkerExists(Guid workerId, bool trackChanges)
    {
        var worker = await _repository.Worker.GetWorkerAsync(workerId, trackChanges);
        
    }
    private async Task<Shift> GetShiftForWorkerAndCheckIfItExists(Guid workerId, Guid id, bool trackChanges)
    {
        var shift = await _repository.Shift.GetShiftAsync(workerId, id, trackChanges);

        
        return shift;
    }
}
