using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int zombieCount;

    private void Start()
    {
        zombieCount = FindObjectsOfType<Zombie>().Length;
    }

    private void Update()
    {
        if (zombieCount <= 0)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
                SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void ZombieKilled()
    {
        --zombieCount;
    }
}