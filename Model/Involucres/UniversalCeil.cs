using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Helpers;

namespace Modbus.Model.Involucres
{
    public class UniversalCeil : ChangeRealization
    {
        private UInt16 address;
        private string value;
        public UniversalCeil(UInt16 address, string value)
        {
            this.address = address;
            this.value = value;
        }

        public UInt16 Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }
        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }
    }
}
