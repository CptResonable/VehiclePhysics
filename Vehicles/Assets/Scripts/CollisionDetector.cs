using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {
    [SerializeField] GameObject contactMarker;
    public ContactPoint[] contacts = new ContactPoint[0]{};
    List<GameObject> markers = new List<GameObject>();

    private void OnCollisionEnter(Collision collision) {
    }
    private void OnCollisionStay(Collision collision) {
        Debug.Log(collision.contactCount);
        contacts = collision.contacts;
        DebugMarkers();
    }

    void DebugMarkers() {
        while (markers.Count != contacts.Length) {
            if(markers.Count > contacts.Length) {
                Destroy(markers[markers.Count - 1]);
                markers.Remove(markers[markers.Count - 1]);
            }
            else {
                markers.Add(Instantiate(contactMarker));
            }
        }
        for (int i = 0; i < markers.Count; i++) {
            markers[i].transform.position = contacts[i].point;
        }
    }
}
