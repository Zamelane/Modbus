using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AdonisUI.Controls;

namespace Modbus.View
{
    /// <summary>
    /// Логика взаимодействия для ConfigureSerialPorts.xaml
    /// </summary>
    public partial class ConfigureSerialPortsWindow : AdonisWindow
    {
        private Controller _controller { get; set; }
        public ConfigureSerialPortsWindow(Controller controller)
        {
            InitializeComponent();

            _controller = controller;
            DataContext = _controller;

            _controller.SelectPort = _controller.Ports.FirstOrDefault();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
