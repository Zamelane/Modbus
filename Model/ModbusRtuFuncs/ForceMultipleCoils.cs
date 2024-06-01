using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using Modbus.Model.Involucres;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class ForceMultipleCoils : DefaultModel
    {
        public ForceMultipleCoils() : base ("Запись значений в несколько регистров флагов (Force Multiple Coils)", 0x0F)
        {
            IsStartAddress = true;
            IsMultipleBooleanValue = true;
        }

        public override void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true)
        {
            bool[] booleanValues = new bool[MultipleBooleanValue.Count];
            for (int i = 0; i < MultipleBooleanValue.Count; i++)
                booleanValues[i] = MultipleBooleanValue[i].Value;

            try
            {
                master.WriteMultipleCoils(device.Id, StartAddress, booleanValues);
            }
            catch
            {
                message += $"Ошибка записи значений: [{device.Id}, {CoilAddress}],\n[{String.Join(",", booleanValues.ToArray())}]";
                LastLogs = message;
                return;
            }

            // Заполняем таблицу значений
            UpdateTableValues(BooleanValues: MultipleBooleanValue.ToList());

            message += $"Записаны значения [{String.Join(",", booleanValues.ToArray())}]\nв регистр [{CoilAddress}] устройства [Slave ID: {device.Id}].";
        }
    }
}
