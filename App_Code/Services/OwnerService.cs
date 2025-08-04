using System.Collections.Generic;

public class OwnerService
{
    private readonly OwnerRepository _repository;

    public OwnerService()
    {
        _repository = new OwnerRepository();
    }

    /// <summary>
    /// Gets all restaurants for a specific owner.
    /// </summary>
    /// <param name="ownerId">The user ID of the owner.</param>
    /// <returns>List of OwnerRestaurant.</returns>
    public List<int> GetRestaurantsForOwner(int ownerId)
    {
        return _repository.GetRestaurantsByOwnerId(ownerId);
    }
}
