using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Interfaces.Services;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Services;

public class FeedbackService: IFeedbackService
{
    private readonly IFeedbackRepository _repository;

    public FeedbackService(IFeedbackRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Feedback> HaalFeedbackOp(Datapunt datapunt, int studentId)
    {
        return _repository.HaalFeedbackOp(datapunt, studentId);
    }
}