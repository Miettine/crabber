using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader> {

    [SerializeField]
    string EasyButtonGameObjectName = "EasyButton";
    [SerializeField]
    string HardButtonGameObjectName = "HardButton";

    [SerializeField]
    string easySceneName = "Tiny lake";
    [SerializeField]
    string hardSceneName = "Tiny lake - hard";

    void Awake()
    {
        GameObject.Find(EasyButtonGameObjectName).GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(easySceneName));
        GameObject.Find(HardButtonGameObjectName).GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(hardSceneName));
    }
}
