using UnityEngine;
using UnityEngine.Video;

namespace YoutubePlayer
{
    public class SimpleYoutubeVideo : MonoBehaviour
    {
        public string videoUrl;

        async void OnEnable()
        {
            Debug.Log("Loading video...");
            var videoPlayer = GetComponent<VideoPlayer>();
            await videoPlayer.PlayYoutubeVideoAsync(videoUrl);
        }
    }
}