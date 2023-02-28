namespace Sellers;

#nullable disable
public class RoleEntity
{
    public long UserSequence { get; set; }
    public Guid ShopId { set; get; }
    public string RoleName { get; set; }
    public UserEntity User { get; set; }
}
#nullable enable