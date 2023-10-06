using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PriceTags
{
    public class PriceTags : ScriptableObject
    {
        private static GameObject instance;

        /// <summary>
        /// Creates PriceTags object and sets up price tags to be created with the shelf
        /// </summary>
        /// <returns></returns>
        public GameObject Create(Transform parent)
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load("PriceTags") as GameObject, parent);
                instance.transform.localPosition = new Vector3(0, 0, 0);
            }
            if (!instance.activeInHierarchy)
                instance.SetActive(true);

            return instance;
        }
    }
}
