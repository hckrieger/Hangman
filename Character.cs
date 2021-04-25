using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hangman
{
    //A class for each character in the answer.  Determines what the character is, whether it's visible or whether it's a letter
    class Character
    {
        readonly char character;
        bool isLetter;
        bool isVisible = false;

        public Character(char character,  bool isLetter)
        {
            this.character = character;
            this.isLetter = isLetter;

            //If the character isn't a than make it visible.  All letters are hidden until selected.
            if (!isLetter)
                isVisible = true;
        }

        public char Char
        {
            get { return character; }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public bool IsLetter
        {
            get { return isLetter; }
            set { isLetter = value; }
        }
    }
}
