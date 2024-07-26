import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Button, Modal, Table, Form } from 'react-bootstrap';
import './ListaDeProdutos.css';
import { Produto } from './types';
import { FaSortAlphaDown, FaSortAlphaUp, FaArrowLeft, FaArrowRight } from 'react-icons/fa';

const ListaProdutos: React.FC = () => {
    const [produtos, setProdutos] = useState<Produto[]>([]);
    const [produtoEdicao, setProdutoEdicao] = useState<Produto | null>(null);
    const [showModalEditar, setShowModalEditar] = useState<boolean>(false);
    const [produtoParaExcluir, setProdutoParaExcluir] = useState<Produto | null>(null);
    const [showModalExcluir, setShowModalExcluir] = useState<boolean>(false);
    const [paginaAtual, setPaginaAtual] = useState<number>(1);
    const [produtosPorPagina] = useState<number>(10);
    const [ordenarAsc, setOrdenarAsc] = useState<boolean>(true);
    const [nomeProduto, setNomeProduto] = useState<string>('');
    const [valorProduto, setValorProduto] = useState<string>('');

    useEffect(() => {
        axios.get('https://localhost:7264/api/produtos')
            .then(response => setProdutos(response.data))
            .catch(error => console.error('Erro ao carregar produtos:', error));
    }, []);

    const handleExcluir = async () => {
        if (produtoParaExcluir) {
            try {
                await axios.delete(`https://localhost:7264/api/produtos/${produtoParaExcluir.idProduto}`);
                setProdutos(produtos.filter(p => p.idProduto !== produtoParaExcluir.idProduto));
                setShowModalExcluir(false);
                setProdutoParaExcluir(null);
            } catch (error) {
                console.error('Erro ao excluir o produto:', error);
                alert('Ocorreu um erro ao excluir o produto. Tente novamente.');
            }
        } else {
            alert('Produto para exclusão não está definido.');
        }
    };

    const handleShowModalExcluir = (produto: Produto) => {
        setProdutoParaExcluir(produto);
        setShowModalExcluir(true);
    };

    const handleShowModalEditar = (produto: Produto) => {
        setProdutoEdicao(produto);
        setNomeProduto(produto.nome);
        setValorProduto(produto.valor.toFixed(2).replace('.', ','));
        setShowModalEditar(true);
    };

    const handleSaveEdicao = async () => {
        if (!produtoEdicao) return;

        const valorNumerico = parseFloat(valorProduto.replace(',', '.'));
        if (isNaN(valorNumerico)) {
            alert('Valor inválido.');
            return;
        }

        try {
            await axios.put(`https://localhost:7264/api/produtos/${produtoEdicao.idProduto}`, {
                idProduto: produtoEdicao.idProduto,
                nome: nomeProduto,
                valor: valorNumerico
            });
            const response = await axios.get('https://localhost:7264/api/produtos');
            setProdutos(response.data);
            resetEdicaoModal();
        } catch (error) {
            console.error('Erro ao editar o produto:', error);
            alert('Ocorreu um erro ao editar o produto. Tente novamente.');
        }
    };

    const resetEdicaoModal = () => {
        setNomeProduto('');
        setValorProduto('');
        setProdutoEdicao(null);
        setShowModalEditar(false);
    };

    const handleOrdenar = () => {
        setOrdenarAsc(!ordenarAsc);
    };

    const formatarValor = (valor: number) => {
        return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    };

    const sortedProdutos = [...produtos].sort((a, b) => {
        if (a.nome && b.nome) {
            return ordenarAsc ? a.nome.localeCompare(b.nome) : b.nome.localeCompare(a.nome);
        }
        return 0;
    });

    const paginatedProdutos = sortedProdutos.slice((paginaAtual - 1) * produtosPorPagina, paginaAtual * produtosPorPagina);
    const totalPaginas = Math.ceil(produtos.length / produtosPorPagina);

    const handlePageChange = (page: number) => {
        setPaginaAtual(page);
    };

    const handlePreviousPage = () => {
        if (paginaAtual > 1) {
            setPaginaAtual(paginaAtual - 1);
        }
    };

    const handleNextPage = () => {
        if (paginaAtual < totalPaginas) {
            setPaginaAtual(paginaAtual + 1);
        }
    };

    return (
        <div className="container-lista lista-produtos">
            <div className="list-container">
                {produtos.length === 0 ? (
                    <div className="empty-message">Nenhum produto disponível.</div>
                ) : (
                    <>
                        <Table striped bordered hover>
                            <thead>
                                <tr>
                                    <th className="nome-col">
                                        Nome
                                        <Button variant="link" onClick={handleOrdenar} className="ordenar-btn">
                                            {ordenarAsc ? <FaSortAlphaUp /> : <FaSortAlphaDown />}
                                        </Button>
                                    </th>
                                    <th>Valor</th>
                                    <th className="acoes-col">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {paginatedProdutos.map(produto => (
                                    <tr key={produto.idProduto}>
                                        <td className="nome-produto" onClick={() => handleShowModalEditar(produto)}>
                                            {produto.nome}
                                        </td>
                                        <td>{formatarValor(produto.valor)}</td>
                                        <td className="acoes-col">
                                            <Button variant="danger" onClick={() => handleShowModalExcluir(produto)} size="sm">
                                                Excluir
                                            </Button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                        <div className="pagination-container">
                            <Button
                                variant="link"
                                onClick={handlePreviousPage}
                                className="pagination-btn"
                                disabled={paginaAtual === 1}
                            >
                                <FaArrowLeft />
                            </Button>
                            {Array.from({ length: totalPaginas }, (_, index) => (
                                <Button
                                    key={index + 1}
                                    variant="link"
                                    onClick={() => handlePageChange(index + 1)}
                                    className={`pagination-btn ${paginaAtual === index + 1 ? 'active' : ''}`}
                                >
                                    {index + 1}
                                </Button>
                            ))}
                            <Button
                                variant="link"
                                onClick={handleNextPage}
                                className="pagination-btn"
                                disabled={paginaAtual === totalPaginas}
                            >
                                <FaArrowRight />
                            </Button>
                        </div>
                    </>
                )}
            </div>

            <Modal show={showModalEditar} onHide={resetEdicaoModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Editar Produto</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group controlId="formNome">
                            <Form.Label>Nome do Produto</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Digite o nome do produto"
                                value={nomeProduto}
                                onChange={(e) => setNomeProduto(e.target.value)}
                                required
                            />
                        </Form.Group>
                        <Form.Group controlId="formValor">
                            <Form.Label>Valor do Produto</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Digite o valor do produto"
                                value={valorProduto}
                                onChange={(e) => setValorProduto(e.target.value)}
                                required
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={resetEdicaoModal}>Fechar</Button>
                    <Button variant="primary" onClick={handleSaveEdicao}>Salvar</Button>
                </Modal.Footer>
            </Modal>

            <Modal show={showModalExcluir} onHide={() => setShowModalExcluir(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirmar Exclusão</Modal.Title>
                </Modal.Header>
                <Modal.Body>Realmente deseja excluir este produto?</Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModalExcluir(false)}>Não</Button>
                    <Button variant="danger" onClick={handleExcluir}>Sim</Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default ListaProdutos;
