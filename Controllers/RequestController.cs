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
        public bool IsViewTypeTable => selectViewType == "Таблица";
    }
}
