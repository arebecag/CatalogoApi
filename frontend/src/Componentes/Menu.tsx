import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import { FaPlus, FaList } from 'react-icons/fa';
import './Menu.css';
import logo from './1126.png';

const Layout: React.FC = () => {
    const toggleSidebar = () => {
        const sidebar = document.querySelector('.sidebar') as HTMLElement;
        sidebar.classList.toggle('collapsed');
    };

    return (
        <div className="layout">
            <aside className="sidebar">
                <img src={logo} alt="Logo" className="logo" />
                <ul>
                    <li>
                        <Link to="/" className="sidebar-btn">
                            <FaList className="sidebar-icon" /> <span className="sidebar-text">Lista de Produtos</span>
                        </Link>
                    </li>
                    <li>
                        <Link to="/cadastrar" className="sidebar-btn">
                            <FaPlus className="sidebar-icon" /> <span className="sidebar-text">Cadastrar Produto</span>
                        </Link>
                    </li>
                </ul>
            </aside>
            <div className="main-content">
                <header className="header">
                    <button className="toggle-btn" onClick={toggleSidebar}>☰</button>
                </header>
                <main>
                    <Outlet />
                </main>
            </div>
        </div>
    );
};

export default Layout;
