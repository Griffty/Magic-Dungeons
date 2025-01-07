
public class NextLevel : InteractableStaticObject
{
    private LevelManager _levelManager;
    private void Start()
    {
        interactableStaticObject = this;
        _levelManager = FindObjectOfType<LevelManager>();
    }
    public override void Interact(Player player)
    {
        _levelManager.gameData.levelToLoad(_levelManager.gameData.lastLevel+1, this);
    }
}
