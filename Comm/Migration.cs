//------------------------------------------------------------------------------
// <copyright file="Migration.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Comm {
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class Migration {
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 0x0100;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("dce29866-ba33-4aff-9e14-f1ec458eeb82");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package package;

		/// <summary>
		/// Initializes a new instance of the <see cref="Migration"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private Migration(Package package) {
			if (package == null) {
				throw new ArgumentNullException("package");
			}

			this.package = package;

			OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null) {
				var menuCommandID = new CommandID(CommandSet, CommandId);
				var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static Migration Instance {
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider {
			get {
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package) {
			Instance = new Migration(package);
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void MenuItemCallback(object sender, EventArgs e) {
			ToolWindowPane window = this.package.FindToolWindow(typeof(MigrationWindow), 0, true);
			if ((null == window) || (null == window.Frame)) {
				throw new NotSupportedException("Cannot create tool window");
			}

			IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
			
			var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
			var selectedFiles = (Array)dte.ToolWindows.SolutionExplorer.SelectedItems;

			if (null != selectedFiles) {
				foreach (UIHierarchyItem selectedItem in selectedFiles) {
					Project projectItem = selectedItem.Object as Project;
					Regex isDatabaseProj = new Regex(@".sqlproj");
					string fullNameItem = projectItem.FullName;
					if (!isDatabaseProj.IsMatch(fullNameItem))
						MessageBox.Show("This isn't database project");
					else
					{
						//string nameRepository = "deezze";
						
						//int indexRepositoryName = prjItem.FullName.IndexOf(nameRepository);


						string databasePath = Path.GetDirectoryName(fullNameItem);

						string repositoryPath = Path.GetDirectoryName(databasePath);

						IRepository repository = RepositoryFactory.GetRepository(repositoryPath);

						
						((MigrationWindowControl)window.Content).Init(repository, databasePath, repositoryPath);
						
						Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
					}
					break;
				}
			}

		}
	}
}
