using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Ressources { corn, stucco, jade, slave };

public class Dice : MonoBehaviour {

    //public Button button;
    public int value;
    public int diceNumber;
    public Ressources ressource;

    public Sprite digit1Sprite;
    public Sprite digit2Sprite;
    public Sprite digit3Sprite;
    public Sprite digit4Sprite;
    public Sprite digit5Sprite;
    public Sprite digit6Sprite;
    public Sprite yellowSprite;
    public Sprite greenSprite;
    public Sprite brownSprite;
    public Sprite redSprite;
    public Sprite greySprite;

    public Image colorImage;
    public Image valueImage;
    public IDictionary<Ressources, Sprite> colorSprites;

    public AudioClip diceSelectionSound;
    public AudioClip diceRollingSound;

    private int minValue = 1;
    private int maxValue = 6;
    public Sprite[] digitSprites;
    private bool selected;

    void Start()
    {
        digitSprites = new Sprite[6];
        digitSprites[0] = digit1Sprite;
        digitSprites[1] = digit2Sprite;
        digitSprites[2] = digit3Sprite;
        digitSprites[3] = digit4Sprite;
        digitSprites[4] = digit5Sprite;
        digitSprites[5] = digit6Sprite;

        colorSprites = new Dictionary<Ressources, Sprite>
        {
            { Ressources.corn, yellowSprite },
            { Ressources.stucco, brownSprite },
            { Ressources.jade, greenSprite },
            { Ressources.slave, redSprite }
        };

        value = 1;
        colorImage = gameObject.GetComponentInChildren<Image>();
        colorImage.sprite = greySprite;
        valueImage = gameObject.transform.Find("Background/Image").gameObject.GetComponent<Image>();
        valueImage.sprite = digit1Sprite;
    }

    public void RollDice()
    {
        value = Random.Range(minValue, maxValue);
        //value = 6;
        valueImage.sprite = digitSprites[value - 1];
        colorImage.sprite = colorSprites[ressource];
        gameObject.GetComponent<Toggle>().interactable = true;
        SoundManager.instance.PlaySfx(diceRollingSound);
    }

    public void OnToggleChange(bool selected)
    {
        if (selected)
        {
            GameManager.instance.DiceSelected(diceNumber);
        }
        else
        {
            GameManager.instance.DiceUnselected(diceNumber);
        }
        SoundManager.instance.PlaySfx(diceSelectionSound);
    }

    public void ChangeImageSprite()
    {
        valueImage.sprite = greySprite;
    }

    /*public Dice(int _value, Ressources _ressource)
    {
        value = _value;
        ressource = _ressource;
    }

    public Dice(Ressources _ressource)
    {
        ressource = _ressource;
    }*/
}
