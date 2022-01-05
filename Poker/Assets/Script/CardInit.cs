using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInit : MonoBehaviour
{
    public GameObject[] CardPack;
    public GameObject[] Player;

    public int turn;
    bool IsTurn;

    public int CardSequence;
    public int PlayerSequence;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSequence = 0;
        turn = 1;

        Shuffle(CardPack);

        //카드 쌓기 과정..01/03
        for(int i=0; i < CardPack.Length; i++)
        {
            CardPack[i].transform.position = new Vector3(CardPack[i].transform.position.x, CardPack[i].transform.position.y + 0.0002f * i, CardPack[i].transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TakeCard();       
    }

    private T[] Shuffle<T>(T[] array)//카드 섞기 알고리즘(Swap)..01/05
    {
        int ran1;
        int ran2;
        T temp;

        for (int i = 0; i < array.Length; i++)
        {
            ran1 = Random.Range(0, array.Length);
            ran2 = Random.Range(0, array.Length);
            
            temp = array[ran1];
            array[ran1] = array[ran2];
            array[ran2] = temp;
        }

        return array;
    }

    void TakeCard()//플레이어가 클릭시 가장 위에 있는 카드 획득..01/05
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsTurn = true; 
        }

        if (IsTurn == true)
        {
            StartCoroutine(Card());
        }
    }

    IEnumerator Card()//01.06
    {
        float dis;
        
        dis = Vector3.Distance(CardPack[CardPack.Length - turn].transform.position, Player[PlayerSequence].transform.position);//카드 좌표와 플레이어 좌표 사이 거리
        if (CardSequence < 4)//각 플레이어가 받은 카드 개수가 4개 미만일 경우
        {                    //CardSequence : 플레이어에게 나눠준 카드 개수,  PlayerSequence : 카드를 받아야하는 플레이어 순서
            if (dis < 0.1f)
            {
                if (PlayerSequence == Player.Length - 1)//마지막 플레이어에게까지 카드를 나누어줬다면 다시 처음 플레이어에게 그 다음 카드를 나누어준다.
                {
                    CardSequence += 1;
                    turn += 1;
                    PlayerSequence = 0;
                }
                else
                {
                    PlayerSequence += 1;
                    turn += 1;
                }
            }
            else//카드 좌표와 플레이어 좌표 사이의 거리가 0.1f 이상이라면 카드 좌표가 플레이어 좌표로 이동한다.
            {
                CardPack[CardPack.Length - turn].transform.position =
                       Vector3.Lerp(CardPack[CardPack.Length - turn].transform.position, Player[PlayerSequence].transform.position, 0.02f);
            }
        }
        else//각 플레이어가 받은 카드 개수가 4개 이상이면 coroutine 탈출
        {
            IsTurn = false;
        }
        yield return new WaitForSeconds(0.25f);
    }
        
    
 }
