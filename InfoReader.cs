using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace Hangman
{
    class InfoReader
    {

        List<string> songInfo = new List<string>();
        string pickSong;
        Random randomSong = new Random();
        Random randomYear = new Random();
        int year;


        public InfoReader()
        {

            Reset();
        }

        public void Reset()
        {
            
            if (songInfo.Count > 0)
                songInfo.Clear();

            year = randomYear.Next(1989, 2002);
            StreamReader songReader = new StreamReader($"Content/Songs/{year}_songs.txt");

            string line = songReader.ReadLine();
            while (line != null)
            {
                songInfo.Add(line);
                line = songReader.ReadLine();
            }
        }

        public string PickSong
        {
            get { return songInfo[randomSong.Next(songInfo.Count)]; }
        }

        public int RandomYear
        {
            get { return year; }
        }
    }
}
