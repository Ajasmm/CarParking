using Ajas.FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] float loadingTime;
    [SerializeField] Slider loadingSlider;

    float tempTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CurrentLevel = 0;

        tempTime = 0;
    }

    private void Update()
    {
        tempTime += Time.deltaTime;
        loadingSlider.value = tempTime / loadingTime;

        if (tempTime >= loadingTime)
        {
            SceneManager.LoadSceneAsync(1);
            this.enabled = false;
        }
    }
}
