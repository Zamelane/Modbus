using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class PresetMultipleRegisters : DefaultModel
    {
        public PresetMultipleRegisters() : base ("Запись значений в несколько регистров хранения (Preset Multiple Registers)", 0x10)
        {
            IsStartAddress = true;
            IsMultipleUInt16Value = true;
        }

        public override void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true)
        {
            UInt16[] UInt16Values = new UInt16[MultipleUInt16Value.Count];
            for (int i = 0; i < MultipleUInt16Value.Count; i++)
                UInt16Values[i] = MultipleUInt16Value[i].Value;

            try
            {
                master.WriteMultipleRegisters(device.Id, StartAddress, UInt16Values);
            }
            catch
            {
                message += $"Ошибка записи значений: [{device.Id}, {CoilAddress}],\n[{String.Join(",", UInt16Values.ToArray())}]";
                LastLogs = message;
                return;
            }

            // Заполняем таблицу значений
            UpdateTableValues(UInt16Values: MultipleUInt16Value.ToList());

            message += $"Записаны значения [{String.Join(",", UInt16Values.ToArray())}]\nв регистр [{CoilAddress}] устройства [Slave ID: {device.Id}].";
        }
    }
}
