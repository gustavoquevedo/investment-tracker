import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { MainLayout } from './layouts/MainLayout';
import { DashboardPage } from './pages/DashboardPage';
import { AssetsPage } from './pages/AssetsPage';
import './App.css';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route element={<MainLayout />}>
                    <Route path="/" element={<DashboardPage />} />
                    <Route path="/assets" element={<AssetsPage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default App;
