namespace navdi2.mono {

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PerfectCamScaler : MonoBehaviour {
	public Camera screenCamera;
	public int gameWidth = 80;
	public int gameHeight = 200;
	int lastScreenWidth = 0;
	int lastScreenHeight = 0;
	float viewportXFraction = 1;
	float viewportYFraction = 1;
	void Update () {
		// discover ratio: is the screen's width/height smaller than the game's width/height? if so, i need to expand
		// float WorldWHToScreenWH = ((1f*gameWidth)/(gameHeight)) / ((1f*Screen.width)/(Screen.height));
		if (screenCamera!=null && (lastScreenWidth!=Screen.width||lastScreenHeight!=Screen.height)) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;

			float rawGameScale = 1f * Screen.height / gameHeight;

			// renderCamera.orthographicSize = 0.5f * gameHeight;

			float maxHGameScale = 1f * Screen.width / gameWidth;
			int actualGameScale = Mathf.FloorToInt(Mathf.Min(rawGameScale,maxHGameScale));
			if (actualGameScale < 1) actualGameScale = 1;

			screenCamera.orthographicSize = gameHeight * rawGameScale / actualGameScale;
			screenCamera.nearClipPlane = -1000;

			// renderCamera.orthographicSize = 1 * Screen.height * desiredScale / rawGameScale / gameHeight;
			
			viewportXFraction = screenCamera.pixelRect.width / (gameWidth * actualGameScale);
			viewportYFraction = screenCamera.pixelRect.height / (gameHeight * actualGameScale);
		}
	}

	public Vector2 MouseWorldPoint() {
		Vector3 rawViewPoint = screenCamera.ScreenToViewportPoint(Input.mousePosition);
		Vector3 accurateViewPoint = new Vector3(
			((rawViewPoint.x - 0.5f) * viewportXFraction + 0.5f),
			((rawViewPoint.y - 0.5f) * viewportYFraction + 0.5f),
			0
		);
		return new Vector2(accurateViewPoint.x*gameWidth, accurateViewPoint.y*gameHeight);
	}
}

}