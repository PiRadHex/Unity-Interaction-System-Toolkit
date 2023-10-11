using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	public Animator animator;

	private string sceneToLoad;
	
	public void FadeToLevel (string scene)
	{
        sceneToLoad = scene;
		animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete ()
	{
		SceneManager.LoadScene(sceneToLoad);
	}
}
