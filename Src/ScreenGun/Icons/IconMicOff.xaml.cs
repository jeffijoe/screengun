namespace ScreenGun.Icons
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ic_mic_off_24px.xaml
    /// </summary>
    public partial class IconMicOff : UserControl
    {
        public IconMicOff()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconMicOff), new PropertyMetadata(Brushes.White));

        public Brush IconBackground
        {
            get
            {
                return (Brush)this.GetValue(IconBackgroundProperty);
            }
            set
            {
                this.SetValue(IconBackgroundProperty, value);
            }
        }
    }
}
