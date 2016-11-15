using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Process = System.Diagnostics.Process;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace Extensibility.VisualStudio.Restart
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(Vsix.Id)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    public sealed class VSPackage : Package
    {
        private DTE __DTE;
        private DteInitializer _dteInitializer;

        protected override void Initialize()
        {
            base.Initialize();

            InitializeDTE();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID restartCommandSingleID = new CommandID(PackageGuids.TopLevelMenuGroup, (int)PackageIds.Restart);
                OleMenuCommand restartSingleMenuItem = new OleMenuCommand(RestartMenuItemCallback, restartCommandSingleID);
                mcs.AddCommand(restartSingleMenuItem);

                restartSingleMenuItem.BeforeQueryStatus += OnBeforeQueryStatusSingle;

                CommandID restartElevatedCommandID = new CommandID(PackageGuids.RestartCommandsGroup, (int)PackageIds.RestartAsAdministrator);
                OleMenuCommand restartElevatedMenuItem = new OleMenuCommand(RestartMenuItemCallback, restartElevatedCommandID);
                mcs.AddCommand(restartElevatedMenuItem);

                restartElevatedMenuItem.BeforeQueryStatus += OnBeforeQueryStatusGroup;

                CommandID restartCommandID = new CommandID(PackageGuids.RestartCommandsGroup, (int)PackageIds.Restart);
                OleMenuCommand restartMenuItem = new OleMenuCommand(RestartMenuItemCallback, restartCommandID);
                mcs.AddCommand(restartMenuItem);

                restartMenuItem.BeforeQueryStatus += OnBeforeQueryStatusGroup;
            }
        }

        private void InitializeDTE()
        {
            try
            {
                __DTE = (DTE)GetService(typeof(DTE));
            }
            catch (Exception)
            {
                __DTE = null;
            }

            if (__DTE == null)
            {
                IVsShell shellService = (IVsShell)this.GetService(typeof(IVsShell));
                _dteInitializer = new DteInitializer(shellService, InitializeDTE);
            }
            else
            {
                _dteInitializer = null;
            }
        }

        private void OnBeforeQueryStatusGroup(object sender, EventArgs e)
        {
            OleMenuCommand item = (OleMenuCommand)sender;
            if (ElevationChecker.CanCheckElevation)
            {
                item.Visible = !ElevationChecker.IsElevated(Process.GetCurrentProcess().Handle);
            }
        }

        private void OnBeforeQueryStatusSingle(object sender, EventArgs e)
        {
            OleMenuCommand item = (OleMenuCommand)sender;
            if (ElevationChecker.CanCheckElevation)
            {
                item.Visible = ElevationChecker.IsElevated(Process.GetCurrentProcess().Handle);
            }
        }

        private void RestartMenuItemCallback(object sender, EventArgs e)
        {
            var dte = __DTE;

            if (dte == null)
            {
                return;
            }

            Debug.Assert(dte != null);

            bool elevated = ((OleMenuCommand)sender).CommandID.ID == PackageIds.RestartAsAdministrator;

            new RestartCommand().Restart(dte, elevated);
        }

        private bool CanClose()
        {

            bool a;
            if (QueryClose(out a) == VSConstants.S_OK)
            {
                return a;
            }

            return false;
        }
    }
}
