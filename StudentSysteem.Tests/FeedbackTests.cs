using Moq;
using NUnit.Framework;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Services;

namespace StudentSysteem.Tests.Services
{
    public class FeedbackFormulierServiceTests
    {
        private Mock<IFeedbackRepository> _mockRepo;
        private FeedbackFormulierService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IFeedbackRepository>();
            _service = new FeedbackFormulierService(_mockRepo.Object);
        }

        [Test]
        public void SlaToelichtingOp_MetGeldigeWaarde_RoeptRepositoryAan()
        {
            // Arrange
            string toelichting = "Dit ging goed";
            int studentId = 5;

            // Act
            _service.SlaToelichtingOp(toelichting, studentId);

            // Assert
            _mockRepo.Verify(
                r => r.VoegToelichtingToe(toelichting, studentId),
                Times.Once
            );
        }

        [Test]
        public void SlaToelichtingOp_LegeToelichting_GooitArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _service.SlaToelichtingOp("")
            );
        }

        [Test]
        public void SlaToelichtingOp_NullToelichting_GooitArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _service.SlaToelichtingOp(null)
            );
        }

        [Test]
        public void SlaToelichtingOp_StandaardStudentId_WerktCorrect()
        {
            // Arrange
            string toelichting = "Test met default id";

            // Act
            _service.SlaToelichtingOp(toelichting);

            // Assert
            _mockRepo.Verify(
                r => r.VoegToelichtingToe(toelichting, 1), 
                Times.Once
            );
        }
    }
}
