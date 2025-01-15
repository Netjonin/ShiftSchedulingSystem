
namespace Entities.Exceptions;
public class ShiftNotFoundException : NotFoundException
{
    public ShiftNotFoundException(Guid shiftId) : base($"The company with id: {shiftId} doesn't exist.")
    {

    }
}
