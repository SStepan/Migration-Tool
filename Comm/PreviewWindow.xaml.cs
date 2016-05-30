using System.Windows;

namespace Comm {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class PreviewWindow : Window {
		public PreviewWindow(string content, bool isReadonly, bool isEdit = true) {
			InitializeComponent();
			Block.Text = content;
			this.Block.IsReadOnly = isReadonly;
			if (!isEdit)
				this.Cancel.Visibility = Visibility.Hidden;
			else
			{
				this.Cancel.Visibility = Visibility.Visible;
			}
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = true;
		}

		public string Answer {
			get { return Block.Text; }
		}
	}
}
