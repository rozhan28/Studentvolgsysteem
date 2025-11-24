using NUnit.Framework;
using StudentVolgSysteem.Core.Models;
using StudentVolgSysteem.Core.Services;
using Assert = NUnit.Framework.Assert;
using Moq;
using System;

namespace StudentSysteem.Tests
{
    public class CoreTests
    {

        [Test]
        public void SelfReflection_DefaultValues_ShouldBeSetCorrectly()
        {
            var reflection = new SelfReflection();

            Assert.That(reflection.PrestatieNiveau, Is.EqualTo(""));
            Assert.That(reflection.Toelichting, Is.EqualTo(""));
            Assert.That(reflection.Datum, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public void SelfReflection_SetProperties_ShouldStoreValues()
        {
            var reflection = new SelfReflection
            {
                Id = 5,
                StudentId = 10,
                PrestatieNiveau = "Op niveau",
                Toelichting = "Goed gedaan",
                Datum = new DateTime(2024, 5, 10)
            };

            Assert.That(reflection.Id, Is.EqualTo(5));
            Assert.That(reflection.StudentId, Is.EqualTo(10));
            Assert.That(reflection.PrestatieNiveau, Is.EqualTo("Op niveau"));
            Assert.That(reflection.Toelichting, Is.EqualTo("Goed gedaan"));
            Assert.That(reflection.Datum, Is.EqualTo(new DateTime(2024, 5, 10)));
        }


        [Test]
        public void SelfReflectionService_Add_ShouldBeCalled()
        {
            var mock = new Mock<ISelfReflectionService>();

            var reflection = new SelfReflection { StudentId = 1 };

            mock.Object.Add(reflection);

            mock.Verify(s => s.Add(reflection), Times.Once);
        }

        [Test]
        public void SelfReflectionService_GetByStudent_ReturnsCorrectList()
        {
            var mock = new Mock<ISelfReflectionService>();
            var expectedList = new List<SelfReflection>
            {
                new SelfReflection { StudentId = 1, PrestatieNiveau = "Op niveau" }
            };

            mock.Setup(s => s.GetByStudent(1)).Returns(expectedList);

            var result = mock.Object.GetByStudent(1);

            Assert.That(result, Is.EqualTo(expectedList));
            Assert.That(result.Count, Is.EqualTo(1));
        }


        [Test]
        public void UserSession_LoginAls_ShouldSetRole()
        {
            UserSession.LoginAls("Docent");

            Assert.That(UserSession.HuidigeRol, Is.EqualTo("Docent"));
        }

        [Test]
        public void UserSession_Loguit_ShouldClearRole()
        {
            UserSession.LoginAls("Student");

            UserSession.Loguit();

            Assert.That(UserSession.HuidigeRol, Is.EqualTo(""));
        }
    }
}
