using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Game_Life
{

    public partial class MainWindow : Window
    {
        readonly GamePlay gplay = new GamePlay();
        readonly Menu menu = new Menu();
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool pause = true;
        private bool game = false;
        private bool btstop = false;
        private bool lsCheck;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                gplay.CountNigbor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Mw_life_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    if (game)
                    {
                        bt_Start.Content = "Продолжить";
                        bt_Save.IsEnabled = game;
                        if (btstop == true)
                        {
                            pause = gplay.StartStop(pause, timer);
                        }

                        s_Menu.Visibility = (s_Menu.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!game)
                {
                    menu.Datestart = DateTime.Now;
                    game = true;
                }
                s_Menu.Visibility = (s_Menu.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btstop = !btstop;
                bt_Stop.Content = pause ? "Стоп" : "Старт";
                pause = gplay.StartStop(pause, timer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menu.CloseGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Mw_life_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = !menu.CloseGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menu.HideBt(b_Saves, bt_Start, bt_Load, bt_Save, bt_Close, bt_Back, bt_LoadSave, bt_DelateSave);
                menu.LoadInMenu(l_Saves);
                bt_LoadSave.Content = "Загрузить";
                lsCheck = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (b_Saves.Visibility == Visibility.Visible) menu.HideBt(b_Saves, bt_Start, bt_Load, bt_Save, bt_Close, bt_Back, bt_LoadSave, bt_DelateSave);
                else menu.HideBt(b_Saves, bt_Start, bt_Load, bt_Save, bt_Close, bt_Back, bt_LoadSave, bt_DelateSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menu.HideBt(b_Saves, bt_Start, bt_Load, bt_Save, bt_Close, bt_Back, bt_LoadSave, bt_DelateSave);
                menu.LoadInMenu(l_Saves);
                bt_LoadSave.Content = "Сохранить";
                lsCheck = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menu.HideBt(b_Saves, bt_Start, bt_Load, bt_Save, bt_Close, bt_Back, bt_LoadSave, bt_DelateSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Mw_life_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.IsEnabled = true;
                timer.Tick += new EventHandler(Timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
                timer.Stop();
                gplay.GenerateRectangleField(c_Field);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_AutoGen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gplay.FieldClearOrPaint(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_LoadSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lsCheck)
                {
                    menu.SaveInDb(l_Saves);
                }
                else
                {
                    menu.LoadOfDb(l_Saves);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_DeleteSave(object sender, RoutedEventArgs e)
        {
            try
            {
                menu.DelateSaves(l_Saves);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Bt_Clean_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gplay.FieldClearOrPaint(2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
