using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Unity.Services.Leaderboards.Models;

public struct Player
{
    public double Score;
    public string Tier;
}

public class Leaderboard : MonoBehaviour
{
    const string LEADERBOARD_ID = "turn-leaderboard";

    private Player _player;

    private async void Start()
    {
        await InitializeServices();

        LeaderboardEntry playerScore = await GetPlayerScore();
        _player.Score = playerScore.Score;
        _player.Tier = playerScore.Tier;
        Debug.Log(JsonConvert.SerializeObject(playerScore));

        // await UpdateScore();
        // await AddScore(_player.Score);

        // playerScore = await GetPlayerScore();
        // _player.Score = playerScore.Score;
        // _player.Tier = playerScore.Tier;
        // Debug.Log(JsonConvert.SerializeObject(playerScore));
    }

    public async Task InitializeServices()
    {
        // Inicializa todos os serviços instalados
        await UnityServices.InitializeAsync();
        // Faz login anonimamente
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task UpdateScore()
    {
        for (int i = 0; i < 10; i++)
        {
            _player.Score += 10;
            Debug.Log("Aumentou score");
            await Task.Delay(1000);
        }
    }

    public async Task<LeaderboardEntry> GetPlayerScore()
    {
        return await LeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
    }

    public async Task<LeaderboardScoresPage> GetScores()
    {
        return await LeaderboardsService.Instance.GetScoresAsync(LEADERBOARD_ID);
    }

    public async Task<LeaderboardEntry> AddScore(double score)
    {
        return await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, score);
    }
}
