import { usePortfolioSummary, usePortfolioHistory, usePortfolioAllocation, usePortfolioReturns } from '../api/hooks';
import { PortfolioValueChart } from '../components/charts/PortfolioValueChart';
import { AllocationChart } from '../components/charts/AllocationChart';
import { PnLTrendChart } from '../components/charts/PnLTrendChart';

export function DashboardPage() {
    const { data: summary, isLoading: isSummaryLoading } = usePortfolioSummary();
    const { data: history, isLoading: isHistoryLoading } = usePortfolioHistory();
    const { data: allocation, isLoading: isAllocationLoading } = usePortfolioAllocation();
    const { data: returns, isLoading: isReturnsLoading } = usePortfolioReturns();

    if (isSummaryLoading || isHistoryLoading || isAllocationLoading || isReturnsLoading) {
        return <div className="loading">Loading dashboard data...</div>;
    }

    if (!summary) return <div>Failed to load portfolio data.</div>;

    return (
        <div className="dashboard">
            <header className="dashboard-header">
                <h1>Portfolio Dashboard</h1>
                <div className="summary-cards">
                    <div className="card">
                        <h3>Total Value</h3>
                        <div className="value">${summary.totalValue.toLocaleString()}</div>
                        <div className="subtitle">{summary.assetCount} Assets</div>
                    </div>
                    <div className="card">
                        <h3>Total Invested</h3>
                        <div className="value">${summary.totalInvested.toLocaleString()}</div>
                    </div>
                    <div className="card">
                        <h3>Total P&L</h3>
                        <div className={`value ${summary.pnL >= 0 ? 'positive' : 'negative'}`}>
                            ${summary.pnL.toLocaleString()} ({summary.pnLPercent}%)
                        </div>
                    </div>
                    <div className="card">
                        <h3>TWR / MWR</h3>
                        <div className="value">
                            {returns?.twr.toFixed(2)}% / {returns?.mwr.toFixed(2)}%
                        </div>
                    </div>
                </div>
            </header>

            <main className="dashboard-content">
                <div className="chart-section">
                    <h2>Portfolio Value History</h2>
                    {history && <PortfolioValueChart data={history.points} />}
                </div>

                <div className="chart-row">
                    <div className="chart-section half">
                        <h2>Asset Allocation</h2>
                        {allocation && <AllocationChart data={allocation.byType} />}
                    </div>
                    <div className="chart-section half">
                        <h2>P&L Trend</h2>
                        {history && <PnLTrendChart data={history.points} />}
                    </div>
                </div>

                <div className="returns-section">
                    <h2>Period Returns</h2>
                    {returns && (
                        <div className="returns-grid">
                            <div className="return-item">
                                <span className="label">YTD</span>
                                <span className="value">{returns.periods.ytd?.toFixed(2)}%</span>
                            </div>
                            <div className="return-item">
                                <span className="label">1 Year</span>
                                <span className="value">{returns.periods.oneYear?.toFixed(2)}%</span>
                            </div>
                            <div className="return-item">
                                <span className="label">3 Year</span>
                                <span className="value">{returns.periods.threeYear?.toFixed(2)}%</span>
                            </div>
                             <div className="return-item">
                                <span className="label">All Time</span>
                                <span className="value">{returns.periods.allTime?.toFixed(2)}%</span>
                            </div>
                        </div>
                    )}
                </div>
            </main>
        </div>
    );
}
