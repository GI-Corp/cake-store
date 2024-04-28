namespace Identity.Application.Dto.Identity;

public class AppUserDto
{
    public virtual Guid Id { get; set; }
    public virtual string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public UserProfileDto UserProfile { get; set; }
    public UserSettingDto UserSetting { get; set; }
}