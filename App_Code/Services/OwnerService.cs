using System.Collections.Generic;

/// <summary>
/// Service class for owner
/// </summary>
public class OwnerService
{
    private readonly OwnerRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="OwnerService"/> class.
    /// </summary>
    public OwnerService()
    {
        _repository = new OwnerRepository();
    }

    /// <summary>
    /// Gets all restaurants for a specific owner.
    /// </summary>
    /// <param name="ownerId">The user ID of the owner.</param>
    /// <summary>
    /// Retrieves the restaurants associated with an owner.
    /// </summary>
    /// <param name="ownerId">The owner's identifier.</param>
    /// <returns>The restaurants associated with the specified owner.</returns>
    public List<Restaurant> GetRestaurantsForOwner(int ownerId)
    {
        return _repository.GetRestaurantsByOwnerId(ownerId);
    }
}
