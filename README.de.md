# WindowsTimeSynchronizer

[🇬🇧 English](README.md) | 🇩🇪 **Deutsch**

Ein kleines Windows-Tool, das die Systemuhr neu synchronisiert. Es startet den Windows-Zeitdienst und erzwingt über `w32tm` eine sofortige Synchronisation.

Beim ersten Start registriert sich die App automatisch als geplante Aufgabe, die bei jeder Anmeldung ausgeführt wird (mit 30 Sekunden Verzögerung). Einfach einmal starten, danach läuft alles automatisch.

## Voraussetzungen

- Windows 10 oder 11
- Administratorrechte (die App fordert sie selbst per UAC an)

## Installation

1. `WindowsTimeSynchronizer.exe` von der [Releases](../../releases)-Seite herunterladen.
2. An einen festen Ort legen, z.B. `C:\Tools\WindowsTimeSynchronizer.exe`.
3. Einmal ausführen — fertig.

Falls SmartScreen warnt: Rechtsklick auf die `.exe` → **Eigenschaften** → **Zulassen** anhaken → **OK**.

## Was beim Start passiert

1. Die App fordert Adminrechte an (UAC-Abfrage bei manuellem Start).
2. Sie registriert (oder aktualisiert) eine geplante Aufgabe für den automatischen Start bei jeder Anmeldung.
3. Sie startet den Zeitdienst und erzwingt eine Synchronisation.

## Prüfen

```powershell
w32tm /query /status
```

Zeile **Letzte erfolgreiche Synchronisierungszeit** prüfen.