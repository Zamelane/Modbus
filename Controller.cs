using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Controllers;
using Modbus.Device;
using Modbus.Model;

namespace Modbus
{
    public class Controller : RequestController
    {
        public Controller()
        {
            LoadPorts();
        }

        public void LoadPorts()
        {
            var portsNames = SerialPort.GetPortNames();

            foreach (var portName in portsNames)
                if (!CheckPortAvailability(portName))
                    Ports.Add(new SerialPortModel(portName));

            DeleteMissingPort(portsNames);
        }

        private bool CheckPortAvailability(string portName)
        {
            foreach (var serialPortModel in Ports)
                if (serialPortModel.Name == portName)
                    return true;
            return false;
        }

        private void DeleteMissingPort(string[] portsNames)
        {
            // Ищем "пропавшие" порты и связанные с ними устройства
            List<SerialPortModel> portsToDelete = new List<SerialPortModel>();
            List<DeviceModel> devicesToDelete = new List<DeviceModel>();
            foreach (var serialPortModel in Ports)
            {
                // Проверяем, есть ли порт из старого списка в актуальном списке портов
                var isFound = false;
                foreach (var portName in portsNames)
                    if (serialPortModel.Name == portName)
                    {
                        isFound = true;
                        break;
                    }

                if (!isFound)
                {
                    // Добавляем порт в список на удаление
                    portsToDelete.Add(serialPortModel);

                    // Добавляем устройство в список на удаление
                    foreach (var device in Devices)
                        if (device.SerialPortModel == serialPortModel)
                            devicesToDelete.Add(device);
                }

            }

            // Удаляем устройства, связанные портами не найденными в списке актуальных портов
            foreach (var device in devicesToDelete)
                Devices.Remove(device);

            // Удаляем не найденные порты в списке актуальных портов
            foreach (var port in portsToDelete)
                Ports.Remove(port);
        }
    }
}
