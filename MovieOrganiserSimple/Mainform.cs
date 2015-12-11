using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace MovieOrganiser2
{
    public partial class Mainform : Form
    
    {
        private string _movieFolder = @"C:\Movies";
        

        private MovieCollection _movieCollection;

        public Mainform()
        {
            InitializeComponent();
            panel1.BackgroundImage = Image.FromFile(Properties.Settings.Default.Backgroundpic);
            panel2.BackgroundImage = Image.FromFile(Properties.Settings.Default.profilepic);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // create a moviecollection
            _movieCollection = new MovieCollection();

            
        }

        // read folder contents and put all directory names in the listbox
        private void ScanFolder()
        {
            // clear listbox
            listBox1.Items.Clear();

            string[] folders = Directory.GetDirectories(_movieFolder, "*", SearchOption.TopDirectoryOnly);

            Movie m;
            foreach (string folder in folders)
            {
                // check if exactly one .nfo file is found. if so, read it. no subfolders
                string[] nfofile = Directory.GetFiles(folder, "*.nfo", SearchOption.TopDirectoryOnly);
                if (nfofile.Length == 1)
                {
                    // exactly 1 found, so read the file and create a movie object
                    NFOHandler h = new NFOHandler();
                    m = h.ReadNFO(nfofile[0]);
                    m.OriginalFolder = folder;
                    m.DisplayName = m.Title;
                }
                else
                {
                    m = new Movie();
                    m.OriginalFolder = folder;
                    m.DisplayName = ExtractMovieName(extractLastPart(folder));
                }

                _movieCollection.Add(m);
            }

            listBox1.Items.AddRange(_movieCollection.ToArray<Movie>() );
        }

        private string[] _ignoreList = { "S01", "S02", "S03", "S04", "S05", "S06", "S07", "S08", "S09",
            "480p", "720p", "1080p", "H264", "AC97", "x264", "BluRay" };
        private object close;

        public string ExtractMovieName(string text)
        {
            // check for items on the ignore list and remove them
            foreach ( string item in _ignoreList)
            {
                text = text.Replace(item, "");
            }

            StringBuilder sb = new StringBuilder(text.Length);

            bool didWeSeeASpaceYet = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                // if a number is encountered then stop
                if (didWeSeeASpaceYet && (c >= '0' && c <= '9')) break;

                // also break on different separators
                if (c == '(' || c == '[' || c == '-' || c == ':') break;

                // if a space, an underscore _ or a point . is found, then add a space
                if (c == ' ' || c == '_' || c == '.')
                {
                    sb.Append(' ');
                    didWeSeeASpaceYet = true;
                }

                // if a character a-z or A-Z is found, add it to the result
                if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9' )
                    sb.Append(text[i]);
            }

            return sb.ToString();
        }

        // give a text and only return the part after the last slash /
        private string extractLastPart(string fullpath)
        {
            return fullpath.Substring(fullpath.LastIndexOf('\\')+1);
        }

        private void identifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            identifySelectedMovie();
        }

        // take name of selected item in listBox1 and send it to TheMovieDb and retrieve possible movies
        private void identifySelectedMovie()
        {
            Movie selectedItem = (Movie) listBox1.SelectedItem;

            string tmdbkey = "c613b8cc795379d232e8ff95ccb24083";
            
            TMDbClient client = new TMDbClient(tmdbkey);
            var results = client.SearchMovie(selectedItem.DisplayName).Results;

            // loop over all the found movies, and put the titles in a list
            var movies = new List<Movie>();
            foreach (SearchMovie mov in results)
            {
                Movie m = new Movie();
                m.TMDBSearchMovieObject = mov;
                m.DisplayName = mov.Title;
                movies.Add(m);
            }

            // send all movies to the dialog listbox
            SelectMovie dialog;
            dialog = new SelectMovie(movies.ToArray() );
            DialogResult res = dialog.ShowDialog();

            if ( res == DialogResult.OK )
            {
                // ask the dialog which item was selected
                Movie selectedMovieObject = dialog.GetSelectedItem();

                // look for selected movie in de _movieCollection
                Movie m = _movieCollection.FindMovieByOriginalFolder(selectedItem.OriginalFolder);

                // store the choosen movie in the Movie object
                if ( m != null )
                {
                    m.TMDBSearchMovieObject = selectedMovieObject.TMDBSearchMovieObject;
                    m.Title = selectedMovieObject.TMDBSearchMovieObject.Title;
                    m.Id = selectedMovieObject.TMDBSearchMovieObject.Id.ToString();
                    m.Year = selectedMovieObject.TMDBSearchMovieObject.ReleaseDate.Value.Year;

                    NFOHandler wr = new NFOHandler();
                    wr.WriteNFO(m);
                }

                listBox1.Items[listBox1.SelectedIndex] = selectedMovieObject;
            }
        }

        private void writeMovieIdToFile(string id, string folderName)
        {
            File.WriteAllText(folderName + "\\movie.text", id);
        }

        private void identifyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            identifySelectedMovie();
        }

        public void chooseFolderToolStripMenuItem_Click(object sender, EventArgs e)



        {
            FolderBrowserDialog Dialog = new FolderBrowserDialog();
            DialogResult Result = Dialog.ShowDialog();
            if (Result == DialogResult.OK)
            {
                Properties.Settings.Default.BaseFolder = Dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }



        }

        private void rescanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ScanFolder();

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //test
        }

        private void profielFeuteuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetDatProfiel();
        }

        private void GetDatProfiel()
        {
            {
                OpenFileDialog Dialog = new OpenFileDialog();
                DialogResult Result = Dialog.ShowDialog();
                if (Result == DialogResult.OK)
                {
                    Properties.Settings.Default.profilepic = Dialog.FileName;
                    Properties.Settings.Default.Save();
                    panel2.BackgroundImage = Image.FromFile(Properties.Settings.Default.profilepic);
                }

            }
        }



        private void achtergrondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetDatAchtergrawnd();
        }

        private void GetDatAchtergrawnd()
        {
            {
                OpenFileDialog Dialog = new OpenFileDialog();
                DialogResult Result = Dialog.ShowDialog();
                if (Result == DialogResult.OK)
                {
                    Properties.Settings.Default.Backgroundpic = Dialog.FileName;
                    Properties.Settings.Default.Save();
                    panel1.BackgroundImage = Image.FromFile(Properties.Settings.Default.Backgroundpic);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var myForm = new Form6();
            myForm.Show();
        }
    }
}
