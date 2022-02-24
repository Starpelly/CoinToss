using EndlessGames.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EndlessGames
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu instance;

        public List<GameObject> buttons;
        public GameObject selection;
        public GameObject preSelection;

        [SerializeField] private Animator previewAnim;

        private void Awake()
        {
            instance = this;
            for (int i = 1; i < 6; i++)
            {
                GameObject button = Instantiate(buttons[0]);
                button.transform.SetParent(buttons[0].transform.parent);
                button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 0.6875005f * i);
                button.GetComponent<MainMenuButton>().index = i;
            }

            SelectionUp(buttons[0]);
        }

        public void SelectionDown(GameObject button)
        {
            preSelection.SetActive(true);
            Jukebox.PlayOneShot("mainmenu_gameselect_button_press");
        }

        public void SelectionUp(GameObject button)
        {
            preSelection.SetActive(false);
            selection.SetActive(true);
            selection.transform.position = button.transform.position;
            PreviewAnim(button.GetComponent<MainMenuButton>().index);
            Jukebox.PlayOneShot("mainmenu_gameselect_button_release");
        }

        public void SelectionEnter(GameObject button)
        {
            preSelection.SetActive(true);
        }

        public void SelectionExit(GameObject button)
        {
            preSelection.SetActive(false);
        }

        public void SetSelectionPos(Vector2 pos)
        {
            preSelection.transform.position = pos;
        }

        public void PreviewAnim(int index)
        {
            switch (index)
            {
                case 0:
                    previewAnim.Play("Previews_Cointoss", 0, 0);
                    break;
                case 1:
                    previewAnim.Play("Previews_Shootemup", 0, 0);
                    break;
                case 2:
                    previewAnim.Play("Previews_Tunnel", 0, 0);
                    break;
                case 3:
                    previewAnim.Play("Previews_SamuraiSlice", 0, 0);
                    break;
                case 4:
                    previewAnim.Play("Previews_GlassTappers", 0, 0);
                    break;
                case 5:
                    previewAnim.Play("Previews_Rhythmove", 0, 0);
                    break;
            }
        }
    }
}