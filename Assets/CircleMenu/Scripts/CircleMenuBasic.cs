using UnityEngine;

namespace CircleMenu
{
    public class CircleMenuBasic : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField]
        private GameObject root;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = root.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0.0f;

            SetActive(false);
        }

        protected void SetActive(bool value)
        {
            root.SetActive(value);

            StopAllCoroutines();
            StartCoroutine(value
                ? Fade.In(canvasGroup, 1.0f, 0.5f)
                : Fade.Out(canvasGroup, 0.0f, 0.5f));
        }
    }
}