# WindowsTimeSynchronizer

A tiny console application that forces Windows to re-synchronize the system clock.
It starts the Windows Time service and triggers an immediate sync via `w32tm`, so
you no longer have to remember to fix a drifting clock by hand.

The app requests administrator rights automatically (a UAC prompt appears on
manual start), because both starting the time service and forcing a resync require
elevation.

## Requirements

- Windows 10 or 11
- Administrator rights (the app elevates itself)

## Installation

1. Go to the **Releases** page of this repository.
2. Download `WindowsTimeSynchronizer.exe` from the latest release.
3. Put it somewhere permanent, for example `C:\Tools\WindowsTimeSynchronizer.exe`.

When you first run the file, Windows SmartScreen may show
"Windows protected your PC" because the file is unsigned. This is expected for a
private tool. To remove the warning, right-click the `.exe` → **Properties** →
tick **Unblock** at the bottom → **OK**. Alternatively, in PowerShell:

```powershell
Unblock-File "C:\Tools\WindowsTimeSynchronizer.exe"
```

## Running manually

Double-click `WindowsTimeSynchronizer.exe` and confirm the UAC prompt.
The app starts the time service and forces a sync, then exits.

## Run automatically at startup (recommended setup)

Use the Windows **Task Scheduler** rather than the Startup folder. The scheduler
can launch the app with elevated rights and **without** showing a UAC prompt
every time you log in.

1. Press `Win + R`, type `taskschd.msc`, and press Enter.
2. Click **Create Task…** (not "Create Basic Task").
3. **General** tab: give it a name and tick
   **Run with highest privileges**. This is the key setting — it runs elevated
   with no UAC popup.
4. **Triggers** tab → **New…** → set "Begin the task" to **At log on**.
5. **Actions** tab → **New…** → **Start a program** → browse to your
   `WindowsTimeSynchronizer.exe`.
6. Click **OK**.

Because the task already launches the app elevated, the in-app self-elevation is
satisfied immediately and no UAC prompt appears.

### Keep the clock correct over time

A single sync at log on is fine, but if your clock drifts heavily it will wander
again before the next restart. To keep it accurate, add a time-based trigger:

- In the **Triggers** tab, add another trigger **On a schedule** → **Daily**,
  and under "Advanced settings" enable **Repeat task every** (e.g. 6 hours).

## Verify that it actually synced

After running the app, check the time service status:

```powershell
w32tm /query /status
```

Look at the **Last Successful Sync Time** — it should be just now.
