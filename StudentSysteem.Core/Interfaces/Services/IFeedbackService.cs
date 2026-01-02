using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Interfaces.Services;

public interface IFeedbackService
{
    IEnumerable<Feedback> HaalFeedbackOp(Datapunt datapunt, int studentId);
}