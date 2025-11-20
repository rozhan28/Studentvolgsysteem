
Welkom bij dit project Hieronder vindt je richtlijnen voor het gebruik van Gitflow, commit conventies, pull requests en branchbeheer.

Gitflow workflow Deze repository gebruikt gitflow om development gestructureerd te houden.
Branch structuur:

Branch type: Doel:

Main - Productieklare code
develop - Integratie van alle features
feature/ - Nieuwe functionaliteit of use case
release/ - Voorbereiding op release
hotfix/ - spoedfixes
Werken met branches
Wanneer je aan een nieuwe use case of functionaliteit begint, maak je altijd eerst een feature branch vanaf develop. Geef de branch een duidelijke naam (feature/UC#-korte-beschrijving), zodat iedereen meteen ziet waar de branch voor bedoeld is. Tijdens het werken commit je en beschrijf je duidelijk wat er veranderd is. Zodra je klaar bent push je de branch en maak je een Pull request naar develop. Na review en succesvolle tests, merge je de branch en verwijder je de feature branch weer.

Feature branches

Maak altijd vanaf develop
Branch naam: feature/UC#-korte-beschrijving-
Commit met duidelijk berichten.
Push naar remote en maak een Pull request naar develop.
Verwijder feature branch na merge.
Als we een release voorbereiden, bijvoorbeeld versie 1.0.0, maken we een release branch van develop. Hierin doen we alleen nog kleine bugfixes, documentatie, updates of andere kleine aanpassingen. Wanneer de release klaar is, merge je deze naar main en ook terug gemergd naar develop zodat er niks verloren gaat.

Release branches

Start vanaf de branch develop wanneer een release gepland is.
Merge naar main en vervolgens terug naar develop.
Soms onstaan er problemen met de code. Hiervoor gebruiken we een hotfix branch. Dit starten we vanaf main. Dit wordt direct doorgevoerd naar main en ook terug gemerged naar develop.

Hotfix branches

start vanaf main bij urgente productieproblemen.
Merge eerst naar main en daarna terug naar develop.
Commits Bij pull request is het belangrijk dat je uitlegt wat er verandert is.
Voor commits geldt het hetzelfde: beschrijf kort wat je heb aangepast voor duidelijkheid.
