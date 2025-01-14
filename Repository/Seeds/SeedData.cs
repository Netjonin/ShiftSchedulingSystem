
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Repository.Seeds;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationContext>>()))
        {
            if (context.Workers.Any())
            {
                return;
            }
            context.Workers.AddRange(
                new Entities.Models.Worker
                {
                    Id = new Guid("2dd736b2-9348-42d0-a690-8cfd33d67511"),
                    Name = "Bola Olamide",
                    Department = "Human Relations",
                    Age = 30,
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("16c96d6b-a8f8-4f3a-9e43-1fa56eb0b80e"),
                    Name = "Oluwashina Ayoade",
                    Department = "Accounting",
                    Age = 26
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("b789a811-69e2-46ba-8e11-ce160dc4139f"),
                    Name = "Gallager Brown",
                    Department = "Engineering",
                    Age = 32
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("822b6cc2-7a1a-4941-a3de-1007328b7758"),
                    Name = "Uwais Awwal",
                    Department = "Marketing",
                    Age = 29
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("7bd2b360-bcaf-49fb-967e-78527716f2e7"),
                    Name = "Ajibike Adeola",
                    Department = "Administration",
                    Age = 29,
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("a48195d4-7e33-4011-a665-088e6b26cd19"),
                    Name = "Tayo Abdullaahi",
                    Department = "Engineering",
                    Age = 25,
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("67d04e3f-7919-4149-a829-6b44dbaf83a2"),
                    Name = "Terry Petersen",
                    Department = "Finance",
                    Age = 31,
                },
                new Entities.Models.Worker
                {
                    Id = new Guid("b156820d-3db2-45bf-a52d-db0d15781477"),
                    Name = "Bintu Jolade",
                    Department = "Human Relations",
                    Age = 31,
                }
             );
            if (context.Shifts.Any())
            {
                return;
            }
            context.Shifts.AddRange(
                new Entities.Models.Shift
                {
                    Id = new Guid("d3f51380-44eb-4ed0-bb0e-d568c3288ffe"),
                    ShiftType = "0-8",
                    Day = "Monday",
                    Date = new DateOnly(2024, 8, 12),
                    StartTime = new TimeOnly(00, 00),
                    EndTime = new TimeOnly(08, 00),
                    WorkerId = new Guid("b156820d-3db2-45bf-a52d-db0d15781477")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("97803ef3-21ae-4057-aa0b-42ebc062cf03"),
                    ShiftType = "8-16",
                    Day = "Tuesday",
                    Date = new DateOnly(2024, 8, 13),
                    StartTime = new TimeOnly(08, 00),
                    EndTime = new TimeOnly(16, 00),
                    WorkerId = new Guid("67d04e3f-7919-4149-a829-6b44dbaf83a2")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("0375dfc5-1834-435b-b986-5c17bd9b7ca2"),
                    ShiftType = "16-24",
                    Day = "Wednesday",
                    Date = new DateOnly(2024, 8, 14),
                    StartTime = new TimeOnly(16, 00),
                    EndTime = new TimeOnly(23, 59),
                    WorkerId = new Guid("2dd736b2-9348-42d0-a690-8cfd33d67511")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("048a3d20-2731-41d8-996a-e9d0c714d42e"),
                    ShiftType = "8-16",
                    Day = "Thursday",
                    Date = new DateOnly(2024, 8, 15),
                    StartTime = new TimeOnly(08, 00),
                    EndTime = new TimeOnly(16, 00),
                    WorkerId = new Guid("2dd736b2-9348-42d0-a690-8cfd33d67511")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("3783d0f4-04b8-4654-aced-58bdb8216f8c"),
                    ShiftType = "0-8",
                    Day = "Friday",
                    Date = new DateOnly(2024, 8, 16),
                    StartTime = new TimeOnly(00, 00),
                    EndTime = new TimeOnly(08, 00),
                    WorkerId = new Guid("822b6cc2-7a1a-4941-a3de-1007328b7758")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("2fbec4fb-9ec1-434c-b8c9-27b3d759165f"),
                    ShiftType = "16-24",
                    Day = "Saturday",
                    Date = new DateOnly(2024, 8, 17),
                    StartTime = new TimeOnly(00, 00),
                    EndTime = new TimeOnly(08, 00),
                    WorkerId = new Guid("b789a811-69e2-46ba-8e11-ce160dc4139f")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("993043fb-825d-4996-8f37-fb7b396bb56a"),
                    ShiftType = "0-8",
                    Day = "Sunday",
                    Date = new DateOnly(2024, 8, 18),
                    StartTime = new TimeOnly(00, 00),
                    EndTime = new TimeOnly(08, 00),
                    WorkerId = new Guid("b789a811-69e2-46ba-8e11-ce160dc4139f")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("4f8d6f58-0604-44f8-b1a7-ccc496e12433"),
                    ShiftType = "8-16",
                    Day = "Monday",
                    Date = new DateOnly(2024, 8, 19),
                    StartTime = new TimeOnly(08, 00),
                    EndTime = new TimeOnly(16, 00),
                    WorkerId = new Guid("822b6cc2-7a1a-4941-a3de-1007328b7758")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("1ba26260-6a86-4c55-8e55-2eebe9a88b20"),
                    ShiftType = "0-8",
                    Day = "Tuesday",
                    Date = new DateOnly(2024, 8, 20),
                    StartTime = new TimeOnly(16, 00),
                    EndTime = new TimeOnly(23, 59),
                    WorkerId = new Guid("7bd2b360-bcaf-49fb-967e-78527716f2e7")
                },
                new Entities.Models.Shift
                {
                    Id = new Guid("3b45dbf8-9740-473b-8f97-2087a95be610"),
                    ShiftType = "0-8",
                    Day = "Monday",
                    Date = new DateOnly(2024, 8, 21),
                    StartTime = new TimeOnly(00, 00),
                    EndTime = new TimeOnly(08, 00),
                    WorkerId = new Guid("822b6cc2-7a1a-4941-a3de-1007328b7758")
                }
            );
            if (context.Roles.Any())
            {
                return;
            }
            context.Roles.AddRange(
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                }
            );
            context.SaveChanges();
        }
    }
}
