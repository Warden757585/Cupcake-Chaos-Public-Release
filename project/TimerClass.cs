using System;
using Microsoft.Xna.Framework;

public class TimerClass
{
	public int tick;
	public double count = 0;

    public TimerClass( int Tick)
	{
		this.tick = tick;
	}

	public void Update(GameTime gt)
	{
		if (count < tick)
		{
			count += gt.ElapsedGameTime.TotalSeconds;
		}
		else if (count > tick)
		{
			count = 0;
		}
	}

	public double getTime()
	{
		return count;
	}

	public bool isTicked()
	{
		if (count >= tick)
		{
			return true;
		}
		else
		{
			return false;
		}
    }

    public void resetTimer()
	{
		count = 0;
	}

	public void setTick(int Tick)
	{
		this.tick = Tick;
    }
}
