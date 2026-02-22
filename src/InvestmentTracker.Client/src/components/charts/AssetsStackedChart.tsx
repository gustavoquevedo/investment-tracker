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
import { useAssetsChartData } from '../../hooks/useAssetsChartData';

const COLORS = ['#8884d8', '#82ca9d', '#ffc658', '#ff7300', '#0088fe', '#00c49f', '#ffbb28', '#ff8042', '#a4de6c', '#d0ed57'];

interface AssetsStackedChartProps {
    assets: AssetWithSnapshots[];
}

export function AssetsStackedChart({ assets }: AssetsStackedChartProps) {
    const { chartData, assetNames } = useAssetsChartData(assets);

    if (!chartData.length) return <div>No snapshot data available.</div>;

    return (
        <div style={{ width: '100%', height: 400 }}>
            <ResponsiveContainer>
                <AreaChart data={chartData} margin={{ top: 10, right: 30, left: 40, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="date" />
                    <YAxis tickFormatter={(val: number) => `$${val.toLocaleString()}`} />
                    <Tooltip wrapperStyle={{ zIndex: 10 }} contentStyle={{ backgroundColor: '#fff', border: '1px solid #ccc' }} formatter={(val: number | string | undefined) => val !== undefined ? `$${Number(val).toLocaleString()}` : ''} />
                    <Legend />
                    {assetNames.map((name, i) => (
                        <Area
                            key={name}
                            type="monotone"
                            dataKey={name}
                            stackId="1"
                            stroke={COLORS[i % COLORS.length]}
                            fill={COLORS[i % COLORS.length]}
                            fillOpacity={0.6}
                        />
                    ))}
                </AreaChart>
            </ResponsiveContainer>
        </div>
    );
}
