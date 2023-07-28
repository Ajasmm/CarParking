using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

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
        public GameObject player { get; private set; }
        private bool isPlayerAvailable = false;
        public MyInput input;

        public PlayerData PlayerData { get { return playerData; } }
        private PlayerData playerData;

        Coroutine loadPlayer;

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

        private AudioMixer audioMixer;


        const string USRKEY = "Userdata";

        Queue<KeyValuePair<string, float>> audioCommands = new Queue<KeyValuePair<string, float>>();

        private float saveInterval;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

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

            StartCoroutine(GetAudioMixer());
            ReadPlayerData();
            InitializeControlMode();
        }

        public void Access() { }
        public void InitializeControlMode()
        {
            if (!PlayerPrefs.HasKey(Controls.CONTROLLER_MODE))
                PlayerPrefs.SetInt(Controls.CONTROLLER_MODE, (int)Controls.SteeringControlMode.Wheel);
        }

        private void Update()
        {
            saveInterval += Time.unscaledDeltaTime;
            if (saveInterval > 2)
            {
                WritePlayerData();
                saveInterval = 0;
            }
        }

        private IEnumerator GetAudioMixer()
        {
            string path = "Assets/Audio/GlobalAudioMixer.mixer";
            AsyncOperationHandle<AudioMixer> asyncOperation = Addressables.LoadAssetAsync<AudioMixer>(path);
            while(!asyncOperation.IsDone && asyncOperation.Status != AsyncOperationStatus.Failed)
            {
                yield return null;
            }
            audioMixer = asyncOperation.Result;
            foreach(KeyValuePair<string, float> command in audioCommands)
                audioMixer.SetFloat(command.Key, command.Value);
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
            if (!isPlayerAvailable)
                LoadPlayer();

            Task task = new Task(() =>
            {
                while (!isPlayerAvailable)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested) return;
                }
            }, cancellationTokenSource.Token);

            task.Start();
            return task;
        }
        public IEnumerator WaitForPlayerEnumerator()
        {
            if (!isPlayerAvailable)
                LoadPlayer();

            while (!isPlayerAvailable)
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
            if(currentLevel > MaxLevel) currentLevel = MaxLevel;

            if (playerData.highestLevelReached < currentLevel)
                playerData.highestLevelReached = currentLevel;

            string jsonData = JsonUtility.ToJson(playerData, true);
            PlayerPrefs.SetString(USRKEY, jsonData);
            PlayerPrefs.Save();
        }

        public void UpdatePlayer()
        {
            if (player != null)
            {
                GameObject tempObj = player;
                player = null;
                Destroy(tempObj);
            }
            StartCoroutine(LoadPlayerFromFile());
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
            StartCoroutine(LoadPlayerFromFile());
        }
        public void LoadPlayer()
        {
            if (player != null || loadPlayer != null) return;
            loadPlayer = StartCoroutine(LoadPlayerFromFile());
        }

        private IEnumerator LoadPlayerFromFile()
        {
            string path = "Assets/Player/" + playerData.vehicleName + ".prefab";
            GameObject playerObj;

            AsyncOperationHandle<GameObject> asyncOperation = Addressables.LoadAssetAsync<GameObject>(path);
            while(!asyncOperation.IsDone)
            {
                if(asyncOperation.Status == AsyncOperationStatus.Failed)
                {
#if UNITY_EDITOR
                    Debug.Log("Error while loading player from file.");
#endif
                    yield break;
                }
                yield return null;
            }

            playerObj = asyncOperation.Result;

            if (playerObj == null)
            {
                playerData.vehicleName = "Car_1_Player";
                path = path = "Assets/Player/" + playerData.vehicleName + ".prefab";
                
                asyncOperation = Addressables.LoadAssetAsync<GameObject>(path);
                while (!asyncOperation.IsDone)
                {
                    if (asyncOperation.Status == AsyncOperationStatus.Failed)
                    {
#if UNITY_EDITOR
                        Debug.Log("Error while loading player from file.");
#endif
                        yield break;
                    }
                    yield return null;
                }
            }
            
            playerObj = asyncOperation.Result;

            playerObj = Instantiate<GameObject>(playerObj);
            playerObj.SetActive(true);

            StartCoroutine(ChangePlayerMaterialAsync(PlayerData.vehicleColor));
        }
        public IEnumerator ChangePlayerMaterial(PlayerData.VehicleColor vehicleColor)
        {
            yield return null;
            playerData.vehicleColor = vehicleColor;

            GameObject player = GameManager.Instance.player;

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

            while(!asyncOperation.IsDone && asyncOperation.Status != AsyncOperationStatus.Failed)
            {
                yield return null;
            }
            mat = asyncOperation.Result;

            if (player == null) yield break;

            PaintChanger paintChanger = player.GetComponent<PaintChanger>();
            if (paintChanger == null) yield break;

            paintChanger.ChangePaint(mat);
        }
        public IEnumerator ChangePlayerMaterialAsync(PlayerData.VehicleColor vehicleColor)
        {
            yield return WaitForPlayerEnumerator();
            StartCoroutine(ChangePlayerMaterial(vehicleColor));
        }

        public void SetVehicleSoundToLow()
        {
            SetAudioMixerParameter("Vehicle Volume", -80);
        }
        public void SetVehicleSoundToHigh()
        {
            SetAudioMixerParameter("Vehicle Volume", 0);
        }

        public void SetMenuSoundToLow()
        {
            SetAudioMixerParameter("Menu Volume", -80);
        }
        public void SetMenuSoundToHigh()
        {
            SetAudioMixerParameter("Menu Volume", 0);
        }
        private void SetAudioMixerParameter(string parameter, float value)
        {
            if(audioMixer == null)
            {
                KeyValuePair<string, float> audioCommand = new KeyValuePair<string, float>(parameter, value);
                audioCommands.Enqueue(audioCommand);
            }
            else
                audioMixer.SetFloat(parameter, value);
        }

        private void OnDestroy()
        {
            if (instatnce == this) isGameEnded = true;
            cancellationTokenSource.Cancel();
            WritePlayerData();
            SetVehicleSoundToLow();
            SetMenuSoundToLow();
        }
    }
}
