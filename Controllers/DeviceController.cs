using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Model;

namespace Modbus.Controllers
{
    public class DeviceController : PortController
    {
        private ObservableCollection<DeviceModel> devices = new ObservableCollection<DeviceModel>();
        private DeviceModel selectDevice = null;

        public ObservableCollection<DeviceModel> Devices
        {
            get => devices;
            set
            {
                devices = value;
                OnPropertyChanged("Devices");
            }
        }
        public DeviceModel SelectDevice
        {
            get => selectDevice;
            set
            {
                selectDevice = value;
                OnPropertyChanged("SelectDevice");
            }
        }

        public bool CheckDeviceAvailability(DeviceModel checkDevice)
        {
            foreach (var device in devices)
                if (device.Id == checkDevice.Id && device.SerialPortModel == checkDevice.SerialPortModel)
                    return true;
            return false;
        }
    }
}
