import type { AssetWithSnapshots } from '../../api/types';

interface SnapshotTableProps {
    assets: AssetWithSnapshots[];
    selectedAssetId: number | null;
    onSelectAsset: (id: number | null) => void;
}

export function SnapshotTable({ assets, selectedAssetId, onSelectAsset }: SnapshotTableProps) {
    if (!assets.length) return <div>No assets found.</div>;

    // Collect all unique dates across all assets, sorted newest first
    const allDates = [...new Set(
        assets.flatMap(a => a.snapshots.map(s => s.snapshotDate.split('T')[0]))
    )].sort((a, b) => b.localeCompare(a));

    // Build a lookup: assetId -> { date -> value }
    const valueLookup = new Map<number, Map<string, number>>();
    for (const asset of assets) {
        const dateMap = new Map<string, number>();
        for (const s of asset.snapshots) {
            dateMap.set(s.snapshotDate.split('T')[0], s.totalValue);
        }
        valueLookup.set(asset.id, dateMap);
    }

    const latestDate = allDates[0];

    return (
        <div className="snapshot-table-container">
            <table className="snapshot-table">
                <thead>
                    <tr>
                        <th className="sticky-col col-name">Name</th>
                        <th className="sticky-col col-type">Type</th>
                        <th className="sticky-col col-ticker">Ticker</th>
                        <th className="sticky-col col-isin">ISIN</th>
                        <th className="sticky-col col-fee">Fee %</th>
                        {allDates.map(date => (
                            <th key={date} className={date === latestDate ? 'date-col latest' : 'date-col'}>
                                {date}
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {assets.map(asset => {
                        const dateMap = valueLookup.get(asset.id)!;
                        return (
                            <tr
                                key={asset.id}
                                className={asset.id === selectedAssetId ? 'selected' : ''}
                                onClick={() => onSelectAsset(asset.id === selectedAssetId ? null : asset.id)}
                                style={{ cursor: 'pointer' }}
                            >
                                <td className="sticky-col col-name">{asset.name}</td>
                                <td className="sticky-col col-type">{asset.assetType}</td>
                                <td className="sticky-col col-ticker">{asset.ticker ?? '—'}</td>
                                <td className="sticky-col col-isin">{asset.isin ?? '—'}</td>
                                <td className="sticky-col col-fee">
                                    {(asset.feePercentagePerYear * 100).toFixed(2)}%
                                </td>
                                {allDates.map(date => {
                                    const val = dateMap.get(date);
                                    return (
                                        <td key={date} className={date === latestDate ? 'date-col latest' : 'date-col'}>
                                            {val !== undefined ? `$${val.toLocaleString()}` : '—'}
                                        </td>
                                    );
                                })}
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );
}
