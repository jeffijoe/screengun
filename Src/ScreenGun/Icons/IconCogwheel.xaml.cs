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

namespace ScreenGun.Icons
{
    /// <summary>
    /// Interaction logic for IconCogwheel.xaml
    /// </summary>
    public partial class IconCogwheel : UserControl
    {
        public IconCogwheel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register(
            "IconBackground",
            typeof(Brush),
            typeof(IconCogwheel),
            new PropertyMetadata(Brushes.White));

        public Brush IconBackground
        {
            get
            {
                return (Brush)GetValue(IconBackgroundProperty);
            }
            set
            {
                SetValue(IconBackgroundProperty, value);
            }
        }
    }
}
