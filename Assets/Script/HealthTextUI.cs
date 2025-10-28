using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthTextUI : MonoBehaviour
{
    public TMP_Text hpText;          // drag the TextMeshProUGUI here
    public Health2D target;          // optional: left empty; it will auto-bind
    public string hpTextTag = "HPText";   // ← tag to find your TMP text object
    private static HealthTextUI _instance;   // <-- singleton


    void Awake()
    {
        // if an instance already exists, kill this duplicate
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }



    void OnEnable() // when script is enabled
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // when scene is down loaded,  call OnSceneLoaded()
        BindToPlayerIfNeeded(true);  //rebind to new player 
        BindToTextIfNeeded(true); // rebind to UI text

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // unsubscribe from scene load event
        Unsubscribe(); // unsubscribe from events
    }

    void BindToTextIfNeeded(bool force = false)
    {
        if (force || hpText == null)
        {
            var textObj = GameObject.FindGameObjectWithTag(hpTextTag);
            if (textObj != null)
            {
                hpText = textObj.GetComponent<TMP_Text>();
            }
            else
            {
                Debug.LogWarning($"HealthTextUI: Could not find TMP_Text with tag '{hpTextTag}'");
            }
        }
    }


    void OnSceneLoaded(Scene s, LoadSceneMode m) // everytime Unity loads a scene
    // rebind the 
    {
        // scene changed -> rebind to the current player
        BindToPlayerIfNeeded(true); // rebind to new player and ge te the Health2D component
        BindToTextIfNeeded(true);

    }

    void BindToPlayerIfNeeded(bool force = false)
    {
        if (force || target == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            var newTarget = go ? go.GetComponent<Health2D>() : null;
            if (newTarget != target)
            {
                Unsubscribe();
                target = newTarget;
                Subscribe();
            }
        }
        UpdateUI();
    }

    void Subscribe()
    {
        if (!target) return;
        // everytime the invoke() function is called, the UpdateUI() method will be called
        target.onDamaged.AddListener(UpdateUI); // add an listener to onDamaged event that will update the UI everytime damage is taken
        target.onDeath.AddListener(UpdateUI); // add an listener to onDeath event that will update the UI everytime death occurs
    }

    void Unsubscribe()
    {
        if (!target) return;
        target.onDamaged.RemoveListener(UpdateUI); // removes the UpdateUI() method from onDamage events
        target.onDeath.RemoveListener(UpdateUI); // removes the UpdateUI() method from onDeath events
    }

    public void UpdateUI() // updates the health text UI
    {
        if (!hpText || !target) return;
        hpText.text = $"HP {target.currentHP}/{target.maxHP}";
    }
    
}
