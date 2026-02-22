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

interface AssetsPercentageChartProps {
    assets: AssetWithSnapshots[];
}

export function AssetsPercentageChart({ assets }: AssetsPercentageChartProps) {
    const { percentageData, assetNames } = useAssetsChartData(assets);

    if (!percentageData.length) return <div>No snapshot data available.</div>;

    return (
        <div style={{ width: '100%', height: 400 }}>
            <ResponsiveContainer>
                <AreaChart data={percentageData} margin={{ top: 10, right: 30, left: 40, bottom: 0 }}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="date" />
                    <YAxis domain={[0, 100]} tickFormatter={(val: number) => `${val}%`} />
                    <Tooltip
                        wrapperStyle={{ zIndex: 10 }}
                        contentStyle={{ backgroundColor: '#fff', border: '1px solid #ccc' }}
                        formatter={(val, name) => {
                            if (val === undefined) return '';
                            const numVal = Number(val);
                            if (!name) return `${numVal.toFixed(1)}%`;
                            const entry = percentageData.find(d =>
                                Object.entries(d).some(([k, v]) => k === name && v === numVal)
                            );
                            const absVal = entry?.[`${name}_abs`] as number | undefined;
                            return absVal !== undefined
                                ? `${numVal.toFixed(1)}% ($${absVal.toLocaleString()})`
                                : `${numVal.toFixed(1)}%`;
                        }}
                    />
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
