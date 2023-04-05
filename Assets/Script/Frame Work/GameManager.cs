using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Ajas.FrameWork
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get
            {
                if (isGameEnded) return null;
                if (instatnce == null)
                {
                    GameObject gameManagerObj = new GameObject("GameManager");
                    gameManagerObj.AddComponent<GameManager>();
                }
                return instatnce;
            }
            private set { }
        }
        public GameObject player { private set; get; }
        public MyInput input;

        public int CurrentLevel {
            get { return currentLevel; }
            set { currentLevel = value;
                WritePlayerData(); } }
        private int currentLevel = 0;
        private int MaxLevel = 10;

        public GamePlayMode CurrentGamePlayMode { 
            set { RegisterGameMode(value); } 
            get { return currentGamePlayMode; } }
        private GamePlayMode currentGamePlayMode;
        public int HighestLevelReached { private set; get; }

        private static bool isGameEnded = false;
        private static GameManager instatnce;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public void Awake()
        {
            if (instatnce == null)
            {
                instatnce = this;
                input = new MyInput();
                input.Disable();
                DontDestroyOnLoad(gameObject);
            } else if (instatnce != this) Destroy(gameObject);
        }

        public void RegisterPlayer(GameObject player)
        {
            this.player = player;
        }
        public Task WaitForPlayer()
        {
            Task task = Task.Run(() =>
            {
                while (player == null)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested) return;
                    Debug.Log("Player is not registered!");
                }
            }, cancellationTokenSource.Token);
            return task;
        }

        private void RegisterGameMode(GamePlayMode gameMode)
        {
            currentGamePlayMode?.OnStop();
            gameMode.OnStart();
            currentGamePlayMode = gameMode;
        }

        public void GameWon()
        {
            Debug.Log("You Won the Game :)");
            currentGamePlayMode?.Won();
        }
        public void GameLost()
        {
            Debug.Log("You Lost the Game :(");
            currentGamePlayMode?.Failed();
        }


        public void ReadPlayerData()
        {
            PlayerData playerData = new PlayerData(0);
            string filePath = Application.persistentDataPath + "/PlayerData.json";
            Debug.Log(filePath);

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            }

           HighestLevelReached = playerData.highestLevelReached;
        }
        public void WritePlayerData()
        {
            ReadPlayerData();

            PlayerData playerData = new PlayerData(HighestLevelReached);
            if(currentLevel > MaxLevel) currentLevel = MaxLevel;
            if (HighestLevelReached < currentLevel)
                HighestLevelReached = currentLevel;
            
            playerData.highestLevelReached = HighestLevelReached;

            string jsonData = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", jsonData);
        }


        private void OnDestroy()
        {
            if (instatnce == this) isGameEnded = true;
            cancellationTokenSource.Cancel();
        }
    }
}