import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Button, Form, Card } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './CadastroDeProduto.css';
import { useNavigate, useParams } from 'react-router-dom';

const CadastrarProduto: React.FC = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const [nomeProduto, setNomeProduto] = useState<string>('');
    const [valorProduto, setValorProduto] = useState<string>('');
    const [modoEdicao, setModoEdicao] = useState<boolean>(false);

    useEffect(() => {
        if (id) {
            setModoEdicao(true);
            axios.get(`https://localhost:7264/api/produtos/${id}`)
                .then(response => {
                    setNomeProduto(response.data.nome);
                    setValorProduto(response.data.valor.toFixed(2).replace('.', ','));
                })
                .catch(error => console.error('Erro ao carregar o produto:', error));
        } else {
            setModoEdicao(false);
            setNomeProduto('');
            setValorProduto('');
        }
    }, [id]);

    const handleCadastro = async () => {
        if (nomeProduto.trim() === '' || valorProduto.trim() === '') {
            alert('Por favor, preencha todos os campos.');
            return;
        }

        const valorNumerico = parseFloat(valorProduto.replace(',', '.'));

        if (isNaN(valorNumerico)) {
            alert('Valor inválido.');
            return;
        }

        try {
            if (modoEdicao && id) {
                await axios.put(`https://localhost:7264/api/produtos/${id}`, {
                    nome: nomeProduto,
                    valor: valorNumerico
                });
            } else {
                await axios.post('https://localhost:7264/api/produtos', {
                    nome: nomeProduto,
                    valor: valorNumerico
                });
            }
            navigate('/');
        } catch (error) {
            if (axios.isAxiosError(error)) {
                alert(error.response?.data || 'Ocorreu um erro ao cadastrar/editar o produto. Tente novamente.');
            } else {
                alert('Ocorreu um erro ao cadastrar/editar o produto. Tente novamente.');
            }
        }
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        handleCadastro();
    };

    return (
        <div className="container cadastro-produto">
            <Card className="product-card">
                <Card.Header>
                    <h1>{modoEdicao ? 'Editar Produto' : 'Cadastrar Produto'}</h1>
                </Card.Header>
                <Card.Body>
                    <Form onSubmit={handleSubmit}>
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
                            <div className="form-control-valor">
                                <div className="prefixo">R$</div>
                                <Form.Control
                                    type="text"
                                    placeholder="Digite o valor do produto"
                                    value={valorProduto}
                                    onChange={(e) => setValorProduto(e.target.value)}
                                    required
                                />
                            </div>
                        </Form.Group>
                        <div className="button-group">
                            <Button type="submit" className="btn-cadastrar">
                                {modoEdicao ? 'Salvar Alterações' : 'Cadastrar'}
                            </Button>
                            <Button variant="secondary" onClick={() => navigate('/')}>Voltar</Button>
                        </div>
                    </Form>
                </Card.Body>
            </Card>
        </div>
    );
};

export default CadastrarProduto;
