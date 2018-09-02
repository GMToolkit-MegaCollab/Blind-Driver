using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This does the magic of showing up PROPERLY.
public class SoundIcon : MonoBehaviour {

	public Vector3 orientation;
	public float loudness {
		set {
			if (value > 0.1f) {
				value = 0.1f;
			}
			if (icon != null) {
				icon.GetComponent<Image>().color = new Color(1f,1f,1f, value * 10);
				Brackets.GetComponent<Image>().color = new Color(1f,1f,1f, value * 40);
				Arrow.GetComponent<Image>().color = new Color(1f,1f,1f, value * 40);
			}
		}
	}

	private GameObject icon;
	private GameObject Arrow;
	private GameObject Brackets;
	[SerializeField]
	public Sprite iconImage;

	private static float border = 42f;
	private static float iconscale = 40f;

	//the ratio of Bracket or Arrow size/Icon size larger value = larger Brackets/Arrows
	private static float typeIndicatorScale = 2.5f;

	void Start () {
		//Setup the icon
		this.transform.SetParent(SoundCamera.soundCamera.origin.transform);
		icon = new GameObject("Icon", new System.Type[] {typeof(RectTransform), typeof(Image)});
		icon.GetComponent<Image>().sprite = iconImage;
		icon.transform.SetParent(SoundCamera.soundCamera.canvas.transform);
		icon.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
		icon.layer = 5;
		Arrow = new GameObject("Arrow", new System.Type[] {typeof(RectTransform), typeof(Image)});
		Arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
		Arrow.transform.localScale = Vector3.one * typeIndicatorScale;
		Arrow.transform.SetParent(icon.transform);
		Arrow.GetComponent<Image>().sprite = SoundIconSpriteManager.instance.data.Arrow;
		Arrow.gameObject.SetActive(false);
		Arrow.layer = 5;
		Brackets = new GameObject("Bracket", new System.Type[] {typeof(RectTransform), typeof(Image)});
		Brackets.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
		Brackets.transform.localScale = Vector3.one * typeIndicatorScale;
		Brackets.transform.SetParent(icon.transform);
		Brackets.GetComponent<Image>().sprite = SoundIconSpriteManager.instance.data.Brackets;
		Brackets.gameObject.SetActive(false);
		Brackets.layer = 5;
		//Iconscale
		icon.transform.localScale = Vector3.one * iconscale;
	}
	
	void Update () {
		float planeDistance = SoundCamera.soundCamera.canvas.planeDistance;
		
		Vector3 up = SoundCamera.soundCamera.transform.up;
		
		//Normal of projection plane (the canvas)
		Vector3 n = SoundCamera.soundCamera.transform.forward;
		//Point on the plane
		Vector3 p0 = planeDistance * n;

		//A point in space clamped into a Half-space given by the plane of which n is the normal.
		Vector3 l = orientation.normalized;
		
		//Clamp the mofokin point
		if (Vector3.Dot(n, l) <= 0f) {
			Vector3 projUp = up * Vector3.Dot(up, l);
			l = (
				projUp + 
				((Mathf.Sign(Vector3.Dot(SoundCamera.soundCamera.transform.right, l - projUp)) * SoundCamera.soundCamera.transform.right) + n * 0.2f).normalized
			).normalized;
			//Debug.Log(Mathf.Sign(Vector3.Dot(SoundCamera.soundCamera.transform.right, l - projUp)));
		}


		Vector3 intersection =  l * Vector3.Dot(n, p0)/Vector3.Dot(n, l);
		
		#if UNITY_EDITOR
		Debug.DrawLine(Vector3.zero, 10*l, Color.cyan);
		Debug.DrawLine(Vector3.zero, orientation.normalized*10);
		Debug.DrawLine(Vector3.zero, up, Color.red);
		Debug.DrawLine(Vector3.zero, n, Color.yellow);
		Debug.DrawLine(Vector3.zero, intersection, Color.grey);
		#endif

		//Convert to canvas coords
		intersection = SoundCamera.soundCamera.canvas.transform.worldToLocalMatrix* ToV4(intersection);
		Vector3 canvasspace = new Vector3(intersection.x / SoundCamera.soundCamera.canvas.scaleFactor, intersection.y / SoundCamera.soundCamera.canvas.scaleFactor, 0f);

		//clamp value
		Vector2 clamping = SoundCamera.soundCamera.canvas.GetComponent<RectTransform>().sizeDelta / 2f - Vector2.one * border / SoundCamera.soundCamera.canvas.scaleFactor;
		Vector3 clampedspace = new Vector3(Mathf.Clamp(canvasspace.x, -clamping.x, clamping.x), Mathf.Clamp(canvasspace.y, -clamping.y, clamping.y), 0f);
		
		if ((canvasspace - clampedspace).magnitude <= 0.5f) {
			Brackets.SetActive(true);
			Arrow.SetActive(false);
		} else {
			Brackets.SetActive(false);
			Arrow.SetActive(true);
			Arrow.transform.localRotation = Quaternion.FromToRotation(up, (canvasspace - clampedspace).normalized);
		}

		icon.transform.localPosition = clampedspace;
		icon.transform.localRotation = Quaternion.Euler(0,0,0);
	}

	public static Vector4 ToV4(Vector3 x) {
		return new Vector4(x.x, x.y, x.z, 1);
	}

	public void Remove() {
		Destroy(icon);
		Destroy(Arrow);
		Destroy(Brackets);
		Destroy(this.gameObject);
	}
}
