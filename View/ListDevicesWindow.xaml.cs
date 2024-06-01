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
    /// Логика взаимодействия для ListDevicesWindow.xaml
    /// </summary>
    public partial class ListDevicesWindow : AdonisWindow
    {
        private Controller _controller {get; set;}
        public ListDevicesWindow(Controller controller)
        {
            InitializeComponent();

            _controller = controller;
            DataContext = _controller;
        }

        private void Add_Click(object sender, RoutedEventArgs e) => new ConfigureModbusDeviceWindow(_controller).ShowDialog();

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.SelectDevice == null)
                return;

            new ConfigureModbusDeviceWindow(_controller, _controller.SelectDevice).ShowDialog();
            _controller.SelectDevice.OnPropertyChanged("SpecificName"); // Обновляем имя для редактируемого устройства
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.SelectDevice == null)
                return;

            var result = AdonisUI.Controls.MessageBox.Show(
                $"Вы собираетесь удалить устройство \"{_controller.SelectDevice.SpecificName}\". Вы уверены?",
                "Подтверждение",
                AdonisUI.Controls.MessageBoxButton.YesNoCancel,
                AdonisUI.Controls.MessageBoxImage.Question
                );
            if (result == AdonisUI.Controls.MessageBoxResult.Yes)
                _controller.Devices.Remove(_controller.SelectDevice);
        }
    }
}
