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
    /// <summary>
    /// http://www.mztools.com/articles/2013/MZ2013029.aspx
    /// </summary>
    internal class DteInitializer : IVsShellPropertyEvents
    {
        private readonly IVsShell _shellService;
        private uint _cookie;
        private readonly Action _callback;

        internal DteInitializer(IVsShell shellService, Action callback)
        {
            int hr;

            _shellService = shellService;
            _callback = callback;

            hr = _shellService.AdviseShellPropertyChanges(this, out _cookie);

            ErrorHandler.ThrowOnFailure(hr);
        }

        int IVsShellPropertyEvents.OnShellPropertyChange(int propid, object var)
        {
            if (propid == (int)__VSSPROPID.VSSPROPID_Zombie)
            {
                var isZombie = (bool)var;

                if (!isZombie)
                {
                    var hr = _shellService.UnadviseShellPropertyChanges(_cookie);

                    ErrorHandler.ThrowOnFailure(hr);

                    _cookie = 0;

                    _callback();
                }
            }

            return VSConstants.S_OK;
        }
    }
}
