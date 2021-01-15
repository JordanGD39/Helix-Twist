using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Help : MonoBehaviour
{
    [SerializeField] private Tip[] tips;
    [SerializeField] private Image tipImage;
    [SerializeField] private TextMeshProUGUI tipText;

    private int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeTip();
        }
    }

    private void ChangeTip()
    {
        index++;

        if (index > tips.Length - 1)
        {
            index = 0;
            gameObject.SetActive(false);
            return;
        }

        tipImage.sprite = tips[index].tipSprite;
        tipText.text = tips[index].tipText;
    }
}

[System.Serializable]
class Tip
{
    public string tipText;
    public Sprite tipSprite;
}
