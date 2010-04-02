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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace Crabwise.PDFtoEPUB.Wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public string OutputDirectory
        {
            get { return (string)GetValue(OutputDirectoryProperty); }
            set { SetValue(OutputDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutputDirectory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutputDirectoryProperty =
            DependencyProperty.Register("OutputDirectory", typeof(string), typeof(MainWindow), new UIPropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));



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
            const string title = "Add PDFs";
            const string defaultExt = "pdf";
            const bool ensureFileExists = true;
            const bool multiselect = true;

            if (CommonOpenFileDialog.IsPlatformSupported)
            {
                var openDialog = new CommonOpenFileDialog
                {
                    EnsureFileExists = ensureFileExists,
                    Multiselect = multiselect,
                    Title = title
                };

                openDialog.Filters.Add(new CommonFileDialogFilter("PDF files", "pdf"));
                openDialog.ShowDialog(this);
            }
            else
            {
                var openDialog = new OpenFileDialog
                {
                    CheckFileExists = ensureFileExists,
                    DefaultExt = defaultExt,
                    Multiselect = multiselect,
                    Title = title
                };

                openDialog.Filter = "PDF files|*.pdf";
                openDialog.ShowDialog();
            }
        }

        private void ExecuteBrowse(object parameter)
        {
            const string title = "Output Folder";
            const bool ensurePathExists = true;

            if (CommonOpenFileDialog.IsPlatformSupported)
            {
                var openDialog = new CommonOpenFileDialog
                {
                    EnsurePathExists = ensurePathExists,
                    InitialDirectory = this.OutputDirectory,
                    IsFolderPicker = true,
                    Title = title
                };

                var result = openDialog.ShowDialog(this);
                if (result == CommonFileDialogResult.OK)
                {
                    this.OutputDirectory = openDialog.FileName;
                }
            }
            else
            {
                var openDialog = new FolderBrowserDialog
                {
                    Description = title,
                    SelectedPath = this.OutputDirectory
                };

                var result = openDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.OutputDirectory = openDialog.SelectedPath;
                }
            }
        }

        private void ExecuteRemove(object parameter)
        {
        }

        private void ShowFileDialog(string caption, bool folderPicker)
        {

        }
    }
}
