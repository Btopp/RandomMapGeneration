using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	[Range (3, 99)]
	public int rows = 3, columns = 3;

	[Range (0f, 1f)]
	public float randomExit = 0.5f;
	private Vector2 exitCoords;

	public List<BaseTile> allTiles;
	private List<BaseTile> possibleTiles;
	private List<List<BaseTile>> placedTiles;

	private void Start () {
		GenerateExitPositions(rows, columns, randomExit);
		GenerateMap(rows, columns);
	}

	// Calculate exit positions
	public void GenerateExitPositions (int rows, int columns, float normalizedRandom) {
		rows -= 1;
		columns -= 1;

		if (normalizedRandom > 0) {
			exitCoords = new Vector2 (
				Mathf.Round (Random.Range ((rows - (rows * normalizedRandom)), (rows - 1))),
				Mathf.Round (Random.Range ((columns - (columns * normalizedRandom)), (columns - 1)))
			);
		} else {
			exitCoords = new Vector2 (rows - 1, columns - 1);
		}
	}

	void GenerateMap (int rows, int columns) {
		rows -= 1;
		columns -= 1;

		placedTiles = new List <List<BaseTile>>();

		for (int nWidth = 0; nWidth <= rows; nWidth++) {
			placedTiles.Add(new List<BaseTile>());

			for (int nHeight = 0; nHeight <= columns; nHeight++) {

				// First tile
				if (nHeight == 0 && nWidth == 0) {
					placedTiles [nWidth].Add (Instantiate (allTiles [0], new Vector3 ((-10.0f * nWidth), 0.0f, (10.0f * nHeight)), Quaternion.identity));

				} else {
					possibleTiles = new List<BaseTile> ();

					// Last tile
					if (nHeight == columns && nWidth == rows) {
						possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight);

					// Last tiles in row
					} else if (nWidth == rows) {
						if (nHeight == 0) {
							possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [0].gateTop && !o.gateTop && !o.gateLeft && !o.isSpecial);
							if (possibleTiles.Count == 0) {
								possibleTiles = allTiles.FindAll (o => o.gateBottom && o.gateRight && !o.gateTop && !o.gateLeft);
							}
						} else {
							
							// End tile top
							if (nHeight == exitCoords.y) {
								possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && o.gateTop);
							} else {
								possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.gateTop);
							}
						}

					// Last tiles in column
					} else if (nHeight == columns) {
						if (nWidth == 0) {
							possibleTiles = allTiles.FindAll (o => o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.gateRight && !o.gateBottom && !o.isSpecial);
							if (possibleTiles.Count == 0) {
								possibleTiles = allTiles.FindAll (o => o.gateTop && o.gateLeft && !o.gateBottom && !o.gateRight);
							}
						} else {
							
							// End tile right
							if (nWidth == exitCoords.x) {
								possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && o.gateRight);
							} else {
								possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.gateRight);
							}
						}
							
					// First tiles in height
					} else if (nHeight == 0) {
						possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [0].gateTop && o.gateLeft == false && !o.isSpecial);

					// First tiles in width
					} else if (nWidth == 0) {
						possibleTiles = allTiles.FindAll (o => o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && o.gateBottom == false && !o.isSpecial);

					// Middle tiles
					} else {
						possibleTiles = allTiles.FindAll (o => o.gateBottom == placedTiles [nWidth - 1] [nHeight].gateTop && o.gateLeft == placedTiles [nWidth] [nHeight - 1].gateRight && !o.isSpecial);
					}

					// Instantiate tile
					placedTiles [nWidth].Add (Instantiate (possibleTiles [Random.Range (0, possibleTiles.Count)], new Vector3 ((-10.0f * nWidth), 0.0f, (10.0f * nHeight)), Quaternion.identity));
				}
			}
		}
	}
}
