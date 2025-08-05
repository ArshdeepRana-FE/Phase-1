/// <summary>
/// Class containing SQL queries related to owner.
/// </summary>
public class OwnerQueries
{
    /// <summary>
    /// Returns restuarant name and id on basis of owner ID
    /// </summary>
    public const string GetRestaurantByOwnerId = @"
        SELECT r.id, r.name
        FROM owner_restaurant orr
        JOIN restaurants r ON orr.restaurant_id = r.id
        WHERE orr.owner_id = @OwnerId";
}
