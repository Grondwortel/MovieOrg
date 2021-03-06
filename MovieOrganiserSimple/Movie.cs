﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace MovieOrganiser2
{
    public class Movie
    {
        
        /// <summary>
        /// Full pathname to the movie folder (e.g. C:\Movies\A Good Year (1999)  )
        /// </summary>
        public string OriginalFolder { get; set; }

        /// <summary>
        /// Human readable, configurable name. Can be changed without breaking reference to external sources. e.g. "A Good Year (romantic)".
        /// </summary>
        public string DisplayName { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string NFOfileFullpath { get; set; }

        /// <summary>
        /// ID is the unique identifier used by themoviedatabase and IMDB.
        /// </summary>
        public string Id { get; set; }

        public string GenerateNFOFileName()
        {
            return OriginalFolder + @"\" + DisplayName.Replace(" ", "_").ToLower() + ".nfo";
        }

        public SearchMovie TMDBSearchMovieObject { get; set; }
        
        // this is for the listbox, so it can display all objects as a list 
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
