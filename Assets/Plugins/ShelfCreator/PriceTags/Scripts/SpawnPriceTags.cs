using JDA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriceTags
{
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
    [ExecuteInEditMode]
#endif
    public class SpawnPriceTags : MonoBehaviour
    {
        public GameObject PriceTagPrefab;

        public void Start()
        {
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
#else
            AddEventHandlers();
#endif
        }

        public void AddEventHandlers()
        {
            PsaShelfBuilder.OnProductPositionObjectCreated += SpawnPriceTag;
        }

        public void OnDestroy()
        {
            PsaShelfBuilder.OnProductPositionObjectCreated -= SpawnPriceTag;
        }

        /// <summary>
        /// Given a particular position, create a price tag for that position and place it in the scene. This is called from PsaIntegrationLibrary when the shelf is being built.
        /// </summary>
        /// <param name="positionGameObject">The game object of the position for which to spawn a price tag.</param>
        /// <param name="planogramPsaData">The PSA data for the planogram this position is part of.</param>
        /// <param name="positionPsaData">The PSA data for this position.</param>
        /// <param name="productPsaData">The PSA data for the product this position is an instance of.</param>
        public void SpawnPriceTag(GameObject positionGameObject, PSA.Planogram planogramPsaData, PSA.Position positionPsaData, PSA.Product productPsaData)
        {
            // Create the price tag (it will position itself correctly when it is spawned)
            GameObject priceTag = Instantiate(PriceTagPrefab, positionGameObject.transform);
            PriceTag priceTagScript = priceTag.GetComponent<PriceTag>();

            // Apply the correct price to the tag
            string priceString = productPsaData.Fields[PSA.Product.FieldNames.Price];
            float price = 0;
            if (priceString != "")
            {
                price = float.Parse(priceString);
            }
            priceTagScript.SetPrice(price);

            if (priceTagScript != null)
            {
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
                priceTagScript.InitializePriceTag();
#else
                // Initializing the price tag will get taken care of in PriceTag's Start() method.
#endif
            }
        }
    }
}
