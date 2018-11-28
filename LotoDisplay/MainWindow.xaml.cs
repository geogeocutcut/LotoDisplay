using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LotoDisplay
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HashSet<Button> AlReady_Click = new HashSet<Button>();
        private IEnumerator<string> run;
        public MainWindow()
        {
            InitializeComponent();
            Init();
            string runTmp = ConfigurationManager.AppSettings["Run"];
            run = runTmp.Split(';').ToList<string>().GetEnumerator();
        }

        private void Init()
        {
            AlReady_Click.Clear();
            Num_Display.Text = "--"; num_grid.Children.Clear();
             var pad = new Thickness { Bottom = 2, Top = 2, Left = 2, Right = 2 };
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var factory = new FrameworkElementFactory(typeof(Viewbox));
                    Button btn = new Button
                    {
                        Padding=pad,
                        HorizontalContentAlignment=HorizontalAlignment.Stretch,
                        Name = "numb_" + int.Parse(j + "" + i) + 1,
                        Content = new Border { HorizontalAlignment=HorizontalAlignment.Stretch, Child = new Viewbox { Stretch = Stretch.Uniform, HorizontalAlignment = HorizontalAlignment.Stretch, Child = new TextBlock { HorizontalAlignment = HorizontalAlignment.Stretch,Padding=pad,  Text = "" + (int.Parse(j + "" + i) + 1) } } },
                    };
                    btn.Click += NumClick_Event;
                    Grid.SetRow(btn, i + 1);
                    Grid.SetColumn(btn, j + 1);
                    num_grid.Children.Add(btn);
                }
            }
        }

        private void NumClick_Event(object sender, RoutedEventArgs e)
        {
            if(AlReady_Click.Contains(sender))
            {
                AlReady_Click.Remove((Button)sender);
                var but = (Button)sender;
                ((Border)but.Content).ClearValue(Border.BackgroundProperty);
                ((TextBlock)((Viewbox)((Border)but.Content).Child).Child).ClearValue(TextBlock.ForegroundProperty);
                Num_Display.Text = AlReady_Click.LastOrDefault()!=null? ((TextBlock)((Viewbox)((Border)AlReady_Click.LastOrDefault().Content).Child).Child).Text:"--";
            }
            else
            {
                AlReady_Click.Add((Button)sender);
                var but = (Button)sender;
                ((Border)but.Content).Background = Brushes.DarkGreen;
                ((TextBlock)((Viewbox)((Border)but.Content).Child).Child).Foreground = Brushes.White;
                Num_Display.Text = ((TextBlock)((Viewbox)((Border)but.Content).Child).Child).Text;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Game_Display.Text = "";
            Init();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if(!run.MoveNext())
            {
                run.Reset();
                run.MoveNext();
            }
            Game_Display.Text = run.Current;
            //Init();
        }
        private void UneLigne_Click(object sender, RoutedEventArgs e)
        {
            Game_Display.Text = "1 Ligne";
            //Init();
        }
        private void DeuxLignes_Click(object sender, RoutedEventArgs e)
        {
            Game_Display.Text = "2 Lignes";
            //Init();
        }
        private void Carton_Click(object sender, RoutedEventArgs e)
        {
            Game_Display.Text = "Carton Complet";
            Init();
        }
        private void Perdant_Click(object sender, RoutedEventArgs e)
        {
            Game_Display.Text = "Loto du Perdant";
            //Init();
        }
    }
}
