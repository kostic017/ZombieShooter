using System.Collections;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private bool activated;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.gameObject.CompareTag("Player"))
            StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        activated = true;
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        gameObject.SetActive(false);
    }
}
