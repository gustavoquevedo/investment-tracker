using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using InvestmentTracker.Api.Features.Assets.GetAssets;
using InvestmentTracker.Api.Features.Assets.GetAssetById;
using InvestmentTracker.Api.Features.Assets.CreateAsset;
using InvestmentTracker.Api.Features.Assets.UpdateAsset;
using InvestmentTracker.Api.Features.Assets.DeleteAsset;
using InvestmentTracker.Api.Features.Assets.ManageSnapshots;
using InvestmentTracker.Api.Features.Assets.ManageContributions;

namespace InvestmentTracker.Api.Features.Assets;

public static class AssetsEndpoints
{
    public static void MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        GetAssetsEndpoint.Map(app);
        GetAssetByIdEndpoint.Map(app);
        CreateAssetEndpoint.Map(app);
        UpdateAssetEndpoint.Map(app);
        DeleteAssetEndpoint.Map(app);
        GetSnapshotsEndpoint.Map(app);
        AddSnapshotEndpoint.Map(app);
        GetContributionsEndpoint.Map(app);
        AddContributionEndpoint.Map(app);
    }
}
