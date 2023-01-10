using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client
{
    partial class Client
    {
        private class MenuItem
        {
            public string Title { get; private set; }
            public Type ScreenType { get; private set; }
            public ICollection<MenuItem> Children { get; private set; }

            public MenuItem(string title, Type screenType, ICollection<MenuItem> children = null)
            {
                Title = title;
                ScreenType = screenType;
                Children = children;
            }
        }

        private static void BuildMenu()
        {
            var menuItem1 = new MenuItem("Clientes", null, new List<MenuItem>()
            {
                new MenuItem("Listar clientes", typeof(FrmCustomers)),
                new MenuItem("Cadastrar cliente", typeof(FrmAddCustomer)),
                new MenuItem("Exemplo - Submenu", null, new List<MenuItem>()
                {
                    new MenuItem("Item 1", null),
                    new MenuItem("Item 2", null),
                })
            });

            s_frmRoot.MainMenu.Items.Add(CreateToolStripMenuItem(menuItem1));
        }

        private static ToolStripMenuItem CreateToolStripMenuItem(MenuItem item)
        {
            var menu = new ToolStripMenuItem();
            menu.Text = item.Title;
            menu.Tag = item.ScreenType;

            if (item.ScreenType != null)
            {
                menu.Click += new EventHandler(Menu_Click);
            }

            if ((item.Children != null) && (item.Children.Count > 0))
            {
                foreach (MenuItem child in item.Children)
                {
                    var childMenu = CreateToolStripMenuItem(child);
                    menu.DropDownItems.Add(childMenu);
                }
            }

            return menu;
        }

        private static void Menu_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var screenType = (Type)menuItem.Tag;
            Screen.Show(screenType);
        }
    }
}
