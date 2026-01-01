using Moq;
using NUnit.Framework;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using StudentSysteem.Core.Services;

namespace StudentSysteem.Tests
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
            var toelichting1 = new Toelichting { Tekst = "Dit ging goed 1!" };
            var toelichting2 = new Toelichting { Tekst = "Dit ging goed 2!" };
            var toelichtingen =  new List<Toelichting> { toelichting1, toelichting2 };
            int studentId = 5;

            // Act
            _service.SlaToelichtingenOp(toelichtingen, studentId);

            // Assert
            _mockRepo.Verify(
                r => r.VoegToelichtingenToe(toelichtingen, studentId),
                Times.Once
            );
        }

        [Test]
        public void SlaToelichtingOp_LegeToelichting_GooitArgumentException()
        {
            var toelichting1 = new Toelichting { Tekst = "Dit ging goed 1!" };
            var toelichting2 = new Toelichting { Tekst = "" };
            var toelichtingen =  new List<Toelichting> { toelichting1, toelichting2 };
            
            Assert.Throws<ArgumentException>(() =>
                _service.SlaToelichtingenOp(toelichtingen)
            );
        }

        [Test]
        public void SlaToelichtingOp_NullToelichting_GooitArgumentException()
        {
            var toelichting1 = new Toelichting { Tekst = "Dit ging goed 1!" };
            var toelichting2 = new Toelichting { Tekst = null };
            var toelichtingen =  new List<Toelichting> { toelichting1, toelichting2 };
            
            Assert.Throws<ArgumentException>(() =>
                _service.SlaToelichtingenOp(toelichtingen)
            );
        }

        [Test]
        public void SlaToelichtingOp_StandaardStudentId_WerktCorrect()
        {
            // Arrange
            var toelichting1 = new Toelichting { Tekst = "Dit ging goed 1!" };
            var toelichting2 = new Toelichting { Tekst = "Dit ging goed 2!" };
            var toelichtingen =  new List<Toelichting> { toelichting1, toelichting2 };

            // Act
            _service.SlaToelichtingenOp(toelichtingen);

            // Assert
            _mockRepo.Verify(
                r => r.VoegToelichtingenToe(toelichtingen, 1), 
                Times.Once
            );
        }
    }
}
