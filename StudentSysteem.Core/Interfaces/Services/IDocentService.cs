using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IDocentService
{
    public Docent? HaalOp();
    public List<Docent> HaalAlleDocentenOp();
    public Docent? LoginDocent();
}