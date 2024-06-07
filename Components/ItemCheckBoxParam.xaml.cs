using System;
using System.Collections.Generic;
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

namespace Modbus.Components
{
    /// <summary>
    /// Логика взаимодействия для ItemCheckBoxParam.xaml
    /// </summary>
    public partial class ItemCheckBoxParam : UserControl
    {
        public ItemCheckBoxParam()
        {
            InitializeComponent();
        }

        /* Настройка полей */

        // Настройка свойств
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
            (
                "Value",
                typeof(bool),
                typeof(ItemCheckBoxParam),
                new PropertyMetadata(false)
            );
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            (
                "Title",
                typeof(string),
                typeof(ItemCheckBoxParam),
                new PropertyMetadata(string.Empty)
            );

        // Настройка переменных
        public bool Value
        {
            get => (bool)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}
