using UnityEngine;
using UnityEngine.UI;

public class BrushButtonController : MonoBehaviour
{
	[SerializeField]
	private TextureEraser textureEraser;
	[SerializeField]
	private Image image;

	public void OnClick ()
	{
		textureEraser.SetBrushSprite (image.sprite);
	}
}
