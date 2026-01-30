export interface PortfolioSummary {
    totalValue: number;
    totalInvested: number;
    pnL: number;
    pnLPercent: number;
    assetCount: number;
    lastUpdated: string | null;
}

export interface PortfolioHistoryPoint {
    date: string; // DateOnly as string
    value: number;
    invested: number;
}

export interface PortfolioHistory {
    points: PortfolioHistoryPoint[];
}

export interface AllocationEntry {
    type: string;
    value: number;
    percentage: number;
}

export interface PortfolioAllocation {
    byType: AllocationEntry[];
}

export interface PeriodReturns {
    ytd: number | null;
    oneYear: number | null;
    threeYear: number | null;
    fiveYear: number | null;
    allTime: number | null;
}

export interface PortfolioReturns {
    twr: number;
    mwr: number;
    periods: PeriodReturns;
}
