using NUnit.Framework;
using Moq;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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

        //UC1 / TC1-01.1.3 - student kan opslaan zonder toelichting (Unhappy Path)
        [Test]
        public void UC1_UnhappyPath_LegeToelichting_GooitArgumentException()
        {
            // Arrange
            Toelichting toelichting = new Toelichting
            {
                Tekst = ""
            };

            Feedback feedback = new Feedback(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 5,
                FeedbackGeverId = 5,
                Toelichtingen = new List<Toelichting> { toelichting }
            };

            List<Feedback> feedbackLijst = new List<Feedback> { feedback };

            // Act + Assert
            Assert.Throws<ArgumentException>(() =>
                _formulierService.SlaFeedbackOp(feedbackLijst)
            );
        }


        // UC1 / TC1-01.1.1 – Docent kan toelichting invoeren en opslaan (Happy Path)
        [Test]
        public void UC1_HappyPath_ToelichtingWordtOpgeslagen()
        {
            // Arrange (Preconditie)
            var criterium = new Criterium(
                id: 1,
                beschrijving: "Domeinmodel",
                niveau: Niveauaanduiding.OpNiveau
            );

            var toelichting = new Toelichting
            {
                Tekst = "Domeinmodel is goed gemaakt",
                GeselecteerdeOptie = criterium
            };

            var feedback = new Feedback(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 5,
                FeedbackGeverId = 5,
                Toelichtingen = new List<Toelichting> { toelichting }
            };

            var feedbackLijst = new List<Feedback> { feedback };

            // Act (Stappen)
            _formulierService.SlaFeedbackOp(feedbackLijst);

            // Assert (Postconditie)
            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.IsAny<List<Feedback>>()),
                Times.Once,
                "Feedback moet worden opgeslagen wanneer de invoer geldig is"
            );            
        }

        [Test]
        public void UC1_HappyPath_MeerdereToelichtingenWordenOpgeslagen()
        {
            int aantalToelichtingen = 5;
            List<Toelichting> toelichtingen = new();
            for (int i = 0; i < aantalToelichtingen; i++)
            {
                Criterium criterium = new Criterium(i, "", Niveauaanduiding.OpNiveau);
                Toelichting toelichting = new Toelichting()
                    { Tekst = $"Meerdere toelichting test {i}: Alles is fout :)", GeselecteerdeOptie = criterium };

                toelichtingen.Add(toelichting);
            }


            Feedback feedback = new(vaardigheidId: 1)
            {
                StudentId = 10,
                DocentId = 5,
                FeedbackGeverId = 5,
                Toelichtingen = toelichtingen
            };

            List<Feedback> feedbackLijst = new();
            feedbackLijst.Add(feedback);

            // Act
            _formulierService.SlaFeedbackOp(feedbackLijst);

            // Assert
            _feedbackRepositoryMock.Verify(repo =>
                    repo.VoegFeedbackToe(It.Is<List<Feedback>>(lijst =>
                        lijst.Single().Toelichtingen.Count == aantalToelichtingen
                        &&
                        lijst.Single().Toelichtingen
                            .Select((t, i) => t.Tekst.StartsWith($"Meerdere toelichting test {i}"))
                            .All(x => x)
                    )),
                Times.Once
            );
        }
    }
}

