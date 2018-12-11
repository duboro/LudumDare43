using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Palace : Building {

    public Button winGameButton;

	// Use this for initialization
	protected override void Start ()
    {
        GameManager.instance.AddBuildingToList("palace", this);
        winGameButton.interactable = false;

        base.Start();
    }

    public override void CheckRule(List<Dice> selection)
    {
        winGameButton.interactable = false;

        if (selection.Count == 4)
        {
            bool stuccoDice = false;
            bool slaveDice = false;
            bool cornDice = false;
            bool jadeDice = false;
            int stuccoDiceValue = 0;
            int slaveDiceValue = 0;
            int cornDiceValue = 0;
            int jadeDiceValue = 0;

            for (int i = 0; i < selection.Count; i++)
            {
                switch (selection[i].ressource)
                {
                    case Ressources.corn:
                        cornDice = true;
                        cornDiceValue = selection[i].value;
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

            if(stuccoDice && slaveDice && cornDice && jadeDice && stuccoDiceValue == 6 && slaveDiceValue == 6 && cornDiceValue == 6 && jadeDiceValue == 6)
            {
                winGameButton.interactable = true;
            }
        }
    }

    public override void LevelUp()
    {
        gameObject.SetActive(true);
        SoundManager.instance.PlaySfx(buildSound);
    }

    public void OnWinGameButtonClic()
    {
        GameManager.instance.WinGame();
    }
}
