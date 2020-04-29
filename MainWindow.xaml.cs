using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Skylines_Problem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Label, Tuple<Building, Rectangle>> buildings = new Dictionary<Label, Tuple<Building, Rectangle>>();
        
        private readonly Random random = new Random();

        private readonly List<UIElement> elements = new List<UIElement>();
        public MainWindow()
        {
            InitializeComponent();
      
            foreach(UIElement child in board.Children)
            {
                elements.Add(child);
            }
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            int X1, X2, H;
            try
            {
                 X1 = int.Parse(x1.Text);
                 X2 = int.Parse(x2.Text);
                if(X2 <= X1)
                {
                    return;
                }
                 H = int.Parse(height.Text);
            }catch (Exception)
            {
                x1.Text = "!";
                x2.Text = "!";
                height.Text = "!";
                return;
            }
            Building building = new Building(X1, X2, H);

            var color = new SolidColorBrush(
                Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
            Rectangle myRec = new Rectangle
            {
                Fill = color,
                Width = building.Width() * 10,
                Height = building.Height * 10,
                Opacity = 0.5,
                
            };
            TranslateTransform translate = new TranslateTransform(20 + building.Left *10, 355 - building.Height*10);
            myRec.RenderTransform = translate;
            
            board.Children.Add(myRec);

            Label newLabel = new Label {
                Content = building.ToString(),
                Background = color,
                Foreground = new SolidColorBrush(
                    Color.FromRgb((byte)(255 - color.Color.R), (byte)(255 - color.Color.G), (byte)(255 - color.Color.B))),
                HorizontalContentAlignment=HorizontalAlignment.Center,
                Padding = new Thickness(20,8,20,8),
                Opacity=0.5,
                HorizontalAlignment=HorizontalAlignment.Stretch
             
            };
            inputListBox.Items.Add(newLabel);
            buildings.Add(newLabel,new Tuple<Building, Rectangle>(building,myRec));
        }

        private void DrawSkylines(object sender, RoutedEventArgs e)
        {
            var pairs = SkylineCalculatorUtil
                .GetSkylines(0, buildings.Count- 1,
                (from pair in buildings.Values select pair.Item1).ToList());

            var color = new SolidColorBrush(Color.FromRgb(150, (byte)random.Next(256), 50));
            var lineColor = new SolidColorBrush(Color.FromRgb(10, 10, 10));

            var previousY = 0;
            var previousX = 0;
            foreach (var pair in pairs.OrderBy(p=>p.Item1))
            {
                AddNode(pair, color);
                AddSillouettes(pair, ref previousX, ref previousY, lineColor);
            }
            AddSillouettes(new Tuple<int, int>(100,0), ref previousX, ref previousY, lineColor);
        }

        private void AddNode(Tuple<int, int> pair, SolidColorBrush color)
        {
            Ellipse ellipse = new Ellipse()
            {
                Height = 8,
                Width = 8,
                Fill = color,
            };
            ToolTip tooltip = new ToolTip();
            tooltip.Placement = PlacementMode.Right;
            tooltip.PlacementRectangle = new Rect(16, 0, 0, 0);
            tooltip.HorizontalOffset = 4;
            tooltip.VerticalOffset = 8;

            BulletDecorator bdec = new BulletDecorator();
            TextBlock tipText = new TextBlock();
            tipText.Text = $"({pair.Item1},{pair.Item2})";
            bdec.Child = tipText;
            tooltip.Content = bdec;

            ellipse.ToolTip = tooltip;

            TranslateTransform translate = new TranslateTransform(16 + pair.Item1 * 10, 351 - pair.Item2 * 10);
            ellipse.RenderTransform = translate;
            board.Children.Add(ellipse);
        }

        private void AddSillouettes(Tuple<int, int> pair,ref int previousX,ref int previousY, SolidColorBrush lineColor)
        {
            Line sillouette1 = new Line()
            {
                X1 = 20 + previousX * 10,
                Y1 = 355 - previousY * 10,
                X2 = 20 + pair.Item1 * 10,
                Y2 = 355 - previousY * 10,
                Fill = lineColor,
                Stroke = lineColor,
                StrokeThickness = 2

            };
            Line sillouette2 = new Line()
            {
                X1 = pair.Item1 * 10 + 20,
                Y1 = 355 - 10 * previousY,
                X2 = pair.Item1 * 10 + 20,
                Y2 = 355 - pair.Item2 * 10,
                Fill = lineColor,
                Stroke = lineColor,
                StrokeThickness = 2
            };
            previousX = pair.Item1;
            previousY = pair.Item2;
            board.Children.Add(sillouette2);
            board.Children.Add(sillouette1);
        }

        private void RemoveButton(object sender, RoutedEventArgs e)
        {
            Label tag = inputListBox.SelectedItem as Label;
            if (tag == null) return;
            Tuple<Building, Rectangle> tuple;
            buildings.TryGetValue(tag, out tuple);
            buildings.Remove(tag);
            inputListBox.Items.Remove(tag);
            board.Children.Remove(tuple.Item2);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            inputListBox.Items.Clear();
            buildings.Clear();
            board.Children.Clear();
            elements.ForEach(x => board.Children.Add(x));
        }
    }
}
