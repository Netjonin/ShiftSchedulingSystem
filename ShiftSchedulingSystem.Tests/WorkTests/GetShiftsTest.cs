using AutoBogus;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Moq;
using Service;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShiftSchedulingSystem.Tests.WorkTests
{
    public class GetShiftsTest
    {
        #region Properties
        Mock<IRepositoryManager> _repository;
        Mock<ILoggerManager> _logger;
        Mock<IMapper> _mapper;

        IAutoFaker faker = AutoFaker.Create();

        #endregion

        #region Constructor
        public GetShiftsTest()
        {
            _repository = new Mock<IRepositoryManager>();
            _logger = new Mock<ILoggerManager>();
            _mapper = new Mock<IMapper>();
        }
        #endregion

        #region Tests
        [Fact]
        private async Task Handle_GetShiftsTests_Valid()
        {
            // Arrange
            var shifts = new AutoFaker<Shift>()
                .RuleFor(x => x.Date, new DateOnly(2025, 3, 1))
                .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
                .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
                .Generate(1);

            var shiftDto = new AutoFaker<ShiftDto>()
                .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
                .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
                .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
                .Generate(1);

            var pagedShifts = new PagedList<Shift>(new AutoFaker<Shift>()
               .RuleFor(x => x.Date, new DateOnly(2025, 3, 1))
               .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
               .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
               .Generate(1), 20, 2, 2);

            var worker = new AutoFaker<Worker>()
                .RuleFor(a => a.shifts, shifts)
                .Generate();

            MockGetShifts(worker, pagedShifts, shiftDto);

            var parameters = faker.Generate<ShiftParameters>();

            // Act
            var result = await CreateShiftService().GetShiftsAsync(Guid.NewGuid(), parameters, false);

            // Assert
            Assert.NotNull(result.shifts);
            Assert.Equal(20, result.metaData.TotalCount);
            Assert.Equal(2, result.metaData.PageSize);

            _repository.Verify(r => r.Worker.GetWorkerAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
            _repository.Verify(r => r.Shift.GetShiftsAsync(It.IsAny<Guid>(), It.IsAny<ShiftParameters>(), It.IsAny<bool>()), Times.Once);
            _mapper.Verify(m => m.Map<IEnumerable<ShiftDto>>(It.IsAny<PagedList<Shift>>()), Times.Once);
        }

        [Fact]
        private async Task Handle_GetShiftsTests_Exception()
        {
            // Arrange
            var shifts = new AutoFaker<Shift>()
                .RuleFor(x => x.Date, new DateOnly(2025, 3, 1))
                .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
                .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
                .Generate(1);

            var shiftDto = new AutoFaker<ShiftDto>()
                .RuleFor(x => x.Date, new DateOnly(2025, 3, 2))
                .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
                .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
                .Generate(1);

            var pagedShifts = new PagedList<Shift>(new AutoFaker<Shift>()
               .RuleFor(x => x.Date, new DateOnly(2025, 3, 1))
               .RuleFor(x => x.StartTime, TimeOnly.FromDateTime(DateTime.Now))
               .RuleFor(x => x.EndTime, TimeOnly.FromDateTime(DateTime.Now))
               .Generate(1), 20, 2, 2);

            var worker = new AutoFaker<Worker>()
                .RuleFor(a => a.shifts, shifts)
                .Generate();

            var parameters = faker.Generate<ShiftParameters>();

            MockCreateShift_Exception(pagedShifts, shiftDto);

            // Act && Assert
            await Assert.ThrowsAsync<WorkerNotFoundException>(() => CreateShiftService().GetShiftsAsync(Guid.NewGuid(), parameters, false));

        }
        #endregion

        #region Mocksetup
        private void MockGetShifts(Worker worker, PagedList<Shift> pagedShifts, IEnumerable<ShiftDto> shiftDtos)
        {
            _repository.Setup(r => r.Worker.GetWorkerAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(worker);
            _repository.Setup(r => r.Shift.GetShiftsAsync(It.IsAny<Guid>(), It.IsAny<ShiftParameters>(), It.IsAny<bool>())).ReturnsAsync(pagedShifts);
            _mapper.Setup(m => m.Map<IEnumerable<ShiftDto>>(It.IsAny<PagedList<Shift>>())).Returns(shiftDtos);
        }

        private void MockCreateShift_Exception(PagedList<Shift> pagedShifts, IEnumerable<ShiftDto> shiftDtos)
        {
            _repository.Setup(r => r.Worker.GetWorkerAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync((Worker)null!);
            _repository.Setup(r => r.Shift.GetShiftsAsync(It.IsAny<Guid>(), It.IsAny<ShiftParameters>(), It.IsAny<bool>())).ReturnsAsync(pagedShifts);
            _mapper.Setup(m => m.Map<IEnumerable<ShiftDto>>(It.IsAny<PagedList<Shift>>())).Returns(shiftDtos);

        }

        private ShiftService CreateShiftService()
        {
            return new ShiftService(_repository.Object, _logger.Object, _mapper.Object);
        }
        #endregion
    }
}
