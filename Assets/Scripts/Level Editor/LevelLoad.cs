using UnityEngine;
using System.Collections;

[System.Serializable]
public class ColorToPrefab {
	public Color32 color;
	public GameObject prefab;
}

public class LevelLoad : MonoBehaviour {

	public Texture2D[] levelMap;
	public ColorToPrefab[] colorToPrefab;

	// Use this for initialization
	void Start () {
		LoadMap ();
	}

	void EmptyMap() {
		while (transform.childCount > 0) {
			Transform c = transform.GetChild (0);
			c.SetParent (null);
			Destroy(c.gameObject);
		}
	}

	void LoadMap () {
		EmptyMap ();

		for (int i = 0; i < levelMap.Length; i++) {
			
			//Get the raw pixels from the imagemaps
			Color32[] allPixels = levelMap[i].GetPixels32 ();
			int width = levelMap[i].width;
			int height = levelMap[i].height;

			for (int x = -width / 2; x < width / 2; x++) {
				for (int y = -height / 2; y < height / 2; y++) {

					SpawnTileAt (allPixels [((y + height / 2) * width) + (x + width / 2)], x, i, y);

				}
			}
		}

	}

	void SpawnTileAt (Color32 c, int x, int y, int z){
		//if transparent, leave empty
		if (c.a <= 0) {
			return;
		}

		foreach (ColorToPrefab ctp in colorToPrefab) {
			if (c.Equals(ctp.color)) {
				//spawn the prefab at right location
				Instantiate (ctp.prefab, new Vector3 (x, y, z), Quaternion.identity);

				return;
			}
		}

		Debug.LogError ("No color to prefab found for" + c.ToString());
	}
}
