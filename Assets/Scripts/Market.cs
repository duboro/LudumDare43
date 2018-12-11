using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : Building {

    public Button buildPalaceButton;

    // Use this for initialization
    protected override void Start ()
    {
        GameManager.instance.AddBuildingToList("market", this);
        buildPalaceButton.interactable = false;
        productedResource = Ressources.jade;

        base.Start();
    }

    public override void CheckRule(List<Dice> selection)
    {
        buildPalaceButton.interactable = false;

        bool jadeDice = false;
        bool slaveDice = false;
        bool stuccoDice = false;
        int jadeDiceValue = 0;
        int slaveDiceValue = 0;
        int stuccoDiceValue = 0;

        if (selection.Count == 3)
        {
            for (int i = 0; i < selection.Count; i++)
            {
                switch (selection[i].ressource)
                {
                    case Ressources.corn:
                        break;
                    case Ressources.stucco:
                        stuccoDice = true;
                        stuccoDiceValue = selection[i].value;
                        break;
                    case Ressources.jade:
                        jadeDice = true;
                        jadeDiceValue = selection[i].value;
                        break;
                    case Ressources.slave:
                        slaveDice = true;
                        slaveDiceValue = selection[i].value;
                        break;
                    default:
                        break;
                }
            }

            //Debug.Log(jadeDice + " " + jadeDiceValue + " " + stuccoDice + " " + stuccoDiceValue + " " + slaveDice + " " + slaveDiceValue);

            if (stuccoDice && slaveDice && jadeDice && stuccoDiceValue == 6 && jadeDiceValue == slaveDiceValue)
            {
                buildPalaceButton.interactable = true;
            }
        }     
    }

    public void OnBuildPalaceButtonClic()
    {
        GameManager.instance.BuildNewPalace();
    }
}
