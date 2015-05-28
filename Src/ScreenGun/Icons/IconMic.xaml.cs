namespace ScreenGun.Icons
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ic_mic_24px.xaml
    /// </summary>
    public partial class IconMic : UserControl
    {
        public IconMic()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconMic), new PropertyMetadata(Brushes.White));

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
