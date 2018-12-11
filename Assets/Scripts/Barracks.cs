using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : Building {

    public Button getJadeButton;

	// Use this for initialization
	protected override void Start ()
    {
        GameManager.instance.AddBuildingToList("barracks", this);
        getJadeButton.interactable = false;
        productedResource = Ressources.slave;

        base.Start();
    }

    public override void CheckRule(List<Dice> selection)
    {
        getJadeButton.interactable = false;

        /*int nbSlaveDice = 0;
        int slaveDice1Value = 0;
        int slaveDice2Value = 0;

        if (selection.Count == 2)
        {
            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].ressource == Ressources.slave)
                {
                    nbSlaveDice++;
                    if (nbSlaveDice == 1)
                    {
                        slaveDice1Value = selection[i].value;
                    }
                    else
                    {
                        slaveDice2Value = selection[i].value;
                    }
                }
            }
        }

        if (nbSlaveDice == 2 && slaveDice1Value == slaveDice2Value)
        {
            getSlavesButton.interactable = true;
        }*/

        if (selection.Count == 2)
        {
            bool slaveOK = false;
            bool cornOK = false;
            int valueCornDice = 0;
            int valueSlaveDice = 0;

            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].ressource == Ressources.corn)
                {
                    Debug.Log("corn" + selection[i].value);
                    cornOK = true;
                    valueCornDice = selection[i].value;
                }
                else if (selection[i].ressource == Ressources.slave)
                {
                    Debug.Log("stucco" + selection[i].value);
                    slaveOK = true;
                    valueSlaveDice = selection[i].value;
                }
            }

            if (slaveOK && cornOK && (valueSlaveDice == valueCornDice))
            {
                getJadeButton.interactable = true;
            }
        }
    }

    public void OnGetSlavesButtonClic()
    {
        GameManager.instance.AddJade(6);
    }
}
