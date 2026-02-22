import { useMemo } from 'react';
import {
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
    Legend
} from 'recharts';
import type { AssetWithSnapshots } from '../../api/types';

interface AssetDetailChartProps {
    asset: AssetWithSnapshots;
}

export function AssetDetailChart({ asset }: AssetDetailChartProps) {
    const data = useMemo(() => {
        // Sort contributions by date to compute running total
        const sortedContributions = [...asset.contributions]
            .sort((a, b) => a.dateMade.localeCompare(b.dateMade));

        // Sort snapshots by date ascending
        const sortedSnapshots = [...asset.snapshots]
            .sort((a, b) => a.snapshotDate.localeCompare(b.snapshotDate));

        if (!sortedSnapshots.length) return [];

        // For each snapshot date, compute accumulated contributions up to that date
        return sortedSnapshots.map(s => {
            const snapshotDate = s.snapshotDate.split('T')[0];
            const invested = sortedContributions
                .filter(c => c.dateMade.split('T')[0] <= snapshotDate)
                .reduce((sum, c) => sum + c.amount, 0);

            return {
                date: snapshotDate,
                value: s.totalValue,
                invested
            };
        });
    }, [asset]);

    if (!data.length) return <div>No snapshot data for this asset.</div>;

    const allValues = data.flatMap(d => [d.value, d.invested]);
    const min = Math.min(...allValues) * 0.95;
    const max = Math.max(...allValues) * 1.05;

    return (
        <div style={{ width: '100%', height: 300 }}>
            <ResponsiveContainer>
                <AreaChart data={data} margin={{ top: 10, right: 30, left: 40, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="date" />
                    <YAxis domain={[min, max]} tickFormatter={(val: number) => `$${val.toLocaleString()}`} />
                    <Tooltip
                        wrapperStyle={{ zIndex: 10 }}
                        contentStyle={{ backgroundColor: '#fff', border: '1px solid #ccc' }}
                        formatter={(val: number | string | undefined) => val !== undefined ? `$${Number(val).toLocaleString()}` : ''}
                    />
                    <Legend />
                    <Area type="monotone" dataKey="value" name="Value" stroke="#8884d8" fill="#8884d8" fillOpacity={0.3} />
                    <Area type="monotone" dataKey="invested" name="Invested" stroke="#82ca9d" fill="#82ca9d" fillOpacity={0.3} />
                </AreaChart>
            </ResponsiveContainer>
        </div>
    );
}
