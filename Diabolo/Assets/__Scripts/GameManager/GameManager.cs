using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using RPG.UI.MainMenu;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    public GameObject player;
    public SavingWrapper savingWrapper;
    public Fader fader;

    public delegate void Change();

    protected override void Awake()
    {
        base.Awake();

        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        savingWrapper = transform.parent.gameObject.GetComponentInChildren<SavingWrapper>();
        fader = transform.parent.gameObject.GetComponentInChildren<Fader>();
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public SavingWrapper GetSavingWrapper()
    {
        return savingWrapper;
    }

    public Fader GetFader()
    {
        return fader;
    }
}
