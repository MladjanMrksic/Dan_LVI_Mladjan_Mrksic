using HTML_ZIPApp.Command;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using HTML_ZIPApp.Validation;
using System.IO.Compression;

namespace HTML_ZIPApp.ViewModel
{
    class MainWindowViewModel
    {
        MainWindowView view;
        StreamWriter sw;
        public MainWindowViewModel(MainWindowView mwv)
        {
            view = mwv;
        }

        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }
        private void CloseExecute()
        {
            try
            {
                view.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message.ToString());
            }
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        private ICommand getHTML;
        public ICommand GetHTML
        {
            get
            {
                if (getHTML == null)
                {
                    getHTML = new RelayCommand(param => GetHTMLExecute(), param => CanGetHTMLExecute());
                }
                return getHTML;
            }
        }
        private void GetHTMLExecute()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string site = view.txtSite.Text;
                    string fileName = string.Format(DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute);
                    string filePath = string.Format(".../.../HTMLAddresses/" + fileName + ".html");
                    File.Create(filePath).Close();
                    client.DownloadFile(site, filePath);
                    MessageBox.Show("URL successfully downloaded.\nCheck HTMLAddresses folder","Success!",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message.ToString());
            }
        }
        private bool CanGetHTMLExecute()
        {
           return URLValidation.ValidateURl(view.txtSite.Text);
        }

        private ICommand zipHTML;
        public ICommand ZipHTML
        {
            get
            {
                if (zipHTML == null)
                {
                    zipHTML = new RelayCommand(param => ZipHTMLExecute(), param => CanZipHTMLExecute());
                }
                return zipHTML;
            }
        }
        private void ZipHTMLExecute()
        {
            try
            {
                DirectoryInfo directorySelected = new DirectoryInfo(".../.../HTMLAddresses/");
                Compress(directorySelected);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex.Message.ToString());
            }
        }
        private bool CanZipHTMLExecute()
        {
            return true;
        }

        public static void Compress(DirectoryInfo directorySelected)
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                        FileInfo info = new FileInfo(".../.../HTMLAddresses/" + Path.DirectorySeparatorChar + fileToCompress.Name + ".gz");
                        MessageBox.Show($"Compressed {fileToCompress.Name} from {fileToCompress.Length.ToString()} to {info.Length.ToString()} bytes.");
                    }
                }
            }
        }
    }
}
