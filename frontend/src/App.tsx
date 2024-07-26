import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import ListaProdutos from './Componentes/ListaDeProdutos';
import CadastrarProduto from './Componentes/CadastroDeProduto';
import Menu from './Componentes/Menu';

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Menu />}>
          <Route index element={<ListaProdutos />} />
          <Route path="cadastrar" element={<CadastrarProduto />} />
        </Route>
      </Routes>
    </Router>
  );
};

export default App;
