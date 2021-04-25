using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Hangman
{
    //This class picks a random textfile year and a random song from that textfile by index number. 
    //The song/artist sequence in each game is randomized
    class InfoReader
    {
        public readonly List<string> songInfo = new List<string>();
        readonly Random randomSong = new Random();
        readonly Random randomYear = new Random();
        int year;
        public int songQuantity;

        public InfoReader()
        {
            Reset();
        }

        public void Reset()
        {
            //If the songInfo list is full then clear it (only relevent for the first game)
            if (songInfo.Count > 0)
                songInfo.Clear();

            //Pick a random year
            year = randomYear.Next(1954, 2021);
            
            //From that random year pull the appropriate text file....
            StreamReader songReader = new StreamReader($"Content/Songs/{year}_songs.txt");

            //Extract the song/artist from every line and put it in an a list to be read
            string line = songReader.ReadLine();
            while (line != null)
            {
                songInfo.Add(line);
                line = songReader.ReadLine();
            }

            //Get the number of songs from that array
            songQuantity = randomSong.Next(songInfo.Count - 1);
        }

        //Property to pick that random song
        public string PickSong
        {
            get { return songInfo[songQuantity]; }
        }

        public int RandomYear
        {
            get { return year; }
        }
    }
}
