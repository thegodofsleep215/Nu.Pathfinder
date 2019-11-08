using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nu.OfficerMiniGame.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IShipDal shipDal;
        List<ShipListItem> ships = new List<ShipListItem>();

        Ship activeShip = null;

        public MainWindow()
        {
            shipDal = new FileBasedShipDal();
            InitializeComponent();
            lvShipList.ItemsSource = ships;
            LoadShips();
        }

        #region Ships View
        private void LoadShips()
        {
            ships.Clear();
            shipDal.GetAll().ForEach(x => ships.Add(new ShipListItem { Name = x.CrewName }));
        }
        private void lvShipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvShipList.SelectedValue is ShipListItem sli)
            {
                activeShip = shipDal.Get(sli.Name);
                if (activeShip != null)
                {
                    lvCrewList.ItemsSource = activeShip.ShipsCrew;
                    icShipStats.ItemsSource = new[] { activeShip };
                }
            }
        }

        #endregion
    }
}

