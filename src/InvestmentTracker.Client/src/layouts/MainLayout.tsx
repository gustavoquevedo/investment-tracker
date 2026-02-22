import { NavLink, Outlet } from 'react-router-dom';

export function MainLayout() {
    return (
        <div className="app-container">
            <nav className="main-nav">
                <NavLink to="/" end className={({ isActive }) => isActive ? 'nav-tab active' : 'nav-tab'}>
                    Dashboard
                </NavLink>
                <NavLink to="/assets" className={({ isActive }) => isActive ? 'nav-tab active' : 'nav-tab'}>
                    Assets
                </NavLink>
            </nav>
            <main className="main-content">
                <Outlet />
            </main>
        </div>
    );
}
