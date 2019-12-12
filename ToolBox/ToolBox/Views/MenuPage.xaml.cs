using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToolBox.Core.Domain;

namespace ToolBox.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        List<HomeMenuItem> menuItems = new List<HomeMenuItem>()
        {
             new HomeMenuItem {Id = MenuItemType.Bus, Title="抽屉" },
             new HomeMenuItem {Id = MenuItemType.Book, Title="书架" }
        };

        public MenuPage()
        {
            InitializeComponent();

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var menuItem = (HomeMenuItem)e.SelectedItem;
                var id = (int)menuItem.Id;
                menuItem.Icon = $"{menuItem.Id.ToString().ToLower()}_selection.png";
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}