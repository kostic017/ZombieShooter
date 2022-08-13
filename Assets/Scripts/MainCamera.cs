using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();   
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
