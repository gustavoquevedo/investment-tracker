import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip, Legend } from 'recharts';
import type { AllocationEntry } from '../../api/types';

interface AllocationChartProps {
  data: AllocationEntry[];
}

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884D8', '#82CA9D'];

export function AllocationChart({ data }: AllocationChartProps) {
    if (!data.length) return <div>No allocation data</div>;

    return (
      <div style={{ width: '100%', height: 400 }}>
        <ResponsiveContainer>
          <PieChart>
            <Pie
              data={data}
              cx="50%"
              cy="50%"
              labelLine={false}
              label={({ name, percent }: any) => `${name} ${(percent ? percent * 100 : 0).toFixed(0)}%`}
              outerRadius={120}
              fill="#8884d8"
              dataKey="value"
              nameKey="type"
            >
              {data.map((_, index) => (
                <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
              ))}
            </Pie>
            <Tooltip formatter={(val: number | string | undefined) => val !== undefined ? `$${Number(val).toLocaleString()}` : ''} />
            <Legend />
          </PieChart>
        </ResponsiveContainer>
      </div>
    );
}
