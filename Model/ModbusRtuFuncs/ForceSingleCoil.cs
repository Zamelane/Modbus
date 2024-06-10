using System.Collections.Generic;
using Modbus.Device;
using Modbus.Model.Involucres;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class ForceSingleCoil : DefaultModel
    {
        public ForceSingleCoil() : base("Запись одного флага (Force Single Coil)", 0x05)
        {
            IsCoilAddress  = true;
            IsBooleanValue = true;
        }

        public override void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true)
        {
            try
            {
                master.WriteSingleCoil(device.Id, CoilAddress, BooleanValue);
            }
            catch
            {
                message += $"Ошибка записи одного флага: [{device.Id}, {CoilAddress}, {BooleanValue}]";
                LastLogs = message;
                return;
            }

            // Заполняем таблицу значений
            UpdateTableValues(BooleanValues: new List<BooleanCeil>() { new BooleanCeil(CoilAddress, BooleanValue) });

            message += $"Записано значение [{BooleanValue}] в регистр [{CoilAddress}] устройства [Slave ID: {device.Id}].";
        }
    }
}
