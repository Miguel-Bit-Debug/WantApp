using Flunt.Notifications;

namespace WantApp.Domain.Models.Product;

public abstract class Entity : Notifiable<Notification>
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }
    public string CreatedBy { get; set; }
    public string EditedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime EditedOn { get; set; }
}
