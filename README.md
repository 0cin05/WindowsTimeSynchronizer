# WindowsTimeSynchronizer

🇬🇧 **English** | [🇩🇪 Deutsch](README.de.md)

A small Windows tool that forces a system clock resync. It starts the Windows Time service and triggers an immediate sync via `w32tm`.

On first run, the app automatically registers itself as a scheduled task that runs at every login (with a 30-second delay). No manual setup needed — just run it once.

## Requirements

- Windows 10 or 11
- Administrator rights (the app elevates itself via UAC)

## Installation

1. Download `WindowsTimeSynchronizer.exe` from the [Releases](../../releases) page.
2. Place it somewhere permanent, e.g. `C:\Tools\WindowsTimeSynchronizer.exe`.
3. Run it once — done.

If SmartScreen warns you, right-click the `.exe` → **Properties** → tick **Unblock** → **OK**.

## What happens on launch

1. The app requests admin rights (UAC prompt on manual start).
2. It registers (or updates) a scheduled task so it runs automatically at every login.
3. It starts the Windows Time service and forces a resync.

## Verify

```powershell
w32tm /query /status
```

Check **Last Successful Sync Time**.