using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

public class UI_Health : MonoBehaviour
{
    [HideInInspector] public IntEvent UpdateHealth;

    [SerializeField] TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth.AddListener(UpdateUI);
    }

    private void OnDestroy()
    {
        UpdateHealth.RemoveListener(UpdateUI);
    }

    private void UpdateUI(int health)
    {
        healthText.text = health.ToString();
    }
}
