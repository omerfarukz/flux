using System;

public class OmrTime
{
	/// <summary>
	/// its current coding date and it's enough about 68 years
	/// </summary>
	private static DateTime _base = new DateTime(2015, 5, 12);
	
	public static int GetCurrent()
	{
		return (int)(DateTime.Now - _base).TotalSeconds;
	}
}