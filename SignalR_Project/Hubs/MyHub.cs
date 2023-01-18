﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;


namespace SignalR_Project.Hubs
{
    public class MyHub : Hub
    {
        private readonly Context _context;

        public MyHub(Context context)
        {
            _context = context;
        }

        public static List<string> Names { get; set; } = new List<string>();
        public static int ClientCount { get; set; } = 0;
        public static int RoomCount { get; set; } = 5;
        public async Task SendName(string name)
        {
            if (Names.Count >= RoomCount)
            {
                await Clients.Caller.SendAsync("Error", $"Bu odada en fazla{RoomCount} kişi olabilir");
            }
            else
            {
                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name);
            }
        }
        public async Task GetName(string Names)
        {
            //Tüm clientlarda bulunan ReceiveNames metodunu çağırır, Names değerini gönderir
            await Clients.All.SendAsync("ReceiveNames", Names);
        }
        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendNameByGroup(string name, string roomname)
        {
            var room = _context.Rooms.Where(x => x.RoomName == roomname).FirstOrDefault();
            if (room != null)
            {
                room.Users.Add(new User { Name = name });
            }
            else
            {
                var newRoom = new Room { RoomName = roomname };
                newRoom.Users.Add(new User { Name = name });
                _context.Rooms.Add(newRoom);
            }
            await _context.SaveChangesAsync();
            await Clients.Group(roomname).SendAsync("ReceiveMessageByGroup", name, room.RoomID);
        }



        public async Task GetNamesByGroup()
        {
            var rooms = _context.Rooms.Include(x => x.Users).Select(y => new
            {
                roomid = y.RoomID,
                users = y.Users.ToList()
            });
            await Clients.All.SendAsync("ReceiveNamesByGroup", rooms);
        }
        public async Task AddToGroup(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task RemoveToGroup(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
