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
        List<string> VoegCriterium = [@"INSERT OR REPLACE  INTO Criterium(criterium_id, beschrijving) VALUES(1, 'Het domeinmodel weerspiegelt de belangrijke onderdelen van het domein')",
                                          @"INSERT OR REPLACE  INTO Criterium(criterium_id, beschrijving) VALUES(2, 'De syntax van het domeinmodel is correct volgens UML')",
                                          @"INSERT OR REPLACE  INTO Criterium(criterium_id, beschrijving) VALUES(3, 'De syntax van het domeinmodel is correct volgens UML')",
                                          @"INSERT OR REPLACE  INTO Criterium(criterium_id, beschrijving) VALUES(4, ' Het domeinmodel is volledig, helder en sluit logisch aan bij de context van het project')"];

        //Feedback
        List<string> VoegFeedback = [@"INSERT OR REPLACE INTO Feedback(niveauaanduiding, toelichting, datum, tijd, student_id, docent_id, vaardigheid_id) 
                                        VALUES('1', 'NULL', NULL, NULL, NULL, NULL, NULL)"];
        

        //Vult database tabellen met standaard waarden
        public void TabelVuller()
        {
            
            
            //VoegMeerdereInMetTransactie(VoegCluster);

            //Criterium
            VoegMeerdereInMetTransactie(VoegCriterium);

            //Feedback
            VoegMeerdereInMetTransactie(VoegFeedback);

            //Ect
        }
    }
}
