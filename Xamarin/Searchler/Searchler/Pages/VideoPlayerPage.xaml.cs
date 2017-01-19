using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Hqub.MusicBrainz.API.Entities;
using Octane.Xam.VideoPlayer.Constants;
using Octane.Xam.VideoPlayer.Events;
using Searchler.MarkupExtensions;
using Searchler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Searchler.Pages
{
    public partial class VideoPlayerPage : ContentPage
    {
        public List<PlaylistItem> Videos;

        public VideoPlayerPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VideoPlayer.Play();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            if (Device.OS == TargetPlatform.WinPhone)
                VideoPlayer.IsVisible = true;
        }

        /// <summary>
        /// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VideoPlayer.Pause();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            if (Device.OS == TargetPlatform.WinPhone)
                VideoPlayer.IsVisible = false;
        }

        private void VideoPlayer_OnPlayerStateChanged(object sender, VideoPlayerStateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case PlayerState.Paused:
                    break;
                case PlayerState.Prepared:
                    break;
                case PlayerState.Completed:
                    Next();
                    break;
                case PlayerState.Initialized:
                    //PauseButton.IsVisible = false;
                    //PlayButton.IsVisible = true;
                    break;
                default:
                    //PlayButton.IsVisible = false;
                    //PauseButton.IsVisible = true;
                    break;
            }
        }

        public void Next()
        {
            var SelectedVideo = Playlist.SelectedItem as PlaylistItem;

            // We can't do anything if we don't have a list of items
            if ((Videos != null) && (Videos.Count > 0))
            {
                // If there is no current item, just go to the first item in the list
                if (SelectedVideo == null)
                {
                    SelectedVideo = Videos[0];
                    return;
                }

                // Try to get the next item
                int idx = Videos.IndexOf(SelectedVideo) + 1;

                // If there's another item after this, play it.
                // Otherwise, go back to the first item.
                if (Videos.Count > idx)
                {
                    SelectedVideo = Videos[idx];
                }
                else
                {
                    SelectedVideo = Videos[0];
                }

                Playlist.SelectedItem = SelectedVideo;
            }
        }

        private void Playlist_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var item = e.SelectedItem as PlaylistItem;

                switch (item.VideoType)
                {
                    case VideoType.YouTube:
                        VideoPlayer.Source = YouTubeVideoIdExtension.Convert(item.VideoPath);
                        break;
                    default:
                        VideoPlayer.Source = item.VideoPath;
                        break;
                }

                //((ListView)sender).SelectedItem = null; // de-select the row
            }
        }

        public async Task LoadVideos(Release release)
        {
            VideoPlayer.Source = null;

            string youtubePrefix = "https://www.youtube.com/watch?v=";
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCraDxlmIeaSwfSlSWXnUg9H-WGIXdDg7g",
                ApplicationName = "Searchler"
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.MaxResults = 1;

            var album = await Release.GetAsync(release.Id, "recordings");
            Videos = new List<PlaylistItem>();

            foreach (var medium in album.MediumList.Items)
            {
                foreach (var track in medium.Tracks.Items)
                {
                    var recording = track.Recording;
                    var length = TimeSpan.FromMilliseconds(recording.Length).ToString("m\\:ss");

                    string query = AppState.SelectedArtist.Name + "-" + recording.Title;

                    searchListRequest.Q = query;
                    var searchListResponse = await searchListRequest.ExecuteAsync();

                    foreach (var searchResult in searchListResponse.Items)
                    {
                        switch (searchResult.Id.Kind)
                        {
                            case "youtube#video":
                                Videos.Add(new PlaylistItem(searchResult.Snippet.Title, null, searchResult.Snippet.Thumbnails.Default__.Url, searchResult.Id.VideoId, VideoType.YouTube));
                                break;
                        }
                    }
                }
            }

            Playlist.ItemsSource = Videos;

            //if (videos.Count > 0)
            //{
            //    Playlist.SelectedItem = videos[0];
            //}
        }
    }
}
