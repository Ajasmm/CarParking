using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ajas.FrameWork
{

    public class GameManager : MonoBehaviour
    {
        public event Action OnPlayerChange;

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
        public GameObject player { get; private set; }
        public MyInput input;

        public PlayerData PlayerData { get { return playerData; } }
        private PlayerData playerData;

        Coroutine playerLoading_Coroutine;
        Coroutine vehicleColorChange_Coroutine;

        public int CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                currentLevel = value;
                if (currentLevel > MaxLevel)
                    currentLevel = 0;
                if (currentLevel > playerData.highestLevelReached)
                    playerData.highestLevelReached = currentLevel;
            }
        }
        private int currentLevel = 0;
        private int MaxLevel = 10;

        public GamePlayMode CurrentGamePlayMode { 
            set { RegisterGameMode(value); } 
            get { return currentGamePlayMode; } }
        private GamePlayMode currentGamePlayMode;

        private static bool isGameEnded = false;
        private static GameManager instatnce;


        const string USRKEY = "Userdata";

        public void Awake()
        {
            if (instatnce == null)
            {
                input = new MyInput();
                input.Disable();
                DontDestroyOnLoad(gameObject);

                Application.targetFrameRate = 30;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;

                instatnce = this;
            } else if (instatnce != this) 
                Destroy(gameObject);

            ReadPlayerData();
            InitializeControlMode();
        }

        public void Initialize()
        {
            // All initializations will be done in OnEnable. This is just to Trigger the OnEnable Through singleton
        }
        private void InitializeControlMode()
        {
            if (!PlayerPrefs.HasKey(Controls.CONTROLLER_MODE))
                PlayerPrefs.SetInt(Controls.CONTROLLER_MODE, (int)Controls.SteeringControlMode.Wheel);
        }


        public void RegisterPlayer(GameObject player)
        {
            if(this.player != null)
                Destroy(this.player);

            this.player = player;
            OnPlayerChange?.Invoke();
        }
        public void UnRegisterPlayer(GameObject gameObject)
        {
            if (player == gameObject)
            {
                player = null;
            }
        }
        public IEnumerator WaitForPlayerEnumerator()
        {
            if (player == null)
                LoadPlayer();

            while (player == null)
            {
                yield return null;
            }
            yield return null;
        }

        private void RegisterGameMode(GamePlayMode gameMode)
        {
            currentGamePlayMode?.OnStop();
            if(gameMode)
                gameMode.OnStart();
            currentGamePlayMode = gameMode;
        }
        public void GameWon()
        {
            currentGamePlayMode?.Won();
        }
        public void GameLost()
        {
            currentGamePlayMode?.Failed();
        }


        public void ReadPlayerData()
        {
            playerData = new PlayerData(0);

            if (PlayerPrefs.HasKey(USRKEY))
            {
                string jsonData = PlayerPrefs.GetString(USRKEY);
                playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            }
        }
        public void WritePlayerData()
        {
            string jsonData = JsonUtility.ToJson(playerData, true);
            PlayerPrefs.SetString(USRKEY, jsonData);
            PlayerPrefs.Save();
        }

        public bool IsLoadingPlayer()
        {
            if (playerLoading_Coroutine == null)
                return false;
            else
                return true;
        }
        public bool UpdatePlayer(string vehicleName)
        {
            if (IsLoadingPlayer())
                return false;

            playerData.vehicleName = vehicleName;
            return UpdatePlayer();
        }
        public bool UpdatePlayer()
        {
            return LoadPlayer();
        }
        private bool LoadPlayer()
        {
            DestroyPlayer();

            if (playerLoading_Coroutine != null)
                return false;

            playerLoading_Coroutine = StartCoroutine(LoadPlayerFromFile());
            return true;    
        }
        private void DestroyPlayer()
        {
            if (player == null)
                return;

            Destroy(player);
            player = null;
        }

        private IEnumerator LoadPlayerFromFile()
        {
            yield return null;

            string path = "Assets/Player/" + playerData.vehicleName + ".prefab";
            GameObject playerObj;

            AsyncOperationHandle<GameObject> asyncOperation = Addressables.LoadAssetAsync<GameObject>(path);
            while(!asyncOperation.IsDone)
            {
                if(asyncOperation.Status == AsyncOperationStatus.Failed)
                {
                    yield break;
                }
                yield return null;
            }

            playerObj = asyncOperation.Result;

            // Load default player incase not found the saved one
            if (playerObj == null)
            {
                playerData.vehicleName = "Car_1_Player";
                path = "Assets/Player/" + playerData.vehicleName + ".prefab";
                
                asyncOperation = Addressables.LoadAssetAsync<GameObject>(path);
                while (!asyncOperation.IsDone)
                {
                    if (asyncOperation.Status == AsyncOperationStatus.Failed)
                    {
                        yield break;
                    }
                    yield return null;
                }
            }
            
            playerObj = asyncOperation.Result;
            yield return null;
            GameObject player = Instantiate(playerObj);
            player.SetActive(false);
            RegisterPlayer(player);

            Resources.UnloadUnusedAssets();
            ChangePlayerMaterial(PlayerData.vehicleColor);
            playerLoading_Coroutine = null;
        }
        public void ChangePlayerMaterial(PlayerData.VehicleColor vehicleColor)
        {
            if(vehicleColorChange_Coroutine != null)
                StopCoroutine(vehicleColorChange_Coroutine);

            vehicleColorChange_Coroutine = StartCoroutine(ChangePlayerMaterial_Coroutine(vehicleColor));
        }
        private IEnumerator ChangePlayerMaterial_Coroutine(PlayerData.VehicleColor vehicleColor)
        {
            playerData.vehicleColor = vehicleColor;
            
            yield return WaitForPlayerEnumerator(); 
            GameObject player = Instance.player;

            Material mat;
            string path = "Assets/Player/Materials/";
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
            path += materialName + ".mat";
            AsyncOperationHandle<Material> asyncOperation = Addressables.LoadAssetAsync<Material>(path);

            while (!asyncOperation.IsDone)
            {
                if (asyncOperation.Status == AsyncOperationStatus.Failed)
                    yield break;

                yield return null;
            }
            mat = asyncOperation.Result;

            if (player == null)
                yield break;

            PaintChanger paintChanger = player.GetComponent<PaintChanger>();
            if (paintChanger == null)
                yield break;

            paintChanger.ChangePaint(mat);
            vehicleColorChange_Coroutine = null;
            Resources.UnloadUnusedAssets();
        }

        private void OnDestroy()
        {
            if (instatnce == this)
                isGameEnded = true;

            WritePlayerData();
        }
    }
}
