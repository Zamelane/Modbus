using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Controllers;
using Modbus.Device;

namespace Modbus
{
    public class Controller : RequestController
    {
        public Controller()
        {
            LoadPorts();
        }
    }
}
