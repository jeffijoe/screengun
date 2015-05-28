namespace ScreenGun.Icons
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ic_fullscreen_24px.xaml
    /// </summary>
    public partial class IconFullscreen : UserControl
    {
        public IconFullscreen()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconFullscreen), new PropertyMetadata(Brushes.White));

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
