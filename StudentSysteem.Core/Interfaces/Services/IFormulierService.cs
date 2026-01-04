using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services
{
    public interface IFormulierService
    {
        void SlaToelichtingenOp(List<Toelichting> toelichtingen, int studentId, int prestatiedoelId, int feedbackgeverId);
    }
}