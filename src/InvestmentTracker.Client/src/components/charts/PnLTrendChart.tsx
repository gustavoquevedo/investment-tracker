import {
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
    ReferenceLine
  } from 'recharts';
  import type { PortfolioHistoryPoint } from '../../api/types';
  
  interface PnLTrendChartProps {
    data: PortfolioHistoryPoint[];
  }
  
  export function PnLTrendChart({ data }: PnLTrendChartProps) {
    if (!data.length) return <div>No data available</div>;

    const pnlData = data.map(d => ({
        date: d.date,
        pnl: d.value - d.invested
    }));
  
    return (
      <div style={{ width: '100%', height: 300 }}>
        <ResponsiveContainer>
          <AreaChart
            data={pnlData}
            margin={{
              top: 10,
              right: 30,
              left: 0,
              bottom: 0,
            }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis tickFormatter={(val: number | string) => `$${Number(val).toLocaleString()}`} />
            <Tooltip formatter={(val: number | string | undefined) => val !== undefined ? [`$${Number(val).toLocaleString()}`, 'P&L'] : []} />
            <ReferenceLine y={0} stroke="#000" />
            <Area type="monotone" dataKey="pnl" name="P&L" stroke="#ffc658" fill="#ffc658" fillOpacity={0.6} />
          </AreaChart>
        </ResponsiveContainer>
      </div>
    );
  }
