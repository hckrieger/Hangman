using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Hangman
{
    //Class that character information from the answer string and puts them in it's own Character classes
    class Answer
    {
        readonly string song, artist;
        public Character[] SongCharObj { get; set; }
        public Character[] ArtistCharObj { get; set; }

        public Dictionary<int, Character[]> SongOrArtist = new Dictionary<int, Character[]>();
      
        int letters = 0;

        public Answer(string answer)
        {

            ////Add array of Character objects  to the list by both song and artist with int key
            SongOrArtist.Add(0, SongCharObj);
            SongOrArtist.Add(1, ArtistCharObj);

            //split the song and artist from the string into different parts of the array and put them in separate string variables
            string[] information = answer.Split(" - ");
            song = information[0];
            artist = information[1];

            //Declar the Character objects for song and artist
            SongCharObj = new Character[song.Length];
            ArtistCharObj = new Character[artist.Length];

            //Method loops through the characters in the string and keeps track of the number of letters and what each letter is
            void AddToCharObj(string str, Character[] character)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    var isLetter = char.IsLetter(str[i]);

                    if (isLetter)
                        letters++;

                    character[i] = new Character(str[i], isLetter);
                }
            }

            AddToCharObj(song, SongCharObj);

            AddToCharObj(artist, ArtistCharObj);
        
        }

        public int SongCharCount
        {
            get { return song.Length; }
        }

        public int ArtistCharCount
        {
            get { return artist.Length; }
        }

        public int LetterCount
        {
            get { return letters; }
        }

    }
}
