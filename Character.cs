using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hangman
{
    class Character
    {
        char character;
        int position;
        bool isLetter;
        bool isVisible = false;
        bool selected = false;

        public Character(char character, int position, bool isLetter)
        {
            this.character = character;
            this.position = position;
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
