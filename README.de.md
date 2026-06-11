# WindowsTimeSynchronizer

[🇬🇧 English](README.md) | 🇩🇪 **Deutsch**

Eine winzige Konsolenanwendung, die Windows zwingt, die Systemuhr neu zu
synchronisieren. Sie startet den Windows-Zeitdienst und stößt über `w32tm`
sofort eine Synchronisation an, damit du nicht mehr daran denken musst, eine
abdriftende Uhr von Hand zu korrigieren.

Die Anwendung fordert automatisch Administratorrechte an (beim manuellen Start
erscheint eine UAC-Abfrage), da sowohl das Starten des Zeitdienstes als auch das
erzwungene Resync erhöhte Rechte benötigen.

## Voraussetzungen

- Windows 10 oder 11
- Administratorrechte (die App fordert sie selbst an)

## Installation

1. Öffne die **Releases**-Seite dieses Repositorys.
2. Lade `WindowsTimeSynchronizer.exe` aus dem neuesten Release herunter.
3. Lege sie an einen festen Ort, zum Beispiel `C:\Tools\WindowsTimeSynchronizer.exe`.

Beim ersten Start kann Windows SmartScreen die Meldung
„Der Computer wurde durch Windows geschützt" anzeigen, weil die Datei nicht
signiert ist. Das ist bei einem privaten Tool normal. Um die Warnung zu
entfernen: Rechtsklick auf die `.exe` → **Eigenschaften** → unten den Haken bei
**Zulassen** setzen → **OK**. Alternativ in PowerShell:

```powershell
Unblock-File "C:\Tools\WindowsTimeSynchronizer.exe"
```

## Manuell ausführen

Doppelklick auf `WindowsTimeSynchronizer.exe` und die UAC-Abfrage bestätigen.
Die App startet den Zeitdienst, erzwingt eine Synchronisation und beendet sich
anschließend.

## Automatisch beim Start ausführen (empfohlene Einrichtung)

Nutze die Windows-**Aufgabenplanung** statt des Autostart-Ordners. Die
Aufgabenplanung kann die App mit erhöhten Rechten starten – und zwar **ohne**
bei jeder Anmeldung eine UAC-Abfrage anzuzeigen.

1. Drücke `Win + R`, gib `taskschd.msc` ein und drücke Enter.
2. Klicke auf **Aufgabe erstellen…** (nicht „Einfache Aufgabe erstellen").
3. Reiter **Allgemein**: einen Namen vergeben und
   **Mit höchsten Privilegien ausführen** anhaken. Das ist der entscheidende
   Haken – damit läuft sie elevated ohne UAC-Popup.
4. Reiter **Trigger** → **Neu…** → bei „Aufgabe starten" **Bei der Anmeldung**
   wählen.
5. Reiter **Aktionen** → **Neu…** → **Programm starten** → deine
   `WindowsTimeSynchronizer.exe` auswählen.
6. Mit **OK** bestätigen.

Da die Aufgabe die App bereits elevated startet, ist die Selbst-Elevation im
Code sofort erfüllt und es erscheint keine UAC-Abfrage.

### Die Uhr dauerhaft korrekt halten

Eine einzelne Synchronisation bei der Anmeldung ist gut, aber wenn deine Uhr
stark driftet, läuft sie bis zum nächsten Neustart wieder weg. Für dauerhafte
Genauigkeit fügst du einen zeitbasierten Trigger hinzu:

- Im Reiter **Trigger** einen weiteren Trigger **Nach einem Zeitplan** →
  **Täglich** anlegen und unter „Erweiterte Einstellungen"
  **Aufgabe wiederholen alle** aktivieren (z. B. 6 Stunden).

## Prüfen, ob wirklich synchronisiert wurde

Nach dem Ausführen den Status des Zeitdienstes abfragen:

```powershell
w32tm /query /status
```

Sieh dir die Zeile **Letzte erfolgreiche Synchronisierungszeit** an – sie sollte
„gerade eben" sein.