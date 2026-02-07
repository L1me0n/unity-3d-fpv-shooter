using TMPro;
using UnityEngine;


public class HUDHealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private TMP_Text hpText;

    private void Start()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerHealth = player.GetComponent<Health>();
        }

        UpdateText();
    }

    private void Update()
    {
        if (playerHealth == null) return;
        UpdateText();
    }

    private void UpdateText()
    {
        hpText.text = $"HP: {Mathf.CeilToInt(playerHealth.CurrentHealth)}";
    }
}

