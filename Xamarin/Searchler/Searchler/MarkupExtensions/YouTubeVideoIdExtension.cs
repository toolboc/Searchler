using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Searchler.Web;
using Octane.Xam.VideoPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubeExtractor;

// http://www.genyoutube.net/formats-resolution-youtube-videos.html

namespace Searchler.MarkupExtensions
{ 
    /// <summary>
    /// Converts a YouTube video ID into a direct URL that is playable by the Xamarin Forms VideoPlayer.
    /// </summary>
    [ContentProperty("VideoId")]
    public class YouTubeVideoIdExtension : IMarkupExtension
    {
        #region Properties

        /// <summary>
        /// The video identifier associated with the video stream.
        /// </summary>
        /// <value>
        /// The video identifier.
        /// </value>
        public string VideoId { get; set; }

        #endregion

        #region IMarkupExtension

        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                Debug.WriteLine($"Acquiring YouTube stream source URL from VideoId='{VideoId}'...");
                var videoInfoUrl = $"https://www.youtube.com/watch?v={VideoId}";

                var videoInfos = DownloadUrlResolver.GetDownloadUrls(videoInfoUrl);

                var video = videoInfos.First(info => info.VideoType == VideoType.Mp4);

                return VideoSource.FromUri(video.DownloadUrl);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured while attempting to convert YouTube video ID into a remote stream path.");
                Debug.WriteLine(ex);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert the specified video ID into a streamable YouTube URL.
        /// </summary>
        /// <param name="videoId">Video identifier.</param>
        /// <returns></returns>
        public static VideoSource Convert(string videoId)
        {
            var markupExtension = new YouTubeVideoIdExtension { VideoId = videoId };
            return (VideoSource)markupExtension.ProvideValue(null);
        }

        #endregion
    }
}

