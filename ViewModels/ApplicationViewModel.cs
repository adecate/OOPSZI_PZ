using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Web_Service.Models;
using Web_Service.Helpers;
using Client_WPF.Models;
using Client_WPF.Cache;
using Client_WPF.Helpers;

namespace Client_WPF.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ApplicationViewModel()
        {
            db = new ParkingAreasdbContext();
            dbSize = db.ParkingAreas.Count();

            ParkingAreas = new ObservableCollection<ParkingAreaInfo>();

            Cache = new Caching<ParkingAreaInfo>();
            CacheList = new ObservableCollection<ParkingAreaInfo>();

            Favorites = JDeserializer<ObservableCollection<ParkingAreaInfo>>.Deser(ReadWriter<ObservableCollection<ParkingAreaInfo>>.Read())
                ?? new ObservableCollection<ParkingAreaInfo>();
        }

        private readonly ParkingAreasdbContext db;
        public int dbSize { get; private set; }
        public ObservableCollection<ParkingAreaInfo> ParkingAreas { get; set; }

        private ParkingAreaInfo selectedArea;
        public ParkingAreaInfo SelectedArea
        {
            get { return selectedArea; }
            set
            {
                if (value != null)
                {
                    if (value.Id != 0)
                    {
                        var item = Cache.GetOrCreate(value.Id.ToString(), value);

                        if (!CacheList.Contains(item))
                            CacheList.Insert(0, item);
                    }
                    else
                    {
                        CacheList.Remove(CacheList.FirstOrDefault(n => n.Id == 0));
                        CacheList.Insert(0, value);
                    }

                }

                selectedArea = value;
                OnPropertyChanged("");
            }
        }

        private Caching<ParkingAreaInfo> Cache { get; set; }
        public ObservableCollection<ParkingAreaInfo> CacheList { get; set; }

        public ObservableCollection<ParkingAreaInfo> Favorites { get; set; }

        // команда подгрузки данных в список
        private RelayCommand downCommand;
        public RelayCommand DownCommand
        {
            get
            {
                return downCommand ??
                  (downCommand = new RelayCommand(obj =>
                  {
                      int sizeList = (int)obj;

                      if (sizeList == 0)
                          sizeList = 1;

                      if (sizeList + 10 >= dbSize)
                      {
                          for (int i = sizeList; i < dbSize; i++)
                          {
                              ParkingAreas.Add(db.ParkingAreas.Include(m => m.BalanceholderPhone).Include(s => s.WorkingHours).FirstOrDefault(p => p.Id == i));
                          }
                      }
                      else
                      {
                          for (int i = sizeList; i < sizeList + 10; i++)
                          {
                              ParkingAreas.Add(db.ParkingAreas.Include(m => m.BalanceholderPhone).Include(s => s.WorkingHours).FirstOrDefault(p => p.Id == i));
                          }
                      }
                  },
                 (obj) => ParkingAreas.Count < dbSize));
            }
        }

        // команда добавления данных в избранное
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      ParkingAreaInfo elem = obj as ParkingAreaInfo;

                      if (elem != null && elem.Id != 0)
                      {
                          ReadWriter<List<ParkingAreaInfo>>.Write(new List<ParkingAreaInfo>() { elem });
                      }
                      else if (elem != null)
                      {
                          System.Windows.MessageBox.Show("Выбранный вами элемент уже находится в избранном.",
                              "Предупреждение", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                      }

                  },
                  (obj) => CacheList.Count > 0));
            }
        }

        // команда удаления данных из кэша
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      ParkingAreaInfo elem = obj as ParkingAreaInfo;

                      if (elem != null && elem.Id != 0)
                      {
                          CacheList.Remove(elem);
                          Cache.Remove(elem.Id.ToString());
                      }
                      else if (elem != null)
                          CacheList.Remove(elem);

                  },
                 (obj) => CacheList.Count > 0));
            }
        }

        // команда обновления вкладки избранное
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(obj =>
                  {
                      List<ParkingAreaInfo> temp_list = JDeserializer<List<ParkingAreaInfo>>.Deser(ReadWriter<List<ParkingAreaInfo>>.Read());

                      if (temp_list != null)
                      {
                          if (Favorites.Count == 0)
                          {
                              foreach (var item in temp_list)
                              {
                                  Favorites.Insert(0, item);
                              }
                          }
                          else
                          {
                              bool found;
                              foreach (ParkingAreaInfo itemT in temp_list)
                              {
                                  found = false;

                                  foreach (var itemD in Favorites)
                                  {
                                       if (itemD.CommonName == itemT.CommonName)
                                        found = true;
                                  }

                                  if (!found)
                                  {
                                      Favorites.Insert(0, itemT);
                                  }

                              }
                          }
                      }

                  }
                  ));
            }
        }

        // команда очистки избранного
        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                  (clearCommand = new RelayCommand(obj =>
                  {
                      ReadWriter<object>.Write(null);
                      Favorites.Clear();

                  },
                  (obj) => Favorites.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}