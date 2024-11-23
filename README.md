# bridgerv2
A cool logic game in which you have to skillfully build bridges to win

## Wie funktioniert das Spielprinzip
Das Spielbrett besteht aus Piers. Jeder die Spieler Rot und Blau haben jeweils 42 Piers. 
![Ansatz2](https://github.com/user-attachments/assets/8e3091a1-dd24-409b-acaa-de1cf77c1ad8)

Grün hat die zwei äußersten Spalten, während Rot die zwei äußersten Reihen der Matrix besitzt.
Das Ziel von Grün ist mit Bücken, die oberster und unterste Spalte in einer Linie zu verbinen. Rot hat hingegen das Ziel eine geschlossene Verbinung zwischen den zwei äußersten Reihen zu erstellen.
Dabei können die Brücken nur auf die weißen Felder gesetzt werden. Die brücken sind dann mit den Piers verbunden und ergeben das sozusagen eine Verbinung. 
Und wenn einer Verbindung beider Spieler vollständig ist, dann hat dieser Gewonnen.

Es soll dann ungefähr so in der Theorie sein (Bzw es sind schon leichte überlegungen für die KI mit eingezeichnet):
![Rechteck](https://github.com/user-attachments/assets/86819cc5-8b2b-4b95-a01d-c8f11f4fab12)

So jetzt erstmal das Spielprinzip dahinter.
Die Schwierigkeit dahinter, ist einen Algorithmus zu finden, der erkennt ob ein Spieler gewonnen hat und einen Algorithmus zu finden, welcher in der Lage ist auf Augenhöhe gegen den Menschen zu spielen (auch Zeiteffizent). Hier alle Überlegungen (unübersichtlich) in einem Bild.
![Ansatz](https://github.com/user-attachments/assets/7f9d8bdf-07e2-4e1d-a9b7-e6f3a793bba7)

Prinzipell kann man das auch in einer Matrix darstellen:
```
- 1 - 1 - 1 - 1 - 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 2 1 0 1 0 1 0 1 - 
2 0 2 0 2 0 2 0 2 0 2 
- 1 0 1 0 1 0 1 0 1 - 
2 2 2 2 2 0 2 0 2 0 2 
- 1 - 1 - 1 - 1 - 1 - 
```

0 = leeres Feld / unbesetztes Feld / Platz für Brücken
1 = grüner Spieler
2 = roter Spieler / KI
- = in der Theorie nie besetztes Feld
