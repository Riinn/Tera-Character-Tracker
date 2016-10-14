﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tera
{
    /// <summary>
    /// Logica di interazione per leftSlide.xaml
    /// </summary>
    public partial class leftSlide : UserControl
    {
        public leftSlide()
        {
            InitializeComponent();
        }
        public void rowHighlight(object sender, MouseEventArgs e)
        {
            var s = sender as Grid;
            var an = new ColorAnimation();
            an.From = Color.FromArgb(0, 0, 0, 0);
            an.To = Color.FromArgb(30, 155, 155, 155);
            an.Duration = TimeSpan.FromMilliseconds(25);
            s.Background.BeginAnimation(SolidColorBrush.ColorProperty, an);
          
        }
        private void rowNormal(object sender, MouseEventArgs e)
        {
            var s = sender as Grid;
            var an = new ColorAnimation();
            an.From = Color.FromArgb(30, 155, 155, 155);
            an.To = Color.FromArgb(0, 0, 0, 0);
            an.Duration = TimeSpan.FromMilliseconds(75);
            s.Background.BeginAnimation(SolidColorBrush.ColorProperty, an);
        }
        private void tabSelected(object sender, MouseButtonEventArgs e)
        {

            string x = (sender as Grid).Tag.ToString().ToLower();
            TeraMainWindow w = (TeraMainWindow)TeraMainWindow.GetWindow(this);
           // w.switchTab(x);
           
            /*close after choice*/
            ThicknessAnimationUsingKeyFrames an = new ThicknessAnimationUsingKeyFrames();
            an.KeyFrames.Add(new SplineThicknessKeyFrame(new Thickness(-420, 0, 0, 0), TimeSpan.FromMilliseconds(220), new KeySpline(.5, 0, .3, 1)));
            TeraMainWindow.leftSlideIsOpen = false;
            this.BeginAnimation(MarginProperty, an);


        }
        private void tc_switch_changed(object sender, MouseButtonEventArgs e)
        {
            var tcOn = new ThicknessAnimationUsingKeyFrames();
            var tcOff = new ThicknessAnimationUsingKeyFrames();
            var tcOnFill = new ColorAnimation(System.Windows.Media.Color.FromArgb(255, 255, 255, 255), System.Windows.Media.Color.FromArgb(255, 255, 120, 42), TimeSpan.FromMilliseconds(150));
            var tcOffFill = new ColorAnimation(System.Windows.Media.Color.FromArgb(255, 255, 120, 42), System.Windows.Media.Color.FromArgb(255, 255, 255, 255), TimeSpan.FromMilliseconds(150));
            var tcOnBackFill = new ColorAnimation(System.Windows.Media.Color.FromArgb(25, 0, 0, 0), System.Windows.Media.Color.FromArgb(100, 255, 120, 42), TimeSpan.FromMilliseconds(150));
            var tcOffBackFill = new ColorAnimation(System.Windows.Media.Color.FromArgb(100, 255, 120, 42), System.Windows.Media.Color.FromArgb(25, 0, 0, 0), TimeSpan.FromMilliseconds(150));

            tcOn.KeyFrames.Add(new SplineThicknessKeyFrame(new Thickness(20, 0, 0, 0), TimeSpan.FromMilliseconds(220), new KeySpline(.5, 0, .3, 1)));
            tcOff.KeyFrames.Add(new SplineThicknessKeyFrame(new Thickness(-20, 0, 0, 0), TimeSpan.FromMilliseconds(220), new KeySpline(.5, 0, .3, 1)));
            
            tcOff.Completed += (x, ev) =>
            {
                //showRestartDiag();

            };
            tcOn.Completed += (x, ev) =>
            {
                //showRestartDiag();

            };
            
            if (Properties.Settings.Default.TeraClub)
            {
                Properties.Settings.Default.TeraClub = false;
                Properties.Settings.Default.Save();
                tc_switch.BeginAnimation(MarginProperty, tcOff);
                tc_switch.Fill.BeginAnimation(SolidColorBrush.ColorProperty, tcOffFill);
                tc_switch_back.Fill.BeginAnimation(SolidColorBrush.ColorProperty, tcOffBackFill);


            }
            else
            {
                Properties.Settings.Default.TeraClub = true;
                Properties.Settings.Default.Save();
                tc_switch.BeginAnimation(MarginProperty, tcOn);
                tc_switch.Fill.BeginAnimation(SolidColorBrush.ColorProperty, tcOnFill);
                tc_switch_back.Fill.BeginAnimation(SolidColorBrush.ColorProperty, tcOnBackFill);
            }

            /* 
            * reload dg controls bindings
            */


        }
        private void showRestartDiag()
        {
            TeraMainWindow d = new TeraMainWindow();
            d.Content = new restartDiag();
            
            var parent = VisualTreeHelper.GetParent(this);
            while (!(parent is TeraMainWindow))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
             var currentMainWindow = (parent as TeraMainWindow);

            d.Owner = currentMainWindow;
            d.WindowStyle = WindowStyle.None;
            d.AllowsTransparency = true;
            d.SizeToContent = SizeToContent.WidthAndHeight;
            d.Background = null;
            d.Topmost = true;
            d.ShowInTaskbar = false;
            d.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var c = new System.Windows.Shapes.Rectangle();
            c.HorizontalAlignment = HorizontalAlignment.Stretch;
            c.VerticalAlignment = VerticalAlignment.Stretch;
            c.Opacity = 0;
            c.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
            Grid.SetRowSpan(c, (d.Owner as TeraMainWindow).MainGrid.RowDefinitions.Count);

            (d.Owner as TeraMainWindow).MainGrid.Children.Add(c);
            var an = new DoubleAnimation(.3, TimeSpan.FromMilliseconds(200));
            var an2 = new DoubleAnimation(1, TimeSpan.FromMilliseconds(200));
            d.Opacity = 0;
            d.Show();
            d.BeginAnimation(OpacityProperty, an2);
            c.BeginAnimation(OpacityProperty, an);

            currentMainWindow.IsEnabled = false;
        }
        private void resetDungs()
        {
            foreach (var c in TeraLogic.CharList)
            {
                foreach (var d in c.Dungeons)
                {
                    d.Runs = 0;
                }
            }
        }
        private void reloadDgCtrl()
        {
            TeraMainWindow w;

                w = Application.Current.MainWindow as TeraMainWindow;
                var s = w.FindName("dgPage") as UserControl;
                var p = s.FindName("dungeonTableGridContent") as StackPanel;
                p.Children.Clear();
                TeraMainWindow.DungStrips.Clear();
                for (int i = 0; i < TeraLogic.CharList.Count; i++)
                {
                    TeraMainWindow.createDungStrips(i);
                }
            
            
        }
        private void animateSwitch()
        {
               
        }

        private void dropFeedback(object sender, MouseButtonEventArgs e)
        {
            var g = (Grid)sender;
            if (g.Children.Contains(TeraMainWindow.FindChild<Ellipse>(g, "droplet")))
            {
                g.Children.Remove(TeraMainWindow.FindChild<Ellipse>(g, "droplet"));
            }
            var el = new Ellipse();
            el.Name = "droplet";
            el.BeginAnimation(WidthProperty, null);
            el.BeginAnimation(HeightProperty, null);
            el.BeginAnimation(MarginProperty, null);
            el.BeginAnimation(OpacityProperty, null);
            el.HorizontalAlignment = HorizontalAlignment.Left;
            el.VerticalAlignment = VerticalAlignment.Top;
            el.Fill = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            el.Opacity = 1;
            Grid.SetColumnSpan(el, g.ColumnDefinitions.Count);

            g.Children.Add(el);


            var p = Mouse.GetPosition(g);
            var animTime = 450;
            el.Width = 1;
            el.Height = 1;
            el.Margin = new Thickness(p.X,p.Y,0,0);

            

            double x = 500;
            var sizeAn = new DoubleAnimationUsingKeyFrames();
            sizeAn.KeyFrames.Add(new SplineDoubleKeyFrame(x, TimeSpan.FromMilliseconds(animTime*1.2), new KeySpline(.5, 0, .3, 1)));
            sizeAn.AutoReverse = true;

            sizeAn.Completed += (a, b) =>
            {
                g.Children.Remove(el);

            };

            var marginAn = new ThicknessAnimationUsingKeyFrames();
            marginAn.KeyFrames.Add(new SplineThicknessKeyFrame(new Thickness(p.X-(x/2),p.Y-(x/2),0,0), TimeSpan.FromMilliseconds(animTime*1.1), new KeySpline(.5, 0, .3, 1)));
            marginAn.AutoReverse = true;

            var opacityAn = new DoubleAnimationUsingKeyFrames();
            opacityAn.KeyFrames.Add(new SplineDoubleKeyFrame(.1, TimeSpan.FromMilliseconds(animTime*.1), new KeySpline(.5, 0, .3, 1)));
            opacityAn.KeyFrames.Add(new SplineDoubleKeyFrame(0, TimeSpan.FromMilliseconds(animTime*.9), new KeySpline(.5, 0, .3, 1)));


            el.BeginAnimation(WidthProperty, sizeAn);
            el.BeginAnimation(HeightProperty, sizeAn);
            el.BeginAnimation(MarginProperty, marginAn);
            el.BeginAnimation(OpacityProperty, opacityAn);

        }

        
    }
    
}