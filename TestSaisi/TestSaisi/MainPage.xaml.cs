using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TestSaisi.Resources;
using System.Windows.Media.Imaging;

namespace TestSaisi
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructeur
        List<string> test = new List<string>() ;
        public MainPage()
        {
            
            InitializeComponent();
            //listBox.ItemsSource = test;
            
        }
        
        private void textChange(object sender, TextChangedEventArgs e)
        {
            if (numserie.Text.Length == 13)
            {
                test.Add(numserie.Text);
                /*listBox.ItemsSource = null;
                listBox.ItemsSource = test;*/
                

                
                listBox.Items.Add(numserie.Text);
                listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
                numserie.Text = "";
            }
        }
        private void cliclproc(object sender, RoutedEventArgs e )
        {
            Button button = sender as Button;
            listBox.Items.Remove(button.DataContext.ToString());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string Items = "" ;
            foreach (string item in listBox.Items)
            {
                Items+=item+";"; // /n to print each item on new line or you omit /n to print text on same line
            }
            ServiceReference1.CommandeServiceClient ser = new ServiceReference1.CommandeServiceClient();
            ser.InsertNumSerieAsync(Items, emplacement.Text);
            ser.InsertNumSerieCompleted += client_InserNumSerie;
        }
        void client_InserNumSerie(object sender, ServiceReference1.InsertNumSerieCompletedEventArgs e)
        {
            if (e.Error == null)
            {
               MessageBox.Show(e.Result +" Lignes insérées avec succés");
            }
            else
                MessageBox.Show(e.Error.ToString());
        }


        /*  private void button_Click(object sender, RoutedEventArgs e)
 {
     string[] txt = tbMultiLine.Text.Split('\r');
     foreach (var t in txt)
        MessageBox.Show(t+"hhhhhh");


 }*/



        // Exemple de code pour la conception d'une ApplicationBar localisée
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}