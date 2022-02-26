using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

// THIS IS A FUCKING MESS
namespace EndlessGames
{
    public class GlobalGameManager : MonoBehaviour
    {
        public static GlobalGameManager instance { get; set; }
    
        static int loadedScene;
        static float fadeInDur;
        static float stayDur;
        static float fadeOutDir;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
            loadedScene = 0;
            fadeInDur = 0;
            stayDur = 0;
            fadeOutDir = 0;
        }
    
        public void Awake()
        {
            Init();
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    
        private static GameObject CreateFade()
        {
            GameObject fade = new GameObject();
            DontDestroyOnLoad(fade);
            fade.transform.localScale = new Vector3(4000, 4000);
            SpriteRenderer sr = fade.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("Sprites/GeneralPurpose/Square");
            sr.sortingOrder = 20000;
            fade.layer = 5;
            return fade;
        }
    
    
        private static void BasicCheck()
        {
            if (FindGGM() == null)
            {
                GameObject GlobalGameManager = new GameObject("GlobalGameManager");
                GlobalGameManager.name = "GlobalGameManager";
                GlobalGameManager.AddComponent<GlobalGameManager>();
            }
        }
    
        private static GameObject FindGGM()
        {
            if (GameObject.Find("GlobalGameManager") != null)
                return GameObject.Find("GlobalGameManager");
            else
                return null;
        }
    
        public static void LoadScene(int sceneIndex, float fadeIn = 0.35f, float stay = 0, float fadeOut = 0.35f)
        {
            BasicCheck();
            loadedScene = sceneIndex;
            fadeInDur = fadeIn;
            stayDur = stay;
            fadeOutDir = fadeOut;
    
            DOTween.Clear(true);
            GlobalGameManager.instance.LoadSc();
        }

        public void LoadSc()
        {
            StartCoroutine(LoadSce());
        }

        private static IEnumerator LoadSce()
        {
            GameObject fade = CreateFade();
            fade.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            fade.GetComponent<SpriteRenderer>().DOColor(Color.black, fadeInDur);
            yield return new WaitForSeconds(fadeInDur);
            yield return new WaitForSeconds(stayDur);
            SceneManager.LoadScene(loadedScene); 
            fade.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), fadeOutDir).OnComplete(() => { Destroy(fade); });
        }
    }
}
