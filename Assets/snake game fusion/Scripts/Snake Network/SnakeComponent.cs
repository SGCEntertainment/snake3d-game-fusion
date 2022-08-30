using Fusion;

public class SnakeComponent : NetworkBehaviour
{
    public SnakeEntity Snake { get; private set; }

    public virtual void Init(SnakeEntity snake)
    {
        Snake = snake;
    }

    public virtual void OnMazeCompleted(bool isFinish) { }
}
