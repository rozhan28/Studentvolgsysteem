using NUnit.Framework;
using Moq;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Models;
using System.Linq;

namespace StudentSysteem.Tests
{
    [TestFixture]
    public class ToelichtingTest
    {
        private Mock<IFeedbackRepository> _feedbackRepositoryMock;
        private FormulierService _formulierService;

        // Bij deze unittesten wordt niet alleen de toelichting getest,
        // maar ook de gehele Feedback.
        [SetUp]
        public void SetUp()
        {
            _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            _formulierService = new FormulierService(_feedbackRepositoryMock.Object);
        }

        // UC1 / TC1-01.1.1 – Docent kan toelichting invoeren en opslaan
        [Test]
        public void UC1_HappyPath_ToelichtingWordtOpgeslagen()
        {
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

            _formulierService.SlaFeedbackOp(new() { feedback });

            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.IsAny<List<Feedback>>()),
                Times.Once
            );
        }

        // UC1 / TC1-01.1.3.1 – Docent kan opslaan zonder toelichting
        [Test]
        public void UC1_HappyPath_Docent_ZonderToelichting_WordtOpgeslagen()
        {
            Feedback feedback = new(vaardigheidId: 1)
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

        // UC1 / TC1-01.4 – Student kan geen toelichting met null tekst opslaan
        [Test]
        public void UC1_UnhappyPath_Student_ToelichtingNull_GooitArgumentException()
        {
            Toelichting toelichting = new()
            {
                Tekst = null
            };

            Feedback feedback = new(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 0,
                FeedbackGeverId = 10,
                Toelichtingen = new List<Toelichting> { toelichting }
            };

            Assert.Throws<ArgumentException>(() =>
                _formulierService.SlaFeedbackOp(new() { feedback })
            );

            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.IsAny<List<Feedback>>()),
                Times.Never
            );
        }

        // UC1 / US4 – Meerdere toelichtingen worden correct opgeslagen
        [Test]
        public void UC1_HappyPath_MeerdereToelichtingenWordenOpgeslagen()
        {
            int aantalToelichtingen = 5;
            List<Toelichting> toelichtingen = new();

            for (int i = 0; i < aantalToelichtingen; i++)
            {
                Criterium criterium = new(i, "", Niveauaanduiding.OpNiveau);
                Toelichting toelichting = new()
                {
                    Tekst = $"Meerdere toelichting test {i}: Alles is fout :)",
                    GeselecteerdeOptie = criterium
                };
                toelichtingen.Add(toelichting);
            }

            Feedback feedback = new(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 5,
                FeedbackGeverId = 5,
                Toelichtingen = toelichtingen
            };

            _formulierService.SlaFeedbackOp(new() { feedback });

            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.Is<List<Feedback>>(lijst =>
                    lijst.Single().Toelichtingen.Count == aantalToelichtingen &&
                    lijst.Single().Toelichtingen
                        .Select((t, i) => t.Tekst.StartsWith($"Meerdere toelichting test {i}"))
                        .All(x => x)
                )),
                Times.Once
            );
        }
    }
}
