using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;
using System;
using System.Collections.Generic;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        private readonly ICriteriumRepository _criteriumRepository;

        public PrestatiedoelRepository(DbConnectieHelper dbConnectieHelper, ICriteriumRepository criteriumRepository)
            : base(dbConnectieHelper)
        {
            _criteriumRepository = criteriumRepository;

            // Prestatiedoel tabel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Prestatiedoel (
                    prestatiedoel_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    niveau TEXT NOT NULL,
                    beschrijving TEXT NOT NULL,
                    criterium_id INTEGER NOT NULL,
                    ai_assessment_scale TEXT NOT NULL,
                    FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
                );
            ");

            // Koppeltabel
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS PrestatiedoelCriterium (
                    prestatiedoel_id INTEGER NOT NULL,
                    criterium_id INTEGER NOT NULL,
                    PRIMARY KEY (prestatiedoel_id, criterium_id),
                    FOREIGN KEY (prestatiedoel_id) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
                );
            ");

            // Seed data
            List<string> seed = new()
            {
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id, ai_assessment_scale)
                  VALUES (
                    'Op niveau',
                    'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten.',
                    1,
                    'Samenwerking'
                  )",

                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id, ai_assessment_scale)
                  VALUES (
                    'Op niveau',
                    'Toepassen van modelleertechnieken door principes toe te passen volgens de ontwerprichtlijnen van HBO-ICT.',
                    2,
                    'Technisch inzicht'
                  )"
            };

            VoegMeerdereInMetTransactie(seed);
        }

        public List<Prestatiedoel> HaalAllePrestatiedoelenOp()
        {
            var lijst = new List<Prestatiedoel>();

            OpenVerbinding();
            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"
                SELECT prestatiedoel_id, niveau, beschrijving, criterium_id, ai_assessment_scale
                FROM Prestatiedoel;
            ";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lijst.Add(new Prestatiedoel
                {
                    Id = reader.GetInt32(0),
                    Niveau = reader.GetString(1),
                    Beschrijving = reader.GetString(2),
                    CriteriumId = reader.GetInt32(3),
                    AiAssessmentScale = reader.GetString(4)
                });
            }

            SluitVerbinding();
            return lijst;
        }
    }
}

