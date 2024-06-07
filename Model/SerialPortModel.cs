using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using Modbus.Helpers;

namespace Modbus.Model
{
    public class SerialPortModel : ChangeRealization
    {
        private SerialPort _sr;

        public string Name => _sr.PortName;
        public int BaudRate
        {
            get => _sr.BaudRate;
            set
            {
                _sr.BaudRate = value;
                OnPropertyChanged("BaudRate");
            }
        }
        public int DataBits
        {
            get => _sr.DataBits;
            set
            {
                _sr.DataBits = value;
                OnPropertyChanged("DataBits");
            }
        }
        public string Parity
        {
            get => _sr.Parity.ToString();
            set
            {
                Parity parity;
                switch (value)
                {
                    case "Even":
                        parity = System.IO.Ports.Parity.Even; break;
                    case "Mark":
                        parity = System.IO.Ports.Parity.Mark; break;
                    case "Odd":
                        parity = System.IO.Ports.Parity.Odd; break;
                    case "Space":
                        parity = System.IO.Ports.Parity.Space; break;
                    default:
                        parity = System.IO.Ports.Parity.None; break;
                }
                _sr.Parity = parity;
                OnPropertyChanged("Parity");
            }
        }
        public double StopBits
        {
            get
            {
                double result = Convert.ToDouble(_sr.StopBits);
                return result == 3 ? 1.5 : result;
            }
            set
            {
                StopBits stopBits;
                switch (value)
                {
                    case 1:
                        stopBits = System.IO.Ports.StopBits.One; break;
                    case 2:
                        stopBits = System.IO.Ports.StopBits.Two; break;
                    default:
                        stopBits = System.IO.Ports.StopBits.OnePointFive; break;
                }
                _sr.StopBits = stopBits;
                OnPropertyChanged("StopBits");
            }
        }
        public int ReadTimeoutMs
        {
            get => _sr.ReadTimeout;
            set
            {
                _sr.ReadTimeout = value;
                OnPropertyChanged("ReadTimeMs");
            }
        }
        public int WriteTimeoutMs
        {
            get => _sr.WriteTimeout;
            set
            {
                _sr.WriteTimeout = value;
                OnPropertyChanged("WriteTimeout");
            }
        }

        public SerialPortModel(string portName)
        {
            _sr = new SerialPort(portName);
        }

        public SerialPort GetSerialPort() => _sr;
    }
}
