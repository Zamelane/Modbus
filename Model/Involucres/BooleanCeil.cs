using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Helpers;

namespace Modbus.Model.Involucres
{
    public class BooleanCeil : ChangeRealization
    {
        private UInt16 address;
        private bool value;

        public BooleanCeil(UInt16 address, bool value)
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
        public bool Value
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
