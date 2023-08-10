using System;
using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class CircleMenuButton : MonoBehaviour
    {
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private MenuType menuType;

        public event Action<MenuType> OnClick;

        private void Start()
        {
            var button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => { OnClick?.Invoke(menuType); });
            }
        }

        public void SetColor(bool active)
        {
            iconImage.color = active ? Color.white : Color.gray;
        }
    }
}