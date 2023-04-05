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
        int highestLevelReached = GameManager.Instance.HighestLevelReached;

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
            button.SetText("Level " + i);
            button.SetInteractionMode((i <= highestLevelReached) ? true : false);
        }
    }

    private void OnButtonClick(int level)
    {
        GameManager.Instance.CurrentLevel = level;
        SceneManager.LoadSceneAsync(1);
    }

    
    

}

public struct PlayerData
{
    public int highestLevelReached;
    public PlayerData(int defaultHighestLevel)
    {
        this.highestLevelReached = defaultHighestLevel;
    }
}
