using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Extensibility.VisualStudio.Restart.Arguments;

namespace Extensibility.VisualStudio.Restart
{
    public static class StringExtension
    {
        public static string ReplaceSmart(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                return value;
            }

            return value.Replace(oldValue, newValue);
        }
    }
}