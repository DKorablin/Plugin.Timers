using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("69c79417-c168-434a-a597-4e224237a527")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://dkorablin.ru/project/Default.aspx?File=119")]
#else

[assembly: AssemblyTitle("Plugin.Timers")]
[assembly: AssemblyDescription("Timers scheduler service plugin")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.Timers")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2018-2024")]
#endif