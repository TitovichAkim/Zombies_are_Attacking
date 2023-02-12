using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class DoorTile : Tile
    {
    void Start ()
        {
        this.gameObject.AddComponent<BoxCollider2D>();

        }
    }
