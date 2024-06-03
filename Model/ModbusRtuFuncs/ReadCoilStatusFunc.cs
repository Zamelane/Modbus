using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Modbus.Device;
using Modbus.Model.Involucres;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class ReadCoilStatusFunc : DefaultModel
    {
        public ReadCoilStatusFunc() : base("Чтение значений из нескольких регистров флагов (Read Coil Status)", 0x01)
        {
            IsStartAddress = true;
            IsNumberOfPoint = true;
        }

        public override void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true)
        {
            bool[] values;
            try
            {
                values = master.ReadCoils(1, StartAddress, NumberOfPoint);
            }
            catch (Exception ex)
            {
                message += $"Ошибка чтения значений: [{device.Id}, {StartAddress} - {StartAddress + NumberOfPoint - 1}]";
                message += $"\n{ex.Message}";
                LastLogs = message;
                return;
            }

            // Заполняем таблицу значений
            var BooleanValues = new List<BooleanCeil>();
            var address = StartAddress;
            foreach (var value in values)
                BooleanValues.Add(new BooleanCeil(address++, value));
            UpdateTableValues(BooleanValues: BooleanValues);

            message += $"Прочитаны значения устройства [Slave ID: {device.Id}] по адресу [{StartAddress} - {StartAddress + NumberOfPoint - 1}].";
        }
    }
}
