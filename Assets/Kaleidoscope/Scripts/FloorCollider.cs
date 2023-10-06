using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour {
    GameObject obj;
    //TODO: On trigger enter: if it's a product clone, destroy it after like 3 seconds
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Cart Product")
        {
            //TODO: De-parent the product
            obj = other.gameObject;
            obj.transform.parent.DetachChildren();
            obj.tag = "Destroy"; //Added to prevent error with multiple collisions
            DestroyMe();
            //Invoke("DestroyMe", 3f);
        }
    }

    private void DestroyMe() {
        Destroy(obj);
    }
}
