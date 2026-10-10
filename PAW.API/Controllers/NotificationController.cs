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
        public async Task<bool> Save([FromBody] IEnumerable<PAW.Models.DTO.NotificationDTO> Notifications)
        {
            foreach (var dto in Notifications)
            {
                var p = PAW.Models.DTO.NotificationDTO.ConvertTo(dto);
                // Update when existing, create when new
                if (p.Id > 0)
                    await notificationRepository.UpdateAsync(p);
                else
                    await notificationRepository.CreateAsync(p);
            }

            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            if (notification == null) return false;
            return await notificationRepository.DeleteAsync(notification);
        }
    }
}