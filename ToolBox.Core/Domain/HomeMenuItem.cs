using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Core.Domain
{
    public enum MenuItemType
    {
        Bus,
        Book
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string IconDefault=> $"{Id.ToString().ToLower()}_unselected.png";
    }
}
