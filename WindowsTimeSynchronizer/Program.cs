using System.Diagnostics;

Process.Start("net", "start w32time").WaitForExit();
Process.Start("w32tm", "/resync /force").WaitForExit();

Environment.Exit(0);