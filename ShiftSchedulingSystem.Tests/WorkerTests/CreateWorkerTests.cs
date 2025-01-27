using AutoBogus;
using AutoMapper;
using Contracts;
using Entities.Models;
using Moq;
using Service;
using Shared.DataTransferObjects;
using Xunit;

namespace ShiftSchedulingSystem.Tests.WorkerTests;

public class CreateWorkerTests
{
    #region Properties
    Mock<IRepositoryManager> _repository;
    Mock<ILoggerManager> _logger;
    Mock<IMapper> _mapper;

    IAutoFaker faker = AutoFaker.Create();

    #endregion

    #region Constructor
    public CreateWorkerTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _logger = new Mock<ILoggerManager>();
        _mapper = new Mock<IMapper>();
    }
    #endregion

    #region Tests
    [Fact]
    private async Task Handle_CreateWorkerTests_Valid()
    {
        // Arrange

        var shifts = new AutoFaker<Shift>()
                .RuleFor(x => x.Date, new DateOnly(2025, 4, 1))
                .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
                .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
                .Generate(1);

        var worker = new AutoFaker<Worker>()
            .RuleFor(fake => fake.shifts, shifts)
            .Generate();

        var workerDto = new AutoFaker<WorkerDto>()
            .Generate();

        var workerForCreation = new AutoFaker<WorkerForCreationDto>()
           .Generate();

        MockGetWorkers(worker, workerDto);

        // Act
        var result = await CreateWorkerService().CreateWorkerAsync(workerForCreation);

        // Assert

        Assert.NotNull(result);
        Assert.IsType<WorkerDto>(result);

        _mapper.Verify(m => m.Map<Worker>(It.IsAny<WorkerForCreationDto>()), Times.Once);
        _repository.Verify(r => r.Worker.CreateWorker(It.IsAny<Worker>()), Times.Once);
        _repository.Verify(r => r.SaveAsync(), Times.Once);
        _mapper.Verify(m => m.Map<WorkerDto>(It.IsAny<Worker>()), Times.Once);
    }
    #endregion

    #region MockSetup
    private void MockGetWorkers(Worker worker, WorkerDto workerDto)
    {
        _mapper.Setup(m => m.Map<Worker>(It.IsAny<WorkerForCreationDto>())).Returns(worker);
        _repository.Setup(r => r.Worker.CreateWorker(It.IsAny<Worker>()));
        _repository.Setup(r => r.SaveAsync());
        _mapper.Setup(m => m.Map<WorkerDto>(It.IsAny<Worker>())).Returns(workerDto);
    }
    private WorkerService CreateWorkerService()
    {
        return new WorkerService(_repository.Object, _logger.Object, _mapper.Object);
    }
    #endregion
}
