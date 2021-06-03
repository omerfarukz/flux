using UnityEngine;

public class HintManager
{
	private static object _lockObject = new object();

	private bool _initialized;

	public static HintManager Instance = new HintManager();

	public int Current { get; private set; }

	public void Initialize()
	{
		lock (_lockObject) {
			if(!_initialized)
			{
				int defaultHintCount = 3;
				
				if(!PlayerPrefs.HasKey("RemainingHintCount"))
				{
					PlayerPrefs.SetInt("RemainingHintCount", defaultHintCount);
					Current = defaultHintCount;
				}
				else
				{
					Current = GetCount();
				}
			}
		}

		_initialized = true;
	}

	public void Add(int count)
	{
		var currentFromPrefs = GetCount();
		currentFromPrefs += count;

		PlayerPrefs.SetInt("RemainingHintCount", currentFromPrefs);

		Current = currentFromPrefs;
	}

	private int GetCount()
	{
		var current = PlayerPrefs.GetInt("RemainingHintCount");
		return current;
	}
}