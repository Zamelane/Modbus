using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using Modbus.Model.Involucres;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class ReadInputRegisters : DefaultModel
    {
        public ReadInputRegisters() : base("Чтение значений из нескольких регистров ввода (Read Input Registers)", 0x04)
        {
            IsStartAddress = true;
            IsNumberOfPoint = true;
        }

        public override void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true)
        {
            UInt16[] values;
            try
            {
                values = master.ReadInputRegisters(1, StartAddress, NumberOfPoint);
            }
            catch
            {
                message += $"Ошибка чтения значений: [{device.Id}, {StartAddress} - {StartAddress + NumberOfPoint - 1}]";
                LastLogs = message;
                return;
            }

            // Заполняем таблицу значений
            var UInt16Values = new List<UInt16Ceil>();
            var address = StartAddress;
            foreach (var value in values)
                UInt16Values.Add(new UInt16Ceil(address++, value));
            UpdateTableValues(UInt16Values: UInt16Values);

            message += $"Прочитаны значения устройства [Slave ID: {device.Id}] по адресу [{StartAddress} - {StartAddress + NumberOfPoint - 1}].";
        }
    }
}
