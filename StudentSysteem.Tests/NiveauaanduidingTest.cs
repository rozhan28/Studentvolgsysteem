using NUnit.Framework;
using Moq;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Services;
using StudentSysteem.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace StudentSysteem.Tests
{
    [TestFixture]
    public class CriteriumNiveauaanduidingTest
    {
        private Mock<IFeedbackRepository> _feedbackRepositoryMock;
        private FormulierService _formulierService;

        [SetUp]
        public void SetUp()
        {
            _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            _formulierService = new FormulierService(_feedbackRepositoryMock.Object);
        }
        
        [Test]
        public void UC1_HappyPath_NiveauaanduidingEnCriteria_WordenOpgeslagen()
        {
            int aantalCriteria = 5;
            
            List<Criterium> criteria = new();
            for (int i = 0; i < aantalCriteria; i++)
            {
                criteria.Add(new Criterium(i, $"criteria: {i}", Niveauaanduiding.OpNiveau));
            }

            Feedback feedback = new(1)
            {
                StudentId = 1,
                DocentId = 1,
                FeedbackGeverId = 0,
                Niveauaanduiding = Niveauaanduiding.OpNiveau,
                Criteria = criteria,
                Toelichtingen = new List<Toelichting>()
            };

            _formulierService.SlaFeedbackOp(new() { feedback });

            _feedbackRepositoryMock.Verify(
                repo => repo.VoegFeedbackToe(It.Is<List<Feedback>>(lijst =>
                    lijst.Single().Criteria.Count == aantalCriteria &&
                    lijst.Single().Criteria
                        .Select((c, i) =>
                            c.Beschrijving == $"criteria: {i}" &&
                            c.Niveau == Niveauaanduiding.OpNiveau
                        )
                        .All(x => x) &&
                    lijst.Single().Niveauaanduiding == Niveauaanduiding.OpNiveau
                )),
                Times.Once
            );

        }
    }
}