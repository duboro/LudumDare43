using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workshop : Building {

    public Button buttonBuildMarket;
    public Button buttonBuildBarracks;

    // Use this for initialization
    protected override void Start()
    {
        GameManager.instance.AddBuildingToList("workshop", this);
        buttonBuildMarket.interactable = false;
        buttonBuildBarracks.interactable = false;
        productedResource = Ressources.stucco;

        base.Start();
    }

    public override void CheckRule(List<Dice> selection)
    {
        buttonBuildMarket.interactable = false;
        buttonBuildBarracks.interactable = false;

        // verif construction marche
        if (selection.Count == 2)
        {
            bool cornOK = false;
            bool stuccoOK = false;
            int valueCornDice = 0;
            int valueStuccoDice = 0;

            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].ressource == Ressources.corn)
                {
                    cornOK = true;
                    valueCornDice = selection[i].value;
                }
                else if (selection[i].ressource == Ressources.stucco)
                {
                    stuccoOK = true;
                    valueStuccoDice = selection[i].value;
                }
            }
            if (cornOK && stuccoOK && valueCornDice > 4 && valueStuccoDice > 4)
            {
                buttonBuildMarket.interactable = true;
            }
        }

        if (selection.Count == 3)
        {
            bool stuccoOK = false;
            int nbCornDice = 0;
            int valueCornDice1 = 0;
            int valueCornDice2 = 0;

            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].ressource == Ressources.corn)
                {
                    nbCornDice++;
                    if(nbCornDice == 1)
                    {
                        valueCornDice1 = selection[i].value;
                    }
                    else
                    {
                        valueCornDice2 = selection[i].value;
                    }
                }
                if(selection[i].ressource == Ressources.stucco)
                {
                    stuccoOK = true;
                }
            }

            if(stuccoOK && nbCornDice == 2 && valueCornDice1 == valueCornDice2)
            {
                buttonBuildBarracks.interactable = true;
            }
        }
        
    }

    public void OnBuildMarketButtonClick()
    {
        GameManager.instance.BuildNewMarket();
    }

    public void OnBuildBarracksButtonClick()
    {
        GameManager.instance.BuildNewBarracks();
    }
}
