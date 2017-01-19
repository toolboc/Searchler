using Searchler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.MusicBrainz.API.Entities;
using Xamarin.Forms;
using Hqub.MusicBrainz.API.Entities.Include;
using System.Collections.ObjectModel;

namespace Searchler.Pages
{
    public partial class MasterPage : ContentPage
    {
        public ObservableCollection<Release> PlayLists { get; set; }

        private MainMenuPage mainMenuPage;
        private SearchResultPage searchResultPage;
        private VideoPlayerPage videoPlayerPage;

        public MasterPage(MainMenuPage MainMenuPage)
        {
            mainMenuPage = MainMenuPage;

            searchResultPage = new SearchResultPage();
            searchResultPage.ResultsListView.ItemSelected += ResultsListView_ItemSelected; ; ;

            videoPlayerPage = new VideoPlayerPage();

            InitializeComponent();

            PlayLists = new ObservableCollection<Release>();
            playListView.ItemsSource = PlayLists;
            playListView.ItemSelected += PlayListView_ItemSelected;

        }

        private void ResultsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var artist = e.SelectedItem as Artist;

            AppState.SelectedArtist = artist;

            //needs to pass over MBID
            LoadPlayLists(artist.Id);
        }

        private async void PlayListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var release = e.SelectedItem as Release;
            await videoPlayerPage.LoadVideos(release);
            mainMenuPage.Detail = videoPlayerPage;
            //mainMenuPage.IsPresented = false;
        }


        private async Task LoadPlayLists(string mbid)
        {
            activityIndicator.IsRunning = true;

            PlayLists.Clear();

            var response = await Artist.GetAsync(mbid, ArtistIncludeEntityHelper.Releases);

            var releases = response.ReleaseLists.Items.GroupBy(x => x.Title).Select(y => y.Last());

            foreach (var release in releases)
            {
                release.CoverArtArchive = new CoverArtArchive();
                release.CoverArtArchive.CoverArtUri = CoverArtArchive.GetCoverArtUri(release.Id).ToString();
                PlayLists.Add(release);
            }

            activityIndicator.IsRunning = false;

        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            //Search for artist or tag
            if(SearchBox.Text?.Length > 0)
            {
                SearchBox.BackgroundColor = Color.White;

                if(SearchBox.Text.StartsWith("#"))
                {
                    //TagSearch
                    mainMenuPage.Detail = searchResultPage;
                    searchResultPage.LoadResults(SearchType.Tag, SearchBox.Text);
                }
                else
                {
                    //ArtistSearch
                    mainMenuPage.Detail = searchResultPage;
                    searchResultPage.LoadResults(SearchType.Artist, SearchBox.Text);
                }
            }
            else
            {
                SearchBox.BackgroundColor = Color.Red;
            }    
        }
    }
}
