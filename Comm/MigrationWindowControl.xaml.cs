//------------------------------------------------------------------------------
// <copyright file="MigrationWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;

namespace Comm
{
	using System.Diagnostics.CodeAnalysis;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for MigrationWindowControl.
	/// </summary>
	public partial class MigrationWindowControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MigrationWindowControl"/> class.
		/// </summary>
		public MigrationWindowControl() {
			this.InitializeComponent();
		}

		private IRepository repository;
		private ChangesList changesList = new ChangesList();

		public void Init(IRepository repository, string databasePath, string RepositoryPath) {
			this.TextBoxDatabaseProjectPath.Text = databasePath;
			TextBoxDatabaseProjectPath.Background = new SolidColorBrush(Colors.White);

			string migrationPath = databasePath + Translation.DefaultFolders.MigrationFolder;
			if (Directory.Exists(migrationPath))
			{
				this.TextBoxMigrationPath.Text = migrationPath;
				TextBoxMigrationPath.Background = new SolidColorBrush(Colors.White);
				this.TextBoxMigrationName.Text = FileManager.GetMigrationName(migrationPath);
			}
			if (repository == null)
			{
				this.TextBoxRepositoryPath.Text = String.Empty;
				this.TextBoxRepositoryPath.Foreground = new SolidColorBrush(Colors.Red);
			}
			else
			{
				this.repository = repository;
				this.TextBoxRepositoryPath.Text = RepositoryPath;
				TextBoxRepositoryPath.Background = new SolidColorBrush(Colors.White);
			}
		}



		/// <summary>
		/// Handles click on the button by displaying a message box.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event args.</param>
		[SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
			Justification = "Default event handler naming pattern")]
		private void button1_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show(
				string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
				"MigrationWindow");
		}

		private void buttonGenerate_Click(object sender, RoutedEventArgs e) {
			bool IsFilled = true;
			if (TextBoxRepositoryPath.Text == String.Empty)
			{
				TextBoxRepositoryPath.Background = new SolidColorBrush(Colors.Red);
				IsFilled = false;
			}
			if (TextBoxMigrationPath.Text == String.Empty)
			{
				TextBoxMigrationPath.Background = new SolidColorBrush(Colors.Red);
				IsFilled = false;
			}
			if (TextBoxDatabaseProjectPath.Text == String.Empty)
			{
				TextBoxDatabaseProjectPath.Background = new SolidColorBrush(Colors.Red);
				IsFilled = false;
			}
			if (!IsFilled)
			{
				System.Windows.Forms.MessageBox.Show("Please fill in all required fields");
				return;
			}

			repository = RepositoryFactory.GetRepository(TextBoxRepositoryPath.Text);
			if (repository == null)
			{
				System.Windows.Forms.MessageBox.Show(
					"You choose folder which isn't under swn or git control version, please choose other folder");
				TextBoxRepositoryPath.Foreground = new SolidColorBrush(Colors.Red);
				return;
			}

			string shortDatabaseFolderName = new DirectoryInfo(TextBoxDatabaseProjectPath.Text).Name;
			changesList.Changes = repository.GetChanges(TextBoxRepositoryPath.Text, shortDatabaseFolderName);
			this.TreeView.ItemsSource = changesList.Changes;

			buttonPreview.IsEnabled = true;
			buttonSave.IsEnabled = true;
		}

		private void buttonBrowseProjectPath_Click(object sender, RoutedEventArgs e) {
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			folderDialog.SelectedPath = "C:\\";

			var result = folderDialog.ShowDialog();
			string databaseProjectPath = String.Empty;
			if (result.ToString() == "OK")
				databaseProjectPath = folderDialog.SelectedPath;
			//this.TextBoxDatabaseProjectPath.Text = folderDialog.SelectedPath;
			bool isDatabaseProject = false;
			if (databaseProjectPath != String.Empty)
				isDatabaseProject = FileManager.IsDatabaseProjectFolder(databaseProjectPath);
			if (isDatabaseProject)
			{
				TextBoxDatabaseProjectPath.Text = databaseProjectPath;
				TextBoxDatabaseProjectPath.Background = new SolidColorBrush(Colors.White);

				string repositoryPath = Path.GetDirectoryName(databaseProjectPath);

				repository = RepositoryFactory.GetRepository(repositoryPath);
				if (repository != null)
					TextBoxRepositoryPath.Text = repositoryPath;
				else
				{
					TextBoxRepositoryPath.Text = String.Empty;
				}
			}
			else
			{
				TextBoxDatabaseProjectPath.Text = String.Empty;
				TextBoxDatabaseProjectPath.Background = new SolidColorBrush(Colors.Red);
				MessageBox.Show("This isn't database project folder");
			}
		}

		private void buttonBrowseMigrationPath_Click(object sender, RoutedEventArgs e) {
			string databaseProjectPath = TextBoxDatabaseProjectPath.Text;
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			if (databaseProjectPath != String.Empty)
				folderDialog.SelectedPath = databaseProjectPath + Translation.DefaultFolders.MigrationFolder;

			var result = folderDialog.ShowDialog();
			string migrationFolderPath = String.Empty;
			if (result.ToString() == "OK")
			{
				migrationFolderPath = folderDialog.SelectedPath;

				TextBoxMigrationPath.Text = migrationFolderPath;
				TextBoxMigrationPath.Background = new SolidColorBrush(Colors.White);

				string migrationName = FileManager.GetMigrationName(migrationFolderPath);
				TextBoxMigrationName.Text = migrationName;
			}
		}

		private void buttonBrowseRepositoryPath_Click(object sender, RoutedEventArgs e) {
			string databasePath = TextBoxDatabaseProjectPath.Text;
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			folderDialog.SelectedPath = databasePath;

			var result = folderDialog.ShowDialog();
			string repositoryPath = "";
			if (result.ToString() == "OK")
				repositoryPath = folderDialog.SelectedPath;
			if (databasePath != String.Empty)
			{
				bool isSubfolder = FileManager.IsSubfolder(repositoryPath, databasePath);
				if (isSubfolder)
				{
					TextBoxRepositoryPath.Text = repositoryPath;
					TextBoxRepositoryPath.Background = new SolidColorBrush(Colors.White);
				}
				else
				{
					TextBoxRepositoryPath.Background = new SolidColorBrush(Colors.Red);
					TextBoxRepositoryPath.Text = String.Empty;
					System.Windows.Forms.MessageBox.Show("You choose Repostiory Path which isn't refers to Database Path");

				}
			}
			else
			{
				if (repositoryPath != String.Empty)
				{
					repository = RepositoryFactory.GetRepository(repositoryPath);
					if (repository == null)
					{
						MessageBox.Show("You chose folder which isn't under swn or git version control");
						TextBoxRepositoryPath.Background = new SolidColorBrush(Colors.Red);
						TextBoxRepositoryPath.Text = String.Empty;
					}
					else
					{
						TextBoxRepositoryPath.Text = repositoryPath;
						TextBoxRepositoryPath.Background = new SolidColorBrush(Colors.White);
					}
				}
			}
		}

		private void OnChecked(object sender, RoutedEventArgs e) {
			CheckBox check = sender as CheckBox;
			if (check != null)
			{
				if (check.IsChecked.Value == true)
				{
					Change row = (Change) check.DataContext;

					if (row.Inserts != null)
						foreach (var VARIABLE in row.Inserts.AddedLines)
						{
							VARIABLE.Value.Include = true;
						}

				}
			}
		}

		private void Unchecked(object sender, RoutedEventArgs e) {
			CheckBox check = sender as CheckBox;
			if (check != null)
			{
				if (check.IsChecked.Value == false)
				{
					Change row = (Change) check.DataContext;

					if (row.Inserts != null)
						foreach (var VARIABLE in row.Inserts.AddedLines)
						{
							VARIABLE.Value.Include = false;
						}

				}
			}
		}

		private void Preview(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			if (button != null)
			{
				KeyValuePair<string, IncludeContent> keyValuePair = (KeyValuePair<string, IncludeContent>) button.DataContext;
				string content = keyValuePair.Value.GetLines();
				PreviewWindow user = new PreviewWindow(content, true);
				string newContent = "";
				if (user.ShowDialog() == true)
					newContent = user.Answer;
				if (newContent != String.Empty)
				{
					keyValuePair.Value.Lines = repository.ParseInserts(newContent);
				}
			}
		}

		private void PreviewChange(object sender, RoutedEventArgs e) {
			Button button = sender as Button;
			if (button != null)
			{
				Change change = (Change) button.DataContext;
				string content = String.Empty;
				if (change.Inserts != null)
				{
					content = change.Inserts.GetContent();
				}
				else
				{
					content = change.Content;
				}

				PreviewWindow user = new PreviewWindow(content, false);

				ElementHost.EnableModelessKeyboardInterop(user);

				string newContent = String.Empty;
				if (user.ShowDialog() == true)
				{
					newContent = user.Answer;
					if (newContent == String.Empty || newContent == "\r\n\r\n")
					{
						changesList.Changes.Remove(change);
					}
					else if (repository.IsPostDiployment(change.FilePath))
					{
						change.Inserts = repository.GetInserts(newContent);
					}
					else
					{
						change.Content = newContent;
					}
				}
			}
		}

		private void buttonPreview_Click(object sender, RoutedEventArgs e) {
			string preview = GetContent();
			PreviewWindow previewWindow = new PreviewWindow(preview, true, false);
			previewWindow.ShowDialog();
		}

		private string GetContent() {
			string preview = String.Empty;
			foreach (var change in changesList.Changes)
			{
				if (change.Include)
				{
					if (change.Inserts != null && (change == changesList.Changes.First()))
					{
						preview += change.Inserts.GetContent();

						preview += "GO";
						preview += System.Environment.NewLine;
						preview += System.Environment.NewLine;
					}
					else if (change.Inserts != null)
					{
						preview += change.Inserts.GetContent();
						preview += System.Environment.NewLine;
						preview += System.Environment.NewLine;
						preview += "GO";
						preview += System.Environment.NewLine;
						preview += System.Environment.NewLine;

					}
					else if (change.Inserts == null && (change == changesList.Changes.First()))
					{
						preview += change.Content;

						preview += "GO";
						preview += System.Environment.NewLine;
						preview += System.Environment.NewLine;
					}
					else
					{
						preview += change.Content;
						preview += System.Environment.NewLine;
						preview += System.Environment.NewLine;
						preview += "GO";
						preview += System.Environment.NewLine;
						preview += System.Environment.NewLine;
					}
				}
			}
			return preview;
		}

		private void buttonSave_Click(object sender, RoutedEventArgs e) {
			string path = TextBoxMigrationPath.Text + "\\" + TextBoxMigrationName.Text;
			string preview = GetContent();
			using (StreamWriter sw = new StreamWriter(path))
			{
				sw.WriteLine(preview);
				sw.Close();
			}
		}
	}
}

