using NUnit.Framework;
using Moq;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Models;


namespace StudentSysteem.Tests
{
    [TestFixture]
    public class ToelichtingTest
    {
        private Mock<IFeedbackRepository> _feedbackRepositoryMock;
        private FormulierService _formulierService;

        [SetUp]
        public void SetUp()
        {
            _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            _formulierService = new FormulierService(_feedbackRepositoryMock.Object);
        }
        //Bij deze unittesten wordt niet alleen de toelichting getest, maar ook de gehele Feedback.
        // UC1 / TC1-01.1.1 – Docent kan toelichting invoeren en opslaan 
        [Test]
        public void UC1_HappyPath_ToelichtingWordtOpgeslagen()
        {
            // Arrange (Preconditie)
            Criterium criterium = new(
                id: 1,
                beschrijving: "Domeinmodel",
                niveau: Niveauaanduiding.OpNiveau
            );

            Toelichting toelichting = new()
            {
                Tekst = "Domeinmodel is goed gemaakt",
                GeselecteerdeOptie = criterium
            };

            Feedback feedback = new(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 5,
                FeedbackGeverId = 5,
                Toelichtingen = new List<Toelichting> { toelichting }
            };

            List<Feedback> feedbackLijst = new() { feedback };

            // Act (Stappen)
            _formulierService.SlaFeedbackOp(feedbackLijst);

            // Assert (Postconditie)
            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.IsAny<List<Feedback>>()),
                Times.Once,
                "Feedback moet worden opgeslagen wanneer de invoer geldig is"
            );
        }

        //UC1 / TC1-01.1.3.1 - Docent kan opslaan zonder toelichting
        [Test]
        public void UC1_HappyPath_Docent_ZonderToelichting_WordtOpgeslagen()
        {
            Feedback feedback = new Feedback(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 5,            
                FeedbackGeverId = 5,
                Toelichtingen = new List<Toelichting>
        {
            new Toelichting { Tekst = "" }
        }
            };

            _formulierService.SlaFeedbackOp(new() { feedback });

            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.IsAny<List<Feedback>>()),
                Times.Once
            );
        }

        // UC1 / TC1-01.4 – Student kan geen toelichting met null tekst opslaan \
        [Test]
        public void UC1_UnhappyPath_Student_ToelichtingNull_GooitArgumentException()
        {
            // Arrange
            Toelichting toelichting = new Toelichting
            {
                Tekst = null
            };

            Feedback feedback = new Feedback(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 0,              
                FeedbackGeverId = 10,
                Toelichtingen = new List<Toelichting> { toelichting }
            };

            List<Feedback> feedbackLijst = new() { feedback };

            // Act + Assert
            Assert.Throws<ArgumentException>(() =>
                _formulierService.SlaFeedbackOp(feedbackLijst)
            );

            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.IsAny<List<Feedback>>()),
                Times.Never
            );
        }

    }
}

