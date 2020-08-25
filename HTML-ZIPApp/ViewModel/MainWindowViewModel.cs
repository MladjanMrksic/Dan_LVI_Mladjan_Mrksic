using HTML_ZIPApp.Command;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using HTML_ZIPApp.Validation;
using Aspose.Zip;

namespace HTML_ZIPApp.ViewModel
{
    class MainWindowViewModel
    {
        MainWindowView view;
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
                    MessageBox.Show("URL successfully downloaded.\nCheck HTMLAddresses folder", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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
                using (FileStream zipFile = File.Open(".../.../HTMLAddresses.zip",FileMode.Create))
                {
                    using (Archive archive = new Archive())
                    {
                        DirectoryInfo folder = new DirectoryInfo(".../.../HTMLAddresses");
                        archive.CreateEntries(folder);
                        archive.Save(zipFile);
                    }
                }
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
    }
}

