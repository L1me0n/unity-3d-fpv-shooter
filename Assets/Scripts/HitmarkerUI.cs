using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaSurvival.UI
{
    public class HitmarkerUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private float showTime = 0.06f;

        private Coroutine routine;

        private void Awake()
        {
            if (image == null) image = GetComponent<Image>();
            SetAlpha(0f);
        }

        public void Show()
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            SetAlpha(1f);
            yield return new WaitForSeconds(showTime);
            SetAlpha(0f);
            routine = null;
        }

        private void SetAlpha(float a)
        {
            Color c = image.color;
            c.a = a;
            image.color = c;
        }
    }
}
