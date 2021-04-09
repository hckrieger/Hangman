using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hangman
{
    class Character
    {
        readonly char character;
        bool isLetter;
        bool isVisible = false;

        public Character(char character,  bool isLetter)
        {
            this.character = character;
            this.isLetter = isLetter;

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
