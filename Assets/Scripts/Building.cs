using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {

    public int level;
    public bool active;
    public int type;
    public AudioClip buildSound;

    protected Ressources productedResource;
    // Use this for initialization
    protected virtual void Start()
    {
        gameObject.SetActive(false);
        level = 0;
    }

    public virtual void LevelUp()
    {
        GameManager.instance.ActivateDice(productedResource);
        level++;
        gameObject.SetActive(true);
        SoundManager.instance.PlaySfx(buildSound);
    }

    public abstract void CheckRule(List<Dice> selection);
}
