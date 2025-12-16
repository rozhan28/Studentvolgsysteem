using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ICriteriumRepository
    {
        List<Criterium> HaalCriteriaOpVoorNiveau(string niveau);
    }
}
