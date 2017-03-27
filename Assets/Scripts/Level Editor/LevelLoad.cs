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
		GameObject map = new GameObject ();
		map.name = "Map";

		for (int i = 0; i < levelMap.Length; i++) {
			GameObject layer = new GameObject ();
			layer.name = "Layer" + i;
			layer.transform.parent = map.transform;
			
			//Get the raw pixels from the imagemaps
			Color32[] allPixels = levelMap[i].GetPixels32 ();
			int width = levelMap[i].width;
			int height = levelMap[i].height;

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {

					SpawnTileAt (allPixels [(y * width) + x], x, i, y, layer);

				}
			}
		}

	}

	void SpawnTileAt (Color32 c, int x, int y, int z, GameObject layer){
		//if transparent, leave empty
		if (c.a <= 0) {
			return;
		}

		foreach (ColorToPrefab ctp in colorToPrefab) {
			if (c.Equals(ctp.color)) {
				//spawn the prefab at right location
				GameObject Block = Instantiate (ctp.prefab, new Vector3 (x, y, z), Quaternion.identity);

				Block.transform.parent = layer.transform;
				return;
			}
		}

		Debug.LogError ("No color to prefab found for" + c.ToString());
	}
}
