using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Helpers;

namespace Modbus.Model.Involucres
{
    public class UInt16Ceil : ChangeRealization
    {
        private UInt16 address;
        private UInt16 value;

        public UInt16Ceil(UInt16 address, UInt16 value)
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
        public UInt16 Value
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
