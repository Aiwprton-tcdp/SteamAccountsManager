using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SteamAccountsManager.Views.Templates;

public partial class LoadingUserControl : UserControl
{
    public static readonly DependencyProperty ArcThicknessProperty =
        DependencyProperty.Register("ArcThickness",
            typeof(double),
            typeof(LoadingUserControl),
            new PropertyMetadata(30D,
                new PropertyChangedCallback(ThicknessChange)));

    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register("Fill",
            typeof(Brush),
            typeof(LoadingUserControl),
            new PropertyMetadata(new SolidColorBrush(DefaultColor),
                new PropertyChangedCallback(ArcFill)));

    public double ArcThickness
    {
        get => (double)GetValue(ArcThicknessProperty);
        set => SetValue(ArcThicknessProperty, value);
    }

    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    static Color DefaultColor =>
        (Color)ColorConverter.ConvertFromString("#FF27B2D1");
    

    public LoadingUserControl()
    {
        InitializeComponent();
    }


    static void ThicknessChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as LoadingUserControl).arc.ArcThickness = (double)e.NewValue;
    }

    static void ArcFill(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        (d as LoadingUserControl).arc.Fill = (Brush)e.NewValue;
    }
}
