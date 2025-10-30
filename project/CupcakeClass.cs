using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Numerics;

public class CupcakeClass
{
	Texture2D texture;
	Vector2 position;
	PlayerClass player;
	SoundEffect collect;
	int random_x, random_y, seed;
    public int max_x, max_y, min_x, min_y;
	public Random random;


    public CupcakeClass(Texture2D Texture,PlayerClass Player, SoundEffect Collect, int Seed)
	{
		this.texture = Texture;
		this.player = Player;
		this.collect = Collect;
		this.seed = Seed;

		random = new Random(seed);
    }

    public void SetCupcakeContent(GraphicsDevice graphicsDevice)
	{
        random = new Random(seed);

        // Finding the min and max spawn values for a cupcake:

        max_x = graphicsDevice.Viewport.Width - texture.Width;
        max_y = graphicsDevice.Viewport.Height - texture.Height;
		min_x = graphicsDevice.Viewport.X + texture.Width;
        min_y = graphicsDevice.Viewport.Y + texture.Height;

		//Generate the Random spawn coordinates:

		random_x = random.Next(min_x, max_x);
        random_y = random.Next(min_y, max_y);

		//Set the coordinates to the generated values:

		position = new Vector2(random_x, random_y);
    }

	public void CollisionLogic()
	{
		if (player.GetPlayerCentre().X > position.X && player.GetPlayerCentre().Y > position.Y && player.GetPlayerCentre().X < position.X + texture.Width && player.GetPlayerCentre().Y < position.Y + texture.Height)
		{
			random_x = random.Next(min_x, max_x);
			random_y = random.Next(min_y, max_y);
			position = new Vector2(random_x, random_y);
			player.score++;
			collect.Play();
        }
	}
	public void CupcakeDrawing(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(texture, position, Microsoft.Xna.Framework.Color.White);
    }
}
