using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortScanner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool ArePortsValid = false;

        private bool IsIpValid = false;

        private Dictionary<int, PortScanningService.Status> dictionary = null;

        private IPAddress IPAddress = null;

        private int LowerThreshold;

        private int UpperThreshold;



        public MainWindow()
        {
            InitializeComponent();
            ScanButton.IsEnabled = false;
            /*PortScanningService.Scan(IPAddress.Parse("127.0.0.1"), 0, 100);*/
        }

        private void IpAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!ValidationService.IsIpStringValid(IpAddressTextBox.Text))
            {
                IpAddressWarningLabel.Visibility = Visibility.Visible;
                IsIpValid = false;
            }
            else
            {
                IpAddressWarningLabel.Visibility = Visibility.Hidden;
                IsIpValid = true;
            }
            ToggleButton();
        }

        private void FromPortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PortsValidation();
            ToggleButton();
        }

        private void ToPortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PortsValidation();
            ToggleButton();
        }

        private void ToggleButton()
        {
            ScanButton.IsEnabled = IsIpValid && ArePortsValid;
        }

        

        private void PortsValidation()
        {
            if(!ValidationService.IsPortValid(FromPortTextBox.Text)
                || !ValidationService.IsPortValid(ToPortTextBox.Text)
                || int.Parse(FromPortTextBox.Text) > int.Parse(ToPortTextBox.Text)
                )
            {
                PortsWarningLabel.Visibility = Visibility.Visible;
                ScanButton.IsEnabled = false;
                ArePortsValid = false;
            }
            else
            {
                PortsWarningLabel.Visibility = Visibility.Hidden;
                ArePortsValid = true;
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            PortTable.ItemsSource = null;
            IPAddress = IPAddress.Parse(IpAddressTextBox.Text);

            UpperThreshold = int.Parse(ToPortTextBox.Text);

            LowerThreshold = int.Parse(FromPortTextBox.Text);

            ScanButton.IsEnabled = false;
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
            
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<PortPresentation> presentations = new List<PortPresentation>();
            foreach(KeyValuePair<int, PortScanningService.Status> pair in dictionary)
            {
                presentations.Add(
                    new PortPresentation
                    {
                        Port = pair.Key,
                        Closed = (pair.Value == PortScanningService.Status.Closed) ? ("Закрыт") : (""),
                        Opened = (pair.Value == PortScanningService.Status.Opened) ? ("Открыт") : ("")

                    }

                    );
            }
            PortTable.ItemsSource = presentations;
            ScanButton.IsEnabled = true;

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            
             dictionary = PortScanningService.Scan(
                IPAddress,
                LowerThreshold,
                UpperThreshold
                
                );


        }
    }
}
