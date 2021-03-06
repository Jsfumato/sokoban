﻿using System;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public GameObject crate;
    public GameObject floor;
    public GameObject goal;
    public GameObject player;
    public GameObject wall;

    [NonSerialized] public GameObject goPlayer;

    static string[] level1 = {
        "#########",
        "#....***#",
        "#.#.#*#*#",
        "#.#..***#",
        "#.ooo.#.#",
        "#.o@o...#",
        "#.ooo##.#",
        "#.......#",
        "#########"
    };

    static readonly string[][] levels = { level1 };

    Transform board;

    public int SetupBoard(int levelIndex) {
        float scale = GameManager.scale;
        int goals = 0;

        board = new GameObject("Board").transform;
        string[] level = levels[levelIndex];
        float maxY = level.Length;
        float maxX = 0;

        for (int y = 0; y < level.Length; y++) {
            string row = level[y];
            maxX = Mathf.Max(row.Length, maxX);
            for (int x = 0; x < row.Length; x++) {
                GameObject tile = null;
                switch (row[x]) {
                    case '#':
                        tile = wall;
                        break;
                    case '@':
                        tile = player;
                        break;
                    case 'o':
                        tile = crate;
                        break;
                    case '*':
                        tile = goal;
                        break;
                    case '.':
                        tile = floor;
                        break;

                }

                GameObject instance = Instantiate(tile, new Vector3(x / scale, (level.Length - y) / scale, 0), Quaternion.identity);
                instance.transform.SetParent(board);

                //
                if (instance.GetComponent<PlayerController>() != null)
                    goPlayer = instance;

                //
                if (tile != floor) {
                    // Add a floor under any tile that isn't already a floor.
                    instance = Instantiate(floor, new Vector3(x / scale, (level.Length - y) / scale, 0), Quaternion.identity);
                    instance.transform.SetParent(board);

                    // Untag floors under goals, so crates don't turn off.
                    if (tile == goal) {
                        instance.tag = "Untagged";
                        goals += 1;
                    }
                }
            }
        }

        // Center the board in the scene.
        float halfTile = 1 / (scale);
        board.position = new Vector3(-(maxX / 2 - halfTile) / scale, -(maxY / 2 + halfTile) / scale, 0);

        return goals;
    }
}