using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IProcesService
{
    public Proces? HaalOp(int procesId);
}