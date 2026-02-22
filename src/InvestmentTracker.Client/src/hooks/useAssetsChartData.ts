import { useMemo } from 'react';
import type { AssetWithSnapshots } from '../api/types';

interface ChartDataPoint {
    date: string;
    [key: string]: string | number;
}

export function useAssetsChartData(assets: AssetWithSnapshots[]) {
    return useMemo(() => {
        if (!assets.length) return { chartData: [], percentageData: [], assetNames: [] };

        const assetNames = assets.map(a => a.name);

        // Collect all unique dates, sorted oldest first (for chart x-axis)
        const allDates = [...new Set(
            assets.flatMap(a => a.snapshots.map(s => s.snapshotDate.split('T')[0]))
        )].sort();

        // Build lookup: assetName -> { date -> value }
        const lookup = new Map<string, Map<string, number>>();
        for (const asset of assets) {
            const dateMap = new Map<string, number>();
            for (const s of asset.snapshots) {
                dateMap.set(s.snapshotDate.split('T')[0], s.totalValue);
            }
            lookup.set(asset.name, dateMap);
        }

        // Absolute stacked data
        const chartData: ChartDataPoint[] = allDates.map(date => {
            const point: ChartDataPoint = { date };
            for (const name of assetNames) {
                point[name] = lookup.get(name)?.get(date) ?? 0;
            }
            return point;
        });

        // Percentage stacked data
        const percentageData: ChartDataPoint[] = allDates.map(date => {
            const point: ChartDataPoint = { date };
            const total = assetNames.reduce((sum, name) => {
                return sum + (lookup.get(name)?.get(date) ?? 0);
            }, 0);

            for (const name of assetNames) {
                const val = lookup.get(name)?.get(date) ?? 0;
                point[name] = total > 0 ? (val / total) * 100 : 0;
                point[`${name}_abs`] = val;
            }
            return point;
        });

        return { chartData, percentageData, assetNames };
    }, [assets]);
}
