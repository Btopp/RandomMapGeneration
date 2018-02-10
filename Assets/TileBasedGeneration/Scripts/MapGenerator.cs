using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	public int rows = 3; // min = 3
	public int columns = 3; // min = 3

	public List<BaseTile> allTiles;
	private List<List<BaseTile>> placedTiles;
	private List<BaseTile> acPossibleTiles;

	private bool isGenerated = false;

	[Range (0f, 1f)]
	public float randomExit = 0.5f;
	private Vector2 exitCoords = new Vector2();

	private void Start () {
		exitCoords = GenerateExitPositions (rows, columns, randomExit);

		while (!isGenerated) {
			placedTiles = new List <List<BaseTile>>();
			GenerateMap();
			Debug.Log ("Generated!");
		}
	}

	// Calculates exits
	public Vector2 GenerateExitPositions (int x, int y, float normalizedRandom) {
		Vector2 coords = new Vector2 ();
		if (normalizedRandom > 0) {
			coords = new Vector2 (Mathf.Ceil (Random.Range ((x - (x * normalizedRandom)), (x - 1))), Mathf.Ceil (Random.Range ((y - (y * normalizedRandom)), (y - 2))));
		} else {
			coords = new Vector2 (x - 1, y - 1);
		}
		return coords;
	}

	void GenerateMap () {

		for (int nWidth = 0; nWidth <= rows; nWidth++) {
			placedTiles.Add(new List<BaseTile>());

			for (int nHeight = 0; nHeight <= columns; nHeight++) {

				// First tile
				if (nHeight == 0 && nWidth == 0) {
					// X tile
					placedTiles [nWidth].Add (Instantiate (allTiles [0], new Vector3 ((-10.0f * nWidth), 0.0f, (10.0f * nHeight)), Quaternion.identity));

				} else {
					acPossibleTiles = new List<BaseTile> ();

					// Last tile
					if (nHeight == columns && nWidth == rows) {
						acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight);
						isGenerated = true;

					// Last tiles in width
					} else if (nWidth == rows) {
						if (nHeight == 0) {
							acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [0].gateTop && !o.gateTop && !o.gateLeft && !o.isSpecial);
							if (acPossibleTiles.Count == 0) {
								acPossibleTiles = allTiles.FindAll (o => o.gateBottom && o.gateRight && !o.gateTop && !o.gateLeft);
							}
						} else {
							
							// End tile top
							if (nHeight == exitCoords.y) {
								acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && o.gateTop);
							} else {
								acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.gateTop);
							}
						}

					// Last tiles in height
					} else if (nHeight == columns) {
						if (nWidth == 0) {
							acPossibleTiles = allTiles.FindAll (o => o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.gateRight && !o.gateBottom && !o.isSpecial);
							if (acPossibleTiles.Count == 0) {
								acPossibleTiles = allTiles.FindAll (o => o.gateTop && o.gateLeft && !o.gateBottom && !o.gateRight);
							}
						} else {
							
							// End tile right
							if (nWidth == exitCoords.x) {
								acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && o.gateRight);
							} else {
								acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.gateRight);
							}
						}
							
					// First tiles in height
					} else if (nHeight == 0) {
						acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [0].gateTop && o.gateLeft == false && !o.isSpecial);

					// First tiles in width
					} else if (nWidth == 0) {
						acPossibleTiles = allTiles.FindAll (o => o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && o.gateBottom == false && !o.isSpecial);

					// Middle tiles
					} else {
						acPossibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.isSpecial);
					}

					// Instantiate tile
					placedTiles [nWidth].Add (Instantiate (acPossibleTiles [Random.Range (0, acPossibleTiles.Count)], new Vector3 ((-10.0f * nWidth), 0.0f, (10.0f * nHeight)), Quaternion.identity));
				}
			}
		}
	}
}
