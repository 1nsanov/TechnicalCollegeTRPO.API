using AspTestStage.BaseClasses;

namespace AspTestStage.Database.Domain;

public class GroupStudent : EntityBase
{
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public int StudentId { get; set; }
    public User Student { get; set; }
}