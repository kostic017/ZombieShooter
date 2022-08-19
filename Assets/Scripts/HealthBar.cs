using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Color low;

    [SerializeField]
    private Color high;

    [SerializeField]
    private Vector3 offset;

    public Transform followCharacter;

    private void LateUpdate()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(followCharacter.position + offset);
    }

    public void SetHealth(float health)
    {
        slider.gameObject.SetActive(health < 100f);
        slider.maxValue = 100f;
        slider.value = health;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
}
