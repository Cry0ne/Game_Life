using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game_Life
{
   

    [Table(Name = "Saves")]
    internal class Save
    {
        [Column(IsPrimaryKey = true, Name = "id")]
        public int Id_db { get; set; }
        [Column(Name = "name")]
        public string Name_db { get; set; }
        [Column(Name = "field")]
        public string Field_db { get; set; }
        [Column(Name = "datestart")]
        public DateTime Dates_db { get; set; }
        [Column(Name = "dateend")]
        public DateTime Datee_db { get; set; }
    }

    internal class Menu
    {
        private readonly GamePlay gplay = new GamePlay();
        private static readonly string con = @"Server = .\SQLEXPRESS;Initial Catalog = GameLife; Integrated Security = true";
        private string savefil;
        private static readonly DataContext db = new DataContext(con);
        private readonly Table<Save> saves = db.GetTable<Save>();

        public DateTime Datestart { get; set; }

        private string ReversStr(string str)/*Отзеркаливает строку*/
        {
            try
            {
                char[] sim = str.ToCharArray();
                Array.Reverse(sim);
                return new string(sim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void SaveInDb(ListBox obj)/*Сохранение в БД*/
        {
            try
            {
                int ido = 0;
                foreach (Save saver in saves.OrderBy(q => q.Id_db))
                {
                    ido = saver.Id_db + 1;
                }

                savefil = "";
                for (int i = 0; i < gplay.Field_h; i++)
                {
                    for (int j = 0; j < gplay.Field_w; j++)
                    {
                        if (GamePlay.Felder[i, j].Fill == Brushes.Black)
                        {
                            savefil = savefil.Insert(savefil.Length, "0");
                        }
                        else
                        {
                            savefil = savefil.Insert(savefil.Length, "1");
                        }
                    }
                }

                foreach (Save field in saves)
                {
                    if (savefil == field.Field_db)
                    {
                        MessageBox.Show("Токая расстановка уже сохранена!");
                        return;
                    }
                }

                Save save = new Save { Id_db = ido, Name_db = "Save " + ido, Field_db = savefil, Dates_db = Datestart, Datee_db = DateTime.Now };

                saves.InsertOnSubmit(save);
                db.SubmitChanges();

                ListBoxItem saveblok = new ListBoxItem
                {
                    Width = obj.Width,
                    Height = 40,
                    MaxHeight = 40,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.White,
                    FontSize = 16,
                    Content = $"{ido} Save Начало: {Datestart} Конец: {Convert.ToString(DateTime.Now)}"
                };
                obj.Items.Add(saveblok);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void LoadOfDb(ListBox obj)/*Выгрузка из БД*/
        {
            try
            {
                int nmitem = Convert.ToInt32(obj.SelectedItem.ToString().Substring(37, 2));

                string rgg = (from c in saves where c.Id_db == nmitem select c.Field_db).Single();
                string grr = ReversStr(rgg);

                for (int i = 0; i < gplay.Field_w; ++i)
                {
                    for (int j = 0; j < gplay.Field_w; ++j)
                    {
                        if (grr.Substring(grr.Length - 1, 1) == "0")
                        {
                            GamePlay.Felder[i, j].Fill = Brushes.Black;
                            grr = grr.Remove(grr.Length - 1, 1);
                        }
                        else
                        {
                            GamePlay.Felder[i, j].Fill = Brushes.Lime;
                            grr = grr.Remove(grr.Length - 1, 1);
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

        public void DelateSaves(ListBox obj)/*Удаление сохранения*/
        {
            try
            {
                int nmitem = Convert.ToInt32(obj.SelectedItem.ToString().Substring(37, 2));

                Save save = (from c in saves where c.Id_db == nmitem select c).Single();
                db.GetTable<Save>().DeleteOnSubmit(save);
                db.SubmitChanges();
                obj.Items.Remove(obj.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void LoadInMenu(ListBox obj)/*Заполнение блоков сохранений при запуска программы*/
        {
            try
            {
                obj.Items.Clear();
                foreach (Save saver in saves.OrderBy(u => u.Id_db))
                {
                    ListBoxItem saveblok = new ListBoxItem
                    {
                        Width = obj.Width,
                        Height = 40,
                        MaxHeight = 40,
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.White,
                        FontSize = 16,

                        Content = $"{saver.Id_db} Save Начало: {saver.Dates_db} Конец: {saver.Datee_db}"
                    };
                    obj.Items.Add(saveblok);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public bool CloseGame()/*Запрашивает подтверждение закрытия программы*/
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Выйти без сохранения?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public void HideBt(Border b_Saves, Button bt_Start, Button bt_Load, Button bt_Save, Button bt_Close, Button bt_Back, Button bt_LoadSave, Button bt_DelateSave)/*Скрывает/показывает меню*/
        {
            try
            {
                b_Saves.Visibility = (b_Saves.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_Start.Visibility = (bt_Start.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_Load.Visibility = (bt_Load.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_Save.Visibility = (bt_Save.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_Close.Visibility = (bt_Close.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_Back.Visibility = (bt_Back.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_LoadSave.Visibility = (bt_LoadSave.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
                bt_DelateSave.Visibility = (bt_DelateSave.Visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
