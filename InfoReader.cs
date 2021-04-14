using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hangman
{
    class InfoReader
    {

        public readonly List<string> songInfo = new List<string>();
        readonly Random randomSong = new Random();
        readonly Random randomYear = new Random();
        int year;
        public int song;


        public InfoReader()
        {

            Reset();
        }

        public void Reset()
        {
            
            if (songInfo.Count > 0)
                songInfo.Clear();


            year = randomYear.Next(1954, 2021);
            
            StreamReader songReader = new StreamReader($"Content/Songs/{year}_songs.txt");

            string line = songReader.ReadLine();
            while (line != null)
            {
                songInfo.Add(line);
                line = songReader.ReadLine();
            }

            song = randomSong.Next(songInfo.Count - 1);
        }

        public string PickSong
        {
            get { return songInfo[song]; }
        }

        public int RandomYear
        {
            get { return year; }
        }
    }
}
