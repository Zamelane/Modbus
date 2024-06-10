using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using Modbus.Message;
using Modbus.Model.Involucres;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class PresetSingleRegister : DefaultModel
    {
        public PresetSingleRegister() : base("Запись значения в один регистр хранения (Preset Single Register)", 0x06)
        {
            IsCoilAddress = true;
            IsUInt16Value = true;
        }

        public override void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true)
        {
            try
            {
                master.WriteSingleRegister(device.Id, CoilAddress, UInt16Value);
            }
            catch
            {
                message += $"Ошибка записи одного значения: [{device.Id}, {CoilAddress}, {UInt16Value}]";
                LastLogs = message;
                return;
            }

            // Заполняем таблицу значений
            UpdateTableValues(UInt16Values: new List<UInt16Ceil>() { new UInt16Ceil(CoilAddress, UInt16Value) });

            message += $"Записано значение [{UInt16Value}] в регистр [{CoilAddress}] устройства [Slave ID: {device.Id}].";
        }
    }
}
