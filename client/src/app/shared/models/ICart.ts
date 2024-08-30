import {nanoid} from 'nanoid';

export type ICart = {
    id: string;
    items: ICartItem[];
}

export type ICartItem = {
    productId: number;
    productName: string; 
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

export class Cart implements ICart {
    id = nanoid();
    items: ICartItem[] = [];
}   