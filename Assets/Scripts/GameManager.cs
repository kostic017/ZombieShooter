using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI zombiesLeftText;

    private int zombieCount;

    private void Start()
    {
        zombieCount = FindObjectsOfType<Zombie>().Length;
    }

    private void Update()
    {
        zombiesLeftText.text = $"Zombies Left: {zombieCount}";
        if (zombieCount <= 0)
            Invoke(nameof(GoToNextScene), 2f);
    }

    public void DecreaseZombieCount()
    {
        --zombieCount;
    }

    private void GoToNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            SceneManager.LoadScene(nextSceneIndex);
    }
}