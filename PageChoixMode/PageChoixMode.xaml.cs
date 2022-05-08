using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Projet2Cp; 
namespace Project
{
    /// <summary>
    /// Interaction logic for PageChoixMode.xaml
    /// </summary>s
    public partial class PageChoixMode : Page
    {
        bool francais = false;
        bool modeEns;
        ResourceDictionary ResLibre= App.ArResLibre;
        Eleve student;
        public static string[] users = File.ReadAllLines("Data/users.txt");
        int studentsCount;
        ComboBox studentsCombo; 
        TextBox studentBox;
        bool combo = false;
          String stdName = null;
        public PageChoixMode()
        {
            InitializeComponent();
            this.Loaded += loaded; 
            passwd.Clear();
            passwd.HorizontalContentAlignment = HorizontalAlignment.Center;
            studentsCount = Int16.Parse(users[0]);
        }
        private void loaded(object sender , RoutedEventArgs e)
        {
            
            if (francais)
            {
                ResLibre = App.FrResLibre;
                this.Resources.MergedDictionaries.Add(App.FrResLibre);

            }
            else
            {
                ResLibre = App.ArResLibre;

                this.Resources.MergedDictionaries.Add(App.ArResLibre);

            }


                studentsCombo = (ComboBox)this.FindResource("stdCombo");
                studentsCombo.ItemsSource = users.Skip(1);  
                studentBox = (TextBox)this.FindResource("eleve");

            if ( !combo )
            {
                toggleToCombo();
               
            }
            else
            {
                toggleToBox();
               
            }
            
        }


        private void Go_Click(object sender, RoutedEventArgs e)
        {

            Go();


        }


        private void Go()
        {
           if (combo)
            {
                if (studentsCombo.SelectedIndex != -1) stdName = studentsCombo.SelectedItem.ToString();
                else stdName = String.Empty;
            }
            else stdName = studentBox.Text; 

            if (String.IsNullOrEmpty(passwd.Password)  & String.IsNullOrEmpty(stdName.Trim()))
            {
                if (francais) MessageBox.Show("Veuillez remplir l'un des champs!");
                else MessageBox.Show("يرجى ملء الخانة المناسبة");
            }
            else
            {

                if (account())
                    symmetrica.symmetricaFrm.NavigationService.Navigate(new MainWindow(modeEns, francais, student));
            }
        }

        private void Fr_Click(object sender, RoutedEventArgs e)
        {
            francais = true;
            Fr.Style = (Style)Application.Current.FindResource("ButtonCentralJaune");
     
            Ar.Style = (Style)Application.Current.FindResource("ButtonCentral");

            this.Resources.MergedDictionaries.Add(App.FrResLibre);
            ResLibre = App.FrResLibre;
            if (combo) addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];
            else  addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];



        }
        private void Ar_Click(object sender, RoutedEventArgs e)
        {
            francais = false;
            Ar.Style = (Style)Application.Current.FindResource("ButtonCentralJaune");

            Fr.Style = (Style)Application.Current.FindResource("ButtonCentral");
            this.Resources.MergedDictionaries.Add(App.ArResLibre);
            ResLibre = App.ArResLibre;
            if (combo) addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];
            else addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];

        }



        private void passwd_GotFocus(object sender, RoutedEventArgs e)
        {
            studentBox.Clear();
            studentsCombo.SelectedValue = -1;
            modeEns = true;

        }
        private void addUser(object sender, RoutedEventArgs e)
        {
            if (userNameContainer.Children.Contains(studentBox)) toggleToCombo();
            else toggleToBox();

        }
        private void eleve_GotFocus(object sender, RoutedEventArgs e)
        {
            passwd.Clear();
            modeEns = false;
        }

        private void toggleToCombo()
        {
            combo = true;
            userNameContainer.Children.Remove(studentBox);
            userNameContainer.Children.Add ( studentsCombo);
            Grid.SetColumn(studentsCombo,0);
            addUserBtn.Content = this.FindResource("addUserImg");
            addUserBtn.ToolTip= (String)ResLibre["addUserTotip"];
        }
        private void  toggleToBox() {
            combo = false;
            userNameContainer.Children.Remove(studentsCombo);
            userNameContainer.Children.Add(studentBox);
            Grid.SetColumn(studentBox, 0);
            addUserBtn.Content = this.FindResource("chooseUserImg");
            addUserBtn.ToolTip = (String)ResLibre["chooseUserTotip"];


        }



        public Boolean account()
        {
            Boolean done = true; 
            if (!modeEns)
            {
                int i;
                for (i = 0; i < users.Length; i++)
                {
                    if (stdName.Trim().Equals(users[i]))
                    {
                        break;
                    }
                }
                if (users.Contains(stdName.Trim()))
                {

                   
                    FileStream fs1 = new FileStream("Data/Users/" + stdName.Trim() + i.ToString() + "/data.bin", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs1);
                    int cent, axe ; 
                    cent = br.ReadInt32();
                    axe = br.ReadInt32();
                    student = new Eleve(i, stdName.Trim(), axe, cent); 
                    br.Close();
                    fs1.Close();

                }
                else
                {
                    string mssg = francais ? "Voulez vous créez un nouveau compte?" : "هل تريد انشاء حساب جديد ؟";

                    if (MessageBox.Show(mssg, "!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        String[] usersUp = new string[users.Length + 1];
                        users.CopyTo(usersUp, 0);
                        usersUp[usersUp.Length - 1] = stdName.Trim();
                        usersUp[0] = (Int32.Parse(users[0]) + 1).ToString();
                        File.WriteAllLines("Data/users.txt", usersUp);
                        users = usersUp;

                        student = new Eleve(i, stdName.Trim(), 0, 0);

                        String folder ="./Data/Users/" + stdName.Trim() + i.ToString();
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);

                            FileStream fs = new FileStream( folder + "/data.bin", FileMode.Create);
                            BinaryWriter bw = new BinaryWriter(fs);
                            bw.Seek(0, SeekOrigin.Begin);
                            bw.Write((int)student.getProgressCen());
                            bw.Write((int)student.getProgressAxe());

                            fs.Close();
                            bw.Close();
                        }

                    }
                    else done = false; 


                    
                }
            }
            else
            {
                string[] pwd = File.ReadAllLines("Data/password.txt");
                if ( ! pwd[0].Equals(passwd.Password))
                {
                    if ( francais ) MessageBox.Show("Mot de passe incorrecte!");
                    else MessageBox.Show("! كلمة المرور خاطئة");
                    done = false; 
                }
            }
            return done; 
        }

      
    }
}
