using System;
using System.Collections.Generic;
using System.Configuration;
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

            bool lookVideosIndirectory;

            bool.TryParse(ConfigurationManager.AppSettings["lookVideosInDirectory"], out lookVideosIndirectory);


            var vocabVideos = new List<string>(File.ReadAllLines("./vocab-videos.txt"));
            var dialogVideos = new List<string>(File.ReadAllLines("./dialog-videos.txt"));

            if(lookVideosIndirectory)
            {
                var videoClassesDir = ConfigurationManager.AppSettings["videoClassesPath"];
                var vocabClassesDir = ConfigurationManager.AppSettings["videoVocabPath"];
                dialogVideos = Directory.EnumerateFiles(videoClassesDir).ToList();
                vocabVideos = Directory.EnumerateFiles(vocabClassesDir).ToList();
            }

            var videos = new List<string>();

            while (vocabVideos.Count > 0 || dialogVideos.Count > 0)
            {
                var vocabVidepo = vocabVideos.FirstOrDefault();
                var dialogVideo = dialogVideos.FirstOrDefault();

                if (vocabVidepo != null)
                {
                    videos.Add(vocabVidepo);
                    vocabVideos.Remove(vocabVidepo);
                }

                if (dialogVideo != null)
                {
                    videos.Add(dialogVideo);
                    dialogVideos.Remove(dialogVideo);
                }

            }

            foreach (var video in videos)
            {
                string url;

                if (video.Contains("/watch?v="))
                {
                    url = "https://www.youtube.com" + video.Substring(0, video.IndexOf("&"));
                }
                else
                {
                    url = video;
                }
                 
                allVideos.Add(new Item
                {
                    Video = url,
                    Interval = 600,
                    LastView = DateTime.MaxValue
                });
            }
            Item lastVideo = null;

            var playList = string.Empty;
            int id = 0;

            while (allVideos.FirstOrDefault(x => x.Interval < 70000) != null)
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
                getNextLearningVideo.Views++;



                playList += string.Format("{0}{1}", getNextLearningVideo.Video, Environment.NewLine);

                if (id % 300 == 0)
                {
                    
                    File.WriteAllText(string.Format("./Playlists/playlist-{0}.m3u", id.ToString("D8")), playList);

                    playList = string.Empty;
                }


                id++;

                // Implement decay

                foreach (Item item in learning)
                {
                    if (item.Video.Equals(getNextLearningVideo.Video))
                    {
                        continue;
                    }

                    if (item.Views < 100)
                    {
                        item.Interval -= item.Interval/100;
                    }

                    // if the last view was three months agos reset the interval
                    //if (item.LastView > DateTime.Now.AddMonths(3))
                    //{
                    //    item.Interval = 600;
                    //}
                }

                //Thread.Sleep(500);
            }

            File.WriteAllText(string.Format("./Playlists/playlist-{0}.m3u", id.ToString("D8")), playList);
        }
    }
}
