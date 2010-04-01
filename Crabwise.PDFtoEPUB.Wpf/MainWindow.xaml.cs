using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace Crabwise.PDFtoEPUB.Wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICommand addButtonCommand;
        private readonly ICommand browseButtonCommand;
        private readonly ICommand removeButtonCommand;
        private readonly ObservableCollection<object> selectedItems = new ObservableCollection<object>();

        public ICommand AddButtonCommand
        {
            get
            {
                return addButtonCommand;
            }
        }

        public ICommand BrowseButtonCommand
        {
            get
            {
                return browseButtonCommand;
            }
        }

        public ICommand RemoveButtonCommand
        {
            get
            {
                return removeButtonCommand;
            }
        }

        public MainWindow()
        {
            InitializeComponent(); this.DataContext = this;

            this.addButtonCommand = new DelegatingCommand(this.CanExecuteAdd, this.ExecuteAdd);
            this.browseButtonCommand = new DelegatingCommand(this.CanExecuteBrowse, this.ExecuteBrowse);
            this.removeButtonCommand = new DelegatingCommand(this.CanExecuteRemove, this.ExecuteRemove);
        }

        private bool CanExecuteAdd(object parameter)
        {
            return true;
        }

        private bool CanExecuteBrowse(object parameter)
        {
            return true;
        }

        private bool CanExecuteRemove(object parameter)
        {
            return true;
        }

        private void ExecuteAdd(object parameter)
        {
            this.ShowFileDialog("Add PDFs", true);
        }

        private void ExecuteBrowse(object parameter)
        {
            this.ShowFileDialog("Output Folder", true);
        }

        private void ExecuteRemove(object parameter)
        {
        }

        private void ShowFileDialog(string caption, bool folderPicker)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckPathExists = true,
                Title = caption
            };

            openFileDialog.ShowDialog();
        }
    }
}
