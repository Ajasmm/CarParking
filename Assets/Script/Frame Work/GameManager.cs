using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System;

namespace Ajas.FrameWork
{

    public class GameManager : MonoBehaviour
    {
        public Action<GameObject> OnPlayerChange;

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
        public GameObject player;
        private bool isPlayerAvailable = false;
        public MyInput input;

        public PlayerData PlayerData { get { return playerData; } }
        private PlayerData playerData;

        public int CurrentLevel {
            get { return currentLevel; }
            set { currentLevel = value;
                if(currentLevel > MaxLevel) currentLevel = MaxLevel;
                playerData.highestLevelReached = (currentLevel > playerData.highestLevelReached) ? currentLevel : playerData.highestLevelReached;
            } }
        private int currentLevel = 0;
        private int MaxLevel = 10;

        public GamePlayMode CurrentGamePlayMode { 
            set { RegisterGameMode(value); } 
            get { return currentGamePlayMode; } }
        private GamePlayMode currentGamePlayMode;

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

            ReadPlayerData();
        }

        public void RegisterPlayer(GameObject player)
        {
            this.player = player;
            isPlayerAvailable = true;
            if(OnPlayerChange != null) OnPlayerChange(player);
        }
        public void UnRegisterPlayer(GameObject gameObject)
        {
            if (player == gameObject)
            {
                isPlayerAvailable = false;
                player = null;
            }
            else isPlayerAvailable = (player) ? true : false;
        }
        public Task WaitForPlayer()
        {
            Task task = new Task(() =>
            {

                Debug.Log("Task started : with " + isPlayerAvailable + " State"); 
                while (!isPlayerAvailable)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested) return;
                    Debug.Log("Player is not registered!");
                }
            }, cancellationTokenSource.Token);

            task.Start();
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
            playerData = new PlayerData(0);
            string filePath = Application.persistentDataPath + "/PlayerData.json";
            Debug.Log(filePath);

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            }
        }
        public void WritePlayerData()
        {
            if(currentLevel > MaxLevel) currentLevel = MaxLevel;
            if (playerData.highestLevelReached < currentLevel)
                playerData.highestLevelReached = currentLevel;

            string jsonData = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", jsonData);
        }

        public void UpdatePlayer()
        {
            if (player != null)
            {
                GameObject tempObj = player;
                player = null;
                Destroy(tempObj);
            }
            LoadPlayerFromFile();
        }
        public void UpdatePlayer(string vehicleName)
        {
            playerData.vehicleName = vehicleName;
            if (player != null)
            {
                GameObject tempObj = player;
                player = null;
                Destroy(tempObj);
            }
            LoadPlayerFromFile();
        }
        public void LoadPlayer()
        {
            if (player != null) return;
            LoadPlayerFromFile();
        }

        private void LoadPlayerFromFile()
        {
            string path = "Player/" + playerData.vehicleName;
            Debug.Log(path);
            GameObject playerObj = Resources.Load(path) as GameObject;
            if (playerObj == null)
            {
                playerData.vehicleName = "Car_1_Player";
                path = path = "Player/" + playerData.vehicleName;
                playerObj = Resources.Load(path) as GameObject;
                Debug.Log(path + " " + playerObj.name);
            }
            playerObj = Instantiate<GameObject>(playerObj);
            playerObj.SetActive(true);

            ChangePlayerMaterialAsync(PlayerData.vehicleColor);
        }
        public void ChangePlayerMaterial(PlayerData.VehicleColor vehicleColor)
        {
            playerData.vehicleColor = vehicleColor;

            GameObject player = GameManager.Instance.player;

            Material mat;
            string path = "Player/Materials/";
            string materialName = "";

            switch (vehicleColor)
            {
                case PlayerData.VehicleColor.Blue:
                    materialName = "Car_Paint_Blue";
                    break;
                case PlayerData.VehicleColor.Purple:
                    materialName = "Car_Paint_Purple";
                    break;
                case PlayerData.VehicleColor.Red:
                    materialName = "Car_Paint_Red";
                    break;
                case PlayerData.VehicleColor.Green:
                    materialName = "Car_Paint_Green";
                    break;
                case PlayerData.VehicleColor.Yellow:
                    materialName = "Car_Paint_Yellow";
                    break;
                case PlayerData.VehicleColor.Silver:
                    materialName = "Car_Paint_Silver";
                    break;
            }
            path += materialName;
            mat = Resources.Load<Material>(path) as Material;

            if (player == null) return;

            PaintChanger paintChanger = player.GetComponent<PaintChanger>();
            if (paintChanger == null) return;

            paintChanger.ChangePaint(mat);
        }
        public async void ChangePlayerMaterialAsync(PlayerData.VehicleColor vehicleColor)
        {
            await WaitForPlayer();
            ChangePlayerMaterial(vehicleColor);
        }

        private void OnDestroy()
        {
            if (instatnce == this) isGameEnded = true;
            cancellationTokenSource.Cancel();
            WritePlayerData();
        }
    }
}
[System.Serializable]
public struct PlayerData
{
    public int highestLevelReached;
    public string vehicleName;
    public VehicleColor vehicleColor;
    public PlayerData(int defaultHighestLevel)
    {
        this.highestLevelReached = defaultHighestLevel;
        vehicleName = "Car_1_Player";
        vehicleColor = VehicleColor.Blue;
    }
    public enum VehicleColor
    {
        Blue,
        Purple,
        Red,
        Green,
        Yellow,
        Silver
    }
}