using AspTestStage.BaseClasses;

namespace AspTestStage.Database.Domain;

public class Group : EntityBase
{
    public string Speciality { get; set; }
    public int Number { get; set; }
    public int TeacherId { get; set; }
    public User Teacher { get; set; }

    public List<GroupStudent> GroupStudents { get; set; } = new();
}