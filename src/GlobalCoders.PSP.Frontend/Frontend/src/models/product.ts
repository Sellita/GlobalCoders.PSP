export interface Product {
    id: string;
    displayName: string;
    description: string;
    stock: number;
    taxName: string;
    taxValue: number;
    categoryId: string;
    category: string;
    price: number;
    productState: number;
    merchantId: string;
    merchant: string;
    creationDate: string;
    lastUpdateDate: string;
}
