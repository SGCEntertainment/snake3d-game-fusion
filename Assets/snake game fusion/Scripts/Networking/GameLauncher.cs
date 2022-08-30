using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
	NetworkRunner networkRunner;
	[SerializeField] RoomPlayer _roomPlayerPrefab;

	[Space(10)]
	[SerializeField] GameObject loadingGO;


	private void Start()
	{
		loadingGO.SetActive(true);

		networkRunner = GetComponent<NetworkRunner>();

		Application.runInBackground = true;
		Application.targetFrameRate = Screen.currentResolution.refreshRate;
		QualitySettings.vSyncCount = 1;

		InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
		DontDestroyOnLoad(gameObject);
	}

	protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress netAddress, SceneRef sceneRef, Action<NetworkRunner> initialize)
	{
		var sceneObjectProvider = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
		if (sceneObjectProvider == null)
		{
			sceneObjectProvider = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
		}

		runner.ProvideInput = true;

		return runner.StartGame(new StartGameArgs
		{
			GameMode = gameMode,
			Address = netAddress,
			Scene = sceneRef,
			SessionName = "snake",
			Initialized = initialize,
			SceneManager = sceneObjectProvider
		});
	}

	public void OnConnectedToServer(NetworkRunner runner)
	{
		Debug.Log("Connected to server");
	}

	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
		Debug.Log($"Player {player} Joined!");

        if (runner.IsServer)
        {
            var _roomPlayer = runner.Spawn(_roomPlayerPrefab, Vector3.zero, Quaternion.identity, player);
			bool IsMasterClient = _roomPlayer.HasInputAuthority;

			if(IsMasterClient)
            {
				MazeGenerator.Instance.GenerateMaze(8, 8);
            }

            MazeGenerator.Instance.SpawnPlayer(runner, _roomPlayer);
        }

        loadingGO.SetActive(false);
	}

	public void OnInput(NetworkRunner runner, NetworkInput input) 
	{
		var frameworkInput = new SnakeNetworkInput
		{
			inputDirection = transform.GetMoveDirection()
        };

        input.Set(frameworkInput);
	}

	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {}

	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

	public void OnDisconnectedFromServer(NetworkRunner runner) {}

	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		Debug.Log($"{player.PlayerId} disconnected.");
		RoomPlayer.RemovePlayer(runner, player);
	}

	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

	public void OnSceneLoadDone(NetworkRunner runner) { }

	public void OnSceneLoadStart(NetworkRunner runner) { }

	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
