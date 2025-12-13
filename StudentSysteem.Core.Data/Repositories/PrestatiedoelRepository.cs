using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;
using StudentSysteem.Core.Models;

namespace StudentSysteem.Core.Data.Repositories
{
    public class PrestatiedoelRepository : DatabaseVerbinding, IPrestatiedoelRepository
    {
        public PrestatiedoelRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            //Tabel: Prestatiedoel (Hoofdtabel)
            MaakTabel(@"CREATE TABLE IF NOT EXISTS Prestatiedoel (
                [processtap_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                [niveau] VARCHAR(255),
                [beschrijving] TEXT,
                [criterium_id] INTEGER,
                FOREIGN KEY([criterium_id]) REFERENCES Criterium(criterium_id)
            )");

            // PrestatiedoelCriteria (koppeltabel)
            MaakTabel(@"CREATE TABLE IF NOT EXISTS PrestatiedoelCriteria (
                prestatiedoel_id INTEGER,
                criterium_id INTEGER,
                PRIMARY KEY (prestatiedoel_id, criterium_id),
                FOREIGN KEY (prestatiedoel_id) REFERENCES Prestatiedoel(processtap_id),
                FOREIGN KEY (criterium_id) REFERENCES Criterium(criterium_id)
            )");

            // Prestatiedoel: OP NIVEAU
            List<string> insertOpNiveauPrestatiedoel =
            [
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id)
                  VALUES (
                      'Op niveau',
                      'Maak een domeinmodel volgens een UML klassendiagram en leg deze vast in je plan en/of ontwerpdocumenten',
                      1
                  )"
            ];
            VoegMeerdereInMetTransactie(insertOpNiveauPrestatiedoel);

            // Criteria bij OP NIVEAU 
            List<string> insertOpNiveauCriteria =
            [
                @"INSERT OR IGNORE INTO PrestatiedoelCriteria (prestatiedoel_id, criterium_id) VALUES (1, 1)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriteria (prestatiedoel_id, criterium_id) VALUES (1, 2)",
                @"INSERT OR IGNORE INTO PrestatiedoelCriteria (prestatiedoel_id, criterium_id) VALUES (1, 3)"
            ];
            VoegMeerdereInMetTransactie(insertOpNiveauCriteria);

            // Prestatiedoel: BOVEN NIVEAU
            List<string> insertBovenNiveauPrestatiedoel =
            [
                @"INSERT OR IGNORE INTO Prestatiedoel (niveau, beschrijving, criterium_id)
                  VALUES (
                      'Boven niveau',
                      'Maak een volledig, helder en contextueel correct domeinmodel volgens UML',
                      4
                  )"
            ];
            VoegMeerdereInMetTransactie(insertBovenNiveauPrestatiedoel);

            // Criteria bij BOVEN NIVEAU 
            List<string> insertBovenNiveauCriteria =
            [
                @"INSERT OR IGNORE INTO PrestatiedoelCriteria (prestatiedoel_id, criterium_id) VALUES (2, 4)"
            ];
            VoegMeerdereInMetTransactie(insertBovenNiveauCriteria);
        }
        public List<Prestatiedoel> HaalAllePrestatiedoelenOp()
        {
            var resultaten = new List<Prestatiedoel>();

            OpenVerbinding();

            using var cmd = Verbinding.CreateCommand();
            cmd.CommandText = @"SELECT processtap_id, niveau, beschrijving, criterium_id 
                        FROM Prestatiedoel";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                resultaten.Add(new Prestatiedoel
                {
                    Id = reader.GetInt32(0),
                    Niveau = reader.GetString(1),
                    Beschrijving = reader.GetString(2),
                    CriteriumId = reader.GetInt32(3)
                });
            }

            SluitVerbinding();
            return resultaten;
        }
    }
}

