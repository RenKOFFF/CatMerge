using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void DestroyChildren(this Component parent)
            => parent.gameObject.DestroyChildren();

        public static void DestroyChildren(this GameObject parent)
        {
            var previousImages = new List<GameObject>();

            for (var i = 0; i < parent.transform.childCount; i++)
                previousImages.Add(parent.transform.GetChild(i).gameObject);

            foreach (var previousImage in previousImages)
                Object.Destroy(previousImage);
        }
    }
}
