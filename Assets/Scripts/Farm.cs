using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : Building {

    public Button buildFarmButton;
    public Button buildWorkshopButton;
    public Button addSlaveButton;

	// Use this for initialization
	protected override void Start ()
    {
        Debug.Log("start ferme");
        GameManager.instance.AddBuildingToList("farm", this);

        buildFarmButton.interactable = false;
        buildWorkshopButton.interactable = false;
        addSlaveButton.interactable = false;
        productedResource = Ressources.corn;
        Debug.Log("fin ferme");

        LevelUp();
    }

    public override void CheckRule(List<Dice> selection)
    {
        buildFarmButton.interactable = false;
        buildWorkshopButton.interactable = false;
        addSlaveButton.interactable = false;

        for (int i = 0; i < selection.Count; i++)
        {
            if (selection[i].ressource == Ressources.corn && selection.Count == 1)
            {
                if (selection[i].value == 5)
                {
                    addSlaveButton.interactable = true;
                }
                else if (selection[i].value == 6)
                {
                    buildWorkshopButton.interactable = true;
                }
            }
        }

        if (selection.Count == 2)
        {
            bool stuccoOK = false;
            bool cornOK = false;
            int valueCornDice = 0;
            int valueStuccoDice = 0;

            for (int i = 0; i < selection.Count; i++)
            {
                if (selection[i].ressource == Ressources.corn)
                {
                    Debug.Log("corn" + selection[i].value);
                    cornOK = true;
                    valueCornDice = selection[i].value;
                }
                else if(selection[i].ressource == Ressources.stucco)
                {
                    Debug.Log("stucco" + selection[i].value);
                    stuccoOK = true;
                    valueStuccoDice = selection[i].value;
                }
            }

            if(stuccoOK && cornOK && (valueStuccoDice == valueCornDice))
            {
                buildFarmButton.interactable = true;
            }
        }
    }

    public void OnBuildFarmButtonClick()
    {
        LevelUp();
        GameManager.instance.MoveDiceToDefausse();
    }

    public void OnBuildWorkshopButtonClick()
    {
        GameManager.instance.BuildNewWorkshop();
    }

    public void OnAddSlaveButtonClick()
    {
        GameManager.instance.AddSlaves(5);
    }
}
