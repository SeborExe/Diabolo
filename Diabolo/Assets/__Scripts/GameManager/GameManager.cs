using UnityEngine;
using RPG.SceneManagement;
using UnityEngine.SceneManagement;
using RPG.Stats;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    public GameObject player;
    public SavingWrapper savingWrapper;
    public Fader fader;
    public BaseStats baseStat;

    public delegate void Change();

    protected override void Awake()
    {
        base.Awake();

        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        baseStat = player.GetComponent<BaseStats>();
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

    /*
    public float GetDefense()
    {
        return GetPlayer().GetComponent<BaseStats>().GetStat(Stat.Damage);
    }

    public float GetHealthRegeneration()
    {
        return GetPlayer().GetComponent<BaseStats>().GetStat(Stat.HealthRegeneration);
    }

    public float GetManaRegeneration()
    {
        return GetPlayer().GetComponent<BaseStats>().GetStat(Stat.ManaRegeneration);
    }

    public float GetHealth()
    {
        return GetPlayer().GetComponent<Health>().GetMaxHealthPoints();
    }

    public float Mana()
    {
        return GetPlayer().GetComponent<Mana>().GetMaxMana();
    }
    */
}
