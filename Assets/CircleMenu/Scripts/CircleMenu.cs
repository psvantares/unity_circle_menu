using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace CircleMenu
{
    public class CircleMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField, Range(0f, 360f)]
        private float angle;

        [SerializeField, Range(-360f, 360f)]
        private float angleOffset = 45.0f;

        [SerializeField, Range(0.01f, 1f)]
        private float spring = 0.3f;

        [SerializeField, Range(1f, 2f)]
        private float selectedScale = 1.2f;

        [SerializeField, Range(0.1f, 0.5f)]
        private float sensitivity = 0.25f;

        [SerializeField]
        private List<GameObject> items;

        public event Action<MenuType> OnClick;

        private Canvas canvas;
        private RadialLayout radialLayout;

        private int inner;
        private int count;

        private Vector2 dragStart;
        private Vector2 dragEnd;
        private Vector2 BasePos => transform.position;

        private const float MIN_ANGLE = 0.03f;
        private float radius = 100.0f;
        private float startAngle;
        private float endAngle;
        private bool isDragging;

        public int SelectedIndex
        {
            private get
            {
                if (count == 0)
                {
                    return 0;
                }

                var ret = (count - inner) % count;
                return ret;
            }
            set
            {
                var cnt = count;
                if (cnt == 0)
                {
                    return;
                }

                inner = (inner + cnt - value) % cnt;
            }
        }

        private static float GetAngle(Vector2 p1, Vector2 p2)
        {
            return -Mathf.Atan2(p1.y - p2.y, p1.x - p2.x);
        }

        private float DiffAngleDeg
        {
            get
            {
                var diffAngle = (endAngle - startAngle) * Mathf.Rad2Deg;

                if (diffAngle >= 180f)
                {
                    diffAngle -= 360f;
                }

                if (diffAngle <= -180f)
                {
                    diffAngle += 360f;
                }

                return diffAngle;
            }
        }

        private float Angle
        {
            get => angle;
            set
            {
                angle = value;
                UpdatePosition(angle);
            }
        }

        private GameObject SelectedItem
        {
            get
            {
                if (items == null)
                {
                    return null;
                }

                var index = SelectedIndex;

                if (index < 0 || index >= count)
                {
                    return null;
                }

                return items[index];
            }
        }

        private void Awake()
        {
            canvas = gameObject.GetComponentInParent<Canvas>();
            radialLayout = GetComponent<RadialLayout>();
            radius = radialLayout.Radius;
            count = items.Count;
        }

        private void Start()
        {
            UpdatePosition(angle);
        }

        private void Update()
        {
            if (isDragging)
            {
                return;
            }

            if (count <= 1)
            {
                return;
            }

            var currentAngle = angle;
            var targetAngle = 360f / count * inner;

            if (targetAngle >= 360f)
            {
                targetAngle -= 360f;
            }

            if (targetAngle < 0f)
            {
                targetAngle += 360f;
            }

            if (currentAngle == targetAngle || targetAngle - currentAngle == 360f)
            {
                return;
            }

            if (currentAngle < 90f && targetAngle > 270f)
            {
                currentAngle += 360f;
            }

            var diff = Mathf.Abs(targetAngle - currentAngle);

            if (diff < MIN_ANGLE)
            {
                currentAngle = targetAngle;
            }
            else
            {
                if (diff >= 180f)
                {
                    targetAngle += 360f;
                }

                currentAngle = targetAngle * spring + currentAngle * (1f - spring);
            }

            Angle = currentAngle;
        }

        private void OnEnable()
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                item.GetComponent<CircleMenuButton>().OnClick += OnOnClick;
            }
        }

        private void OnDisable()
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                item.GetComponent<CircleMenuButton>().OnClick -= OnOnClick;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, canvas.worldCamera, out var worldPoint);

            dragStart = dragEnd = worldPoint;
            startAngle = GetAngle(BasePos, dragStart);
            endAngle = GetAngle(BasePos, dragEnd);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, canvas.worldCamera, out var worldPoint);

            dragEnd = worldPoint;
            startAngle = GetAngle(BasePos, dragStart);
            endAngle = GetAngle(BasePos, dragEnd);
            UpdatePosition(Angle + DiffAngleDeg);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, canvas.worldCamera, out var worldPoint);

            dragEnd = worldPoint;
            startAngle = GetAngle(BasePos, dragStart);
            endAngle = GetAngle(BasePos, dragEnd);
            angle += DiffAngleDeg;

            while (angle > 360f)
            {
                angle -= 360f;
            }

            while (angle < 0f)
            {
                angle += 360f;
            }

            UpdatePosition(angle);

            var region = 360f / (count * 2);
            var indexCnt = (int)(angle / region);
            var nearIndex = indexCnt / 2 + (indexCnt % 2 == 0 ? 0 : 1);
            nearIndex %= count;
            inner = nearIndex;

            if (startAngle > endAngle)
            {
                var result = startAngle - endAngle;

                if (result > sensitivity && result < 1.0f)
                {
                    inner--;
                }
            }
            else if (startAngle < endAngle)
            {
                var result = endAngle - startAngle;

                if (result > sensitivity && result < 1.0f)
                {
                    inner++;
                }
            }
        }

        public void Focus(int index)
        {
            if (index >= count)
            {
                index %= count;
            }

            while (index < 0)
            {
                index += count;
            }

            SelectedIndex = index;

            var currentAngle = 0f;

            if (count != 0)
            {
                currentAngle = 360f / count * index;
            }

            Angle = currentAngle;

            if (SelectedItem == null || items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                if (item == SelectedItem)
                {
                    OnSelect(item);
                }
                else
                {
                    OnInSelect(item);
                }
            }
        }

        private void UpdatePosition(float diffAngle)
        {
            var selectedItem = SelectedItem;

            for (var i = 0; i < count; i++)
            {
                if (items[i] == null)
                {
                    continue;
                }

                if (count == 0)
                {
                    continue;
                }

                var currentAngle = diffAngle + 360f / count * i + angleOffset;

                if (currentAngle < 0)
                {
                    currentAngle += 360f;
                }

                var current = items[i];
                var vPos = new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0);
                current.transform.localPosition = vPos * radius;

                if (items[i] == selectedItem)
                {
                    OnSelect(items[i]);
                }
                else
                {
                    OnInSelect(items[i]);
                }
            }
        }

        // Events

        private void OnSelect(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            obj.transform.localScale = Vector3.one * selectedScale;

            foreach (var btn in obj.GetComponentsInChildren<Button>(true))
            {
                btn.interactable = true;
                var element = btn.GetComponent<CircleMenuButton>();
                element.SetColor(true);
            }
        }

        private static void OnInSelect(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            obj.transform.localScale = Vector3.one;

            foreach (var btn in obj.GetComponentsInChildren<Button>(true))
            {
                btn.interactable = false;
                var element = btn.GetComponent<CircleMenuButton>();
                element.SetColor(false);
            }
        }

        private void OnOnClick(MenuType menuType)
        {
            OnClick?.Invoke(menuType);
        }
    }
}