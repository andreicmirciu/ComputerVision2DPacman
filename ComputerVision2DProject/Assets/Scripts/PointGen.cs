using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointGen : MonoBehaviour
{
    public GameObject SmallPoint;
    public GameObject BigPoint;
    public GameObject Player;
    public float points;              //totalpuncte
    public int smallPoints;         //puncte date de sfera mica
    public int bigPoints;           //puncte date de sfera mare
    public float turnTarget;          //cu cat turn se apropie de turntarget, nr. de puncte primite scade
    GameObject myPlayer;
    float playerX = -9.5f;          //pozitia de start a player-ului
    float playerY = -4.5f;
    float xStart = -10.5f;          //pozitia de start a hartii
    float yStart = -5.5f;
    float xBound = 11f;             //limita hartii
    float yBound = 6f;
    float turn = 0f;
    public List<Vector3> allowedPositions = new List<Vector3>(); //lista de pozitii fara zid
    public int smallWeight = 10;    //probabilitatea ca un camp sa fie sfera mica/mare/gol
    public int bigWeight = 2;
    public int emptyWeight = 3;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = Instantiate(Player, new Vector3(playerX, playerY, 0), Quaternion.identity);
        allowedPositions.Add(new Vector3(playerX, playerY, 0));
        int count = 0;
        int totalWeight = smallWeight + bigWeight + emptyWeight;
        System.Random rnd = new System.Random();
        int randomValue = 0;
        for (float x = xStart; x <= xBound; x++)
        {
            for (float y = yStart; y <= yBound; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                if (isObjectHere(pos) == true)
                {
                    allowedPositions.Add(pos);
                    Debug.Log(allowedPositions.Find(element => element == pos));
                    randomValue = rnd.Next(1, totalWeight);
                    if (randomValue < bigWeight + smallWeight)
                    {
                        if (randomValue < bigWeight)
                        {
                            Instantiate(BigPoint, pos, Quaternion.identity);
                            continue;
                        }
                        Instantiate(SmallPoint, pos, Quaternion.identity);
                    }


                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            PlayerMoveUp();
        }
        if (Input.GetKeyDown("a"))
        {
            PlayerMoveLeft();
        }
        if (Input.GetKeyDown("s"))
        {
            PlayerMoveDown();
        }
        if (Input.GetKeyDown("d"))
        {
            PlayerMoveRight();
        }
    }
    bool isObjectHere(Vector3 position)  //verifica prezenta obiectelor, folosit pentru gasit ziduri
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        if (intersecting.Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool EatPoints(Vector3 position)     //verifica prezenta obiectelor, folosit pentru puncte
    {
        turn++;
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        if (intersecting.Length != 0)
        {
            foreach (var collider in intersecting)
            {
                if (collider.gameObject.transform.parent.name == "SmallPoint(Clone)")
                {
                    Debug.Log(collider.gameObject.transform.parent.name);
                    points += smallPoints * (turnTarget - turn) / turnTarget;
                }
                else
                    if (collider.gameObject.transform.parent.name == "BigPoint(Clone)")
                {
                    Debug.Log(collider.gameObject.transform.parent.name);
                    points += bigPoints * (turnTarget - turn) / turnTarget;
                }
                Destroy(collider.gameObject);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    void PlayerMoveUp()     //functii de miscare
    {
        Vector3 tempPosition = new Vector3(playerX, playerY + 1, 0);
        if (allowedPositions.Contains(tempPosition))
        {
            EatPoints(tempPosition);
            playerY += 1;
            myPlayer.transform.position = new Vector3(playerX, playerY, 0);
        }
        Debug.Log("Done moving up");   
    }
    void PlayerMoveDown()
    {
        Vector3 tempPosition = new Vector3(playerX, playerY - 1, 0);
        if (allowedPositions.Contains(tempPosition))
        {
            EatPoints(tempPosition);
            playerY -= 1;
            myPlayer.transform.position = new Vector3(playerX, playerY, 0);
        }
        Debug.Log("Done moving down");
    }
    void PlayerMoveLeft()
    {
        Vector3 tempPosition = new Vector3(playerX - 1, playerY, 0);
        if (allowedPositions.Contains(tempPosition))
        {
            EatPoints(tempPosition);
            playerX -= 1;
            myPlayer.transform.position = new Vector3(playerX, playerY, 0);
        }
        Debug.Log("Done moving left");
    }
    void PlayerMoveRight()
    {
        Vector3 tempPosition = new Vector3(playerX + 1, playerY, 0);
        if (allowedPositions.Contains(tempPosition))
        {
            EatPoints(tempPosition);
            playerX += 1;
            myPlayer.transform.position = new Vector3(playerX, playerY, 0);
        }
        Debug.Log("Done moving right");
    }

}
