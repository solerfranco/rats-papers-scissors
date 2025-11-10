using UnityEngine;
using UnityEngine.UI;

public class ComboSliderUI : MonoBehaviour
{
    public Slider comboSlider;
    public PlayerMovementSlap player;
    public float comboDuration; 
    private float comboTimeLeft;
    private bool comboActive = false;

    private void Start()
    {
        comboDuration = player.comboResetTime;
    }
    void Update()
    {
        if (player.killStreak == 0)
        {
            ResetComboSlider();
        }
        if (comboActive)
        {
            comboTimeLeft -= Time.deltaTime;
            comboSlider.value = comboTimeLeft / comboDuration;

            if (comboTimeLeft <= 0f)
            {
                ResetComboSlider();
            }
        }
    }

    public void StartCombo()
    {
        comboSlider.gameObject.SetActive(true);
        comboTimeLeft = comboDuration;
        comboActive = true;
        comboSlider.value = 1f;
    }
    public void ResetComboSlider()
    {
        comboActive = false;
        comboSlider.value = 0f;
        comboSlider.gameObject.SetActive(comboActive);
    }

}
