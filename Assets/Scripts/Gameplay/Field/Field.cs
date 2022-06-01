using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using UnityEditor;
using UnityEngine;
using Util;
#if UNITY_EDITOR
#endif

namespace Gameplay.Field
{
    
    public class Field : MonoBehaviour
    {
        public int width;
        public int length;

        public Transform fieldParent;
        public BoxCollider mainCollider;
        public GameObject fieldObject;
        public LineRenderer lineRenderer;
        public SpriteRenderer spriteRenderer;
        public Transform moneyDisplayTransform;
        public TextMeshProUGUI moneyDisplayText;
        public SingleField[] fields;
        public int yRotation;

        private Coroutine _spawnRoutine;
        private WaitForSeconds _wait;
        private bool _unlocked;
        public int unlockPrice;
        public int tookMoney;
        public bool defaultUnlocked;
        private int _indexOfData;

        public void Initialize(bool unlocked, int tookM, int indexOfData)
        {
            fields = fieldParent.GetComponentsInChildren<SingleField>();
            
            _unlocked = unlocked;
            _indexOfData = indexOfData;
            tookMoney = tookM;
            
            
            
            ToggleUI(unlocked);
            ToggleFieldRenderers(unlocked);
            DisplayPriceText(unlocked);
            ToggleMainCollider(unlocked);
            
            if (!_unlocked) return;


            var childCount = fieldParent.childCount;
            var w = 20f / childCount;
            _wait = new WaitForSeconds(w);
            _spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        private void ToggleMainCollider(bool unlocked)
        {
            mainCollider.enabled = !unlocked;
        }

        private void DisplayPriceText(bool unlocked)
        {
            moneyDisplayText.text = !unlocked ? "$" + Extensions.AbbreviationUtility.AbbreviateNumber(unlockPrice-tookMoney) : string.Empty;
        }

        private void ToggleFieldRenderers(bool unlocked)
        {
            foreach (var singleField in fields)
            {
                singleField.Toggle(unlocked);
            }
        }

        private void ToggleUI(bool unlocked)
        {
            moneyDisplayTransform.gameObject.SetActive(!unlocked);
            spriteRenderer.enabled = !unlocked;
            lineRenderer.enabled = !unlocked;
        }


        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(1);
            while (true)
            {
                yield return new WaitUntil(() => fields.ToList().Find(field => field.growth == false));
                yield return _wait;
                var unGrowthSingleField = fields.ToList().Find(field => field.growth == false);
                unGrowthSingleField.Grow();
            }
        }

        public void TakeMoney(int amount)
        {
            if (_unlocked) return;
            if (tookMoney + amount >= unlockPrice)
            {
                _unlocked = true;
                tookMoney += amount;
                Unlock();
                return;
            }
            tookMoney += amount;
            DisplayPriceText(_unlocked);
            GameManager.Instance.SaveFieldData(_indexOfData,_unlocked,tookMoney);
            
        }

        private void Unlock()
        {
            Initialize(_unlocked,tookMoney,_indexOfData);
            GameManager.Instance.SaveFieldData(_indexOfData,_unlocked,tookMoney);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Field))]
    public class FieldEditor : Editor
    {
        private Field _target;


        private bool modify;
        private int width;
        private int length;
        private int yRotation;

        private bool canModify;

        private void OnEnable()
        {
            _target = (Field) target;
            if (Application.isPlaying) return;


            CheckPrefabInstance();
        }

        private void CheckPrefabInstance()
        {
            if (IsPrefabInstance(_target.gameObject))
            {
                if (_target.gameObject.scene.name == _target.gameObject.name)
                {
                    canModify = true;
                }
                else
                {
                    canModify = false;
                }
            }
            else
            {
                if (_target.gameObject.scene.IsValid() && _target.gameObject.scene.name == _target.gameObject.name)
                {
                    canModify = false;
                }
                else
                {
                    if (_target.gameObject.scene.IsValid())
                    {
                        canModify = true;
                    }

                    else
                    {
                        canModify = false;
                    }
                }
            }
        }

        bool IsPrefabInstance(GameObject go)
        {
            return PrefabUtility.GetCorrespondingObjectFromSource(go) != null && PrefabUtility.GetCorrespondingObjectFromSource(go) != null;
        }

        public override void OnInspectorGUI()
        {
            if (!canModify)
            {
                if (_target.gameObject.scene.IsValid() && _target.gameObject.scene.name != _target.gameObject.name)
                    EditorGUILayout.HelpBox("Please unpack prefab to modify field.", MessageType.Warning);
                return;
            }

            if (_target.spriteRenderer &&
                _target.lineRenderer &&
                _target.fieldObject &&
                _target.fieldParent &&
                _target.moneyDisplayTransform &&
                _target.moneyDisplayText &&
                _target.mainCollider)
            {
                modify = EditorGUILayout.Toggle("Modify Field", modify);
                EditorGUILayout.Space(15);
            }
            else
            {
                EditorGUILayout.HelpBox("Fill all the fields to modify field.", MessageType.Warning);
            }


            if (!modify)
            {
                EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
                DrawHorizontalLine(2);
                EditorGUILayout.Space(5);
                _target.spriteRenderer = (SpriteRenderer) EditorGUILayout.ObjectField("Sprite Renderer", _target.spriteRenderer, typeof(SpriteRenderer), true);
                _target.lineRenderer = (LineRenderer) EditorGUILayout.ObjectField("Line Renderer", _target.lineRenderer, typeof(LineRenderer), true);
                _target.fieldObject = (GameObject) EditorGUILayout.ObjectField("Field Prefab", _target.fieldObject, typeof(GameObject), true);
                _target.fieldParent = (Transform) EditorGUILayout.ObjectField("Field Transform", _target.fieldParent, typeof(Transform), true);
                _target.moneyDisplayTransform = (Transform) EditorGUILayout.ObjectField("Text Canvas", _target.moneyDisplayTransform, typeof(Transform), true);
                _target.moneyDisplayText = (TextMeshProUGUI) EditorGUILayout.ObjectField("Price Display Text", _target.moneyDisplayText, typeof(TextMeshProUGUI), true);
                _target.mainCollider = (BoxCollider) EditorGUILayout.ObjectField("Main Collider", _target.mainCollider, typeof(BoxCollider), true);
                EditorGUILayout.Space(25);

                EditorGUILayout.LabelField("Runtime", EditorStyles.boldLabel);
                DrawHorizontalLine(2);
                EditorGUILayout.Space(5);
                _target.unlockPrice = EditorGUILayout.IntField("Unlock Price", _target.unlockPrice);
                _target.defaultUnlocked = EditorGUILayout.Toggle("Default Unlocked", _target.defaultUnlocked);
            }


            if (modify)
            {
                _target.width = EditorGUILayout.IntSlider("Width", _target.width, 1, 25);
                _target.length = EditorGUILayout.IntSlider("Length", _target.length, 1, 25);
                _target.yRotation = EditorGUILayout.IntSlider("Y Rotation", _target.yRotation, 0, 90);
            }


            CreateField();
        }

        private void CreateField()
        {
            if (!modify ||
                (_target.width == width && _target.length == length && Math.Abs(yRotation - _target.yRotation) < 90) ||
                Application.isPlaying ||
                _target.fieldObject == null) return;

            if (_target.fieldParent.childCount > 0)
            {
                for (int f = _target.fieldParent.childCount - 1; f >= 0; f--)
                {
                    DestroyImmediate(_target.fieldParent.GetChild(f).gameObject);
                }
            }

            length = _target.length;
            width = _target.width;
            yRotation = _target.yRotation;

            var price = length * width;
            price *= 10;


            _target.unlockPrice = price;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    var ins = Instantiate(_target.fieldObject, _target.fieldParent);

                    var pos = new Vector3(x, -0.75f, z) + Vector3.left * (width / 2) + Vector3.back * (length / 2);
                    var euler = new Vector3(0, _target.yRotation, 0);
                    ins.name = _target.fieldObject.name;
                    ins.transform.localPosition = pos;
                    ins.transform.localEulerAngles = euler;
                    ins.isStatic = true;
                }
            }

            SetPositionsOfLine();
            SetScaleOfSprite();
            SetScaleOfMainCollider();
        }

        private void SetScaleOfMainCollider()
        {
            var bounds = CalculateBounds();

            _target.mainCollider.gameObject.transform.localScale = bounds;
            
            var isWidthEven = _target.width % 2 == 0;
            var isLengthEven = _target.length % 2 == 0;

            var colliderLocalPos = new Vector3(isWidthEven ? -0.5f : 0f, _target.mainCollider.transform.localPosition.y, isLengthEven ? -0.5f : 0f);
            
            _target.mainCollider.transform.localPosition = colliderLocalPos;
            
        }

        private Vector3 CalculateBounds()
        {
            var bounds = new Bounds(_target.fieldParent.transform.position, Vector3.one);
            var renderers = _target.fieldParent.GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds.size;
        }
        private void SetScaleOfSprite()
        {

            var bounds = CalculateBounds();

            _target.spriteRenderer.size = new Vector2(bounds.x, bounds.z ) *1.02f;

            var isWidthEven = _target.width % 2 == 0;
            var isLengthEven = _target.length % 2 == 0;

            var spriteLocalPos = new Vector3(isWidthEven ? -0.5f : 0f, _target.spriteRenderer.transform.localPosition.y, isLengthEven ? -0.5f : 0f);


            _target.spriteRenderer.transform.localPosition = spriteLocalPos;
            var localScaleOfMoneyDisplay = Vector3.zero;


            localScaleOfMoneyDisplay.x = bounds.x / 1.5f;
            localScaleOfMoneyDisplay.y = bounds.z / 1.5f;

            _target.moneyDisplayTransform.GetComponent<RectTransform>().sizeDelta = localScaleOfMoneyDisplay;

            var canvasLocalPos = new Vector3(isWidthEven ? -0.5f : 0f, _target.moneyDisplayTransform.localPosition.y, isLengthEven ? -0.5f : 0f);
            _target.moneyDisplayTransform.localPosition = canvasLocalPos;
        }

        private void SetPositionsOfLine()
        {
            if (_target.lineRenderer == null) return;

            _target.lineRenderer.positionCount = 4;

            var first = _target.fieldParent.GetChild(0).transform;
            var last = _target.fieldParent.GetChild(_target.fieldParent.childCount - 1).transform;

            _target.lineRenderer.SetPosition(0, first.position + new Vector3(-0.75f, 0.85f, -0.75f));
            _target.lineRenderer.SetPosition(1, new Vector3(first.position.x, first.position.y, last.position.z) + new Vector3(-0.75f, 0.85f, 0.75f));
            _target.lineRenderer.SetPosition(2, last.position + new Vector3(0.75f, 0.85f, 0.75f));
            _target.lineRenderer.SetPosition(3, new Vector3(last.position.x, first.position.y, first.position.z) + new Vector3(0.75f, 0.85f, -0.75f));

            var localPositionOfLineRenderer = new Vector3(-_target.transform.position.x, 0, -_target.transform.position.z);

            _target.lineRenderer.transform.localPosition = localPositionOfLineRenderer;
        }

        void DrawHorizontalLine(int height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, new Color(1, 1, 1, 1));
        }
    }
#endif
}