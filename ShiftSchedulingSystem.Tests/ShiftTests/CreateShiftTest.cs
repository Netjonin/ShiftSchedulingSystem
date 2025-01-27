using AutoBogus;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Moq;
using Service;
using Shared.DataTransferObjects;
using Xunit;

namespace ShiftSchedulingSystem.Tests.ShiftTests;
public class CreateShiftTest
{
    #region Properties
    Mock<IRepositoryManager> _repository;
    Mock<ILoggerManager> _logger;
    Mock<IMapper> _mapper;

    #endregion

    #region Constructor
    public CreateShiftTest()
    {
        _repository = new Mock<IRepositoryManager>();
        _logger = new Mock<ILoggerManager>();
        _mapper = new Mock<IMapper>();
    }
    #endregion

    #region Tests
    [Fact]
    private async Task Handle_CreateShiftTests_Valid()
    {
        // Arrange
        var shifts = new AutoFaker<Shift>()
            .RuleFor(x => x.Date, new DateOnly(2025, 3, 1))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate(1);

        var shift = new AutoFaker<Shift>()
            .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate();

        var shiftDto = new AutoFaker<ShiftDto>()
            .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate();

        var shiftCreation = new AutoFaker<ShiftForCreationDto>()
            .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate();

        var worker = new AutoFaker<Worker>()
            .RuleFor(a => a.shifts, shifts)
            .Generate();

        MockCreateShift(worker, shifts, shift, shiftDto);

        // Act
        var result = await CreateShiftService().CreateShiftAsync(Guid.NewGuid(), shiftCreation, false);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ShiftDto>(result);
        Assert.Equal(result.Day, shiftDto.Day);
        Assert.Equal(result.StartTime, shiftDto.StartTime);
        Assert.Equal(result.EndTime, shiftDto.EndTime);

        _repository.Verify(r => r.Worker.GetWorkerAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
        _repository.Verify(r => r.Shift.GetShiftsForValidation(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
        _repository.Verify(r => r.Shift.CreateShiftForWorker(It.IsAny<Guid>(), It.IsAny<Shift>()), Times.Once);
        _repository.Verify(r => r.SaveAsync());

        _mapper.Verify(m => m.Map<Shift>(It.IsAny<ShiftForCreationDto>()), Times.Once);
        _mapper.Verify(m => m.Map<ShiftDto>(It.IsAny<Shift>()), Times.Once);

    }

    [Fact]
    private async Task Handle_CreateShiftTests_Exception()
    {
        // Arrange
        var shifts = new AutoFaker<Shift>()
            .RuleFor(x => x.Date, new DateOnly(2025, 4, 1))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate(1);

        var shift = new AutoFaker<Shift>()
            .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate();

        var shiftDto = new AutoFaker<ShiftDto>()
            .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate();

        var shiftCreation = new AutoFaker<ShiftForCreationDto>()
            .RuleFor(x => x.Date, new DateOnly(2025, 4, 1))
            .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
            .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
            .Generate();

        var worker = new AutoFaker<Worker>()
            .RuleFor(a => a.shifts, shifts)
            .Generate();

        MockCreateShift_Exception(worker, shifts, shift, shiftDto);

        // Act && Assert
        await Assert.ThrowsAsync<ConflictException>(() => CreateShiftService().CreateShiftAsync(Guid.NewGuid(), shiftCreation, false));

    }
    #endregion

    #region Mocksetup
    private void MockCreateShift(Worker worker, IEnumerable<Shift> shifts, Shift shift, ShiftDto shiftDto)
    {
        _repository.Setup(r => r.Worker.GetWorkerAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(worker);
        _repository.Setup(r => r.Shift.GetShiftsForValidation(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(shifts);
        _repository.Setup(r => r.Shift.CreateShiftForWorker(It.IsAny<Guid>(), It.IsAny<Shift>()));
        _repository.Setup(r => r.SaveAsync());

        _mapper.Setup(m => m.Map<Shift>(It.IsAny<ShiftForCreationDto>())).Returns(shift);
        _mapper.Setup(m => m.Map<ShiftDto>(It.IsAny<Shift>())).Returns(shiftDto);
    }

    private void MockCreateShift_Exception(Worker worker, IEnumerable<Shift> shifts, Shift shift, ShiftDto shiftDto)
    {
        _repository.Setup(r => r.Worker.GetWorkerAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(worker);
        _repository.Setup(r => r.Shift.GetShiftsForValidation(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(shifts);
        _repository.Setup(r => r.Shift.CreateShiftForWorker(It.IsAny<Guid>(), It.IsAny<Shift>()));
        _repository.Setup(r => r.SaveAsync());

        _mapper.Setup(m => m.Map<Shift>(It.IsAny<ShiftForCreationDto>())).Returns(shift);
        _mapper.Setup(m => m.Map<ShiftDto>(It.IsAny<Shift>())).Returns(shiftDto);
    }

    private ShiftService CreateShiftService()
    {
        return new ShiftService(_repository.Object, _logger.Object, _mapper.Object);
    }
    #endregion
}
