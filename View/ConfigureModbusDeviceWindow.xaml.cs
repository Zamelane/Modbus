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
using Modbus.Model;

namespace Modbus.View
{
    /// <summary>
    /// Логика взаимодействия для ConfigureModbusDeviceWindow.xaml
    /// </summary>
    public partial class ConfigureModbusDeviceWindow : AdonisWindow
    {
        public Controller _controller { get; set; }
        public DeviceModel Device { get; set; }
        private bool isEdit;
        public ConfigureModbusDeviceWindow(Controller controller, DeviceModel device = null)
        {
            InitializeComponent();

            if (device == null)
                Title = "Добавление modbus-устройства";

            Device = device == null ? new DeviceModel() : device;
            isEdit = device != null;
            _controller = controller;

            if (isEdit)
                Save.Visibility = Visibility.Collapsed;

            if (Device.SerialPortModel == null && _controller.Ports.Count > 0)
                Device.SerialPortModel = _controller.Ports.First();

            DataContext = this;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем уникальность Salve ID
            if (_controller.CheckDeviceAvailability(Device))
            {
                AdonisUI.Controls.MessageBox.Show($"Устройство с Salve ID ({Device.Id}) уже существует.", 
                    "Предупреждение", 
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Warning);
                return;
            }
            // Если было создание, то добавляем
            if (!isEdit)
                _controller.Devices.Add(Device);
            // Иначе просто выходим, т.к. редактирование было по переданной ссылке
            Close();
        }
    }
}
