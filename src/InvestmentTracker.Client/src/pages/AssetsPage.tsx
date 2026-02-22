import { useState } from 'react';
import { useAssetsWithSnapshots } from '../api/hooks';
import { SnapshotTable } from '../components/assets/SnapshotTable';
import { AssetDetailChart } from '../components/charts/AssetDetailChart';
import { AssetsStackedChart } from '../components/charts/AssetsStackedChart';
import { AssetsPercentageChart } from '../components/charts/AssetsPercentageChart';

export function AssetsPage() {
    const { data: assets, isLoading } = useAssetsWithSnapshots();
    const [selectedAssetId, setSelectedAssetId] = useState<number | null>(null);

    if (isLoading) {
        return <div className="loading">Loading assets data...</div>;
    }

    if (!assets || assets.length === 0) {
        return (
            <div className="assets-page">
                <h1>Assets</h1>
                <div className="card">No assets found.</div>
            </div>
        );
    }

    const selectedAsset = selectedAssetId !== null
        ? assets.find(a => a.id === selectedAssetId) ?? null
        : null;

    return (
        <div className="assets-page">
            <h1>Assets</h1>

            <div className="chart-section">
                <h2>Snapshot History</h2>
                <SnapshotTable
                    assets={assets}
                    selectedAssetId={selectedAssetId}
                    onSelectAsset={setSelectedAssetId}
                />
            </div>

            {selectedAsset && (
                <div className="chart-section">
                    <h2>{selectedAsset.name} — Value Over Time</h2>
                    <AssetDetailChart asset={selectedAsset} />
                </div>
            )}

            <div className="chart-section">
                <h2>Asset Values Over Time</h2>
                <AssetsStackedChart assets={assets} />
            </div>

            <div className="chart-section">
                <h2>Asset Allocation Over Time (%)</h2>
                <AssetsPercentageChart assets={assets} />
            </div>
        </div>
    );
}
