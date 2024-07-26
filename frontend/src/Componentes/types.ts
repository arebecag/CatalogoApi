export interface Produto {
    idProduto: number;
    nome: string;
    valor: number;
}

export interface PagedResponse<Produto> {
    Data: Produto[];
    TotalRecords: number;
    PageNumber: number;
    PageSize: number;
    TotalPages: number;
}
