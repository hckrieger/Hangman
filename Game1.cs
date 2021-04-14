using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Input;
using MonoGame.Extended;

namespace Hangman
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        string letters, answerStr, display;
        int wrongTries, revealedLetters, spacing;
        string[] divider;
        char[][] rowLetters;
        SpriteFont letterSelect, letterDisplay, title;
        Vector2 offset = new Vector2(50, 325);
        Answer answerObj;
        LetterSelect[,] letterGrid;
        readonly InfoReader infoReader = new InfoReader();
        bool finish = false;
        readonly Dictionary<int, Texture2D> images = new Dictionary<int, Texture2D>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            letterSelect = Content.Load<SpriteFont>("Fonts/LetterSelect");
            letterDisplay = Content.Load<SpriteFont>("Fonts/LetterDisplay");
            title = Content.Load<SpriteFont>("Fonts/Title");




            //The letter selection in string form
            letters = "ABCDEFGHI-JKLMNOPQR-STUVWXYZ";
            spacing = 40;

            //split the letter string by the dashes
            divider = letters.Split("-");

            //Set the length of the row of letters put them into and array as characters
            rowLetters = new char[divider.Length][];

            for (int i = 0; i < divider.Length; i++)
                rowLetters[i] = divider[i].ToCharArray();

            //set the object in which to display those letters in a grid
            letterGrid = new LetterSelect[rowLetters[0].Length, divider.Length];


            //See reset function at the bottom to know more of what happens
            Reset();
            for (int i = 0; i < 7; i++)
                images[i] = Content.Load<Texture2D>($"Images/{i}_wrong");
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseStateExtended mouse = MouseExtended.GetState();
            KeyboardStateExtended keyboard = KeyboardExtended.GetState();

            // TODO: Add your update logic here

  


            for (int y = 0; y < divider.Length; y++)
            {
                for (int x = 0; x < rowLetters[y].Length; x++)
                {
                    //If you click on a letter that's not been selected previously.....
                    if (letterGrid[x, y].BoundingBox.Contains(mouse.Position) && !letterGrid[x, y].Selected && mouse.WasButtonJustDown(MouseButton.Left) && !finish)
                    {
                        //..then mark it's state as Selected
                        letterGrid[x, y].Selected = true;

                        //..and if the letter doesn't exist in the song/artist name then mark it as a wrong try
                        if (!letterGrid[x, y].LetterExistsInString)
                        {
                            wrongTries++;
                        }

                        void CharLogic(int charCount, Character[] character)
                        {
                            for (int i = 0; i < charCount; i++)
                            {
                                //if the selected letter is one of the characters in the song/artist name
                                if ((letterGrid[x, y].Char == character[i].Char ||
                                    char.ToLower(letterGrid[x, y].Char) == char.ToLower(character[i].Char)))
                                {
                                    //Then make it visible and mark it as revealed
                                    character[i].IsVisible = true;
                                    revealedLetters++;

                                }
                            }
                        }

                        //call the function twice for both the song and the artist
                        CharLogic(answerObj.SongCharCount, answerObj.SongCharObj);
                        CharLogic(answerObj.ArtistCharCount, answerObj.ArtistCharObj);



                        //if the number of revealed letters is equal to the number of letters in the song/artist name...
                        if (revealedLetters >= answerObj.LetterCount)
                        {
                            //Then display the win text and mark the game as finished
                            display = "You Win! Press any button to play again";
                            finish = true;
                        } 
                        // but if the number of wrong tries is equal to six....
                        else if (wrongTries >= 6)
                        {
                            //Then reveal all the letters of the song/artist name, display the loss text and mark the game as finished
                            for (int i = 0; i < answerObj.SongCharCount; i++)
                                answerObj.SongCharObj[i].IsVisible = true;

                            for (int i = 0; i < answerObj.ArtistCharCount; i++)
                                answerObj.ArtistCharObj[i].IsVisible = true;
                            display = "You Lose! Press any button to play again";
                            finish = true;
                        }
                    }
                }
            }

            //if you finish the game and click the mouse then reset the game 
            if (finish && keyboard.WasAnyKeyJustDown())
            {
                Reset();
                finish = false;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            _spriteBatch.Begin();

            string gameName = "Pop, Rock 'n' Hang 'em!";
            _spriteBatch.DrawString(title, gameName, new Vector2(CenterToWidth(), 52) - CenterText(gameName, title), Color.Black);

            for (int y = 0; y < divider.Length; y++)
            {
                for (int x = 0; x < rowLetters[y].Length; x++)
                {
                    Color fontColor;
                    if (!letterGrid[x, y].Selected)
                        fontColor = Color.Black;
                    else
                        fontColor = Color.Red;

                    _spriteBatch.DrawString(letterSelect, letterGrid[x, y].Char.ToString(), new Vector2(x, y) * spacing + offset, fontColor);
                }
            }
            
            Vector2 CenterImage(Texture2D image) { return new Vector2(image.Width, image.Height) / 2; }
            Vector2 CenterText(string text, SpriteFont font) { return font.MeasureString(text) / 2; }
            int CenterToWidth() { return _graphics.PreferredBackBufferWidth / 2; }

            string hint = $"Hint: Popular song from {infoReader.RandomYear}";

            _spriteBatch.DrawString(letterDisplay, hint, new Vector2(CenterToWidth(), 135) - CenterText(hint, letterDisplay), Color.Black);
            _spriteBatch.DrawString(letterDisplay, "Song: ", new Vector2(50, 200), Color.Black);
            _spriteBatch.DrawString(letterDisplay, "Artist: ", new Vector2(50, 250), Color.Black);

            var xOffset = 125;
            var xSpacing = 23;
            var lineLength = 18;
            var lineHeight = 1.75f;
            var lineCentering = lineLength / 2;

            void AnswerDraw(int charCount, Character[] character, int charPos, int linePos)
            {
                for (int i = 0; i < charCount; i++)
                {
                    Vector2 measure = letterDisplay.MeasureString(character[i].Char.ToString());

                    if (character[i].IsVisible)
                        _spriteBatch.DrawString(letterDisplay, character[i].Char.ToString(), new Vector2(xOffset + ((i * xSpacing)), charPos) - measure / 2, Color.Black);

                    if (character[i].IsLetter)
                        _spriteBatch.FillRectangle(new RectangleF((xOffset + (i * xSpacing) - lineCentering), linePos, lineLength, lineHeight), Color.Black);
                }
            }


            AnswerDraw(answerObj.SongCharCount, answerObj.SongCharObj, 210, 220);

            AnswerDraw(answerObj.ArtistCharCount, answerObj.ArtistCharObj, 260, 270);


            _spriteBatch.Draw(images[wrongTries], new Vector2(475, 380) - CenterImage(images[wrongTries]), Color.Black);

            //_spriteBatch.DrawString(letterDisplay, infoReader.songInfo.Count.ToString(), new Vector2(100, 350), Color.Black);
        
            _spriteBatch.DrawString(letterDisplay, display, new Vector2(CenterToWidth(), 300) - CenterText(display, letterDisplay), Color.Black);

            _spriteBatch.End();



            base.Draw(gameTime);
        }

        void Reset()
        {
            int windowWidth = 570;

            // TODO: use this.Content to load your game content here


            //The reset function of the infoReader object, where the song is chosen from textfiles
            infoReader.Reset();
            
            //Pick a song
            answerStr = infoReader.PickSong;

            //The display that enters win/loss status
            display = "";

            
            revealedLetters = 0;
            wrongTries = 0;

            answerObj = new Answer(answerStr); //put the song in the answer object 

            int lastSongCharPos = 125 + (answerObj.SongCharCount * 23) + 50;  // position of the last character of the song
            int lastArtistCharPos = 125 + (answerObj.ArtistCharCount * 23) + 50; // position of the last character of the artist
            //int hangManPos = 

            int lastCharPos = MathHelper.Max(lastSongCharPos, lastArtistCharPos);

            //If the last character of the name or artist is longer than the window width or the other than expand the window size to fit
            if (lastCharPos > windowWidth)
                _graphics.PreferredBackBufferWidth = lastCharPos;
            else
                _graphics.PreferredBackBufferWidth = windowWidth;

            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();


            //Display the letters the array and display them into a grid
            for (int y = 0; y < divider.Length; y++)
            {
                for (int x = 0; x < rowLetters[y].Length; x++)
                {
                    Vector2 letterSize = letterSelect.MeasureString(rowLetters[y][x].ToString());

                    RectangleF bounds = new RectangleF(x * spacing + offset.X, y * spacing + offset.Y, letterSize.X, letterSize.Y);

                    letterGrid[x, y] = new LetterSelect(rowLetters[y][x], bounds, answerStr);

                }
            }
        }

    }


}
