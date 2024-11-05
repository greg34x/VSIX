using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel.Design;
using System.Windows;
using Task = System.Threading.Tasks.Task;

namespace VSIX;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[Guid(VSIXPackage.PackageGuidString)]
[ProvideAutoLoad(UIContextGuids.NoSolution, PackageAutoLoadFlags.BackgroundLoad)] // Ensure it loads when VS starts
public sealed class VSIXPackage : AsyncPackage
{
    public const string PackageGuidString = "f9a8aea3-f579-4816-9cb5-4ae3a5d68ef7";

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
        Assumes.Present(commandService);

        var menuCommandID = new CommandID(PackageIds.guidVSIXPackageCmdSet, PackageIds.MyCommandId);
        var menuItem = new OleMenuCommand((s, e) => Execute(), menuCommandID);
        commandService.AddCommand(menuItem);
    }

    public static void Execute()
    {
        MessageBox.Show("Hello World!");
    }

    public static class PackageIds
    {
        public const string CmdSetGuid = "d858b0de-b9fa-49e8-a7ae-e080d04a78be"; // Match the GUID in .vsct
        public const int MyCommandId = 0x0100;
        public static readonly Guid guidVSIXPackageCmdSet = new Guid(CmdSetGuid);
    }
}
