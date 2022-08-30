using System.Collections.Generic;
using System.Linq;
using Fusion;

public class RoomPlayer : NetworkBehaviour
{
	public static readonly List<RoomPlayer> Players = new List<RoomPlayer>();

	public static RoomPlayer Local;

	[Networked] public NetworkBool HasFinished { get; set; }
	[Networked] public SnakeController Snake { get; set; }
	[Networked] public int SnakeID { get; set; }

	public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;

	public override void Spawned()
	{
		base.Spawned();

		if (Object.HasInputAuthority)
		{
			Local = this;
		}

		Players.Add(this);
		DontDestroyOnLoad(gameObject);
	}


	public static void RemovePlayer(NetworkRunner runner, PlayerRef p)
	{
		var roomPlayer = Players.FirstOrDefault(x => x.Object.InputAuthority == p);
		if (roomPlayer != null)
		{
			if (roomPlayer.Snake != null)
			{
				runner.Despawn(roomPlayer.Snake.Object);
			}

			Players.Remove(roomPlayer);
			runner.Despawn(roomPlayer.Object);
		}
	}
}
