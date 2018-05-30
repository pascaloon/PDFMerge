using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PDFMerge.Model;
using PDFMerge.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PDFMerge.ViewModel
{
    public class PDFMergerViewModel : ViewModelBase
    {
        public ObservableCollection<PDFFile> Files { get; set; } = new ObservableCollection<PDFFile>();
        public RelayCommand<PDFFile> SetPDFPath { get; set; }
        public RelayCommand<PDFFile> RemovePDFFileCommand { get; set; }
        public RelayCommand AddNewEmptyFileCommand { get; set; }
        public RelayCommand MergeFilesCommand { get; set; }
        public RelayCommand AddManyFilesCommand { get; set; }

        private string _outputPath = @"C:\Output.pdf";
        public string OutputPath { get { return _outputPath; } set { Set<string>(() => this.OutputPath, ref _outputPath, value); } } 
        public RelayCommand SetOutputPathCommand { get; set; }


        public PDFMergerViewModel()
        {
            if (IsInDesignMode)
            {
                Files.Add(new PDFFile { Path = @"C:\Test1.pdf" });
                Files.Add(new PDFFile { Path = @"C:\Test2.pdf" });
                Files.Add(new PDFFile { Path = @"C:\Test3.pdf" });
            }
            else
            {

            }

            SetPDFPath = new RelayCommand<PDFFile>(UpdateFilePath);

            RemovePDFFileCommand = new RelayCommand<PDFFile>(RemoveFile, CanRemoveFile);

            AddNewEmptyFileCommand = new RelayCommand(AddNewEmptyFile);

            MergeFilesCommand = new RelayCommand(MergeFiles, CanMergeFiles);

            SetOutputPathCommand = new RelayCommand(SetOutputPath);

            AddManyFilesCommand = new RelayCommand(AddManyFiles);
        }


        private void UpdateFilePath(PDFFile file)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF|*.pdf";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = false;
            var r = ofd.ShowDialog();
            if (r.HasValue && r.Value && ofd.CheckFileExists)
            {
                file.Path = ofd.FileName;
            }
            MergeFilesCommand.RaiseCanExecuteChanged();
        }

        private void RemoveFile(PDFFile file)
        {
            Files.Remove(file);
            RaisePropertyChanged(() => this.Files);
            RemovePDFFileCommand.RaiseCanExecuteChanged();
            MergeFilesCommand.RaiseCanExecuteChanged();
        }

        private bool CanRemoveFile(PDFFile file)
        {
            return Files.Count > 2;
        }

        private void AddNewEmptyFile()
        {
            Files.Add(new PDFFile());
            RaisePropertyChanged(() => this.Files);
            RemovePDFFileCommand.RaiseCanExecuteChanged();
            MergeFilesCommand.RaiseCanExecuteChanged();
        }

        private void MergeFiles()
        {
            PDFMerger merger = new PDFMerger();
            var r = merger.MergePDFs(Files.ToList(), OutputPath);
            if (r.Success)
            {
                MessageBox.Show("All files have been merged.", "PDFs Merged");
            }
            else
            {
                MessageBox.Show("Error while merging files: \n" + r.ErrorMessage, "Error");
            }
        }

        private bool CanMergeFiles()
        {
            return Files.Count >= 2 && !String.IsNullOrWhiteSpace(OutputPath) && Files.All(f => !String.IsNullOrWhiteSpace(f.Path));
        }

        private void SetOutputPath()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "pdf";
            sfd.Filter = "PDF|*.pdf";
            sfd.FileName = OutputPath;
            var r = sfd.ShowDialog();
            if (r.HasValue && r.Value)
            {
                OutputPath = sfd.FileName;
            }

            MergeFilesCommand.RaiseCanExecuteChanged();

        }

        private void AddManyFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF|*.pdf";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = true;
            var r = ofd.ShowDialog();
            if (r.HasValue && r.Value && ofd.CheckFileExists)
            {
                foreach (var f in ofd.FileNames)
                {
                    Files.Add(new PDFFile { Path = f });
                }
            }
            RemovePDFFileCommand.RaiseCanExecuteChanged();
            MergeFilesCommand.RaiseCanExecuteChanged();
        }
    }
}
