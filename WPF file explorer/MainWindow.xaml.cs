using System;
using System.IO;
using System.Collections.Generic;
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

namespace WPF_File_Explorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Defualt Constructor
        /// </summary>
        
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region On Loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Geting every logical drive
            foreach ( var drive in Directory.GetLogicalDrives() )
            {
                //create an item from drives
                var item = new TreeViewItem()
                {
                    //settings the header
                    Header = drive,
                    //settings the fullpath
                    Tag = drive
                };

                // add dummy item
                item.Items.Add(null);

                //listen for expanding items
                item.Expanded += Folder_Expanded;

                //adding to tree
                TreeVieverName.Items.Add(item);
            }
        }
        #endregion

        #region Folder Expanded

        /// <summary>
        /// If any folder is expanded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Init

            var item = (TreeViewItem)sender;

            //check exist of dummy item if not exist exit
            if(item.Items.Count != 1 || item.Items[0] != null)
                return;

            // clear dummy item
            item.Items.Clear();

            //get full path
            var fullPath = (string)item.Tag;
            #endregion

            #region Get Folders

            var directorisList = new List<string>();

            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                
                if(dirs.Length > 0)
                {
                    directorisList.AddRange(dirs);
                }
            }
            catch { }

            //foreach dir..
            directorisList.ForEach(directoryPath =>
            {
                //create item
                var subItem = new TreeViewItem()
                {
                    //settings the header
                    Header = GetFileFolderName(directoryPath),
                    //settings the fullpath
                    Tag = directoryPath
                };

                // add dummy item
                subItem.Items.Add(null);

                //listen for expanding items
                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });
            #endregion

            #region Get File

            var filesList = new List<string>();

            try
            {
                var fils = Directory.GetFiles(fullPath);

                if (fils.Length > 0)
                {
                    filesList.AddRange(fils);
                }
            }
            catch { }

            //foreach file..
            filesList.ForEach(directoryPath =>
            {
                //create item
                var subItem = new TreeViewItem()
                {
                    //settings the header
                    Header = GetFileFolderName(directoryPath),
                    //settings the fullpath
                    Tag = directoryPath
                };

                //listen for expanding items
                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });
            #endregion
        }

        #endregion

        #region Get File/Folder Name

        /// <summary>
        /// Get file or folder name from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        public static string GetFileFolderName(string path)
        {
            if( string.IsNullOrEmpty(path) )
                return string.Empty;

            var normalizePath = path.Replace('/', '\\');

            var lastIndex = normalizePath.LastIndexOf('\\');

            if (lastIndex <= 0)
                return path;
            return path.Substring(lastIndex + 1);
        }
        #endregion
    }
}
