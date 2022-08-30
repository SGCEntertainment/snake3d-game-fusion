using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public SnakeDefinition snakeDefinition;

	public static ResourceManager Instance => Singleton<ResourceManager>.Instance;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
