using StudentSysteem.Core.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSysteem.Core.Data
{
    public class DatabaseVuller : DatabaseVerbinding
    {
        //ClusterRepository data


        //CriteriumRepository data
        CriteriumRepository db = new();
        List<string> VoegCriterium = [@"INSERT OR IGNORE INTO Criterium(criterium_id, beschrijving) VALUES(NULL, 'Het domeinmodel weerspiegelt de belangrijke onderdelen van het domein')",
                                          @"INSERT OR IGNORE INTO Criterium(criterium_id, beschrijving) VALUES(NULL, 'De syntax van het domeinmodel is correct volgens UML')",
                                          @"INSERT OR IGNORE INTO Criterium(criterium_id, beschrijving) VALUES(NULL, 'Het domeinmodel is op een logische locatie vastgelegd')"];

        //Ect

        //Vult database tabellen met standaard waarden
        public void TabelVuller()
        {
            //Cluster

            //db.VerwijderInhoud("Cluster");
            //VoegMeerdereInMetTransactie(VoegCluster);

            //Criterium
            db.VerwijderInhoud("Criterium");
            VoegMeerdereInMetTransactie(VoegCriterium);

            //Ect
        }
    }
}
