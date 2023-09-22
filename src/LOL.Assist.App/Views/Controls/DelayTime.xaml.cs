using System.Windows;

namespace LOL.Assist.App.Views.Controls
{
    /// <summary>
    /// DelayTime.xaml 的交互逻辑
    /// </summary>
    public partial class DelayTime
    {
        public DelayTime()
        {
            InitializeComponent();
            DelayTimePanel.DataContext = this;
        }
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(DelayTime), new PropertyMetadata(0));



        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(DelayTime), new PropertyMetadata(0));



        public int MinValue
        {
            get => (int)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(int), typeof(DelayTime), new PropertyMetadata(0));




    }
}
