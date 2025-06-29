using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace auta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadXml(object sender, RoutedEventArgs e)
        {
            List<Car> cars = GetCars();
            DisplayCars(cars);
        }

        //private void CreateXml()
        //{
        //    var cars = new List<Car>();
        //    cars.Add(new Car() { Model = "Škoda Oktávia", Price = 500000, SellDate = new DateOnly(2010,12,2), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Felicia", Price = 210000, SellDate = new DateOnly(2000, 12, 3), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Fabia", Price = 350000, SellDate = new DateOnly(2010, 12, 4), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Oktávia", Price = 500000, SellDate = new DateOnly(2010, 12, 4), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Oktávia", Price = 500000, SellDate = new DateOnly(2010, 12, 5), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Fabia", Price = 350000, SellDate = new DateOnly(2010, 12, 5), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Fabia", Price = 350000, SellDate = new DateOnly(2010, 12, 6), Vat = 20 });
        //    cars.Add(new Car() { Model = "Škoda Forman", Price = 100000, SellDate = new DateOnly(2000, 12, 4), Vat = 19 });
        //    cars.Add(new Car() { Model = "Škoda Favorit", Price = 80000, SellDate = new DateOnly(2000, 12, 5), Vat = 19 });
        //    cars.Add(new Car() { Model = "Škoda Forman", Price = 100000, SellDate = new DateOnly(2000, 12, 6), Vat = 19 });
        //    cars.Add(new Car() { Model = "Škoda Felicia", Price = 210000, SellDate = new DateOnly(2000, 12, 3), Vat = 19 });
        //    cars.Add(new Car() { Model = "Škoda Felicia", Price = 210000, SellDate = new DateOnly(2000, 12, 2), Vat = 19 });
        //    cars.Add(new Car() { Model = "Škoda Oktávia", Price = 500000, SellDate = new DateOnly(2010, 12, 7), Vat = 20 });
        //    string path = "C:\\Users\\SPRAVCE\\Documents\\cars.xml"; 
        //    if (!File.Exists(path))
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(cars.GetType());

        //        using (StringWriter textWriter = new StringWriter())
        //        {
        //            xmlSerializer.Serialize(textWriter, cars);
        //            File.WriteAllText(path, textWriter.ToString());
        //        }
        //    }
        //}

        private List<Car> GetCars() 
        {
            //CreateXml();
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Access files (*.xml)|*.xml|Old Access files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            openFileDialog.ShowDialog();

            var path = openFileDialog.FileName;
            if (File.Exists(path))
            {
                string readText = File.ReadAllText(path);
                XmlSerializer ser = new XmlSerializer(typeof(List<Car>));

                using (StringReader sr = new StringReader(readText))
                {
                    return (List<Car>) (ser.Deserialize(sr) ?? new List<Car>());
                }
            }

            return new List<Car>();
        }

        private void DisplayCars(List<Car> cars)
        {
            CarGrid.Children.Clear();
            CarGrid.RowDefinitions.Clear();

            List<CarViewModel> carViewModels = CarsToViewModel(cars);
            int rowCounter = 1;

            if (cars.Count == 0)
            {
                return;
            }

            DisplayCarCell("Název modelu", "Cena bez DPH", "Cena s DPH", 0, Brushes.LightGray);

            foreach (CarViewModel carViewModel in carViewModels)
            {
                DisplayCarCell(carViewModel.Model, string.Format("{0:N2}", carViewModel.Price), string.Format("{0:N2}", carViewModel.Vat), rowCounter, Brushes.White);

                rowCounter++;
            }
        }

        private void DisplayCarCell (string model, string price, string vat, int row, SolidColorBrush background)
        {
            CarGrid.RowDefinitions.Add(new RowDefinition());
            var headerBorder = new Border();
            headerBorder.BorderBrush = Brushes.Black;
            headerBorder.BorderThickness = new Thickness(1);
            headerBorder.Background = background;
            Grid.SetRow(headerBorder, row);
            Grid.SetColumn(headerBorder, 0);
            CarGrid.Children.Add(headerBorder);

            Grid headerModelGrid = new Grid();
            Grid.SetRow(headerModelGrid, row);
            Grid.SetColumn(headerModelGrid, 0);
            headerModelGrid.Visibility = Visibility.Visible;
            CarGrid.Children.Add(headerModelGrid);
            headerModelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            headerModelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            headerModelGrid.RowDefinitions.Add(new RowDefinition());
            headerModelGrid.RowDefinitions.Add(new RowDefinition());

            var headerModel = new Label();
            headerModel.Content = model; 
            Grid.SetRow(headerModel, 0);
            Grid.SetColumn(headerModel, 0);
            Grid.SetRowSpan(headerModel, 1);
            headerModelGrid.Children.Add(headerModel);

            var headerPrice = new Label();
            headerPrice.Content = price; 
            Grid.SetRow(headerPrice, 1);
            Grid.SetColumn(headerPrice, 0);
            headerPrice.HorizontalContentAlignment = HorizontalAlignment.Left;
            headerModelGrid.Children.Add(headerPrice);

            var headerVat = new Label();
            headerVat.Content = vat; 
            Grid.SetRow(headerVat, 1);
            Grid.SetColumn(headerVat, 1);
            headerVat.HorizontalContentAlignment = HorizontalAlignment.Right;
            headerModelGrid.Children.Add(headerVat);
        }


        private List<CarViewModel> CarsToViewModel(List<Car> cars) 
        {
            var ret = new List<CarViewModel>();
            foreach (var car in cars) 
            {
                var carViewModel = ret.Where(c => c.Model == car.Model).SingleOrDefault();
                if (carViewModel != null)
                {
                    carViewModel.Price += car.Price;
                    carViewModel.Vat += PriceWithVat(car.Price, car.Vat);
                }
                else {
                    ret.Add(new CarViewModel() { Model = car.Model, Price = car.Price, Vat = PriceWithVat(car.Price, car.Vat) });
                }
            }
            return ret;
        }

        private double PriceWithVat(double price, double vat) 
        {
            return (vat / 100 + 1) * price;
        }
    }
}