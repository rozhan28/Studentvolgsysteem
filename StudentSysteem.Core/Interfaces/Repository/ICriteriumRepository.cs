using StudentSysteem.Core.Models;

public interface ICriteriumRepository
{
    List<Criterium> HaalCriteriaOpVoorNiveau(string niveau);

    List<Criterium> HaalCriteriaOpVoorPrestatiedoel(
        int prestatiedoelId,
        string niveau);

    void SlaGeselecteerdeCriteriaOp(
        int feedbackId,
        IEnumerable<Criterium> geselecteerdeCriteria,
        string niveau);
}
