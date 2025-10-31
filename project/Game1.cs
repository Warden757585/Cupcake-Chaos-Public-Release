using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace CupcakeChaos
{
    enum Scenes
    {
        MENU,
        GAME,
        TIMED,
        GAMEOVER,
        CREDITS
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        CupcakeClass cupcake;
        PlayerClass P1;
        TimerClass t;
        SpriteFont score_font;
        SpriteFont menu_font;
        Vector2 score_position;
        Song track1_song1;
        Song track1_song2;
        Song track1_song3;
        Song track1_song4;
        Song track1_song5;
        Song[] track1;
        SoundEffect cupcake_collect;
        Random r;
        int seed;
        Scenes active_scene;
        Vector2 menu_heading;
        Vector2 menu_subheading;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            active_scene = Scenes.MENU;
        }

        protected override void Initialize()
        {
            r = new Random();
            seed = r.Next();
            r = new Random(seed);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            t = new TimerClass(30);

            //Load songs and audio effects
            //SFX:

            cupcake_collect = Content.Load<SoundEffect>("Audio/SFX/score");

            //Track 1:
            track1_song1 = Content.Load<Song>("Audio/Songs/Cupcake Chaos");
            track1_song2 = Content.Load<Song>("Audio/Songs/Cupcake Calm");
            track1_song3 = Content.Load<Song>("Audio/Songs/Canny Cupcakes");
            track1_song4 = Content.Load<Song>("Audio/Songs/Cupcake Crazy");
            track1_song5 = Content.Load<Song>("Audio/Songs/Cool Cupcakes");
            track1 = new Song[5] { track1_song1, track1_song2, track1_song3, track1_song4, track1_song5 };
            SongIndex = r.Next(1,track1.Length);

            //Load Player Content
            P1 = new PlayerClass(Content.Load<Texture2D>("Sprites/Blue Square Guy"), seed);
            P1.SetPlayerContent(GraphicsDevice);

            //Load Cupcake Content
            cupcake = new CupcakeClass(Content.Load<Texture2D>("Sprites/Cupcake"), P1, cupcake_collect, seed);
            cupcake.SetCupcakeContent(GraphicsDevice);

            //Load Score Text
            score_position = new Vector2(10, 10);
            
            //Load Menu Content
            menu_font = Content.Load<SpriteFont>("Fonts/MenuFont");
            Vector2 HeadStringSize = menu_font.MeasureString("Cupcake Chaos");
            Vector2 SubStringSize = menu_font.MeasureString("Press ENTER to Start\nPress R-SHIFT to open Menu");
            Vector2 CentreScreen = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            menu_heading = CentreScreen - HeadStringSize / 2;
            menu_subheading = CentreScreen - SubStringSize / 2;
            menu_subheading.Y += HeadStringSize.Y + 30;

            menu_heading.Y -= 200;
            menu_subheading.Y -= 200;
        }

        int SongIndex = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (active_scene)
            {
                case Scenes.MENU:
                    if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        active_scene = Scenes.GAME;
                        P1.SetPlayerContent(GraphicsDevice);
                        cupcake.SetCupcakeContent(GraphicsDevice);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        active_scene = Scenes.CREDITS;
                        menu_subheading.Y += 45;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.T))
                    {
                        active_scene = Scenes.TIMED;
                        P1.SetPlayerContent(GraphicsDevice);
                        cupcake.SetCupcakeContent(GraphicsDevice);
                        t.setTick(30);
                        t.resetTimer();
                    }

                    MediaPlayer.Stop();

                    break;

                case Scenes.GAME:
                    if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                    {
                        active_scene = Scenes.MENU;
                    }

                    P1.PlayerLogic_Input();
                    cupcake.CollisionLogic();

                    //Music Logic:

                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        if (SongIndex < track1.Length - 1)
                        {
                            SongIndex++;
                            MediaPlayer.Play(track1[SongIndex]);
                        }
                        else
                        {
                            SongIndex = 0;
                        }
                    }
                    break;

                case Scenes.TIMED:
                    if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                    {
                        active_scene = Scenes.MENU;
                    }

                    P1.PlayerLogic_Input();
                    cupcake.CollisionLogic();
                    t.Update(gameTime);
                    if (t.isTicked())
                    {
                        active_scene = Scenes.GAMEOVER;
                    }

                    //Music Logic:

                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        if (SongIndex < track1.Length - 1)
                        {
                            SongIndex++;
                            MediaPlayer.Play(track1[SongIndex]);
                        }
                        else
                        {
                            SongIndex = 0;
                        }
                    }
                    break;

                case Scenes.GAMEOVER:
                    if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                    {
                        active_scene = Scenes.MENU;
                    }
                    MediaPlayer.Stop();
                    break;

                case Scenes.CREDITS:
                    if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                    {
                        active_scene = Scenes.MENU;
                        menu_subheading.Y -= 45;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            //Preparing the screen for drawing

            GraphicsDevice.Clear(Color.DarkSlateGray);

            switch (active_scene)
            {
                case Scenes.MENU:

                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(menu_font, "Cupcake Chaos", menu_heading, Color.LightPink);
                    _spriteBatch.DrawString(menu_font, "   Press ENTER to Start\nPress R-SHIFT to open Menu\n  Press C to open Credits\n   Press T to play Timed", menu_subheading, Color.LightPink);
                    _spriteBatch.End();

                    break;

                case Scenes.GAME:

                    _spriteBatch.Begin();
                    cupcake.CupcakeDrawing(_spriteBatch);
                    P1.PlayerDrawing(_spriteBatch);
                    _spriteBatch.DrawString(menu_font, $"Score: {P1.score}\nSpeed: {P1.GetSpeed()}", score_position, Color.LightPink);
                    _spriteBatch.End();

                    break;

                case Scenes.TIMED:

                    _spriteBatch.Begin();
                    cupcake.CupcakeDrawing(_spriteBatch);
                    P1.PlayerDrawing(_spriteBatch);
                    _spriteBatch.DrawString(menu_font, $"Time Remaining: {Math.Round(t.tick - t.getTime(), 3)}\nSpeed: {P1.GetSpeed()}", score_position, Color.LightPink);
                    _spriteBatch.End();

                    break;

                case Scenes.GAMEOVER:
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(menu_font, "GAME OVER", menu_heading, Color.LightPink);
                    _spriteBatch.DrawString(menu_font, $"Press R-SHIFT to return to Menu\nScore:{P1.score}", menu_subheading, Color.LightPink);
                    _spriteBatch.End();
                    break;

                case Scenes.CREDITS:

                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(menu_font, "Cupcake Chaos\n   Credits", menu_heading, Color.LightPink);
                    _spriteBatch.DrawString(menu_font, "Font: \"VT323\" by Peter Hull\nTile palette: \"Kitchen and more\" \nby LemonZu", menu_subheading, Color.LightPink);
                    _spriteBatch.End();
                    
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
