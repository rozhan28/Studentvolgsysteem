namespace StudentSysteem.Core.Interfaces.Repository
{
    public interface ICriteriumRepository
    {
        List<Criterium> HaalCriteriaOpVoorNiveau(string niveau);

        void SlaGeselecteerdeCriteriaOp(
            int feedbackId,
            IEnumerable<Criterium> geselecteerdeCriteria,
            string niveau);
    }
}