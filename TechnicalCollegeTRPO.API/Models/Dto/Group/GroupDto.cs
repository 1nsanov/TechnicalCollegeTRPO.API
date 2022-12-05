namespace TechnicalCollegeTRPO.API.Models.Dto.Group;

public class GroupDto : EntityDto
{
    public string Speciality { get; set; }
    public int Number { get; set; }
    public int TeacherId { get; set; }
}