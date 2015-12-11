using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2
{
    public class MovieCollection: List<Movie>
    {
        public Movie FindMovieByOriginalFolder(string folder)
        {
            foreach (Movie m in this)
            {
                if ( m.OriginalFolder.Equals(folder) )
                {
                    return m;
                }
            }

            // this.Single(p => p.OriginalFolder.Equals(folder));

            return null;
        }
    }
}
