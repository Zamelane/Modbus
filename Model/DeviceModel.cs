using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Helpers;
using Newtonsoft.Json.Linq;

namespace Modbus.Model
{
    public class DeviceModel : ChangeRealization
    {
        private byte            id;                       // Salve id
        private string          name;                    // Псевдоним
        private SerialPortModel serialPortModel = null; // Модель для взаимодействия с портом

        public byte Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string SpecificName => $"{name} [Salve id: {id}]";
        public SerialPortModel SerialPortModel
        {
            get => serialPortModel;
            set
            {
                serialPortModel = value;
                OnPropertyChanged("SerialPortModel");
            }
        }

        public DeviceModel()
        {
            name = "Безымянное устройство";
        }
    }
}
