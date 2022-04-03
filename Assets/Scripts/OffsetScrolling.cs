using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour {
    public float scrollSpeed;

        public Renderer render;
        private Vector2 savedOffset;

        void Start () {
            render = GetComponent<Renderer> ();
        }

        void Update () {
        float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2 (x, 0);
        render.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}