using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Modbus.Model.ModbusRtuFuncs;

namespace Modbus.Components
{
    /// <summary>
    /// Логика взаимодействия для ItemMassValuesParam.xaml
    /// </summary>
    public partial class ItemMassValuesParam : UserControl
    {
        public ItemMassValuesParam()
        {
            InitializeComponent();
        }

        /* Настройка полей */

        // Настройка свойств
        public static readonly DependencyProperty FuncProperty = DependencyProperty.Register
            (
                "Func",
                typeof(DefaultModel),
                typeof(ItemMassValuesParam),
                new PropertyMetadata()
            );
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            (
                "Title",
                typeof(string),
                typeof(ItemMassValuesParam),
                new PropertyMetadata(string.Empty)
            );

        // Настройка переменных
        public DefaultModel Func
        {
            get => (DefaultModel)GetValue(FuncProperty);
            set => SetValue(FuncProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Функионал работы с таблицами
        private void AddMassValue_Click(object sender, RoutedEventArgs e)
        {
            var func = Func;
            if (func.IsMultipleUInt16Value)
                func.AddMultipleUInt16Value();
            else func.AddMultipleBooleanValue();
        }

        private void RemoveMassValue_Click(object sender, RoutedEventArgs e)
        {
            var func = Func;
            bool isNotSelected = false;
            if (func.IsMultipleUInt16Value)
            {
                if (MultipleUInt16Values.SelectedIndex == -1)
                    isNotSelected = true;
                else func.RemoveMultipleUInt16Value(MultipleUInt16Values.SelectedIndex);
            }
            else
            {
                if (MultipleBooleanValues.SelectedIndex == -1)
                    isNotSelected = true;
                else func.RemoveMultipleBooleanValue(MultipleBooleanValues.SelectedIndex);
            }

            if (isNotSelected)
                AdonisUI.Controls.MessageBox.Show("Не выбран ни один элемент для удаления!", "Обратите внимание", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Information);
        }

        private void MultipleValues_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column is DataGridBoundColumn column)
                {
                    var el = e.EditingElement as TextBox;
                    var header = column.Header.ToString();
                    var rowIndex = e.Row.GetIndex();

                    UInt16 result;
                    if (header == "UInt16" && UInt16.TryParse(el.Text, out result))
                    {
                       Func.EditMultipleUInt16Value(rowIndex, result);
                    }
                }
            }
        }

        private void MultipleBooleanValue_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox.Tag == null)
                return;

            var index = Convert.ToInt32(checkBox.Tag) - Func.CoilAddress;

            Func.EditMultipleBooleanValue(index, checkBox.IsChecked == true);
        }
    }
}
