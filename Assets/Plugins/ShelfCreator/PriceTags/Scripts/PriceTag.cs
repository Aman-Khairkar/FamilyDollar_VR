using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriceTags
{
    public class PriceTag : MonoBehaviour
    {

        public delegate GameObject OurShelf(GameObject ourProduct);
        public delegate bool IsHangingProduct(GameObject ourProduct);

        public static OurShelf FindShelf;
        public static IsHangingProduct FindIsHangingProduct;
        public bool isHangingProduct = false;

        public GameObject TagObject;
        public TextMesh PriceText;

        private bool updatingPosition = false;

        //Is the product deleted or placed in parking lot
        public bool productHidden;

        private void Start()
        {
#if ALLOW_SHELF_BUILD_IN_EDITOR
#else
            InitializePriceTag();
#endif
        }

        public void InitializePriceTag()
        {
            // Add our price tag to the manager's list
            PriceTagManager.Instance.PriceTags.Add(this);

            // Make sure the price tag starts in the correct position
            StartCoroutine(DelayedRefreshPosition());
            
            // Hide this price tag if price tags are currently hidden
            StartCoroutine(UpdateVisibility());

            //Check if this product's tag should be togglable or is considered hidden
            CheckProductActive();
        }

        /// <summary>
        /// Set the text on the price tag.
        /// </summary>
        /// <param name="price">The new price to display on the pricetag</param>
        public void SetPrice(float price)
        {
            PriceText.text = string.Format("${0:0.00}", price);

            // If the price is zero
            if (price == 0)
            {
                // Then just get rid of this gameObject altogether
                PriceTagManager.Instance.PriceTags.Remove(this);
#if UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
                DestroyImmediate(gameObject);
#else
                Destroy(gameObject);
#endif
            }
        }

        public void RefreshPosition()
        {
            Collider positionCollider = transform.parent.GetComponent<Collider>();
            
            Vector3 productSize = Vector3.zero;
            if(positionCollider != null && positionCollider.bounds.size != Vector3.zero)
            {
                productSize = positionCollider.bounds.size;
            }
            else
            {
                ProductInformation prodInfo = transform.parent.GetComponent<ProductInformation>();
                // if the collider's bounds size is 0 it likely means that it is deleted or parked 
                // in which case use the ProductInformation in PsaIntegrationLibrary to attain the actual size
                if (prodInfo != null)
                    productSize = prodInfo.Size;
            }

            // Get the product and shelf corresponding to this price tag
            GameObject ourProduct = transform.parent.gameObject;
            //Find our Shelf
            GameObject ourShelf = null;
            if (FindShelf != null)
            {
                ourShelf = FindShelf(ourProduct);
            }

            // Figure out the correct position for the price tag's parent and child game objects
            Vector3 tagContainerLocalPosition = PriceTagPosition(ourProduct, ourShelf, productSize);
            Vector3 tagChildLocalPosition = TagObject.transform.localPosition;

            // Determine whether this is a hanging product
            isHangingProduct = false;

            if (FindIsHangingProduct != null)
            {
                isHangingProduct = FindIsHangingProduct(transform.parent.gameObject);
            }

            // Adjust position based on whether this is a hanging product
            if (isHangingProduct)
            {
                //tag is above product and is flush with the product's front
                tagContainerLocalPosition.y = positionCollider.bounds.size.y;
                tagContainerLocalPosition.z = -(productSize.z + 0.005f);

                // Anchor the tag from the bottom
                tagChildLocalPosition.y = 0.5f;
            }
            else if (ourShelf != null)
            {
                // Anchor the tag in the center
                tagChildLocalPosition.y = 0;
            }
            else
            {
                // Anchor the tag from the top
                tagChildLocalPosition.y = -0.5f;
            }

            // Apply the new positions
            transform.localPosition = tagContainerLocalPosition;
            TagObject.transform.localPosition = tagChildLocalPosition;
        }

        /// <summary>
        /// Find the position to place the price tag so that it is in front of the shelf its product sits on
        /// </summary>
        public Vector3 PriceTagPosition(GameObject ourProduct, GameObject ourShelf, Vector3 productSize)
        {
            float shelfPosZ = 0;
            float productPosZ = 0;

            float tagY = 0;

            //space between back of shelf and back of product
            float backSpace = 0;
            //space between front of shelf and front of product
            float frontSpace = 0;         

            if (ourShelf != null)
            {
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR && ALLOW_SHELF_BUILD_IN_EDITOR
                shelfPosZ = ourShelf.transform.localPosition.z + .5f * ourShelf.transform.localScale.z;
#else
                shelfPosZ = ourShelf.transform.position.z + .5f * ourShelf.transform.localScale.z;
#endif
                productPosZ = ourProduct.transform.position.z;

                //Find how much space is between the back of the shelf and the product
                backSpace = Mathf.Abs(shelfPosZ) - Mathf.Abs(productPosZ);
                //Find how much space is between the front of the shelf and the product
                frontSpace = ourShelf.transform.localScale.z - productSize.z - backSpace;

                // Set the tag placement to be halfway down the shelf
                tagY = -0.5f * ourShelf.transform.localScale.y;
            }

            Vector3 priceTagPos;
            //Determine how much in front of product the price tag needs to be to be in front of the shelf
            priceTagPos = new Vector3(productSize.x / 2, tagY, -(productSize.z + frontSpace + .005f));
            return priceTagPos;
        }

        /// <summary>
        /// Wait a moment so that the product gets associated with the correct shelf, then refresh the price tag position.
        /// </summary>
        /// <returns></returns>
        public IEnumerator DelayedRefreshPosition()
        {
            updatingPosition = true;
            yield return new WaitForSeconds(0.5f);
            RefreshPosition();
            updatingPosition = false;
            yield return null;
            StopCoroutine(DelayedRefreshPosition());
        }

        public IEnumerator UpdateVisibility()
        {
            yield return new WaitUntil(DoneUpdatingPosition);

            // Hide this price tag if price tags are currently hidden
            TagObject.SetActive(PriceTagManager.Instance.ShowPriceTags);

            yield return null;
        }

        /// <summary>
        /// Set the abilty for this price tag to be viewable (when switching on and off price tags)
        /// </summary>
        /// <param name="viewable">When the user toggles visibility of price tags this determines if this price tag is responsive</param>
        public void ResetPriceTagViewability(bool viewable)
        {
            if (viewable)
            {
                PriceTagManager.Instance.PriceTags.Add(this);
                TagObject.SetActive(PriceTagManager.Instance.ShowPriceTags);
            }
            else
            {                  
                PriceTagManager.Instance.PriceTags.Remove(this);
                if (PriceTagManager.Instance.ShowPriceTags)
                    TagObject.SetActive(false);
            }
            productHidden = !viewable;
        }

        private bool DoneUpdatingPosition()
        {
            return !updatingPosition;
        }

        /// <summary>
        /// Check whether the price tag should be able to be toggled into view (we don't want it visible if the product isn't)
        /// </summary>
        public void CheckProductActive()
        {
            //Detect if the product is visible by using the product rows 
            for (int i = 1; i < transform.parent.childCount; i++)//skips over default mesh renderer
            {
                MeshRenderer productRow = transform.parent.GetChild(i).GetComponent<MeshRenderer>();

                if (productRow != null)
                {
                    //if product is visible then this price tag is not hidden (and would therefore be togglable)
                    productHidden = !productRow.enabled;
                    break;
                }
            }
        }
    }   
}
