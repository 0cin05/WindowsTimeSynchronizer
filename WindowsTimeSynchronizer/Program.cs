using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32.TaskScheduler;
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

EnsureScheduledTask();

Process.Start("net", "start w32time").WaitForExit();
Process.Start("w32tm", "/resync /force").WaitForExit();

Environment.Exit(0);


static void EnsureScheduledTask()
{
    const string taskName = "WindowsTimeSynchronizer";
    const string taskDescription = "Synchronizes Windows Time";
    string exePath = Process.GetCurrentProcess().MainModule!.FileName;

    using var taskService = new TaskService();
    var existingTask = taskService.GetTask(taskName);
    
    if (existingTask is not null)
    {
        var currentAction = existingTask.Definition.Actions.OfType<ExecAction>().FirstOrDefault();
        if (currentAction?.Path == exePath)
            return;
    }
    
    var task = taskService.NewTask();
    task.RegistrationInfo.Description = taskDescription;
    task.Principal.RunLevel = TaskRunLevel.Highest;
    task.Triggers.Add(new LogonTrigger
    {
        Delay = TimeSpan.FromSeconds(30),
    });
    task.Actions.Add(new ExecAction(exePath));

    taskService.RootFolder.RegisterTaskDefinition(taskName, task);
}