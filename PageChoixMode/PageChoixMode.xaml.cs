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
    
    public partial class PageChoixMode : Page
    {
        public static bool francais = false;
        public static bool ChangedModeEns = false;
        bool modeEns;

        bool ancientPwd = true, modifyingPwd = false;
        bool oldMode; 
        ResourceDictionary ResLibre= App.ArResLibre;
        Eleve student;

        public static MainWindow mainWin;
        public static string[] users =  File.ReadAllLines("Data/users.txt");
        public static List<String> userCombo ; 

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
        }
        private void loaded(object sender , RoutedEventArgs e)
        {
            oldMode = modeEns;


            updateUserCombo();
            


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
                studentsCombo.ItemsSource = userCombo;  
                studentBox = (TextBox)this.FindResource("eleve");

            if ( combo )
            {
                toggleToCombo();
               
            }
            else
            {
                toggleToBox();
               
            }
            if (pwdButtons.Child != null)
            {
                if (!pwdButtons.Child.Equals(this.FindResource("modifyPwd"))) toggleToModifyPwd();
            }else
            {
                toggleToModifyPwd();
            }
            
        }


        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (modeEns != oldMode) ChangedModeEns = true;
            Go();


        }


        private void Go()
        {
            if (!modifyingPwd) {
                if (combo)
                {
                    if (studentsCombo.SelectedIndex != -1) stdName = studentsCombo.SelectedItem.ToString();
                    else stdName = String.Empty;
                }
                else { stdName = studentBox.Text;
                    if (!System.Text.RegularExpressions.Regex.IsMatch(stdName, "^[a-zA-Z0-9]*$"))
                    {
                        if ( francais )MessageBox.Show("Le nom peut contenir que des caractères alphanumeriques");
                        else MessageBox.Show("    يمكن للاسم أن يحتوي  على أحرف أبجدية  أو أرقام فقط");
                        return;
                    }
                   

                }

            if (String.IsNullOrEmpty(passwd.Password) & String.IsNullOrEmpty(stdName.Trim()))
            {
                if (francais) MessageBox.Show("Veuillez remplir l'un des champs!");
                else MessageBox.Show("يرجى ملء الخانة المناسبة");
            }
            else
            {

                    if (account())
                    {
                        
                        if ( mainWin == null ) mainWin = new MainWindow(modeEns, francais, student);
                       

                        MainWindow.eleve = student;
                        mainWin.showmodecours();
                        symmetrica.symmetricaFrm.NavigationService.Navigate(mainWin);
                    }
            }
        }
        }

        private void Fr_Click(object sender, RoutedEventArgs e)
        {
            if (mainWin != null) MainWindow.francais = francais = true;
            else francais = true;

            Fr.Style = (Style)Application.Current.FindResource("ButtonCentralJaune");
     
            Ar.Style = (Style)Application.Current.FindResource("ButtonCentral");

            this.Resources.MergedDictionaries.Add(App.FrResLibre);
            ResLibre = App.FrResLibre;
            if (combo) addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];
            else  addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];




        }
        private void Ar_Click(object sender, RoutedEventArgs e)
        {

            if (mainWin != null) MainWindow.francais = francais = false;

            else francais = false;
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

           
            if (mainWin != null) MainWindow.modeEns = modeEns = true;
            else modeEns = true;

        }

        private void modifPwd(object sender, RoutedEventArgs e)
        {
            pwdButtons.Child=(StackPanel) this.FindResource("confCancel");
            pwdText.SetResourceReference(TextBlock.TextProperty, "ancPwd");

            pwdText.Foreground = Brushes.Red;
            modifyingPwd = true;

            


        }
        private void confirmerPwd(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(passwd.Password))
            {
                if (ancientPwd)
                {
                    String[] pwd = File.ReadAllLines("Data/password.txt");
                    if (passwd.Password == pwd[0])
                    {


                        pwdText.SetResourceReference(TextBlock.TextProperty, "nouvPwd");

                        passwd.Clear();
                        ancientPwd = false;
                    }
                    else
                    {
                        if (francais) MessageBox.Show("Mot de passe incorrecte!");
                        else MessageBox.Show("! كلمة المرور خاطئة");

                    }

                }
                else
                {
                    string[] pwd = { passwd.Password };
                    File.WriteAllLines("Data/password.txt", pwd);
                    toggleToModifyPwd();
                    if (francais) MessageBox.Show("Mot de passe modifié!");
                    else MessageBox.Show("! تم تغيير كلمة المرور");


                }
            }



        }
        private void annulerPwd(object sender, RoutedEventArgs e)
        {

            toggleToModifyPwd();




        }


        private void addUser(object sender, RoutedEventArgs e)
        {
            if (userNameContainer.Children.Contains(studentBox)) toggleToCombo();
            else toggleToBox();

        }

     

        private void delUser(object sender, RoutedEventArgs e)
        {
            if (combo)
            {
                if (studentsCombo.SelectedIndex != -1) stdName = studentsCombo.SelectedItem.ToString();
                else stdName = String.Empty;
            }
            else stdName = studentBox.Text;
            if (users.Length != 0 && stdName!= null)
            {
                if (stdName.Trim().Length != 0)
                {

                    if (combo)
                    {
                        if (studentsCombo.SelectedIndex != -1) stdName = studentsCombo.SelectedItem.ToString();
                        else stdName = String.Empty;
                    }
                    else stdName = studentBox.Text;

                    int i;
                    bool contains = false;
                    for (i = 0; i < users.Length; i++)
                    {
                        if (users[i].Contains(stdName.Trim()))
                        {
                            contains = true;
                            break;
                        }
                    }



                    if (!contains)
                    {
                        if (MainWindow.francais) MessageBox.Show("Ce compte n'existe pas");
                        else MessageBox.Show("هذا الحساب غير موجود");
                    }
                    else
                    {
                        string mssg = francais ? "Voulez vous supprimer ce compte?" : "هل تريد حذف هذا الحساب  ؟";

                        if (MessageBox.Show(mssg, "!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            string[] tmp = users[i].Split(';');
                            users = users.Where(w => w != users[i]).ToArray();
                            File.WriteAllText("Data/users.txt", string.Empty);
                            File.WriteAllLines("Data/users.txt", users);

                            i = Int32.Parse(tmp[1]);
                            updateUserCombo();
                            studentsCombo.ItemsSource = userCombo;
                            studentsCombo.SelectedIndex = -1;
                            String folder = "./Data/Users/" + tmp[0] + i.ToString();
                            try { Directory.Delete(folder, true); }
                            catch (Exception exe)
                            {
                                MessageBox.Show(exe.Message);
                            }
                        }

                    }
                }
            }
        }


        private void eleve_GotFocus(object sender, RoutedEventArgs e)
        {
            passwd.Clear();
            if (mainWin != null) MainWindow.modeEns = modeEns = false;
            else modeEns = false;

            if (modifyingPwd) { toggleToModifyPwd();  }
        }


        private void toggleToModifyPwd()
        {
            pwdText.SetResourceReference(TextBlock.TextProperty, "pwd");

            pwdText.Foreground = Brushes.Black;
            ancientPwd = true;
            modifyingPwd = false;
            passwd.Clear();
            pwdButtons.Child = (Button)this.FindResource("modifyPwd");
        }

        private void toggleToCombo()
        {
            combo = true;
            if (!userNameContainer.Children.Contains(studentsCombo))
            {


                userNameContainer.Children.Remove(studentBox);
                userNameContainer.Children.Add(studentsCombo);
                Grid.SetColumn(studentsCombo, 0);
                addUserBtn.Content = this.FindResource("addUserImg");
                addUserBtn.ToolTip = (String)ResLibre["addUserTotip"];
            }
        }
        private void  toggleToBox() {
            if (!userNameContainer.Children.Contains(studentBox))
            {
                combo = false;
                userNameContainer.Children.Remove(studentsCombo);
                userNameContainer.Children.Add(studentBox);
                Grid.SetColumn(studentBox, 0);
                addUserBtn.Content = this.FindResource("chooseUserImg");
                addUserBtn.ToolTip = (String)ResLibre["chooseUserTotip"];
            }


        }

        private  void updateUserCombo()
        {
            userCombo = users.ToList();
            for (int i = 0; i < userCombo.Count; i++)
            {
                userCombo[i] = userCombo[i].Substring(0, userCombo[i].Length - 2);
            }
        }

        public Boolean account()
        {
            Boolean done = true; 

            if (!modeEns)
            {
                if (combo)
                {
                    if (studentsCombo.SelectedIndex != -1) stdName = studentsCombo.SelectedItem.ToString();
                    else stdName = String.Empty;
                    if (stdName != null && student != null)
                    {
                        if (!String.Equals(stdName.Trim(), student.getNom()))
                        {
                            ChangedModeEns = true;

                        }
                    }
                }

                int i;
                bool contains = false ;
                for (i = 0; i < users.Length; i++)
                {
                    if (users[i].Contains(stdName.Trim()))
                    {
                        contains = true;
                        break;
                    }
                }



                if (contains)
                {


                    string [] tmp = users[i].Split(';');

                    i = Int32.Parse(tmp[1]);

                    FileStream fs1 = new FileStream("Data/Users/" + stdName.Trim() + i.ToString() + "/data.bin", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs1);
                    int cent, axe ; 
                    cent = br.ReadInt32();
                    axe = br.ReadInt32();
                    student = new Eleve(Int32.Parse(tmp[1]), stdName.Trim(), axe, cent); 
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
                        usersUp[usersUp.Length - 1] = stdName.Trim()+";"+i.ToString();

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

                        userCombo.Add(stdName.Trim());
                        updateUserCombo();
                        studentsCombo.ItemsSource = userCombo;

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
