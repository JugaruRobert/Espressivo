using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionBasedMusicPlayer.Business.Utils.Client
{
    public class YoutubeClient
    {
        public string GetVideoUrl(string artist,string title)
        {
            string apiKey = ConfigurationManager.AppSettings["youtubeKey"];
            if (String.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("Youtube Key is missing for app configuration!");

            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("id");
            searchListRequest.Q = artist + title;
            searchListRequest.Type = "video";
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            searchListRequest.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;
            searchListRequest.VideoSyndicated = SearchResource.ListRequest.VideoSyndicatedEnum.True__;
            searchListRequest.Alt = YouTubeBaseServiceRequest<Google.Apis.YouTube.v3.Data.SearchListResponse>.AltEnum.Json;
            searchListRequest.VideoCategoryId = "10";
            searchListRequest.MaxResults = 1;
            searchListRequest.VideoDefinition = SearchResource.ListRequest.VideoDefinitionEnum.Any;

            SearchListResponse searchListResponse = searchListRequest.ExecuteAsync().Result;

            if (searchListResponse.Items.Count > 0)
                return searchListResponse.Items[0].Id.VideoId;
            return String.Empty;
        }
    }
}
