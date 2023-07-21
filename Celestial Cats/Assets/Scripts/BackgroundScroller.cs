using UnityEngine;
using UnityEngine.UI;

// tutorial: https://youtu.be/-6H-uYh80vc

public class BackgroundScroller : MonoBehaviour {

    public RawImage Background;

    public float XSpeed;
    public float YSpeed;

    private void Update() {
        Background.uvRect = new Rect(Background.uvRect.position + new Vector2(XSpeed, YSpeed) * Time.deltaTime, Background.uvRect.size);
    }
}