using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;

public class PlayerClass
{
	Texture2D texture;
	public int x, y, score;
	public float speed;

	public PlayerClass(Texture2D Texture, int X, int Y, float Speed = 1.75f)
	{
		this.texture = Texture;
        this.x = X;
		this.y = Y;
		this.speed = Speed;
	}

	//Player content load function

	Vector2 position;
    public int max_x, max_y, min_x, min_y;
    public Random random = new Random();

    public void SetPlayerContent(GraphicsDevice graphicsDevice)
	{
        score = 0;
        speed = 1.75f;

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
            position.Y -= 1 * speed;
        }
        if (state.IsKeyDown(Keys.S))
        {
            position.Y += 1 * speed;
        }
        if (state.IsKeyDown(Keys.A))
        {
            position.X -= 1 * speed;
        }
        if (state.IsKeyDown(Keys.D))
        {
            position.X += 1 * speed;
        }
        if(speed < 10)
        {
            speed = 1.75f + (score * 0.05f);
            speed = (float)Math.Round(speed, 2);
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
