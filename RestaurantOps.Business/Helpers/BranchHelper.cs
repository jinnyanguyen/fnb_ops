using System.Security.Claims;

namespace RestaurantOps.Business.Helpers;

/// <summary>
/// Helper methods for retrieving branch information
/// from authenticated users.
/// </summary>
public static class BranchHelper
{
    /// <summary>
    /// Retrieves current user's branch ID.
    /// </summary>
    public static int GetBranchId(ClaimsPrincipal user)
    {
        var claim = user.FindFirst("BranchId");

        if (claim == null)
            throw new Exception("Branch claim not found.");

        return int.Parse(claim.Value);
    }
}