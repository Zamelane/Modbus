using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Modbus.Helpers;
using Modbus.Model;

namespace Modbus
{
    public class PortController : ChangeRealization
    {
        private ObservableCollection<SerialPortModel> ports = new ObservableCollection<SerialPortModel>();
        private SerialPortModel selectPort;

        public ObservableCollection<SerialPortModel> Ports
        {
            get => ports;
            set
            {
                ports = value;
                OnPropertyChanged("Ports");
            }
        }
        public SerialPortModel SelectPort
        {
            get => selectPort;
            set
            {
                selectPort = value;
                OnPropertyChanged("SelectPort");
            }
        }

        // Списки значений
        public int[] BAUDRATES
        {
            get; set;
        } = new int[]
        {
            110,
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            14400,
            19200,
            38400,
            56000,
            57600,
            115200,
            128000,
            256000
        };
        public int[] DATABITS
        {
            get; set;
        } =
        {
            5,
            6,
            7,
            8
        };
        public string[] PARITYS
        {
            get; set;
        } =
        {
            "Even",
            "Mark",
            "Odd",
            "Space",
            "None"
        };
        public double[] STOP_BITS
        {
            get; set;
        } =
        {
            1,
            1.5,
            2
        };
    }
}
