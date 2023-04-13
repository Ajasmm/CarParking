using Ajas.FrameWork;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelWindow : MonoBehaviour
{
    [SerializeField] private LevelBtn levelButtonPrefab;
    [SerializeField] private RectTransform parentOfButtons;
    [SerializeField] private int lastLevel = 10;


    private void OnEnable()
    {
        GameManager.Instance.input.Menu.Escape.performed += OnEscape;
    }
    private void OnDisable()
    {
        GameManager.Instance.input.Menu.Escape.performed -= OnEscape;
    }
    private void OnEscape(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.ReadPlayerData();
        int highestLevelReached = GameManager.Instance.PlayerData.highestLevelReached;

        LevelBtn button;

        for(int i = 0; i <= lastLevel; i++)
        {
            button = Instantiate(levelButtonPrefab) as LevelBtn;
            int value = i;
            button.OnButtonClick += (() =>
            {
                OnButtonClick(value);
            });
            button.transform.SetParent(parentOfButtons);
            button.SetText((i + 1).ToString());
            button.SetInteractionMode((i <= highestLevelReached) ? true : false);
        }
    }

    private void OnButtonClick(int level)
    {
        GameManager.Instance.CurrentLevel = level;
        GameManager.Instance.player.transform.parent = null;
        DontDestroyOnLoad(GameManager.Instance.player);
        SceneManager.LoadSceneAsync(2);
    }

    
    

}


