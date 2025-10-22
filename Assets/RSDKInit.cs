using UnityEngine;
using Sonic_CD;
using Retro_Engine;

public class RSDKInit : MonoBehaviour
{
	[Tooltip("If enabled, adds the Sonic_CD.Game component to this GameObject when missing.")]
	public bool autoAttachGameComponent = true;
	
	[Header("Render Settings")]
	[Tooltip("The render texture to use for the game. If null, will create a default one.")]
	public RenderTexture renderTexture;
	
	[Tooltip("The camera to use for rendering. If null, will find or create one.")]
	public Camera renderCamera;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		InitializeRenderSettings();
		
		if (!autoAttachGameComponent)
			return;

		var existing = FindObjectOfType<Game>();
		if (existing == null)
		{
			var game = GetComponent<Game>();
			if (game == null)
				game = gameObject.AddComponent<Game>();
		}
	}
	
	private void InitializeRenderSettings()
	{
		if (renderTexture == null)
		{
			renderTexture = new RenderTexture(1280, 720, 24);
			renderTexture.name = "RetroEngineRenderTexture";
			renderTexture.filterMode = FilterMode.Point;
		}
		
		if (renderCamera == null)
		{
			renderCamera = FindObjectOfType<Camera>();
			if (renderCamera == null)
			{
				GameObject cameraObj = new GameObject("RetroEngineCamera");
				cameraObj.transform.SetParent(transform);
				renderCamera = cameraObj.AddComponent<Camera>();
				renderCamera.orthographic = true;
				renderCamera.orthographicSize = 120f;
				renderCamera.nearClipPlane = 0.1f;
				renderCamera.farClipPlane = 1000f;
				renderCamera.clearFlags = CameraClearFlags.SolidColor;
				renderCamera.backgroundColor = Color.black;
			}
		}
		
		GraphicsSystem.renderTexture = renderTexture;
		GraphicsSystem.renderCamera = renderCamera;
		
		Camera mainCamera = Camera.main;
		if (mainCamera != null)
		{
			mainCamera.enabled = false;
		}
	}
	
	void OnGUI()
	{
		if (renderTexture != null)
		{
			float aspectRatio = (float)renderTexture.width / renderTexture.height;
			float screenAspectRatio = (float)Screen.width / Screen.height;
			
			float displayWidth, displayHeight;
			if (aspectRatio > screenAspectRatio)
			{
				displayWidth = Screen.width;
				displayHeight = Screen.width / aspectRatio;
			}
			else
			{
				displayHeight = Screen.height;
				displayWidth = Screen.height * aspectRatio;
			}
			
			float x = (Screen.width - displayWidth) * 0.5f;
			float y = (Screen.height - displayHeight) * 0.5f;
			
			GUI.DrawTexture(new Rect(x, y, displayWidth, displayHeight), renderTexture);
		}
	}
}
