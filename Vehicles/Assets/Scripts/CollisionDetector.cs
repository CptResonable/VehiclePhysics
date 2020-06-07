using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {
    [SerializeField] GameObject contactMarker;
    public List<ContactPoint> contacts = new List<ContactPoint>();
    List<GameObject> markers = new List<GameObject>();

    bool firstCollider = true;
    bool noCollision = true;

    private void OnCollisionStay(Collision collision) {
        //Debug.Log(collision.contactCount);
        if (firstCollider) {
            contacts = new List<ContactPoint>();
            firstCollider = false;
        }
        foreach (ContactPoint contact in collision.contacts) {
            contacts.Add(contact);
        }
        DebugMarkers();
        noCollision = false;
    }

    private void LateUpdate() {
        firstCollider = true;
        if (noCollision)
            contacts = new List<ContactPoint>();
        else
            noCollision = true;
    }

    void DebugMarkers() {
        //while (markers.Count != contacts.Length) {
        //    if(markers.Count > contacts.Length) {
        //        Destroy(markers[markers.Count - 1]);
        //        markers.Remove(markers[markers.Count - 1]);
        //    }
        //    else {
        //        markers.Add(Instantiate(contactMarker));
        //    }
        //}
        //for (int i = 0; i < markers.Count; i++) {
        //    markers[i].transform.position = contacts[i].point;
        //}
    }
}
