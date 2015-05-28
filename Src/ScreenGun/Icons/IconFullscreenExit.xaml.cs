namespace ScreenGun.Icons
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ic_fullscreen_exit_24px.xaml
    /// </summary>
    public partial class IconFullscreenExit : UserControl
    {
        public IconFullscreenExit()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconFullscreenExit), new PropertyMetadata(Brushes.White));

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
