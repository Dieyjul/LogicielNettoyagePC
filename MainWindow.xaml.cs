using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string version = "1.0.0";
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\Temp");
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
            CheckActu();
            GetDate();
        }

        // affichage mise à jour 
        public void CheckVersion()
        {
            string url = " http://localhost/siteweb/version.txt";
            using (WebClient client = new WebClient())
            {
                string v = client.DownloadString(url);
                if (version != v)
                {
                    MessageBox.Show("Une mise à jour est dispo !", "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Votre logiciel est à jour !", "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }


        // affichage actualité
        public void CheckActu()
        {
            string url = " http://localhost/siteweb/actu.txt";
            using (WebClient client = new WebClient())
            {
                string actu = client.DownloadString(url);
                if (actu != String.Empty)
                {
                    actuTxt.Content = actu;
                    actuTxt.Visibility = Visibility.Visible;
                    bandeau.Visibility = Visibility.Visible;
                }
            }
            
        }

        // Calcul de la taille d'un dossier 

        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }

        // Vider un dossier

        public void ClearTempData(DirectoryInfo di)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                    // totalRemovedfiles++;

                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                    Console.WriteLine(dir.FullName);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
        }
        /*void button_Click(object sender, RoutedEventArgs e)
        {
            // Show message box when button is clicked.
            MessageBox.Show("Hello, Windows Presentation Foundation!");
        }*/

        private void Button_ANAL_click(object sender, RoutedEventArgs e)
        {

            AnalyseFolders();
        }

        private void Button_NET_click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Nettoyage en cours...");
            btnClean.Content = "NETTOYAGE EN COURS";
            Clipboard.Clear();
            try
            {
                ClearTempData(winTemp);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Erreur : " +  ex.Message);
            }

            try
            {
                ClearTempData(appTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            btnClean.Content = "NETTOYAGE TERMINE";
            Titre.Content = "Nettoyage effectué";
            espace.Content = "0 Mb";

        }

        private void Button_HIS_click(object sender, RoutedEventArgs e)
        {


        }

        private void Button_MAJ_click(object sender, RoutedEventArgs e)
        {
            CheckVersion();


        }

        private void Button_WEB_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://www.udemy.com/course/apprendre-csharp-et-le-developpement-de-logiciels-avec-wpf-net-core/learn/lecture/20387103#overview")
                {
                    UseShellExecute = true
                });  
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur" + ex.Message);
            }

        }

        public void AnalyseFolders()
        {
            Console.WriteLine("Début de l'analyse...");
            long totalSize = 0;
            try
            {
                totalSize += DirSize(winTemp) / 1000000;
                totalSize += DirSize(appTemp) / 1000000;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Impossible d'analyser les dossiers : " + ex.Message);
            }
            

            espace.Content = totalSize + " Mb";
            Titre.Content = "Analyse effectué";
            date.Content = DateTime.Today;
            SaveDate();
        }

        //Stockage du date dans un fichier date.txt
        public void SaveDate()
        {
            string date = DateTime.Today.ToString();
            File.WriteAllText("date.txt", date);
        }

        public void GetDate()
        {
            string dateFichier = File.ReadAllText("date.txt");
            if (dateFichier != String.Empty)
            {
                date.Content = dateFichier;
            }
        }


    }
}
