using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    public static class Extensions
    {

        public static Vector3 FindCenterOfTransforms(List<Transform> transforms)
        {
            var bound = new Bounds(transforms[0].position, Vector3.zero);
            for (int i = 1; i < transforms.Count; i++)
            {
                bound.Encapsulate(transforms[i].position);
            }

            return bound.center;
        }
        public static Vector3 FindCenterOfTransforms(Transform[] transforms)
        {
            var bound = new Bounds(transforms[0].position, Vector3.zero);
            for (int i = 1; i < transforms.Length; i++)
            {
                bound.Encapsulate(transforms[i].position);
            }

            return bound.center;
        }
        
        public static bool IsPointerOverUIObject()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        
        public static void ChangeLayer(GameObject gameobject, int layer)
        {
            gameobject.layer = layer;
        }


        public static void ChangeLayer(GameObject gameobject,string layer)
        {
            gameobject.layer = LayerMask.NameToLayer(layer);
        }

        public static bool IsAllBooleansTrue(bool[] booleans)
        {
            return booleans.All(t => t != false);
        }

        public static class AbbreviationUtility
        {
            private static readonly SortedDictionary<int, string> Abbreviations = new SortedDictionary<int, string>
            {
                {1000,"K"},
                {1000000, "M" },
                {1000000000, "B" }
            };
 
            public static string AbbreviateNumber(float number)
            {
                for (int i = Abbreviations.Count - 1; i >= 0; i--)
                {
                    var pair = Abbreviations.ElementAt(i);
                    if (number >= pair.Key)
                    {
                        
                        var roundedNumber = number / pair.Key;
                        return roundedNumber.ToString("0.0") + pair.Value;
                    }
                }
                return number.ToString("0");
            }
        }
        

        public static List<GameObject> SortByDistance(List<GameObject> objects, Vector3 measureFrom)
        {
            return objects.OrderBy(x => Vector3.Distance(x.transform.position, measureFrom)).ToList();
        }
        
       
    }
}

