using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using NuGet.Common;
using Console = NuGet.Common.Console;
using FubuCore;

namespace NuGetSample {
	public class Program {
		public static void Main(params string[] args) {
			var packageId = args.Length > 0 ? args[0] : "NUnit";
			var installer = new PackageInstaller(
				PackageRepositoryFactory.Default.CreateRepository(NuGetConstants.DefaultFeedUrl), new Console());
			var projectPath =
				Assembly.GetExecutingAssembly().Location.ParentDirectory().ParentDirectory().ParentDirectory().AppendPath("NuGetSample.csproj");
			System.Console.WriteLine(projectPath);
			var packagePath = projectPath.ParentDirectory().AppendPath("testPackages");
			installer.InstallPackage(packageId, projectPath, packagePath);
			System.Console.ReadLine();
		}
	}
}
