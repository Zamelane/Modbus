using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Modbus.Device;
using Modbus.Helpers;
using Modbus.Model.Involucres;

namespace Modbus.Model.ModbusRtuFuncs
{
    public class DefaultModel : ChangeRealization
    {
        public DefaultModel(string name, Byte code)
        {
            Name = name;
            Code = code;
        }

        // Описание функции
        public string Name
        {
            get;
        }
        public Byte Code
        {
            get;
        }

        // Значения обязательности полей для ввода (опциональные)
        public bool IsStartAddress
        {
            get; set;
        }   // Начальный адрес чтения
        public bool IsNumberOfPoint
        {
            get; set;
        }   // Количество адресов для чтения
        public bool IsCoilAddress
        {
            get; set;
        }   // Адрес для записи
        public bool IsBooleanValue
        {
            get; set;
        }   // Значение для записи (логического типа)
        public bool IsUInt16Value
        {
            get; set;
        }   // Значение для записи (UInt16)
        public bool IsMultipleUInt16Value
        {
            get; set;
        }   // Значения для записи (UInt16[])
        public bool IsMultipleBooleanValue
        {
            get; set;
        }   // Значения для записи (Boolean[])


        // Значения полей для ввода
        private UInt16 startAddress = 0;
        private UInt16 numberOfPoints = 0;
        private byte coilAddress = 0;
        private bool booleanValue = false;
        private UInt16 uInt16Value = 0;
        private ObservableCollection<UInt16> multipleUInt16Value = new ObservableCollection<UInt16>() { 0 };
        private ObservableCollection<bool> multipleBooleanValue = new ObservableCollection<bool>() { true };

        // Результаты работы
        private string logs = "";
        public ObservableCollection<UniversalCeil> TableValues { get; set; } = new ObservableCollection<UniversalCeil>();

        public string LastLogs
        {
            get => logs;
            set
            {
                logs = value;
                OnPropertyChanged("LastLogs");
            }
        }

        // Обёртка для MVVM
        public UInt16 StartAddress
        {
            get => startAddress;
            set
            {
                startAddress = value;
                OnPropertyChanged("StartAddress");
                OnPropertyChanged("MultipleUInt16Value");
                OnPropertyChanged("MultipleBooleanValue");
            }
        }
        public UInt16 NumberOfPoint
        {
            get => numberOfPoints;
            set
            {
                numberOfPoints = value;
                OnPropertyChanged("NumberOfPoint");
            }
        }
        public byte CoilAddress
        {
            get => coilAddress;
            set
            {
                coilAddress = value;
                OnPropertyChanged("CoilAddress");
            }
        }
        public bool BooleanValue
        {
            get => booleanValue;
            set
            {
                booleanValue = value;
                OnPropertyChanged("BooleanValue");
            }
        }
        public UInt16 UInt16Value
        {
            get => uInt16Value;
            set
            {
                uInt16Value = value;
                OnPropertyChanged("UInt16Value");
            }
        }
        public ObservableCollection<UInt16Ceil> MultipleUInt16Value
        {
            get
            {
                var uInt16Ceils = new ObservableCollection<UInt16Ceil>();
                var address = startAddress;
                foreach (var ceil in multipleUInt16Value)
                    uInt16Ceils.Add(new UInt16Ceil(address++, ceil));
                return uInt16Ceils;
            }
            /*            set
                        {
                            var uInt16Ceils = value;
                            var collection = new ObservableCollection<UInt16>();
                            foreach (var ceil in uInt16Ceils)
                                collection.Add(ceil.Value);
                            multipleUInt16Value = collection;
                            OnPropertyChanged("MultipleUInt16Value");
                        }*/
        }
        public ObservableCollection<BooleanCeil> MultipleBooleanValue
        {
            get
            {
                var booleanCeils = new ObservableCollection<BooleanCeil>();
                var address = startAddress;
                foreach (var ceil in multipleBooleanValue)
                    booleanCeils.Add(new BooleanCeil(address++, ceil));
                return booleanCeils;
            }
            /*            set
                        {
                            var booleanCeils = value;
                            var collection = new ObservableCollection<bool>();
                            foreach (var ceil in booleanCeils)
                                collection.Add(ceil.Value);
                            multipleBooleanValue = collection;
                            OnPropertyChanged("MultipleBooleanValue");
                        }*/
        }

        // Специфичные поля
        public string SpecificName => $"(0x{BitConverter.ToString(new byte[] { Code })}) {Name}";

        // Функционал
        /* Добавление значений */
        public void AddMultipleUInt16Value()
        {
            multipleUInt16Value.Add(0);
            OnPropertyChanged("MultipleUInt16Value");
        }
        public void AddMultipleBooleanValue()
        {
            multipleBooleanValue.Add(false);
            OnPropertyChanged("MultipleBooleanValue");
        }
        /* Удаление значений */
        public void RemoveMultipleUInt16Value(int index)
        {
            multipleUInt16Value.RemoveAt(index);
            OnPropertyChanged("MultipleUInt16Value");
        }
        public void RemoveMultipleBooleanValue(int index)
        {
            multipleBooleanValue.RemoveAt(index);
            OnPropertyChanged("MultipleBooleanValue");
        }
        /* Редактирование значений */
        public void EditMultipleUInt16Value(int index, UInt16 value)
        {
            multipleUInt16Value[index] = value;
            OnPropertyChanged("MultipleUInt16Value");
        }
        public void EditMultipleBooleanValue(int index, bool value)
        {
            multipleBooleanValue[index] = value;
            OnPropertyChanged("MultipleBooleanValue");
        }
        public void Send(DeviceModel device, bool isRTU = true)
        {
            //UpdateTableValues(); // Очищаем таблицу значений

            var message = GenerateResultMessage(Name);

            SerialPort port;
            try
            {
                port = device.SerialPortModel.GetSerialPort();

                if (!port.IsOpen)
                    port.Open();
            }
            catch
            {
                message += $"Ошибка открытия порта \"{device.SerialPortModel.Name}\"";
                LastLogs = message;
                return;
            }

            ModbusSerialMaster master;
            try
            {
                // Теперь конфигурируется через настройки портов
                //port.ReadTimeout = 1000;
                //port.WriteTimeout = 1000;
                master = isRTU ? ModbusSerialMaster.CreateRtu(port) : ModbusSerialMaster.CreateAscii(port);
            }
            catch
            {
                message += $"Ошибка создания мастера на порту \"{device.SerialPortModel.Name}\"";
                LastLogs = message;
                return;
            }

            SendMessage(device, master, ref message, isRTU);

            LastLogs = message;
            port.Close();
        }
        public void UpdateTableValues(List<UInt16Ceil> UInt16Values = null, List<BooleanCeil> BooleanValues = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Ноовое содержимое таблицы
                List<UniversalCeil> ceils = new List<UniversalCeil>();

                if (BooleanValues != null)
                    foreach (var ceil in BooleanValues)
                        ceils.Add(new UniversalCeil(ceil.Address, ceil.Value.ToString()));
                else if (UInt16Values != null)
                    foreach (var ceil in UInt16Values)
                        ceils.Add(new UniversalCeil(ceil.Address, ceil.Value.ToString()));

                // Удаляем лишние строки
                if (TableValues.Count > ceils.Count)
                    for (var i = TableValues.Count - 1; i > ceils.Count - 1; i--)
                        TableValues.RemoveAt(i);

                // Переписываем или добавляем строки
                for (var i = 0; i < ceils.Count(); i++)
                    if (TableValues.Count() <= i)
                        TableValues.Add(ceils.ElementAt(i));
                    else
                    {
                        var line     = TableValues.ElementAt(i);
                        var newLine  = ceils.ElementAt(i);

                        line.Address = newLine.Address;
                        line.Value   = newLine.Value.ToString();
                    }

                // Посылаем уведомление об обновлении
                //OnPropertyChanged("TableValues");
            });
        }
        public virtual void SendMessage(DeviceModel device, ModbusSerialMaster master, ref string message, bool isRTU = true) => message += "Метод не переопределён!";
        public string GenerateResultMessage(string message) => $"[{DateTime.Now.ToString()}] {message}\n";
        public void ClearLogs()
        {
            LastLogs = "";
            TableValues.Clear();
        }
    }
}
