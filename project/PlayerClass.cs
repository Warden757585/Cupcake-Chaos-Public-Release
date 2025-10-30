using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;

public class PlayerClass
{
    Texture2D texture;
    public float x_velocity;
    public float y_velocity;
    public int x, y, score;
    public float speed;
    Vector2 position;
    public int max_x, max_y, min_x, min_y;
    public Random random;
    int seed;


    public PlayerClass(Texture2D Texture,int Seed)
    {
        this.texture = Texture;
        this.seed = Seed;

        random = new Random(seed + 1);
    }

    //Player content load function

    public void SetPlayerContent(GraphicsDevice graphicsDevice)
    {
        random = new Random(seed + 1);
        score = 0;
        speed = 1;

        // Finding the min and max spawn values for a player:

        max_x = graphicsDevice.Viewport.Width - texture.Width;
        max_y = graphicsDevice.Viewport.Height - texture.Height;
        min_x = graphicsDevice.Viewport.X + texture.Width;
        min_y = graphicsDevice.Viewport.Y + texture.Height;

        //Generate the Random spawn coordinates:

        x = random.Next(min_x, max_x);
        y = random.Next(min_y, max_y);

        //Set the coordinates to the generated values:

        position = new Vector2(x, y);

    }

    public void PlayerLogic_Input()
    {
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            y_velocity -= 1 * (speed / 1.5f);
        }
        if (state.IsKeyDown(Keys.S))
        {
            y_velocity += 1 * (speed / 1.5f);
        }
        if (state.IsKeyDown(Keys.A))
        {
            x_velocity -= 1 * (speed / 1.5f);
        }
        if (state.IsKeyDown(Keys.D))
        {
            x_velocity += 1 * (speed / 1.5f);

        }
        if (state.IsKeyDown(Keys.Z))
        {

        }

        if (speed <= 3 && speed >= 1)
        {
            speed = 1 + (score * 0.025f);
            speed = (float)Math.Round(speed, 3);
        }

        position.X += x_velocity;
        position.Y += y_velocity;

        //Damping Effect:

        x_velocity *= 0.80f;
        y_velocity *= 0.80f;
        if (x_velocity < 0.5f && x_velocity > -0.5f)
        {
            x_velocity = 0;
        }
        if (y_velocity < 0.5f && y_velocity > -0.5f)
        {
            y_velocity = 0;
        }
    }
    public void PlayerDrawing(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
    }

    public Vector2 GetPlayerCentre()
    {
        return new Vector2(position.X + (texture.Width / 2), position.Y + (texture.Height / 2));
    }
    public float GetSpeed()
    {
        return speed;
    }
}