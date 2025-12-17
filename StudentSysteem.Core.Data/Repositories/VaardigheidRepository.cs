using StudentSysteem.Core.Data.Helpers;
using StudentSysteem.Core.Interfaces.Repository;

namespace StudentSysteem.Core.Data.Repositories
{
    public class VaardigheidRepository : DatabaseVerbinding, IVaardigheidRepository
    {
        public VaardigheidRepository(DbConnectieHelper dbConnectieHelper)
            : base(dbConnectieHelper)
        {
            MaakTabel(@"
                CREATE TABLE IF NOT EXISTS Vaardigheid (
                    [vaardigheid_id] INTEGER PRIMARY KEY AUTOINCREMENT,
                    [naam] VARCHAR(255),
                    [beschrijving] TEXT,
                    [hboi_activiteit] VARCHAR(255),
                    [leertaken_url] VARCHAR(255),
                    [prestatiedoel_id] INTEGER,
                    [processtap_id] INTEGER,
                    FOREIGN KEY([prestatiedoel_id]) REFERENCES Prestatiedoel(prestatiedoel_id),
                    FOREIGN KEY([processtap_id]) REFERENCES Processtap(processtap_id)
                )");

            List<string> insertQueries = new()
            {
                @"INSERT OR IGNORE INTO Vaardigheid
                (naam, beschrijving, hboi_activiteit, leertaken_url, prestatiedoel_id, processtap_id)
                VALUES (
                    'Analyseren',
                    'Het maken van een domeinmodel volgens een UML klassendiagram',
                    'Maken domeinmodel',
                    'https://leertaken.nl/2.-Processen/1.-Requirementsanalyseproces/1.-Uitleg-defini%C3%ABren-probleemdomein',
                    1, 1)",

                @"INSERT OR IGNORE INTO Vaardigheid
                (naam, beschrijving, hboi_activiteit, leertaken_url, prestatiedoel_id, processtap_id)
                VALUES (
                    'Ontwerpen',
                    'Modelleertechnieken toepassen conform richtlijnen',
                    'Toepassen modelleertechnieken',
                    'https://leertaken.nl/2.-Processen/3.-Ontwerpproces/2.-Uitleg-toepassen-modelleertechnieken',
                    2, 2)"
            };

            VoegMeerdereInMetTransactie(insertQueries);
        }
    }
}