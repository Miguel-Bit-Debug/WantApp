using Flunt.Validations;

namespace WantApp.Domain.Models.Product;

public  class Category : Entity
{
    public Category(string name, string createdBy, string editedBy)
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "Name");

        AddNotifications(contract);

        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
    }

    public string Name { get; set; }
    public bool Active { get; set; }
}
