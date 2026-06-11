using System.Diagnostics;
using System.Security.Principal;
using static System.Security.Principal.WindowsBuiltInRole;

var identity = WindowsIdentity.GetCurrent();
var principal = new WindowsPrincipal(identity);

if (!principal.IsInRole(Administrator))
{
    var exePath = Environment.ProcessPath!;
    Process.Start(new ProcessStartInfo(exePath)
    {
        UseShellExecute = true,
        Verb = "runas"
    });
    return;
}

Process.Start("net", "start w32time").WaitForExit();
Process.Start("w32tm", "/resync /force").WaitForExit();

Environment.Exit(0);