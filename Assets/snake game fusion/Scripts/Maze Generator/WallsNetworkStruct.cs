using Fusion;

[System.Serializable]
public struct WallsNetworkStruct : INetworkStruct
{
    public NetworkBool setActiveLeft;
    public NetworkBool setActiveRight;

    public NetworkBool setActiveDown;
    public NetworkBool setActiveUp;

    public static WallsNetworkStruct Defaults
    {
        get
        {
            var result = new WallsNetworkStruct
            {
                setActiveLeft = false,
                setActiveRight = false,
                setActiveDown = false,
                setActiveUp = false
            };

            return result;
        }
    }
}
