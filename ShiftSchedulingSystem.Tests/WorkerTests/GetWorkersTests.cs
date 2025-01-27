using AutoBogus;
using AutoMapper;
using Contracts;
using Entities.Models;
using Moq;
using Service;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using Xunit;

namespace ShiftSchedulingSystem.Tests.WorkerTests;

public class GetWorkersTests
{
    #region Properties
    Mock<IRepositoryManager> _repository;
    Mock<ILoggerManager> _logger;
    Mock<IMapper> _mapper;

    IAutoFaker faker = AutoFaker.Create();

    #endregion

    #region
    public GetWorkersTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _logger = new Mock<ILoggerManager>();
        _mapper = new Mock<IMapper>();
    }
    #endregion

    #region Tests
    [Fact]
    private async Task Handle_GetWorkersTests_Valid()
    {
        // Arrange

        var shifts = new AutoFaker<Shift>()
                .RuleFor(x => x.Date, new DateOnly(2025, 4, 1))
                .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
                .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
                .Generate(1);

        var pagedWorkers = new PagedList<Worker>(new AutoFaker<Worker>()
            .RuleFor(fake => fake.shifts, shifts)
            .Generate(1), 20, 2, 2);

        var workerDtos = faker.Generate<IEnumerable<WorkerDto>>();

        var parameters = faker.Generate<WorkerParameters>();

        MockGetWorkers(pagedWorkers, workerDtos);

        // Act
        var result = await CreateWorkerService().GetWorkersAsync(parameters, false);

        // Assert
        Assert.NotNull(result.workers);
        Assert.Equal(20, result.metaData.TotalCount);
        Assert.Equal(2, result.metaData.PageSize);

        _repository.Verify(r => r.Worker.GetWorkersAsync(It.IsAny<WorkerParameters>(), It.IsAny<bool>()), Times.Once);
        _mapper.Verify(m => m.Map<IEnumerable<WorkerDto>>(It.IsAny<PagedList<Worker>>()), Times.Once);


    }
    #endregion

    #region MockSetup
    private void MockGetWorkers(PagedList<Worker> workers, IEnumerable<WorkerDto> workerDtos)
    {
        _repository.Setup(r => r.Worker.GetWorkersAsync(It.IsAny<WorkerParameters>(), It.IsAny<bool>())).ReturnsAsync(workers);

        _mapper.Setup(m => m.Map<IEnumerable<WorkerDto>>(It.IsAny<PagedList<Worker>>())).Returns(workerDtos);
    }
    private WorkerService CreateWorkerService()
    {
        return new WorkerService(_repository.Object, _logger.Object, _mapper.Object);
    }
    #endregion
}
