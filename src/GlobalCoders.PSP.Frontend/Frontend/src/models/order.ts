import { Product } from "./product";

export interface Order {

    id: string;
    merchantId: string;
    merchantName: string;
    employeeId: string;
    employeeName: string;
    clientName: string;
    totalTax: number;
    discount: number;
    price: number;
    priceWithTax: number;
    totalPrice: number;
    paidSum: number;
    tips: number;
    status: number;
    products: Product[];
    payments: any[];
    discounts: any[];
    date: string;

}
