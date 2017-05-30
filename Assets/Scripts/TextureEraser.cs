using UnityEngine;
using UnityEngine.UI;
using System;

public class TextureEraser : MonoBehaviour
{
	[SerializeField]
	private Texture texture;
	[SerializeField]
	private Image textureImage;
	[SerializeField]
	private Image brushImage;
	private RenderTexture renderTexture;
	[SerializeField]
	private Material eraseMaterial;
	private Rect textureImageRect;

	private void Start ()
	{
		renderTexture = new RenderTexture (texture.width, texture.height, 0);
		renderTexture.Create ();

		Graphics.Blit (texture, renderTexture);
		textureImage.material = new Material (Shader.Find ("UI/Unlit/Transparent"));
		textureImage.material.mainTexture = renderTexture;

		eraseMaterial.mainTexture = brushImage.sprite.texture;

		Vector3[] textureImageWorldCorners = new Vector3[4];
		textureImage.rectTransform.GetWorldCorners (textureImageWorldCorners);
		textureImageRect = Rect.MinMaxRect (
			textureImageWorldCorners [0].x, textureImageWorldCorners [0].y,
			textureImageWorldCorners [2].x, textureImageWorldCorners [2].y);
	}

	private void Update ()
	{
		bool pressed = Input.GetMouseButton (0);
		bool released = Input.GetMouseButtonUp (0);

		if (released) {
			brushImage.gameObject.SetActive (false);
			return;
		}

		if (!pressed)
			return;

		brushImage.rectTransform.position = Input.mousePosition;

		Vector3[] brushWorldCorners = new Vector3[4];
		brushImage.rectTransform.GetWorldCorners (brushWorldCorners);
		Rect brushWorldRect = Rect.MinMaxRect (
			brushWorldCorners [0].x, brushWorldCorners [0].y,
			brushWorldCorners [2].x, brushWorldCorners [2].y);

		bool brushAboveTexture = textureImageRect.Overlaps (brushWorldRect);
		if (!brushAboveTexture)
			return;

		brushImage.gameObject.SetActive (true);

		Graphics.SetRenderTarget (renderTexture);
		GL.PushMatrix ();
		GL.LoadPixelMatrix (
			textureImageRect.xMin, textureImageRect.xMax,
			Screen.height - textureImageRect.yMin, Screen.height - textureImageRect.yMax);

		Graphics.DrawTexture (
			Rect.MinMaxRect (
				brushWorldRect.xMin, Screen.height - brushWorldRect.yMax,
				brushWorldRect.xMax, Screen.height - brushWorldRect.yMin),
			brushImage.sprite.texture,
			0, 0, 0, 0, eraseMaterial);	

		Graphics.SetRenderTarget (null);
		GL.PopMatrix ();
	}

	public void SetBrushSprite (Sprite sprite)
	{
		brushImage.sprite = sprite;
	}
}
