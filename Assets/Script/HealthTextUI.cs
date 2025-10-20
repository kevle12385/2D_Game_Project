using UnityEngine;
using TMPro;

public class HealthTextUI : MonoBehaviour
{
    public Health2D target;     // drag your Player (with Health2D)
    public TMP_Text hpText;     // drag the TMP text component

    void Start()
    {
        if (target != null)
        {
            target.onDamaged.AddListener(UpdateUI);
            target.onDeath.AddListener(UpdateUI);
            UpdateUI();
        }
    }

    void OnDestroy()
    {
        if (target != null)
        {
            target.onDamaged.RemoveListener(UpdateUI);
            target.onDeath.RemoveListener(UpdateUI);
        }
    }

    void UpdateUI()
    {
        if (!target || !hpText) return;
        hpText.text = $"HP {target.currentHP}/{target.maxHP}";
    }
}
