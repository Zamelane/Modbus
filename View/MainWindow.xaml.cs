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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdonisUI.Controls;
using Modbus.View;

namespace Modbus
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        private Controller _controller { get; set; } = new Controller();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _controller;
        }

        private void ConfigureComPortsItem_Click(object sender, RoutedEventArgs e) => new ConfigureSerialPortsWindow(_controller).ShowDialog();

        private void ListDevicesItem_Click(object sender, RoutedEventArgs e) => new ListDevicesWindow(_controller).ShowDialog();

        private void OneSendButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.SelectDevice == null)
                AdonisUI.Controls.MessageBox.Show("Не выбрано устройство для отправки запроса!", "Проверьте данные", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
            else
                _controller.SelectFunc.Send(_controller.SelectDevice);
        }

        private void AddMassValue_Click(object sender, RoutedEventArgs e)
        {
            var func = _controller.SelectFunc;
            if (func.IsMultipleUInt16Value)
                func.AddMultipleUInt16Value();
            else func.AddMultipleBooleanValue();
        }

        private void RemoveMassValue_Click(object sender, RoutedEventArgs e)
        {
            var func = _controller.SelectFunc;
            bool isNotSelected = false;
            if (func.IsMultipleUInt16Value)
            {
                if (MultipleUInt16Values.SelectedIndex == -1)
                    isNotSelected = true;
                else func.RemoveMultipleUInt16Value(MultipleUInt16Values.SelectedIndex);
            }
            else
            {
                if (MultipleBooleanValues.SelectedIndex == -1)
                    isNotSelected = true;
                else func.RemoveMultipleBooleanValue(MultipleBooleanValues.SelectedIndex);
            }

            if (isNotSelected)
                AdonisUI.Controls.MessageBox.Show("Не выбран ни один элемент для удаления!", "Обратите внимание", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
        }

        private void MultipleValues_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column is DataGridBoundColumn column)
                {
                    var el = e.EditingElement as TextBox;
                    var header = column.Header.ToString();
                    var rowIndex = e.Row.GetIndex();

                    UInt16 result;
                    if (header == "UInt16" && UInt16.TryParse(el.Text, out result))
                    {
                        _controller.SelectFunc.EditMultipleUInt16Value(rowIndex, result);
                    }
                }
            }
        }

        private void MultipleBooleanValue_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox.Tag == null)
                return;

            var index = Convert.ToInt32(checkBox.Tag) - _controller.SelectFunc.CoilAddress;

            _controller.SelectFunc.EditMultipleBooleanValue(index, checkBox.IsChecked == true);
        }

        private void ReloadComPorts_Click(object sender, RoutedEventArgs e)
        {
            _controller.LoadPorts();
            AdonisUI.Controls.MessageBox.Show("Список COM-портов обновлён", "Успешно", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
        }
    }
}
