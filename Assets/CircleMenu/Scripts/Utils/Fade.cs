using System.Collections;
using UnityEngine;

namespace CircleMenu
{
    public static class Fade
    {
        public static IEnumerator In(CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }

        public static IEnumerator Out(CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }
    }
}