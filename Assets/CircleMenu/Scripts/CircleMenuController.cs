using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class CircleMenuController : CircleMenuBasic
    {
        [SerializeField]
        private CircleMenu circleMenu;

        [SerializeField]
        private Button menuButton;

        [SerializeField]
        private Text menuText;

        private bool isShow = true;

        private void Start()
        {
            menuButton.onClick.AddListener(() =>
            {
                if (isShow)
                {
                    isShow = false;
                    SetActive(true);
                    circleMenu.Focus(0);
                }
                else
                {
                    isShow = true;
                    SetActive(false);
                }
            });

            circleMenu.OnClick += OnClick;
        }

        // Events

        private void OnClick(MenuType menuType)
        {
            if (menuText != null)
            {
                menuText.text = menuType.ToString();
            }
        }
    }
}