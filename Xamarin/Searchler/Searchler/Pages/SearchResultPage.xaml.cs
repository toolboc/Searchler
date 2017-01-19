using Hqub.MusicBrainz.API.Entities;
using IF.Lastfm.Core.Api;
using Searchler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Searchler.Pages
{
    public partial class SearchResultPage : ContentPage
    {

        // Get your own API_KEY and API_SECRET from http://www.last.fm/api/account
        //string API_KEY = "d3e975f0903f96a9625fb2890f23fac0";
        //string API_SECRET = "apiSecret";


        public ListView ResultsListView { get { return restulListView; } }
        public ObservableCollection<Artist> Results { get; set; }

        public SearchResultPage()
        {
            InitializeComponent();

            Results = new ObservableCollection<Artist>();
            restulListView.ItemsSource = Results;

        }

        public async Task LoadResults(SearchType type, string query)
        {
            Results.Clear();

            activityIndicator.IsRunning = true;

            if (type == SearchType.Tag)
            {
                var tags = query.Split('#');

                var tagQuery = "tag:" + tags[1];

                if (tags.Count() > 2)
                {
                    for (int i = 2; i < tags.Count(); i++)
                    {
                        tagQuery = tagQuery + " AND tag:" + tags[i];
                    }
                }

                var artists = await Artist.SearchAsync(tagQuery);

                foreach (var artist in artists.Items)
                {
                    Results.Add(artist);
                }

            }

            //For Last.FM Results

            //if (type == SearchType.Tag)
            //{
            //    Create your session
            //   LastfmClient client = new LastfmClient(API_KEY, API_SECRET);

            //    var artists = await client.Tag.GetTopArtistsAsync(query.Remove(0, 1));

            //    foreach (var artist in artists)
            //    {
            //        var a = new Artist { Id = artist.Id, Name = artist.Name };
            //        Results.Add(a);
            //    }

            //}

            if (type == SearchType.Artist)
            {
                var artists = await Artist.SearchAsync(query);
                foreach (var artist in artists.Items)
                {
                    Results.Add(artist);
                }
            }

            activityIndicator.IsRunning = false;

        }


    }
}
