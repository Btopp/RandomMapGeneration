using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateBaseGeneration : MonoBehaviour {

	public GameObject[] tiles;

	public GameObject startTile;

	public GameObject endTile;

	private int maxLevel = 5;

	private int level = 0;


	void Awake() {
		startTile.GetComponent<Template>().level = 0;
		GenerateIt (startTile);
	}

	void GenerateIt(GameObject startTile) {

		GameObject previous = Instantiate (startTile, Vector3.zero, Quaternion.identity);
		level++;
		previous.GetComponent<Template> ().level = level;
		GameObject current = Instantiate (tiles[Random.Range(0, tiles.Length - 1)], startTile.GetComponent<Template>().gates [0].transform.position, transform.rotation);

		SetTemplate (current.GetComponent<Template>(), 0, previous.GetComponent<Template>(), 0);

		current.transform.parent = previous.transform;

		for (int i = 0; i < maxLevel; i++) {

			previous = current;
			if (i == maxLevel - 2) {
				current = Instantiate (endTile, Vector3.zero, Quaternion.identity);
			} else {
				current = Instantiate (tiles[Random.Range(0, tiles.Length - 1)], Vector3.zero, Quaternion.identity);
			}
			SetTemplate (current.GetComponent<Template>(), 0, previous.GetComponent<Template>(), 0);
			current.transform.parent = previous.transform;

		}


	}



	void SetTemplate(Template template, int anchorId, Template targetTemplate, int targetAnchorId) {
		template.transform.rotation = Quaternion.Euler (template.transform.rotation.eulerAngles.x, targetTemplate.transform.rotation.eulerAngles.y + template.gateRotations[targetAnchorId], targetTemplate.transform.rotation.eulerAngles.z);
		template.gates [anchorId].transform.parent = null; //
		template.transform.parent = template.gates[anchorId].transform;
		template.gates [anchorId].transform.position = targetTemplate.gates [targetAnchorId].transform.position;
		template.transform.parent = null;
		template.gates [anchorId].transform.parent = template.transform;
	}

}
