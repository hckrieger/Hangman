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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        string letters, answerStr, display;
        int wrongTries, revealedLetters, spacing;
        string[] divider;
        char[][] rowLetters;
        SpriteFont font;
        Vector2 offset = new Vector2(50, 325);
        Answer answerObj;
        LetterSelect[,] letterGrid;
        InfoReader infoReader = new InfoReader();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {

            int windowWidth = 430;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Letter");
            // TODO: use this.Content to load your game content here

            letters = "ABCDEFGHI-JKLMNOPQR-STUVWXYZ";
            answerStr = infoReader.PickSong;
            display = "";

            spacing = 40;
            revealedLetters = 0;
            wrongTries = 0;

            answerObj = new Answer(answerStr);
            int lastSongCharPos = 125 + (answerObj.SongCharCount * 20) + 50;
            int lastArtistCharPos = 125 + (answerObj.ArtistCharCount * 20) + 50;

            int lastCharPos = MathHelper.Max(lastSongCharPos, lastArtistCharPos);
           

            if (lastCharPos > windowWidth)
                _graphics.PreferredBackBufferWidth = lastCharPos;
            else
                _graphics.PreferredBackBufferWidth = windowWidth;

            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();

            divider = letters.Split("-");

            rowLetters = new char[divider.Length][];

            for (int i = 0; i < divider.Length; i++)
                rowLetters[i] = divider[i].ToCharArray();

            letterGrid = new LetterSelect[rowLetters[0].Length, divider.Length];
            

            for (int y = 0; y < divider.Length; y++)
            {
                for (int x = 0; x < rowLetters[y].Length; x++)
                {
                    Vector2 letterSize = font.MeasureString(rowLetters[y][x].ToString());

                    RectangleF bounds = new RectangleF(x * spacing + offset.X, y * spacing + offset.Y, letterSize.X, letterSize.Y);

                    letterGrid[x, y] = new LetterSelect(rowLetters[y][x], bounds, answerStr);

                }
            }      
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseStateExtended mouse = MouseExtended.GetState();

            // TODO: Add your update logic here

            

            for (int y = 0; y < divider.Length; y++)
            {
                for (int x = 0; x < rowLetters[y].Length; x++)
                {
                    if (letterGrid[x, y].BoundingBox.Contains(mouse.Position) && !letterGrid[x, y].Selected && mouse.WasButtonJustDown(MouseButton.Left))
                    {
                        letterGrid[x, y].Selected = true;

                        if (!letterGrid[x, y].LetterExistsInString)
                        {
                            wrongTries++;
                        }

                        void CharLogic(int charCount, Character[] character)
                        {
                            for (int i = 0; i < charCount; i++)
                            {
                                if ((letterGrid[x, y].Char == character[i].Char ||
                                    char.ToLower(letterGrid[x, y].Char) == char.ToLower(character[i].Char)))
                                {
                                    character[i].IsVisible = true;
                                    revealedLetters++;

                                }
                            }
                        }

                        CharLogic(answerObj.SongCharCount, answerObj.SongCharObj);
                        CharLogic(answerObj.ArtistCharCount, answerObj.ArtistCharObj);

                        if (revealedLetters >= answerObj.LetterCount)
                        {
                            display = "You Win!";
                        } else if (wrongTries >= 6)
                        {
                            for (int i = 0; i < answerObj.SongCharCount; i++)
                                answerObj.SongCharObj[i].IsVisible = true;

                            for (int i = 0; i < answerObj.ArtistCharCount; i++)
                                answerObj.ArtistCharObj[i].IsVisible = true;
                            display = "You Lose!";
                        }
                            

                    } 
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            _spriteBatch.Begin();

            for (int y = 0; y < divider.Length; y++)
            {
                for (int x = 0; x < rowLetters[y].Length; x++)
                {
                    Color fontColor;
                    if (!letterGrid[x, y].Selected)
                        fontColor = Color.Black;
                    else
                        fontColor = Color.Red;
                        
                    _spriteBatch.DrawString(font, letterGrid[x, y].Char.ToString(), new Vector2(x, y) * spacing + offset, fontColor);                   
                }
            }

            _spriteBatch.DrawString(font, $"Hint: Popular song from {infoReader.RandomYear}", new Vector2(100, 25), Color.Black);

            _spriteBatch.DrawString(font, "Song: ", new Vector2(50, 95), Color.Black);
            _spriteBatch.DrawString(font, "Artist: ", new Vector2(50, 145), Color.Black);

            var xOffset = 125;
            var xSpacing = 20;
            var lineLength = 16;
            var lineHeight = 1.75f;
            var lineCentering = lineLength / 2;

            void AnswerDraw(int charCount, Character[] character, int charPos, int linePos)
            {
                for (int i = 0; i < charCount; i++)
                {
                    Vector2 measure = font.MeasureString(character[i].Char.ToString());

                    if (character[i].IsVisible)
                        _spriteBatch.DrawString(font, character[i].Char.ToString(), new Vector2(xOffset + ((i * xSpacing)), charPos) - measure / 2, Color.Black);

                    if (character[i].IsLetter)
                        _spriteBatch.FillRectangle(new RectangleF((xOffset + (i * xSpacing) - lineCentering), linePos, lineLength, lineHeight), Color.Black);
                }
            }


                AnswerDraw(answerObj.SongCharCount, answerObj.SongCharObj, 100, 110);
       
                AnswerDraw(answerObj.ArtistCharCount, answerObj.ArtistCharObj, 150, 160);
          

            _spriteBatch.DrawString(font, display, new Vector2(300, 450), Color.Black);

            _spriteBatch.End();



            base.Draw(gameTime);
        }

   

        
    }
}
