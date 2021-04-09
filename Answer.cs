using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Hangman
{
    class Answer
    {
        readonly string song, artist;
        public Character[] SongCharObj { get; set; }
        public Character[] ArtistCharObj { get; set; }

        public Dictionary<int, Character[]> SongOrArtist = new Dictionary<int, Character[]>();
      
        int letters = 0;

        public Answer(string answer)
        {


            SongOrArtist.Add(0, SongCharObj);
            SongOrArtist.Add(1, ArtistCharObj);

            string[] information = answer.Split(" - ");
            song = information[0];
            artist = information[1];

            SongCharObj = new Character[song.Length];
            ArtistCharObj = new Character[artist.Length];

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
