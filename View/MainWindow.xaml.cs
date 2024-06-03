using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
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
        private Task _task;
        private CancellationTokenSource _taskController;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _controller;
        }

        private void ConfigureComPortsItem_Click(object sender, RoutedEventArgs e) => new ConfigureSerialPortsWindow(_controller).ShowDialog();

        private void ListDevicesItem_Click(object sender, RoutedEventArgs e) => new ListDevicesWindow(_controller).ShowDialog();

        private void OneSendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _controller.SendIsRunning = true;
                if (_controller.SelectDevice == null)
                    AdonisUI.Controls.MessageBox.Show("Не выбрано устройство для отправки запроса!", "Проверьте данные", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
                else
                    Task.Factory.StartNew(() =>
                    {
                        _controller.SelectFunc.Send(_controller.SelectDevice);
                        _controller.SendIsRunning = false;
                    }, new CancellationToken(), TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch
            {
                _controller.SendIsRunning = false;
            }
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

        // Нажатие кнопки переодической отправки данных
        private void RepeatSendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Если периодическа отправка не запущена, то запускаем
                if (!_controller.RepeatIsRunning)
                {
                    // Блокируем интерфейс
                    _controller.RepeatIsRunning = true;

                    // Добавляю задачу на отправку
                    _taskController = new CancellationTokenSource();
                    _task = new Task(() =>
                    {
                        // Каждые [_controller.RepeatMs] миллисекунд выполняем функцию задачи
                        while (true)
                        {
                            // Если пришла отмена отправки, то завершаем цикл и снимаем блокировку GUI
                            if (_taskController.IsCancellationRequested)
                            {
                                _controller.RepeatIsRunning = false;
                                break;
                            }
                            // Иначе отправляем сообщение
                            _controller.SelectFunc.Send(_controller.SelectDevice);
                            Thread.Sleep(_controller.RepeatMs);
                        }
                    }, _taskController.Token, TaskCreationOptions.LongRunning);
                    _task.Start();
                }
                // Если периодическая отправка запущена, то отправляем статус на закрытие
                else _taskController.Cancel();
            } catch (Exception ex) // Если ошибка, то выводим сообщение для отладки и высвобождаем ресурсы задачи
            {
                AdonisUI.Controls.MessageBox.Show(ex.Message, "Ошибка запроса");
                _taskController.Cancel();
                _task.Dispose();
                _controller.RepeatIsRunning = false;
            }
        }

    }
}
