using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Vector3 [] spawnPositions;
  //  public int count;


    void Start()
    {

    }
    public void SelectCharacter(int index)
    {
        int randomPosition = Random.Range(0, spawnPositions.Length);
      //  PhotonNetwork.Instantiate(playerPrefabs[index].name, spawnPositions[randomPosition], Quaternion.identity);
    }
}
