using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using NuGet;
using NuGet.Common;
using System.Collections.Generic;
using FubuCore;
using IFileSystem = NuGet.IFileSystem;

namespace NuGetSample {
	public class PackageInstaller {
		private const string PackagesFolder = "packages";
		private readonly IPackageRepository _packageRepository;
		private readonly IConsole _console;

		// I register IPackageRepository in my container as follows:
		// For<IPackageRepository>().Singleton().Use(() => PackageRepositoryFactory.Default.CreateRepository(NuGetConstants.DefaultFeedUrl));
		// As for IConsole, use NuGet.Common.Console, or your own implementation
		public PackageInstaller(IPackageRepository packageRepository, IConsole console) {
			_packageRepository = packageRepository;
			_console = console;
		}


		public void InstallPackage(string packageId, string projectPath, string targetFolder) {
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var projectSystem = new BetterThanMSBuildProjectSystem(projectPath) { Logger = _console };
			var localRepository = new BetterThanLocalPackageRepository(packagePathResolver, packagesFolderFileSystem, projectSystem);
			var projectManager = new ProjectManager(_packageRepository, packagePathResolver, projectSystem,
													localRepository) {Logger = _console};

			projectManager.PackageReferenceAdded += (sender, args) => args.Package.GetLibFiles()
			                                                              .Each(file => SaveAssemblyFile(args.InstallPath, file));
			projectManager.AddPackageReference(packageId);
			projectSystem.Save();
		}

		private void SaveAssemblyFile(string installPath, IPackageFile file) {
			var targetPath = installPath.AppendPath(file.Path);
			Directory.CreateDirectory(targetPath.ParentDirectory());
			using (Stream outputStream = File.Create(targetPath)){
                file.GetStream().CopyTo(outputStream);
            }

		}

		public IEnumerable<IPackage> GetAllPackages(string rootFolder) { //rootFolder is repository root
			var targetFolder = rootFolder.AppendPath(PackagesFolder);
			var packagePathResolver = new DefaultPackagePathResolver(targetFolder);
			var packagesFolderFileSystem = new PhysicalFileSystem(targetFolder);
			var localRepository = new LocalPackageRepository(packagePathResolver, packagesFolderFileSystem);
			return localRepository.GetPackages();
		}

		public void ClearPackages(string repositoryPath) {
			Directory.Delete(repositoryPath.AppendPath(PackagesFolder), true);
		}

	}
	public class BetterThanMSBuildProjectSystem : MSBuildProjectSystem {

		public override void AddFile(string path, System.IO.Stream stream) {
			base.AddFile(path, stream);
			var rootElement = ProjectRootElement.Open(ProjectPath);
			rootElement.AddItem("Content", path);
		}

		public string ProjectPath { get; private set; }

		public BetterThanMSBuildProjectSystem(string projectFile)
			: base(projectFile) {
			ProjectPath = projectFile;
		}
	}

	// The problem with the default LocalPackageRepository class is that it returns true for its Exists method whenever a package physically exists in a folder
	// So, if a package exists on disk, but isn't referenced by the project, ProjectManager skips it and doesn't add references to the project 
	// Hence I subclassed LocalPackageRepository.
	// Using this class makes sure that we skip a package only if it exists on disk, and all its assemblies are referenced by our project (this doesn't cover content-only packages, like jQuery).
	public class BetterThanLocalPackageRepository: LocalPackageRepository {
		private readonly MSBuildProjectSystem _projectSystem;

		public BetterThanLocalPackageRepository(IPackagePathResolver pathResolver, IFileSystem fileSystem, MSBuildProjectSystem projectSystem) : base(pathResolver, fileSystem) {
			_projectSystem = projectSystem;
		}


		public override bool Exists(string packageId, SemanticVersion version) {
			//if no package file exists, return false
			if (!base.Exists(packageId, version))
				return false;
			//find the package and check whether all its assemblies are referenced
			var package = this.FindPackage(packageId, version);
			return package.AssemblyReferences.All(reference => _projectSystem.ReferenceExists(reference.Name)); 
		}
	}

}