using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MovieOrganiser2
{
    /// <summary>
    /// When given a Movie object, a NFO file is created with the same name as the movie title, in the moviefolder.
    /// </summary>
    public class NFOHandler
    {
        public void WriteNFO(Movie m)
        {
            // where does the file need to go?
            string fullNFOpath = m.GenerateNFOFileName();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<movie>");
            sb.AppendLine("<title>" + m.Title + "</title>");
            sb.AppendLine("<year>" + m.Year + "</year>");
            sb.AppendLine("<id>" + m.Id + "</id>");
            sb.AppendLine("</movie>");
            
            // write the file
            System.IO.File.WriteAllText(fullNFOpath, sb.ToString() );

        }

        public Movie ReadNFO(string nfofile)
        {
            Movie m = new Movie();
            m.NFOfileFullpath = nfofile;

            // read the movie file
            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNode xmlnodeMovie;

            FileStream fs = new FileStream(nfofile, FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            xmlnodeMovie = xmldoc.ChildNodes[0];

            XmlNode xmlnodeTitle = xmlnodeMovie.SelectSingleNode("title");
            m.Title = xmlnodeTitle.InnerText;

            XmlNode xmlnodeYear = xmlnodeMovie.SelectSingleNode("year");
            m.Year = Int32.Parse(xmlnodeYear.InnerText);

            XmlNode xmlnodeId = xmlnodeMovie.SelectSingleNode("id");
            m.Id = xmlnodeId.InnerText;

            // retrieve Title, Year and Id 

            // return the new movie object
            return m;
        }

    }
}
