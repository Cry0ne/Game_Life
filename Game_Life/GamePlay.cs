using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game_Life
{
    internal class GamePlay : Window
    {
        private const int field_h = 60;
        private const int field_w = 60;
        private readonly Random rand = new Random();
        
        public int Field_w
        {
            get
            {
                return field_w;
            }
        }
        public int Field_h
        {
            get
            {
                return field_h;
            }
        }

        public static Rectangle[,] Felder { get; set; } = new Rectangle[field_h, field_w];

        private void Draw_Mouse(object sender, MouseEventArgs e) /*Меняет цвет прямоугольника над которой ноходится курсор*/
        {
            try
            {
                if (sender.Equals(sender))
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        ((Rectangle)sender).Fill = Brushes.Lime;
                    }
                    else if (e.RightButton == MouseButtonState.Pressed)
                    {
                        ((Rectangle)sender).Fill = Brushes.Black;
                    }
                    else ((Rectangle)sender).Fill = ((Rectangle)sender).Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void CountNigbor() /*Рассчитывает ближайших соседий*/
        {
            try
            {
                int[,] cn = new int[Field_h, Field_w];

                for (int i = 0; i < Field_h; i++)
                {
                    for (int j = 0; j < Field_w; j++)
                    {
                        int iUp = i - 1;
                        if (iUp < 0) iUp = Field_h - 1;
                        int iUnd = i + 1;
                        if (iUnd >= Field_h) iUnd = 0;
                        int jLeft = j - 1;
                        if (jLeft < 0) jLeft = Field_w - 1;
                        int jRight = j + 1;
                        if (jRight >= Field_w) jRight = 0;

                        int neighbor = 0;

                        if (Felder[iUp, jLeft].Fill == Brushes.Lime) neighbor++;
                        if (Felder[iUp, j].Fill == Brushes.Lime) neighbor++;
                        if (Felder[iUp, jRight].Fill == Brushes.Lime) neighbor++;
                        if (Felder[i, jLeft].Fill == Brushes.Lime) neighbor++;
                        if (Felder[i, jRight].Fill == Brushes.Lime) neighbor++;
                        if (Felder[iUnd, jLeft].Fill == Brushes.Lime) neighbor++;
                        if (Felder[iUnd, j].Fill == Brushes.Lime) neighbor++;
                        if (Felder[iUnd, jRight].Fill == Brushes.Lime) neighbor++;

                        cn[i, j] = neighbor;
                    }
                }

                for (int i = 0; i < field_h; i++)
                {
                    for (int j = 0; j < field_w; j++)
                    {
                        if (cn[i, j] < 2 || cn[i, j] > 3)
                        {
                            Felder[i, j].Fill = Brushes.Black;
                        }
                        else if (cn[i, j] == 3)
                        {
                            Felder[i, j].Fill = Brushes.Lime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void GenerateRectangleField(Canvas canv) /*Генерирует поле из прямоугольников*/
        {
            try
            {
                canv.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                canv.Arrange(new Rect(0.0, 0.0, canv.DesiredSize.Width, canv.DesiredSize.Height));

                for (int i = 0; i < field_h; i++)
                {
                    for (int j = 0; j < field_w; j++)
                    {
                        Rectangle r = new Rectangle
                        {
                            Width = canv.ActualWidth / field_w,
                            Height = canv.ActualHeight / field_h,
                            Fill = Brushes.Black
                        };
                        canv.Children.Add(r);
                        Canvas.SetLeft(r, j * canv.ActualWidth / field_w);
                        Canvas.SetTop(r, i * canv.ActualHeight / field_h);
                        r.MouseMove += Draw_Mouse;

                        Felder[i, j] = r;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        ///<summary>
        ///1 - Раскрашивает поле, рандомно;
        ///2 - Отчищает поле.
        ///</summary>
        public void FieldClearOrPaint(int param) /*1 - Раскрашивает поле, рандомно; 2 - Отчищает поле*/
        {
            try
            {
                for (int i = 0; i < field_h; i++)
                {
                    for (int j = 0; j < field_w; j++)
                    {
                        if (param == 1)
                        {
                            Felder[i, j].Fill = (rand.Next(0, 2) == 1) ? Brushes.Black : Brushes.Lime;
                        }
                        else if (param == 2)
                        {
                            Felder[i, j].Fill = Brushes.Black;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        
        public bool StartStop(bool pause, DispatcherTimer timer)/*Включает/отключает таймер*/
        {
            try
            {
                if (pause)
                {
                    timer.Start();
                    pause = false;
                }
                else
                {
                    timer.Stop();
                    pause = true;
                }
                return pause;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
