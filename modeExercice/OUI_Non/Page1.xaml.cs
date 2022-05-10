using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Data;
using System.Globalization;
using Projet2Cp;
namespace OUI_Non
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        
        private int i = 0;// ce variable sera utilise pour savoir le question qui est affiche
     //   private string[] image_folder_paths  ;
        private string[] answers = new string[3];
        private string[] images = new string[3] ;
        private string path_images;
        private string path_answers;
        private bool mode_editer_active ;
      




        public Page1()
        {
            InitializeComponent();
        }


        private BitmapImage convertImg(string path)
        {
            BitmapImage image = new BitmapImage();
            if (!string.IsNullOrEmpty(path))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(path);
                image.EndInit();
            
            }

            return image ;
        }
         

        public Page1(int j,String path)
        {
           
            InitializeComponent();

            String_load(j);
            this.path_images = path+"\\"+"img.txt";
            this.path_answers = path + "\\" + "ans.txt";
         
            Read_answers();
            Imageselector();

        }
        public void Imageselector()  //La methode qui charge les images 
        {


            // ici on change le source d'image a chaque click de button oui utilisant l'attribut src
            
            
            
                    
                     if (File.Exists(Path.GetFullPath( images[i]))) // si l'image existe
                    {
                        if (i== 0) {
                            prebtn.Visibility = Visibility.Hidden;
                            suivbtn.Visibility = Visibility.Visible;
                 
                        }
                         if (i== 1) {
                            prebtn.Visibility = Visibility.Visible;
                            suivbtn.Visibility = Visibility.Visible;
                             
                        }
                         if (i == 2) {
                            prebtn.Visibility = Visibility.Visible;
                            suivbtn.Visibility = Visibility.Hidden;
                    
                             }



                        ONimg.Visibility = Visibility.Visible;
               
                       ONimg.Source=  convertImg( Path.GetFullPath(images[i]));
               
                   
                    }
                    else
                    {                
                        ONimg.Visibility = Visibility.Hidden;
                    }

            }


        
        public void String_load(int j ) //la methode qui charge les string d'apres le recourse 
        {


            if (j == 0) { // hadi pour les strings de la version francaise
                
                suiv_txt.Text = Strings.Btn_suiv; 
                preced_txt.Text = Strings.Btn_prece;
                describtion_txt1.Text = Strings.describtion_txt1;
                describtion_txt2.Text = Strings.describtion_txt2;
                oui_txt.Text = Strings.oui_txt;
                non_txt.Text = Strings.non_txt;
                retry_txt.Text = Strings.retry_txt;
                anstxt.Text = Strings.ans_txt;
                sauv_btn.Content =Strings.btn_sauvgarder;
                upload_btn.Content= Strings.btn_modifier;
                

            }


            if (j == 1)// hadi pour la version  arabe
            {
                bulletdec1.FlowDirection = FlowDirection.RightToLeft;
                bulletdec2.FlowDirection = FlowDirection.RightToLeft;
                retry_txt.Text = StringsAr.retry_txt;
                describtion_txt2.TextAlignment = TextAlignment.Right;
                describtion_txt1.TextAlignment = TextAlignment.Right;
                suiv_txt.Text = StringsAr.Btn_suiv;
                preced_txt.Text = StringsAr.Btn_prece;
                describtion_txt1.Text = StringsAr.describtion_txt1;
                describtion_txt2.Text =StringsAr.describtion_txt2;
                oui_txt.Text = StringsAr.non_txt;
                non_txt.Text = StringsAr.oui_txt;
                anstxt.Text = StringsAr.ans_txt;
                sauv_btn.Content = StringsAr.btn_sauvgarder;
                upload_btn.Content = StringsAr.btn_modifier;
               
            }
        }

        
        public void Read_answers()// la methode qui charge les images et les reponses d'un niveau
         {
            StreamReader sr = new StreamReader(path_answers);
            StreamReader sr1 = new StreamReader(path_images);
            images[0] = sr1.ReadLine();
            images[1] = sr1.ReadLine();
            images[2] = sr1.ReadLine();
            answers[0] = sr.ReadLine();
            answers[1] = sr.ReadLine();
            answers[2] = sr.ReadLine();
            sr.Close();
            sr1.Close();
           
            

        }

        public void write_answer() //la methode qui ecrit les images et les reponses d'un niveau
        {
            StreamWriter sr = new StreamWriter(path_answers);
            StreamWriter sr1 = new StreamWriter(path_images);
            sr1.WriteLine(images[0]);
            sr1.WriteLine(images[1]);
            sr1.WriteLine(images[2]);
            sr.WriteLine(answers[0]);
            sr.WriteLine(answers[1]);
            sr.WriteLine(answers[2]);
            sr.Close();
            sr1.Close();

        }

        private void Btn_oui(object sender, RoutedEventArgs e)
        {

            Read_answers();
         


            if (btnretry.Visibility == Visibility.Hidden)
            {
                
                // on definit ce qui se passe quand on click sur btn oui reponse vraie donc incremenation de value de prgbar et affichage de prochaine image
                // si la reponse est fausse on affiche le btn ressayer + changement de couleur (background..ect)
               
                   

                        if (answers[i].Equals("OUI", StringComparison.OrdinalIgnoreCase))
                        {
                            if(i<2)      i++;
                            btnretry.Visibility = Visibility.Visible;
                            retry_txt.Text = "Bravo!";
                            btnretry.Background = (Brush)(new BrushConverter().ConvertFrom("#FF32DA85"));
                            Border.Background = (Brush)(new BrushConverter().ConvertFrom("#FF32DA85"));
                           
                           

                        }
                        else
                        {
                            retry_txt.Text = "Ressayer";
                            btnretry.Background = (Brush)(new BrushConverter().ConvertFrom("#EC3D3D")); 
                            Border.Background = (Brush)(new BrushConverter().ConvertFrom("#DFEC3D3D"));
                            ouibtn.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#EE2E2E"));
                            nonbtn.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#EE2E2E"));
                            btnretry.Visibility = Visibility.Visible;
                        }
                      
                    
                }



            



        }



        private void Btn_non(object sender, RoutedEventArgs e)
        {
            Read_answers();
            
            if (btnretry.Visibility == Visibility.Hidden)
            {
               
                // on definit ce qui se passe qui on click sur btn oui reponse vraie donc incremenation de value de prgbar et affichage de prochaine image
                // si la reponse est fausse on affiche le btn ressayer + changement de couleur (background..ect)
                
                        if (answers[i].Equals("NON", StringComparison.OrdinalIgnoreCase))
                        {
                             if (i < 2) i++;
                            btnretry.Visibility = Visibility.Visible;
                            retry_txt.Text = "Bravo!";
                            btnretry.Background = (Brush)(new BrushConverter().ConvertFrom("#FF32DA85"));
                            Border.Background = (Brush)(new BrushConverter().ConvertFrom("#FF32DA85"));
                            
                }
                        else
                        {
                            retry_txt.Text = "Ressayer";
                            btnretry.Background = (Brush)(new BrushConverter().ConvertFrom("#EC3D3D"));
                            Border.Background = (Brush)(new BrushConverter().ConvertFrom("#DFEC3D3D"));
                            ouibtn.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#EE2E2E"));
                            nonbtn.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#EE2E2E"));
                            btnretry.Visibility = Visibility.Visible;
                }

            


        }
        }

        private void Btnretry_Click(object sender, RoutedEventArgs e)
        {
            if(retry_txt.Text == "Bravo!")
            {
                Imageselector();

            }        
                btnretry.Visibility = Visibility.Hidden;
                Border.Background = (Brush)(new BrushConverter().ConvertFrom("#A2DBA1"));
                ouibtn.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#801CB81C"));
                nonbtn.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#801CB81C"));
           
          



        }

        private void Button_suiv_Click(object sender, RoutedEventArgs e)
        {        
            if (i < 2)   i++;
            Imageselector();
           
        }


        private void Button_prece_Click(object sender, RoutedEventArgs e)
        {            
            if (i >= 0) i--;
            Imageselector();
        }





        private void Upload_button(object sender,RoutedEventArgs e)
        {
            
            
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (JPG,PNG,bmp)|*.JPG;*.PNG;*.bmp"
            };

            bool? response = openFileDialog.ShowDialog();       
                if (response == true)
                {
                    try
                    {
                        String filepath = openFileDialog.FileName;
                        String name = Path.GetFileName(filepath);
                        File.Copy(filepath, Path.GetFullPath(images[i]), true);
                    }catch(IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                Imageselector();
           
             





            }

        private void sauv_btn_Click(object sender, RoutedEventArgs e)
        {
            answers[i] = reponse.Text;
            if ((reponse.Text.Equals("oui", StringComparison.OrdinalIgnoreCase) || reponse.Text.Equals("non", StringComparison.OrdinalIgnoreCase)) && File.Exists(images[i]))
            { write_answer();
                sauv_btn.Visibility = Visibility.Collapsed;
                upload_btn.Visibility = Visibility.Collapsed;
                ouibtn.Visibility = Visibility.Visible;
                nonbtn.Visibility = Visibility.Visible;
                anstxt.Visibility = Visibility.Collapsed;
                ans_input.Visibility = Visibility.Collapsed;

            }
            else MessageBox.Show("SVP entrer oui ou non");
           


        }

        private void editer_Click(object sender, RoutedEventArgs e)
        {
            if (!mode_editer_active)
            {
                nonbtn.Visibility = Visibility.Collapsed;
                ouibtn.Visibility = Visibility.Collapsed;
                sauv_btn.Visibility = Visibility.Visible;
                anstxt.Visibility = Visibility.Visible;
                ans_input.Visibility = Visibility.Visible;
                upload_btn.Visibility = Visibility.Visible;
                mode_editer_active = true;

            }
            
            else {
                sauv_btn.Visibility = Visibility.Collapsed;
                upload_btn.Visibility = Visibility.Collapsed;
                ouibtn.Visibility = Visibility.Visible;
                nonbtn.Visibility = Visibility.Visible;
                anstxt.Visibility = Visibility.Collapsed;
                ans_input.Visibility = Visibility.Collapsed;
                mode_editer_active = false;
            }


        }
    }
}

