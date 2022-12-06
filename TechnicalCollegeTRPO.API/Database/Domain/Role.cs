using AspTestStage.BaseClasses;

namespace AspTestStage.Database.Domain;

public partial class Role : EntityBase
{
    public string Code { get; set; }
    public string Name { get; set; }

    public List<User> Users { get; set; } = new();
}