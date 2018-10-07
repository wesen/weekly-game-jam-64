using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileMapRandomizer : MonoBehaviour {
	private Tilemap _tilemap;

	void Awake() {
		_tilemap = GetComponent<Tilemap>();
	}

	void Start () {
		BoundsInt bounds = _tilemap.cellBounds;
		for (int x = bounds.min.x; x < bounds.max.x; x++) {
			for (int y = bounds.min.y; y < bounds.max.y; y++) {
				TileBase tile = _tilemap.GetTile(new Vector3Int(x, y, 0));
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		Player player = FindObjectOfType<Player>();
		Vector3Int cellPosition = _tilemap.WorldToCell(player.transform.position);
		if (Input.GetKeyDown(KeyCode.F)) {
			RandomizeTile(cellPosition);
		}
	}

	public void RandomizeTile(Vector3Int position) {
		TileBase[] tiles = new TileBase[9];
		int idx = 0;
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				tiles[idx] = _tilemap.GetTile(new Vector3Int(position.x + x, position.y + y, 0));
				idx++;
			}
		}

		// randomize array
		for (int i = 0; i < 9; i++) {
			TileBase tmp = tiles[i];
			int other = Random.Range(0, tiles.Length - 1);
			tiles[i] = tiles[other];
			tiles[other] = tmp;
		}

		idx = 0;
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				_tilemap.SetTile(new Vector3Int(position.x + x, position.y + y, 0), tiles[idx]);
				idx++;
			}
		}
	}
}
