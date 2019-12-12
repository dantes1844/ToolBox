using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ToolBox.Models;
using ToolBox.Views;

using DoubanJiang.Bus;
using Android.Widget;

namespace ToolBox.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public IBusAppService BusAppService => DependencyService.Get<IBusAppService>();

        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await BusAppService.GetBusListAsync();
                foreach (var item in items)
                {
                    Items.Add(new Item { Description = item.Value, Text = item.Key });
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.Message, ToastLength.Long).Show();

                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}