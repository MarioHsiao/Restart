// ------------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by Extensibility Tools v1.10.184
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Extensibility.VisualStudio.Restart
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidRestartPackageString = "11fc685d-f80b-4265-bcd3-2ddbded2ea33";
        public const string RestartCommandsGroupString = "6ba54fee-8b16-423c-8d07-8e771f7fcf12";
        public const string TopLevelMenuGroupString = "aac71a77-2eb0-454c-a211-f6e70035c7ef";
        public static Guid guidRestartPackage = new Guid(guidRestartPackageString);
        public static Guid RestartCommandsGroup = new Guid(RestartCommandsGroupString);
        public static Guid TopLevelMenuGroup = new Guid(TopLevelMenuGroupString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int RestartGroup = 0x1020;
        public const int RestartAsAdministrator = 0x0100;
        public const int Restart = 0x0101;
        public const int MenuGroup = 0x0100;
        public const int TopLevelMenu = 0x0201;
    }
}
