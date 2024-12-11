import { Product } from './product.model';

export interface Order {
  ID: number;
  Date: Date;
  Client: string;
  Merchant: string;
  Service: string;
  'Nº of people': number;
  Products: Product[];
  Price: number;
  Status: string;
}
