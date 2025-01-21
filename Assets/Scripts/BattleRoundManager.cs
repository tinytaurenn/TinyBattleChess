using Coherence.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BattleRoundManager : MonoBehaviour
{

    [SerializeField] int m_BattlePlayerSize = 2;
    [SerializeField] List<TinyPlayer> m_PlayersInBattle = new List<TinyPlayer>();
    MainSimulator m_MainSimulator;


    private void Awake()
    {
        m_MainSimulator = GetComponent<MainSimulator>();    
    }
    public void StartBattleRound(List<CoherenceSync> players)
    {
        m_PlayersInBattle.Clear();
        SetIndexes(players, m_BattlePlayerSize); 

    }

    void SetIndexes(List<CoherenceSync> players,int arenaSize)
    {
        
        int i = 0; 
        foreach (var player in players)
        {
            int index = Mathf.FloorToInt(i / arenaSize);

            player.SendCommand<TinyPlayer>(nameof(TinyPlayer.ChangeBattleIndex), Coherence.MessageTarget.AuthorityOnly, index);
            m_PlayersInBattle.Add(player.GetComponent<TinyPlayer>());

            i++; 
        }
    }

    List<TinyPlayer> FindPlayersWithSameIndex(int index)
    {
        List<TinyPlayer> players = new List<TinyPlayer>();
        foreach (var item in m_PlayersInBattle)
        {
            if((item.BattleIndex == index) && item.m_IntPlayerState == (int)TinyPlayer.EPlayerState.Player) players.Add(item);
        }
        return players;
    }

    public void PlayerDeath(CoherenceSync playerSync)
    {
        m_PlayersInBattle.Remove(playerSync.GetComponent<TinyPlayer>());

        Debug.Log("index of dead player :" + playerSync.GetComponent<TinyPlayer>().BattleIndex);
        Debug.Log("players in battle with same index  :" + FindPlayersWithSameIndex(playerSync.GetComponent<TinyPlayer>().BattleIndex).Count);

        if (FindPlayersWithSameIndex(playerSync.GetComponent<TinyPlayer>().BattleIndex).Count == 1)
        {
            Debug.Log("all players from the arena : " + playerSync.GetComponent<TinyPlayer>().BattleIndex +" are dead");
        }

        CheckEndBattle();
    }

    bool IsEveryBattleFinished()
    {
        foreach (var player in m_PlayersInBattle)
        {
            if (FindPlayersWithSameIndex(player.BattleIndex).Count > 1)
            {
                Debug.Log("not every battle is done");
                return false;
            }
        }
        return true;
    }

    void CheckEndBattle()
    {
        if (IsEveryBattleFinished())
        {

            EndBattle();
        
        }
    }

    public void EndBattle()
    {
        Debug.Log("ending battle");

        StartCoroutine(DelayedEndTurn());
    }

    IEnumerator DelayedEndTurn()
    {
        yield return new WaitForSeconds(3f);
        m_MainSimulator.EndTurn();
    }


}
