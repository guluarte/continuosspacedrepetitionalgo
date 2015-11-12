using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContinuosSpacedRepetitionAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            var learning = new List<Item>();
            var allVideos = new List<Item>();

            string[] vocabVideos = File.ReadAllLines("./vocab-videos.txt");
            string[] dialogVideos = File.ReadAllLines("./dialog-videos.txt");

            var videos = new List<string>();

            var biggerList = (vocabVideos.Length > dialogVideos.Length) ? new List<string>(vocabVideos) : new List<string>(dialogVideos);
            var smallList = (vocabVideos.Length < dialogVideos.Length) ? new List<string>(vocabVideos) : new List<string>(dialogVideos);

            foreach (var item in biggerList)
            {
                videos.Add(item);

                var otherList = smallList.FirstOrDefault();

                if (otherList != null)
                {
                    videos.Add(otherList);
                    smallList.Remove(otherList);
                }
            }

 
            foreach (var video in videos)
            {
                var url = "https://www.youtube.com" + video.Substring(0, video.IndexOf("&"));
                allVideos.Add(new Item
                {
                    Video = url,
                    Interval = 600,
                    LastView = DateTime.MaxValue
                });
            }
            Item lastVideo = null;

            var headerPLaylist = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><playlist xmlns=\"http://xspf.org/ns/0/\" xmlns:vlc=\"http://www.videolan.org/vlc/playlist/ns/0/\" version=\"1\"><title>Playlist</title><trackList>";

            var footerPlayList = "</trackList><extension application=\"http://www.videolan.org/vlc/playlist/0\"><vlc:item tid=\"0\"/></extension></playlist>";

            var playList = headerPLaylist;
            int id = 0;

            while (allVideos.Count > 0)
            {
                Item getNextLearningVideo;

                if (lastVideo == null)
                {
                    getNextLearningVideo = learning.OrderBy(x => x.Interval).FirstOrDefault(x => x.Interval < 1800);
                }
                else
                {
                    getNextLearningVideo = learning.OrderBy(x => x.Interval).FirstOrDefault(x => x.Interval < 1800 && !x.Video.Equals(lastVideo.Video));
                }
                
                var numVideosLearning = learning.Where(x => x.Interval < 70000);

                if (getNextLearningVideo == null && numVideosLearning.Count() < 5)
                {
                    var videoFromThePool = allVideos.FirstOrDefault();

                    if (videoFromThePool != null)
                    {
                        learning.Add(videoFromThePool);
                        allVideos.Remove(videoFromThePool);

                        getNextLearningVideo = videoFromThePool;
                    }

                }

                if (getNextLearningVideo == null) // just grab next
                {
                    if (lastVideo == null)
                    {
                        getNextLearningVideo = learning.OrderBy(x => x.Interval).FirstOrDefault();
                    }
                    else
                    {
                        getNextLearningVideo = learning.OrderBy(x => x.Interval).FirstOrDefault(x => !x.Video.Equals(lastVideo.Video));
                    }
                    
                }

                lastVideo = getNextLearningVideo;

                Console.WriteLine("{0}", allVideos.Count);

                getNextLearningVideo.Interval *= 2;
                getNextLearningVideo.LastView = DateTime.Now;

              

                playList += string.Format("<track><location>{0}</location><extension application=\"http://www.videolan.org/vlc/playlist/0\"><vlc:option>network-caching=5000</vlc:option></extension></track>", getNextLearningVideo.Video);

                if (id%100 == 0)
                {
                    playList += footerPlayList;

                    File.WriteAllText(string.Format("./Playlists/playlist-{0}.xspf", id.ToString("D8")), playList);
                    
                    playList = headerPLaylist;
                }


                id++;

                // Implement decay

                foreach (Item item in learning)
                {
                    if (item.Video.Equals(getNextLearningVideo.Video))
                    {
                        continue;
                    }

                    item.Interval -= 100;

                    // if the last view was three months agos reset the interval
                    //if (item.LastView > DateTime.Now.AddMonths(3))
                    //{
                    //    item.Interval = 600;
                    //}
                }

                //Thread.Sleep(500);
            }

            playList += footerPlayList;

            File.WriteAllText(string.Format("./Playlists/playlist-{0}.xspf", id.ToString("D8")), playList);
        }
    }
}
