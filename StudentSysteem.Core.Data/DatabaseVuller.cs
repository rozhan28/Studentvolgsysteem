namespace StudentSysteem.Core.Data
{
    public class DatabaseVuller : DatabaseVerbinding
    {
        // ClusterRepository data
        
        // Tabellen aanmaken (Cruciaal voor "no such table" fix)
        List<string> CreatieTabellen = [
            // Criterium Tabel
            @"CREATE TABLE IF NOT EXISTS Criterium (
                criterium_id INTEGER PRIMARY KEY,
                beschrijving TEXT NOT NULL
            );",
            
            // Feedback Tabel
            // Let op: Ik heb aangenomen dat je ook een Student, Docent en Vaardigheid tabel hebt
            @"CREATE TABLE IF NOT EXISTS Feedback (
                feedback_id INTEGER PRIMARY KEY,
                niveauaanduiding TEXT,
                toelichting TEXT,
                datum TEXT,
                tijd TEXT,
                student_id INTEGER,
                docent_id INTEGER,
                vaardigheid_id INTEGER,
                FOREIGN KEY(student_id) REFERENCES Student(student_id),
                FOREIGN KEY(docent_id) REFERENCES Docent(docent_id),
                FOREIGN KEY(vaardigheid_id) REFERENCES Vaardigheid(vaardigheid_id)
            );"
            // Voeg hier de CREATE TABLE commando's voor alle andere tabellen toe!
        ];


        // CriteriumRepository data
        List<string> VoegCriterium = [
            @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(1, 'Het domeinmodel weerspiegelt de belangrijke onderdelen van het domein')",
            @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(2, 'De syntax van het domeinmodel is correct volgens UML')",
            @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(3, 'De syntax van het domeinmodel is correct volgens UML')",
            @"INSERT OR REPLACE INTO Criterium(criterium_id, beschrijving) VALUES(4, ' Het domeinmodel is volledig, helder en sluit logisch aan bij de context van het project')"
        ];

        // Feedback
        List<string> VoegFeedback = [
            @"INSERT OR REPLACE INTO Feedback(niveauaanduiding, toelichting, datum, tijd, student_id, docent_id, vaardigheid_id) 
            VALUES('1', 'NULL', NULL, NULL, NULL, NULL, NULL)"
        ];
        

        // Vult database tabellen met standaard waarden
        public void TabelVuller()
        {
            // STAP 1: Zorg ervoor dat alle tabellen bestaan
            foreach(var sql in CreatieTabellen)
            {
                MaakTabel(sql);
            }
            
            // VoegMeerdereInMetTransactie(VoegCluster);

            // Criterium
            VoegMeerdereInMetTransactie(VoegCriterium);

            // Feedback
            VoegMeerdereInMetTransactie(VoegFeedback);

            // Ect
        }
    }
}