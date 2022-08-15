using UnityEngine;

public class Bloodsplat : MonoBehaviour
{
    [SerializeField]
    private Sprite[] bloodsplats;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = bloodsplats[Random.Range(0, bloodsplats.Length)];
    }
}
