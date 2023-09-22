using System.Windows;
using System.Windows.Media;

namespace LOL.Assist.App.Views.Controls
{
    /// <summary>
    /// HeroControl.xaml 的交互逻辑
    /// </summary>
    public partial class HeroControl
    {
        public HeroControl()
        {
            InitializeComponent();
            HeroPhotoRoot.DataContext = this;
        }


        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image),
            typeof(ImageSource),
            typeof(HeroControl), 
            new PropertyMetadata(null));

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
            typeof(string),
            typeof(HeroControl),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// 英雄id
        /// </summary>
        public int ChampionId
        {
            get => (int)GetValue(ChampionIdProperty);
            set => SetValue(ChampionIdProperty, value);
        }

        public static readonly DependencyProperty ChampionIdProperty = DependencyProperty.Register(nameof(ChampionId),
            typeof(int),
            typeof(HeroControl),
            new PropertyMetadata(0));

        /// <summary>
        /// 优先权限
        /// </summary>
        public int Priority
        {
            get => (int)GetValue(PriorityProperty);
            set => SetValue(PriorityProperty, value);
        }

        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register(nameof(Priority),
            typeof(int),
            typeof(HeroControl),
            new PropertyMetadata(0));

        /// <summary>
        /// 位置信息
        /// </summary>
        public string Position
        {
            get => (string)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position),
            typeof(string),
            typeof(HeroControl),
            new PropertyMetadata(string.Empty));
    }
}
