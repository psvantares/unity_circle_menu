using UnityEngine;
using UnityEngine.UI;

namespace CircleMenu
{
    public class CircleMenuSpriteSwipe : MonoBehaviour
    {
        [SerializeField]
        private Image showIcon;

        [SerializeField]
        private Image hideIcon;

        private bool isShow;

        private void Start()
        {
            var button = GetComponent<Button>();

            hideIcon.gameObject.SetActive(false);
            showIcon.gameObject.SetActive(true);

            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    if (isShow)
                    {
                        isShow = false;

                        hideIcon.gameObject.SetActive(false);
                        showIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        isShow = true;

                        showIcon.gameObject.SetActive(false);
                        hideIcon.gameObject.SetActive(true);
                    }
                });
            }
        }
    }
}