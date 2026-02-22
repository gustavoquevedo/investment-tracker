import { useQuery } from '@tanstack/react-query';
import { apiClient } from './client';
import type {
    PortfolioSummary,
    PortfolioHistory,
    PortfolioAllocation,
    PortfolioReturns,
    AssetWithSnapshots
} from './types';

export function usePortfolioSummary() {
    return useQuery({
        queryKey: ['portfolio', 'summary'],
        queryFn: () => apiClient<PortfolioSummary>('/portfolio/summary')
    });
}

export function usePortfolioHistory(from?: string, to?: string) {
    return useQuery({
        queryKey: ['portfolio', 'history', from, to],
        queryFn: () => {
             const params = new URLSearchParams();
             if (from) params.append('from', from);
             if (to) params.append('to', to);
             const queryString = params.toString() ? `?${params.toString()}` : '';
             return apiClient<PortfolioHistory>(`/portfolio/history${queryString}`);
        }
    });
}

export function usePortfolioAllocation() {
    return useQuery({
        queryKey: ['portfolio', 'allocation'],
        queryFn: () => apiClient<PortfolioAllocation>('/portfolio/allocation')
    });
}

export function usePortfolioReturns(asOf?: string) {
    return useQuery({
        queryKey: ['portfolio', 'returns', asOf],
        queryFn: () => {
            const queryString = asOf ? `?asOf=${asOf}` : '';
            return apiClient<PortfolioReturns>(`/portfolio/returns${queryString}`);
        }
    });
}

export function useAssetsWithSnapshots() {
    return useQuery({
        queryKey: ['portfolio', 'assets-with-snapshots'],
        queryFn: () => apiClient<AssetWithSnapshots[]>('/portfolio/assets-with-snapshots')
    });
}
