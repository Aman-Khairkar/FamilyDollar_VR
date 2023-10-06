using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PriceTags
{
    public class PriceTagManager : MonoBehaviour
    {
        private static PriceTagManager instance;
        public static PriceTagManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PriceTagManager>();
                }
                return instance;
            }
        }

        public delegate void ToggleEvent();
        public static ToggleEvent OnToggle;

        public delegate float PriceTagScale();
        public static PriceTagScale GetPriceTagScale;
        public float priceTagScaler = 1;

        public bool ShowPriceTags = false;

        public List<PriceTag> PriceTags = new List<PriceTag>();

        public GameObject PriceTagPopupPrefab;
        public GameObject PriceTagPopup;
        public Text PopupText;

        private void Start()
        {
            // If it's possible that price tags already existed before the game was started (i.e., shelves can be built in the editor)
            // Note that this doesn't require us to actually be using the editor right now, since we'd want this to happen even if shelves
            //   were built in the editor and then the project was built to a standalone.
#if ALLOW_SHELF_BUILD_IN_EDITOR
            // Find all the price tags that are in the scene already and add them to our list
            PriceTags.AddRange(FindObjectsOfType<PriceTag>());
            if (!ShowPriceTags)
            {
                TogglePriceTags();
            }
#else
            DontDestroyOnLoad(gameObject);
#endif
        }

        /// <summary>
        /// Scale the price tags properly according to the scale of their shelf (adjusting for metric) and adjust its position accordingly
        /// </summary>
        public void ScaleTags()
        {
            if (GetPriceTagScale != null)
                priceTagScaler = GetPriceTagScale();

            foreach(PriceTag pTag in PriceTags)
            {
                pTag.transform.localScale = new Vector3(priceTagScaler * pTag.transform.localScale.x, 
                                                        priceTagScaler * pTag.transform.localScale.y, 
                                                        PriceTagPopupPrefab.transform.localScale.z);
            }
        }

        /// <summary>
        /// Clears out the data for current price tags. Used when creating or loading a new project to ensure settings aren't carried over between projects.
        /// </summary>
        public void ResetSettings()
        {
            PriceTags.Clear();
            ShowPriceTags = false;
        }

        public void TogglePriceTags()
        {
            SetPriceTagsEnabled(!ShowPriceTags);
        }

        public void SetPriceTagsEnabled(bool enabled)
        {
            ShowPriceTags = enabled;

            List<PriceTag> hiddenTags = new List<PriceTag>();

            if(PriceTags.Count > 0)
            {
                if(OnToggle != null)
                    OnToggle();

                foreach (PriceTag priceTag in PriceTags)
                {               

                    if (priceTag != null)
                    {
                        priceTag.CheckProductActive();
                        //if the product isn't hidden the price tag is toggleable
                        if (!priceTag.productHidden)
                            priceTag.TagObject.SetActive(ShowPriceTags);
                        else
                            hiddenTags.Add(priceTag);
                    }
                }

                //keep hidden products' price tags hidden and not viewable
                foreach(PriceTag tag in hiddenTags)
                {
                    PriceTags.Remove(tag);
                    tag.TagObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// When there are no price tags, activate a popup window to notify the user
        /// </summary>
        public void NoPriceTags()
        {           
            if (PriceTagPopup == null)
            {
                CreatePopupUI();
            }

            if (!PriceTagPopup.activeSelf)
            {
                if (PopupText == null)
                    PopupText = PriceTagPopup.transform.GetChild(0).GetChild(0).GetComponent<Text>();

                PopupText.text = "There are no prices listed for the products in this project's PSA file.";

                TogglePopup();
            }                                
        }

        /// <summary>
        /// Create the popup window and the UI necessary to display it
        /// </summary>
        public void CreatePopupUI()
        {
            GameObject PriceTagUICanvas = null;

            //Find a canvas so we can properly create the Popup in the scene
            GameObject UI = GameObject.Find("UI");
            if (UI != null)
                PriceTagUICanvas = UI.transform.GetChild(0).gameObject;

            //If a pre-existing UI canvas could not be found, make one for price tags to use for dialogues 
            if (PriceTagUICanvas == null)
            {
                PriceTagUICanvas = new GameObject("PriceTagUICanvas");
                PriceTagUICanvas.AddComponent<Canvas>();
                PriceTagUICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            }

            if(PriceTagUICanvas != null)
                PriceTagPopup = Instantiate(PriceTagPopupPrefab, PriceTagUICanvas.transform);
        }

        /// <summary>
        /// Set the Popup's visibility accordingly
        /// </summary>
        public void TogglePopup()
        {
            if (PriceTagPopup != null)
            {
                PriceTagPopup.SetActive(!PriceTagPopup.activeSelf);
            }
        }
    }
}
