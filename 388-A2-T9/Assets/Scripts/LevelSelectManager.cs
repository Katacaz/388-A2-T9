using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    Game_Manager gM;
    Player player;
    public AudioClip customMainMusic;
    private void Awake()
    {
        gM = Game_Manager.Instance;
        player = FindObjectOfType<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (customMainMusic == null)
        {
            Audio_Manager.Instance.ResetToOriginalMusic(true, true);
        } else
        {
            Audio_Manager.Instance.ChangeMainMusic(customMainMusic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int levelID)
    {
        StartCoroutine(LoadAsync(levelID));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            int loadPercent = Mathf.RoundToInt(progress * 100);


            yield return null;
        }
        
    }
}
