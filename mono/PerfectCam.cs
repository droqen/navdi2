namespace navdi2.mono
{

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PerfectCam : MonoBehaviour {

	public int gameWidth = 160;

	public int gameHeight = 144;

	public PerfectCamScaler screenCamScaler;

	public MeshRenderer screenMeshRenderer;

	public Camera renderCam;

	public RenderTexture renderTexture;
	
	public Material renderTextureMaterial;
	
	// Update is called once per frame
	void Update () {
		if (screenCamScaler != null) {
			if (screenCamScaler.gameWidth != gameWidth || screenCamScaler.gameHeight != gameHeight) {
				screenCamScaler.gameWidth = gameWidth;
				screenCamScaler.gameHeight = gameHeight;
			}
		}
		if (renderTexture != null) {
			if (renderTexture.width != gameWidth || renderTexture.height != gameHeight) {
				renderTexture = new RenderTexture(gameWidth,gameHeight,0);
				renderTexture.filterMode = FilterMode.Point;
			}
		} else {
			renderTexture = new RenderTexture(gameWidth,gameHeight,0);
			renderTexture.filterMode = FilterMode.Point;
		}

		if (screenMeshRenderer != null) {
			screenMeshRenderer.transform.localScale = new Vector3(gameWidth, 1, gameHeight);
			screenMeshRenderer.sharedMaterial = renderTextureMaterial;
		}
		if (renderCam != null) {
			renderCam.transform.localPosition = new Vector3(gameWidth*.5f, gameHeight*.5f);
			renderCam.orthographicSize = gameHeight*.5f;
			renderCam.targetTexture = renderTexture;
			renderCam.nearClipPlane = -1000;
		}
		if (renderTextureMaterial != null) {
			renderTextureMaterial.mainTexture = renderTexture;
		}
		if (renderTexture != null) {
			// renderTexture.filterMode = FilterMode.Point;
		}
	}
	public Vector2 MouseWorldPoint() {
		return screenCamScaler.MouseWorldPoint();
	}
}
	
}