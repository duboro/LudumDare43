using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    

    public Button transformIntoResourcesButton;
    public Button newTurn;
    public Button yumSacrificeButton;
    public Button chaahkSacrificeButton;
    public Button chuahSacrificeButton;
    public Button xamenSacrificeButton;
    public Button itzamnaSacrificeButton;
    public Button hunahpuSacrificeButton;

    public List<Dice> stock;
    public List<Dice> tirage;
    public List<Dice> selection;
    public List<Dice> defausse;

    public Text cornText;
    public Text stuccoText;
    public Text jadeText;
    public Text slaveText;
    public Text remainingTurnsText;

    public AudioClip sacrificeSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip harvestSound;
    public AudioClip slaveSound;
    public AudioClip transformSound;
    public AudioClip jadeSound;

    private int corn;
    private int stucco;
    private int jade;
    private int slave;
    private int remainingTurns;
    private IDictionary<string, Building> buildings;
    private IDictionary<Ressources, int> resourcesAvailable;
    private int nbDiceActivated;
    private int nbDiceMax;

    // Use this for initialization
    void Awake()
    {
        Debug.Log("awake GM");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }

        resourcesAvailable = new Dictionary<Ressources, int>
        {
            { Ressources.corn, 0 },
            { Ressources.jade, 0 },
            { Ressources.slave, 0 },
            { Ressources.stucco, 0 }
        };

        buildings = new Dictionary<string, Building>();
        selection = new List<Dice>();
        defausse = new List<Dice>();
        tirage = new List<Dice>();

        chaahkSacrificeButton.interactable = false;
        chuahSacrificeButton.interactable = false;
        xamenSacrificeButton.interactable = false;
        itzamnaSacrificeButton.interactable = false;
        hunahpuSacrificeButton.interactable = false;
        yumSacrificeButton.interactable = false;

        remainingTurns = 20;

        nbDiceActivated = 0;
        nbDiceMax = 8;

        transformIntoResourcesButton.interactable = false;

        FillStock();
        RefreshUI();
    }

    public void NextTurn()
    {
        remainingTurns--;

        if (IsGameOver())
        {
            OnGameOver();
        }
        else
        {
            if (selection != null)
            {
                foreach (Dice dice in selection)
                {
                    tirage.Add(dice);
                }
                selection.Clear();
            }

            if (defausse != null)
            {
                foreach (Dice dice in defausse)
                {
                    tirage.Add(dice);
                }
                defausse.Clear();
            }

            foreach (Dice dice in tirage)
            {
                dice.gameObject.GetComponent<Toggle>().isOn = false;
            }

            /*Debug.Log("fin de tour nb tirage " + tirage.Count);
            foreach (Dice dice in defausse)
            {
                if(!CheckIfDiceExistInList(dice, tirage))
                {
                    tirage.Add(dice);
                }
            }

            selection.Clear();
            defausse.Clear();*/

            RollDice();
            CheckSacrifices();
            RefreshUI();
        }
    }

    private bool IsGameOver()
    {
        bool isGameOver = false;

        if (remainingTurns < 0)
        {
            isGameOver = true;
        }

        return isGameOver;
    }

    private void OnGameOver()
    {
        GameObject.FindGameObjectWithTag("DialogLose").GetComponent<MenuScript>().Show();
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySfx(loseSound);
    }

    private bool CheckIfDiceExistInList(Dice _dice, List<Dice> diceList)
    {
        bool isInList = false;

        foreach (Dice dice in diceList)
        {
            if (_dice.diceNumber == dice.diceNumber)
            {
                isInList = true;
            }
        }

        return isInList;
    }

    public void RollDice()
    {      
        for (int i = 0; i < tirage.Count; i++)
        {
            tirage[i].RollDice();
        }
    }

    public void AddBuildingToList(string name, Building building)
    {
        buildings.Add(name, building);
    }

    /*public void AddDiceToTirage(Dice dice)
    {
        tirage.Add(dice);
    }*/

    public void MoveDiceToDefausse()
    {
        if (selection == null)
            return;


        /*foreach (Dice dice in selection)
        {
            dice.colorImage.sprite = dice.greySprite;
            defausse.Add(dice);
            dice.gameObject.GetComponent<Toggle>().interactable = false;
            dice.gameObject.GetComponent<Toggle>().isOn = false;
        }*/
        for (int i = 0; i < selection.Count; i++)
        {
            selection[i].colorImage.sprite = selection[i].greySprite;
            defausse.Add(selection[i]);
            selection[i].gameObject.GetComponent<Toggle>().interactable = false;
            //selection[i].gameObject.GetComponent<Toggle>().isOn = false;
        }
        selection.Clear();

        CheckBuildingRules();
        CheckTransformResources();
        CheckSacrifices();
    }

    public void OnTransformIntoResourceButtonClic()
    {
        if (selection == null)
            return;

        for (int i = 0; i < selection.Count; i++)
        {
            AddDiceResource(selection[i]);
        }

        MoveDiceToDefausse();
        SoundManager.instance.PlaySfx(transformSound);
    }

    public void ActivateDice (Ressources _ressource)
    {
        if (nbDiceActivated < nbDiceMax)
        {
            for (int i = 0; i < stock.Count; i++)
            {
                if (stock[i].diceNumber == nbDiceActivated)
                {
                    stock[i].ressource = _ressource;
                    stock[i].GetComponent<Toggle>().interactable = false;
                    stock[i].gameObject.SetActive(true);
                    defausse.Add(stock[i]);
                    stock.Remove(stock[i]);
                    nbDiceActivated++;
                }
            }
        }
    }

    public void DiceSelected (int diceNumber)
    {
        for (int i = tirage.Count - 1; i >=0; i--)
        {
            if (tirage[i].diceNumber == diceNumber)
            {
                selection.Add(tirage[i]);
                tirage.Remove(tirage[i]);
            }
        }
        Debug.Log("de selectionné tirage : " + tirage.Count);
        Debug.Log("de selectionné selection : " + selection.Count);
        CheckBuildingRules();
        CheckTransformResources();        
    }

    public void DiceUnselected(int diceNumber)
    {
        for (int i = selection.Count - 1; i >= 0; i--)
        {
            if (selection[i].diceNumber == diceNumber)
            {
                tirage.Add(selection[i]);
                selection.Remove(selection[i]);
            }
        }
        Debug.Log("de déselectionné tirage : " + tirage.Count);
        Debug.Log("de déselectionné selection : " + selection.Count);
        CheckBuildingRules();
        CheckTransformResources();
    }

    private void CheckTransformResources()
    {
        if (selection == null)
            return;

        if (selection.Count != 0)
        {
            transformIntoResourcesButton.interactable = true;
        }
        else
        {
            transformIntoResourcesButton.interactable = false;
        }
    }

    void AddDiceResource(Dice dice)
    {
        if (dice == null)
            return;

        resourcesAvailable[dice.ressource] += dice.value;
        CheckSacrifices();
        RefreshUI();
    }

    void AddResource(int amount, Ressources resource)
    {
        resourcesAvailable[resource] += amount;
        CheckSacrifices();
        RefreshUI();
    }

    void RefreshUI()
    {
        cornText.text = resourcesAvailable[Ressources.corn].ToString();
        stuccoText.text = resourcesAvailable[Ressources.stucco].ToString();
        jadeText.text = resourcesAvailable[Ressources.jade].ToString();
        slaveText.text = resourcesAvailable[Ressources.slave].ToString();
        remainingTurnsText.text = remainingTurns.ToString();
    }

    private void FillStock()
    {
        Debug.Log("fillstock");
        stock = new List<Dice>
        {
            GameObject.FindGameObjectWithTag("Dice1").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice2").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice3").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice4").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice5").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice6").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice7").GetComponent<Dice>(),
            GameObject.FindGameObjectWithTag("Dice8").GetComponent<Dice>()
        };

        for (int i = 0; i < stock.Count; i++)
        {
            stock[i].diceNumber = i;
            stock[i].gameObject.SetActive(false);
        }
        Debug.Log("stockFiled");
    }

    public void AddDiceToDiceStock(Dice dice)
    {
        dice.diceNumber = stock.Count;
        stock.Add(dice);
    }

    private void CheckBuildingRules()
    {
        foreach (KeyValuePair<string, Building> building in buildings)
        {
            building.Value.CheckRule(selection);
        }
    }

    // Actions des bâtiments

    //Action ferme
    public void BuildNewWorkshop()
    {
        MoveDiceToDefausse();
        buildings["workshop"].LevelUp();        
    }
    //Action ferme
    public void AddSlaves(int nbSlaves)
    {
        AddResource(nbSlaves, Ressources.slave);
        MoveDiceToDefausse();
        SoundManager.instance.PlaySfx(slaveSound);
    }

    //Action atelier
    public void BuildNewMarket()
    {        
        MoveDiceToDefausse();
        buildings["market"].LevelUp();
    }
    //Action atelier
    public void BuildNewBarracks()
    {
        MoveDiceToDefausse();
        buildings["barracks"].LevelUp();        
    }

    //Action marché
    public void BuildNewPalace()
    {
        MoveDiceToDefausse();
        buildings["palace"].LevelUp();
    }

    //action caserne
    public void AddJade(int nbJade)
    {
        AddResource(nbJade, Ressources.jade);
        MoveDiceToDefausse();
        SoundManager.instance.PlaySfx(jadeSound);
    }

    //action palace
    public void WinGame()
    {
        GameObject.FindGameObjectWithTag("DialogWin").GetComponent<MenuScript>().Show();
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySfx(winSound);
    }    

    //Action des sacrifices
    private void CheckSacrifices()
    {
        chaahkSacrificeButton.interactable = false;
        chuahSacrificeButton.interactable = false;
        xamenSacrificeButton.interactable = false;
        itzamnaSacrificeButton.interactable = false;
        hunahpuSacrificeButton.interactable = false;
        yumSacrificeButton.interactable = false;

        if (selection.Count != 0 || tirage.Count != 0)
        {
            if (resourcesAvailable[Ressources.corn] >= 3)
            {
                yumSacrificeButton.interactable = true;
            }

            if (resourcesAvailable[Ressources.corn] >= 2 && resourcesAvailable[Ressources.jade] >= 1)
            {
                chuahSacrificeButton.interactable = true;
            }

            if (resourcesAvailable[Ressources.slave] >= 2 && resourcesAvailable[Ressources.stucco] >= 5)
            {
                hunahpuSacrificeButton.interactable = true;
            }

            if (resourcesAvailable[Ressources.slave] >= 1 && resourcesAvailable[Ressources.stucco] >= 1 && resourcesAvailable[Ressources.corn] >= 1 && resourcesAvailable[Ressources.jade] >= 1)
            {
                xamenSacrificeButton.interactable = true;
            }

            if (resourcesAvailable[Ressources.slave] >= 10)
            {
                chaahkSacrificeButton.interactable = true;
            }

            if (resourcesAvailable[Ressources.jade] >= 10)
            {
                itzamnaSacrificeButton.interactable = true;
            }
        }        
    }

    public void OnYumSacrificeButtonClick()
    {
        for (int i = 0; i < selection.Count; i++)
        {
            if (selection[i].ressource == Ressources.corn)
            {
                if (selection[i].value != 6)
                {
                    selection[i].value += 1;
                    selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
                }
            }
        }

        for (int i = 0; i < tirage.Count; i++)
        {
            if (tirage[i].ressource == Ressources.corn)
            {
                if (tirage[i].value != 6)
                {
                    tirage[i].value += 1;
                    tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
                }
            }
        }            
        
        resourcesAvailable[Ressources.corn] -= 3;
        SoundManager.instance.PlaySfx(sacrificeSound);
        RefreshUI();
        CheckBuildingRules();
        CheckSacrifices();
    }

    public void OnHunahpuSacrificeButtonClick()
    {
        for (int i = 0; i < selection.Count; i++)
        {
            if (selection[i].ressource == Ressources.stucco)
            {
                if (selection[i].value != 6)
                {
                    selection[i].value += 1;
                    selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
                }
            }
            else if (selection[i].ressource == Ressources.jade)
            {
                if (selection[i].value != 6)
                {
                    selection[i].value += 1;
                    selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
                }
            }
        }

        for (int i = 0; i < tirage.Count; i++)
        {
            if (tirage[i].ressource == Ressources.stucco)
            {
                if (tirage[i].value != 6)
                {
                    tirage[i].value += 1;
                    tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
                }
            }
            else if (tirage[i].ressource == Ressources.jade)
            {
                if (tirage[i].value != 6)
                {
                    tirage[i].value += 1;
                    tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
                }
            }
        }

        resourcesAvailable[Ressources.slave] -= 2;
        resourcesAvailable[Ressources.stucco] -= 5;
        SoundManager.instance.PlaySfx(sacrificeSound);
        RefreshUI();
        CheckBuildingRules();
        CheckSacrifices();
    }

    public void OnChaahkSacrificeButtonClick()
    {
        for (int i = 0; i < selection.Count; i++)
        {
            selection[i].value = 7 - selection[i].value;
            selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
        }

        for (int i = 0; i < tirage.Count; i++)
        {
            tirage[i].value = 7 - tirage[i].value;
            tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];            
        }

        resourcesAvailable[Ressources.slave] -= 10;
        SoundManager.instance.PlaySfx(sacrificeSound);
        RefreshUI();
        CheckBuildingRules();
        CheckSacrifices();
    }

    public void OnChuahSacrificeButtonClick()
    {
        for (int i = 0; i < selection.Count; i++)
        {
            if (selection[i].ressource == Ressources.slave)
            {
                if (selection[i].value != 6)
                {
                    selection[i].value += 1;
                    selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
                }
            }
            else if (selection[i].ressource == Ressources.jade)
            {
                if (selection[i].value != 6)
                {
                    selection[i].value += 1;
                    selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
                }
            }
        }

        for (int i = 0; i < tirage.Count; i++)
        {
            if (tirage[i].ressource == Ressources.slave)
            {
                if (tirage[i].value != 6)
                {
                    tirage[i].value += 1;
                    tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
                }
            }
            else if (tirage[i].ressource == Ressources.jade)
            {
                if (tirage[i].value != 6)
                {
                    tirage[i].value += 1;
                    tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
                }
            }
        }

        resourcesAvailable[Ressources.corn] -= 2;
        resourcesAvailable[Ressources.jade] -= 1;
        SoundManager.instance.PlaySfx(sacrificeSound);
        RefreshUI();
        CheckBuildingRules();
        CheckSacrifices();
    }

    public void OnXamenSacrificeButtonClick()
    {
        for (int i = 0; i < selection.Count; i++)
        {
            if (selection[i].ressource == Ressources.slave || selection[i].ressource == Ressources.jade)
            {
                selection[i].RollDice();
            }
        }

        for (int i = 0; i < tirage.Count; i++)
        {
            if (tirage[i].ressource == Ressources.slave || tirage[i].ressource == Ressources.jade)
            {
                tirage[i].RollDice();
            }
        }

        resourcesAvailable[Ressources.corn] -= 1;
        resourcesAvailable[Ressources.jade] -= 1;
        resourcesAvailable[Ressources.slave] -= 1;
        resourcesAvailable[Ressources.stucco] -= 1;
        SoundManager.instance.PlaySfx(sacrificeSound);
        RefreshUI();
        CheckBuildingRules();
        CheckSacrifices();
    }

    public void OnItzamnaSacrificeButton()
    {

        for (int i = 0; i < selection.Count; i++)
        {
            if (selection[i].value < 5)
            {
                selection[i].value += 2;
                selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
            }
            else if(selection[i].value == 5)
            {
                selection[i].value += 1;
                selection[i].valueImage.sprite = selection[i].digitSprites[selection[i].value - 1];
            }
        }

        for (int i = 0; i < tirage.Count; i++)
        {
            if (tirage[i].value < 5)
            {
                tirage[i].value += 2;
                tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
            }
            else if (tirage[i].value == 5)
            {
                tirage[i].value += 1;
                tirage[i].valueImage.sprite = tirage[i].digitSprites[tirage[i].value - 1];
            }
        }
        
        resourcesAvailable[Ressources.jade] -= 10;
        SoundManager.instance.PlaySfx(sacrificeSound);
        RefreshUI();
        CheckBuildingRules();
        CheckSacrifices();
    }
}
