using System.Collections;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponentInParent<Player>();
        if (player)
            StartCoroutine(Play(player));
    }

    private IEnumerator Play(Player player)
    {
        player.enabled = false;
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        player.enabled = true;
        gameObject.SetActive(false);
    }
}
