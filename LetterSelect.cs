using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Hangman
{
    //Class that handles information pertaining to the grid of letters that player selects from
    class LetterSelect
    {
        readonly char letter;
        public RectangleF BoundingBox { get; set; }

        public string answerStr;
        bool selected = false;

        readonly bool letterExistsInString;

        public LetterSelect(char letter, RectangleF boundingBox, string answerStr)
        {
            this.letter = letter;
            this.answerStr = answerStr;
            BoundingBox = boundingBox;

            //Character is true if it is a letter and false if it's not. 
            if (answerStr.Contains(letter.ToString()) || answerStr.Contains(letter.ToString().ToLower()))
                letterExistsInString = true;
            else
                letterExistsInString = false;
            
        }

        public char Char
        {
            get { return letter; }
        }

        public bool LetterExistsInString
        {
            get { return letterExistsInString; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

    }
}
