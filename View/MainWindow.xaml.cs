using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (_controller.SelectDevice == null)
                AdonisUI.Controls.MessageBox.Show("Не выбрано устройство для отправки запроса!", "Проверьте данные", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
            else
            {
                _controller.SendIsRunning = true;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        _controller.SelectFunc.Send(_controller.SelectDevice);
                        _controller.SendIsRunning = false;
                    }
                    finally
                    {
                        _controller.SendIsRunning = false;
                    }
                }, new CancellationToken(), TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

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
                            _controller.SelectFunc.Send(_controller.SelectDevice, _controller.IsRTU);
                            _controller.OnPropertyChanged("TableValues");
                            Thread.Sleep(_controller.RepeatMs);
                        }
                    }, _taskController.Token, TaskCreationOptions.LongRunning);
                    _task.Start();
                }
                // Если периодическая отправка запущена, то отправляем статус на закрытие
                else _taskController.Cancel();
            }
            catch (Exception ex) // Если ошибка, то выводим сообщение для отладки и высвобождаем ресурсы задачи
            {
                AdonisUI.Controls.MessageBox.Show(ex.Message, "Ошибка запроса");
                _taskController.Cancel();
                _task.Dispose();
                _controller.RepeatIsRunning = false;
            }
        }
    }
}
