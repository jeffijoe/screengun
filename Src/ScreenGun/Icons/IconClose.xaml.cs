namespace ScreenGun.Icons
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ic_close_24px.xaml
    /// </summary>
    public partial class IconClose : UserControl
    {
        public IconClose()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconClose), new PropertyMetadata(Brushes.White));

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
