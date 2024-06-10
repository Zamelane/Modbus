using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Model.ModbusRtuFuncs;

namespace Modbus.Controllers
{
    public class RequestController: DeviceController
    {
        private ObservableCollection<DefaultModel> funcs;
        private DefaultModel selectFunc;
        private string[] viewTypes = new string[] { "Текст", "Таблица" };
        private string selectViewType = "Текст";
        private int repeatMs = 1000;
        private bool repeatIsRunning = false;
        private bool sendIsRunning = false;
        private bool isRTU = true;
        public  List<string> ProtocolTypes { get; set; } = new List<string>() { "RTU", "ASCII" };
        public string SelectedProtocol { 
            get => ProtocolTypes.ElementAt(isRTU ? 0 : 1);
            set => isRTU = value == "RTU";
        }

        public RequestController()
        {
            Funcs = new ObservableCollection<DefaultModel>()
            {
                new ReadCoilStatusFunc      (),
                new ReadDiscreteInputs      (),
                new ReadHoldingRegisters    (),
                new ReadInputRegisters      (),
                new ForceSingleCoil         (),
                new PresetSingleRegister    (),
                new ForceMultipleCoils      (),
                new PresetMultipleRegisters ()
            };
            SelectFunc = Funcs.First();
        }

        // MVVM
        public ObservableCollection<DefaultModel> Funcs
        {
            get => funcs;
            set
            {
                funcs = value;
                OnPropertyChanged("Funcs");
            }
        }
        public DefaultModel SelectFunc
        {
            get => selectFunc;
            set
            {
                selectFunc = value;
                OnPropertyChanged("SelectFunc");
            }
        }
        public string[] ViewTypes
        {
            get => viewTypes;
            set
            {
                viewTypes = value;
                OnPropertyChanged("ViewTypes");
            }
        }
        public string SelectViewType
        {
            get => selectViewType;
            set
            {
                selectViewType = value;
                OnPropertyChanged("SelectViewType");
                OnPropertyChanged("IsViewTypeTable");
            }
        }
        public int RepeatMs
        {
            get => repeatMs;
            set
            {
                repeatMs = value;
                OnPropertyChanged("RepeatMs");
            }
        }
        public bool RepeatIsRunning
        {
            get => repeatIsRunning;
            set
            {
                repeatIsRunning = value;
                OnPropertyChanged("RepeatIsRunning");
            }
        }
        public bool SendIsRunning
        {
            get => sendIsRunning;
            set
            {
                sendIsRunning = value;
                OnPropertyChanged("SendIsRunning");
            }
        }
        public bool IsRTU
        {
            get => isRTU;
            set
            {
                isRTU = value;
                OnPropertyChanged("IsRTU");
            }
        }
        public bool IsViewTypeTable => selectViewType == "Таблица";
    }
}
