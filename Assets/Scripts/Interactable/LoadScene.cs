using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : Interactable
{
    public string sceneToLoad;
    public LevelChanger levelChanger;
    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);
        levelChanger.FadeToLevel(sceneToLoad);
    }
}
