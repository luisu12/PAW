using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepository) : ControllerBase
    {
        [HttpGet(Name = "GetNotifications")]
        public async Task<IEnumerable<NotificationDTO>> GetAll()
        {
            var notifications = await notificationRepository.ReadAsync() ?? [];
            return notifications.Select(NotificationDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            return NotificationDTO.ConvertFrom(notification);
        }

        /*[HttpPost("filter", Name = "FilterNotifications")]
        public async Task<IEnumerable<Notification>> Filter(ConditionViewModel condition)
        {
            var predicate = ConditionResolver<Notification>.ResolveCondition(condition.Criteria, condition.Property, condition.Value, condition.Start, condition.End);
            var results = await businessNotification.Filter(predicate);
            return results;
        }*/

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Notification> Notifications)
        {
            foreach (var p in Notifications)
            {
                if (p.Id > 0)
                    await notificationRepository.CreateAsync(p);
                else
                    await notificationRepository.UpdateAsync(p);
            }

            /*Notifications.ToList().ForEach(async x =>
            {
                if (x.Id > 0)
                    await notificationRepository.CreateAsync(x);
                else
                    await notificationRepository.UpdateAsync(x);
            });*/
            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(Notification notification)
        {
            return await notificationRepository.DeleteAsync(notification);
        }
    }
}