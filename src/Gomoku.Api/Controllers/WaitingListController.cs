﻿using Gomoku.Api.Hubs;
using Gomoku.Core.Requests;
using Gomoku.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Gomoku.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WaitingListController(IWaitingListService waitingListService, IHubContext<GameHub> hub) : ControllerBase
{
    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetPlayersWaiting()
    {
        return Ok(await waitingListService.Count());
    }

    [HttpPost]
    [Route("join")]
    public async Task<IActionResult> JoinWaitingList(JoinWaitingListRequest player)
    {
        await waitingListService.Add(player.PlayerName);

        await hub.Clients.All.SendAsync("PlayerJoinedWaitingList", player.PlayerName);
        
        return Ok();
    }

    [HttpDelete]
    [Route("{playerName}")]
    public async Task<IActionResult> RemoveFromWaitingList(string playerName)
    {
        await waitingListService.Remove(playerName);

        await hub.Clients.All.SendAsync("PlayerLeftWaitingList", playerName);
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await waitingListService.GetAll());
    }
}
