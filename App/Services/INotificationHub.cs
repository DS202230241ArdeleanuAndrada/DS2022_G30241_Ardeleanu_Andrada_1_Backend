using App.Models;
using App.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface INotificationHub
    {
        Task SendMessage(NotifyMessage message);
    }
}