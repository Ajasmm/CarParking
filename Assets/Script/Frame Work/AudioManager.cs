using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] string menu_SnapshotName = "Menu";
    [SerializeField] string gameplay_SnapshotName = "Gameplay";

    AudioMixerSnapshot[] snapshots;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        MakeSingleton();
        GetSnapshots();
    }


    private void MakeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetSnapshots()
    {
        snapshots = new AudioMixerSnapshot[2];
        snapshots[0] = mainMixer.FindSnapshot(menu_SnapshotName);
        snapshots[1] = mainMixer.FindSnapshot(gameplay_SnapshotName);
    }


    public void ChangeState(AudioState currentState, float duration)
    {
        float[] weights = new float[2];
        switch(currentState)
        {
            case AudioState.Menu:
                weights[0] = 1.0f;
                weights[1] = 0.0f;
                break;
            case AudioState.Gameplay:
                weights[0] = 0;
                weights[1] = 1.0f;
                break;
        }

        mainMixer.TransitionToSnapshots(snapshots, weights, duration);
    }
}
