using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Map_Scr : MonoBehaviour {

    public Sprite           floor_Sprite;
    public Sprite           wall_Sprite;
    public Sprite           door_Sprite;
    public Sprite           grass_Sprite;

    public TextAsset        floor_Map;
    public GameObject       floor_go;
    private int             H, W;
    private int[,]          FLOOR;

    public List<AudioClip>  doorRepair;          // Будет хранить звуки ремонта двери
    public List<AudioClip>  doorHitting;         // Будет хранить звуки ударов по двери
    public List<AudioClip>  doorFalls;           // Будет хранить звуки разрушения двери


    void Start() {
        string[] floor_str = floor_Map.text.Split('\n');
        H = floor_str.Length;
        string[] floor_base = floor_str[0].Split(' ');
        W = floor_base.Length;

        // Создать родительский объект пола
        //floor_go = new GameObject("Floor_GO");
        //floor_go.transform.position = Vector2.zero;

        FLOOR = new int[W, H];
        for(int  j = 0; j < H; j++) {
            floor_base = floor_str[H-j-1].Split(' ');
            for (int i = 0; i < W; i++) {
                string sw = floor_base[i];
                switch(sw) {
                    case "0":
                        //CREATE_WALL(i, j);
                        break;
                    case "1":
                        //CREATE_FLOOR(i, j);
                        break;
                    case "2":
                        //CREATE_DOOR(i, j);
                        break;
                    case "g":
                        //CREATE_GRASS(i, j);
                        break;
                    default:
//                        print(floor_base[i]); 
//                        print(sw);
                        break;
                }
            }
        }

        Time.timeScale = 1;     // Запустить время в игре
    }

    public void CREATE_GRASS(int i, int j) {
        GameObject floor_element = new GameObject("grass_element" + j + "x" + i, typeof(SpriteRenderer));
        floor_element.transform.parent = floor_go.transform;
        floor_element.GetComponent<SpriteRenderer>().sortingLayerName = "Surface";
        floor_element.GetComponent<SpriteRenderer>().sprite = grass_Sprite;
        floor_element.transform.position = new Vector2(i, j);
        floor_element.transform.localScale = new Vector3(1f, 1f, 1);
        }

    public void CREATE_FLOOR(int i, int j) {
        GameObject floor_element = new GameObject("floor_element" + j + "x" + i, typeof(SpriteRenderer));
        floor_element.transform.parent = floor_go.transform;
        floor_element.GetComponent<SpriteRenderer>().sortingLayerName = "Surface";
        floor_element.GetComponent<SpriteRenderer>().sprite = floor_Sprite;
        floor_element.transform.position = new Vector2(i, j);
        floor_element.transform.localScale = new Vector3(6.25f, 6.25f, 1);
    }
    public void CREATE_WALL(int i, int j) {
        GameObject floor_element = new GameObject("wall_element" + j + "x" + i, typeof(SpriteRenderer), typeof(Rigidbody2D));
        floor_element.transform.parent = floor_go.transform;
        floor_element.GetComponent<SpriteRenderer>().sortingLayerName = "Wall";
        floor_element.GetComponent<SpriteRenderer>().sprite = wall_Sprite;
        floor_element.layer = 14;
        floor_element.transform.position = new Vector2(i, j);
        floor_element.transform.localScale = new Vector3(6.25f, 6.25f, 1);
        Rigidbody2D rigid2 = floor_element.GetComponent<Rigidbody2D>();
        rigid2.bodyType = RigidbodyType2D.Static;
        floor_element.AddComponent<BoxCollider2D>().size = new Vector2(0.16f, 0.16f);
    }
    public void CREATE_DOOR(int i, int j) {
        GameObject floor_element = new GameObject("door_element" + j + "x" + i, typeof(SpriteRenderer), typeof(Rigidbody2D));
        floor_element.transform.parent = floor_go.transform;
        floor_element.GetComponent<SpriteRenderer>().sortingLayerName = "Wall";
        floor_element.GetComponent<SpriteRenderer>().sprite = door_Sprite;
        floor_element.layer = 13;
        floor_element.transform.position = new Vector2(i, j);
        floor_element.transform.localScale = new Vector3(6.25f, 6.25f, 1);
        Rigidbody2D rigid2 = floor_element.GetComponent<Rigidbody2D>();
        rigid2.bodyType = RigidbodyType2D.Static;
        floor_element.AddComponent<BoxCollider2D>().size = new Vector2(0.16f, 0.16f);
        GameObject door_children = new GameObject("door_children", typeof(Door_Scr));
        door_children.transform.parent = floor_element.transform;
        door_children.transform.localPosition = Vector2.zero;
        door_children.layer = 13;
        door_children.AddComponent<BoxCollider2D>().size = new Vector2(1.1f, 1.1f);
        door_children.GetComponent<BoxCollider2D>().isTrigger = true;
        door_children.GetComponent<Door_Scr>().doorCollider = floor_element.GetComponent<BoxCollider2D>();
        door_children.GetComponent<Door_Scr>().doorSprite = floor_element.GetComponent<SpriteRenderer>();
        door_children.GetComponent<Door_Scr>().came = this.gameObject;       // Сделать ссылку на главную камеру


        }
}
