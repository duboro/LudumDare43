using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    Vector3 pos;


    // Use this for initialization
    void Start () {
        Hide();
    }
	
	// Update is called once per frame
	void Update () {
        if (pos != transform.position)
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 80);
    }

    public void Show()
    {
        pos = new Vector3(40.0f, 27.0f, 0.0f); ;
        //Game.Instance.enPause = true;
        //gameObject.SetActive(true);
    }

    public void Hide()
    {
        pos = new Vector3(40.0f, 80.0f, 0.0f); ;
        //Game.Instance.enPause = false;
        //gameObject.SetActive(false);
    }

    public void Finish()
    {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
        Destroy(GameManager.instance.gameObject);
        Destroy(SoundManager.instance.gameObject);
    }
}
