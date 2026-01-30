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
  import type { PortfolioHistoryPoint } from '../../api/types';
  
  interface PortfolioValueChartProps {
    data: PortfolioHistoryPoint[];
  }
  
  export function PortfolioValueChart({ data }: PortfolioValueChartProps) {
    if (!data.length) return <div>No data available</div>;
  
    // Determine bounds for Y axis to make chart look better
    const values = data.map(d => d.value);
    const invested = data.map(d => d.invested);
    const all = [...values, ...invested];
    const min = Math.min(...all) * 0.95;
    const max = Math.max(...all) * 1.05;
  
    return (
      <div style={{ width: '100%', height: 400 }}>
        <ResponsiveContainer>
          <AreaChart
            data={data}
            margin={{
              top: 10,
              right: 30,
              left: 40,
              bottom: 0,
            }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis domain={[min, max]} tickFormatter={(val: number | string) => `$${Number(val).toLocaleString()}`} />
            <Tooltip formatter={(val: number | string | undefined) => val !== undefined ? `$${Number(val).toLocaleString()}` : ''} />
            <Legend />
            <Area type="monotone" dataKey="value" name="Total Value" stroke="#8884d8" fill="#8884d8" fillOpacity={0.3} />
            <Area type="monotone" dataKey="invested" name="Invested" stroke="#82ca9d" fill="#82ca9d" fillOpacity={0.3} />
          </AreaChart>
        </ResponsiveContainer>
      </div>
    );
  }
